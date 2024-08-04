namespace Generator
{
    using CppAst;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    public partial class CsCodeGenerator
    {
        private readonly CsCodeGeneratorSettings settings;

        public CsCodeGenerator(CsCodeGeneratorSettings settings)
        {
            this.settings = settings;
        }

        public CsCodeGenerator(string settingsPath)
        {
            settings = CsCodeGeneratorSettings.Load(settingsPath);
        }

        public CsCodeGeneratorSettings Settings => settings;

        private CppParserOptions GetParserOptions()
        {
            var options = new CppParserOptions
            {
                ParseMacros = true,
            };

            options.Defines.Add("CIMGUI_DEFINE_ENUMS_AND_STRUCTS");

            return options;
        }

        public bool Generate(string headerFile, string outputPath)
        {
            var compilation = CppParser.ParseFile(headerFile, GetParserOptions());

            // Print diagnostic messages
            if (compilation.HasErrors)
            {
                for (int i = 0; i < compilation.Diagnostics.Messages.Count; i++)
                {
                    CppDiagnosticMessage? message = compilation.Diagnostics.Messages[i];
                    if (message.Type == CppLogMessageType.Error)
                    {
                        var currentColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message);
                        Console.ForegroundColor = currentColor;
                    }
                }

                return false;
            }

            Generate(compilation, outputPath);
            return true;
        }

        public void Generate(CppCompilation compilation, string outputPath)
        {
            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);

            if (settings.GenerateConstants)
            {
                GenerateConstants(compilation, outputPath);
            }
            if (settings.GenerateEnums)
            {
                GenerateEnums(compilation, outputPath);
            }
            if (settings.GenerateExtensions)
            {
                GenerateExtensions(compilation, outputPath);
            }
            if (settings.GenerateHandles)
            {
                GenerateHandles(compilation, outputPath);
            }
            if (settings.GenerateStructs)
            {
                GenerateStructAndUnions(compilation, outputPath);
            }
            if (settings.GenerateFunctions)
            {
                GenerateFunctions(compilation, outputPath);
            }
            if (settings.GenerateDelegates)
            {
                GenerateDelegates(compilation, outputPath);
            }
        }

        public GeneratorMetadata GetMetadata()
        {
            GeneratorMetadata metadata = new();
            metadata.CopyFrom(this);
            return metadata;
        }

        public void DefineLib(GeneratorMetadata metadata)
        {
            metadata.CopyTo(this);
        }

        private static bool WriteCsSummary(CppComment? comment, ICodeWriter writer)
        {
            if (comment is CppCommentFull full)
            {
                writer.WriteLine("/// <summary>");
                for (int i = 0; i < full.Children.Count; i++)
                {
                    WriteCsSummary(full.Children[i], writer);
                }
                writer.WriteLine("/// </summary>");
                return true;
            }
            if (comment is CppCommentParagraph paragraph)
            {
                for (int i = 0; i < paragraph.Children.Count; i++)
                {
                    WriteCsSummary(paragraph.Children[i], writer);
                }
                return true;
            }
            if (comment is CppCommentText text)
            {
                writer.WriteLine($"/// " + text.Text);
                return true;
            }

            if (comment == null || comment.Kind == CppCommentKind.Null)
            {
                return false;
            }

            throw new NotImplementedException();
        }

        private static void WriteCsSummary(CppComment? cppComment, out string? comment)
        {
            StringBuilder sb = new();
            if (cppComment is CppCommentFull full)
            {
                sb.AppendLine("/// <summary>");
                for (int i = 0; i < full.Children.Count; i++)
                {
                    WriteCsSummary(full.Children[i], out var subComment);
                    sb.Append(subComment);
                }
                sb.AppendLine("/// </summary>");
                comment = sb.ToString();
                return;
            }
            if (cppComment is CppCommentParagraph paragraph)
            {
                for (int i = 0; i < paragraph.Children.Count; i++)
                {
                    WriteCsSummary(paragraph.Children[i], out var subComment);
                    sb.Append(subComment);
                }
                comment = sb.ToString();
                return;
            }
            if (cppComment is CppCommentText text)
            {
                sb.AppendLine($"/// " + text.Text);
                comment = sb.ToString();
                return;
            }

            if (cppComment == null || cppComment.Kind == CppCommentKind.Null)
            {
                comment = null;
                return;
            }

            throw new NotImplementedException();
        }
    }
}