namespace Generator
{
    using CppAst;
    using System.Reflection.Metadata;

    public class ArrayMapping
    {
        private CppPrimitiveKind primitive;
        private int size;
        private string name;

        public ArrayMapping(CppPrimitiveKind primitive, int size, string name)
        {
            this.primitive = primitive;
            this.size = size;
            this.name = name;
        }

        public CppPrimitiveKind Primitive { get => primitive; set => primitive = value; }

        public int Size { get => size; set => size = value; }

        public string Name { get => name; set => name = value; }
    }

    public class FunctionMapping
    {
        public FunctionMapping(string exportedName, string friendlyName, Dictionary<string, string> defaults)
        {
            ExportedName = exportedName;
            FriendlyName = friendlyName;
            Defaults = defaults;
        }

        public string ExportedName { get; set; }

        public string FriendlyName { get; set; }

        public Dictionary<string, string> Defaults { get; set; }
    }

    internal unsafe class Program
    {
        private static void Main(string[] args)
        {
            GenerateImGui();
            GenerateImGuizmo();
            GenerateImNodes();
            GenerateImPlot();
        }

        private static int GenerateImPlot()
        {
            CsCodeGeneratorSettings.Load("cimplot/generator.json");
            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom("cimgui");

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    CsCodeGeneratorSettings.Default.IgnoredFunctions.Add(functionDefinition.Overloads[j].ExportedName);
                }
            }
            for (int i = 0; i < imguiDefinitions.Enums.Length; i++)
            {
                var enumDefinition = imguiDefinitions.Enums[i];
                CsCodeGeneratorSettings.Default.IgnoredEnums.Add(enumDefinition.Names[0]);
            }
            for (int i = 0; i < imguiDefinitions.Types.Length; i++)
            {
                var typeDefinition = imguiDefinitions.Types[i];
                CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typeDefinition.Name);
            }
            for (int i = 0; i < imguiDefinitions.Typedefs.Length; i++)
            {
                var typedefDefinition = imguiDefinitions.Typedefs[i];
                CsCodeGeneratorSettings.Default.IgnoredTypedefs.Add(typedefDefinition.Name);
                if (typedefDefinition.IsStruct)
                {
                    CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typedefDefinition.Name);
                }
            }

            ImguiDefinitions implotDefinitions = new();
            implotDefinitions.LoadFrom("cimplot");

            for (int i = 0; i < implotDefinitions.Functions.Length; i++)
            {
                var functionDefinition = implotDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    var overload = functionDefinition.Overloads[j];
                    CsCodeGeneratorSettings.Default.FunctionMappings.Add(new(overload.ExportedName, overload.FriendlyName, overload.DefaultValues));

                    if (overload.IsMemberFunction && !overload.IsConstructor)
                    {
                        if (CsCodeGeneratorSettings.Default.KnownMemberFunctions.TryGetValue(overload.StructName, out var knownFunctions))
                        {
                            if (!knownFunctions.Contains(overload.ExportedName))
                                knownFunctions.Add(overload.ExportedName);
                        }
                        else
                        {
                            CsCodeGeneratorSettings.Default.KnownMemberFunctions.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                }
            }

            string headerFile = "cimplot/cimplot.h";

            var options = new CppParserOptions
            {
                ParseMacros = true,
            };

            var compilation = CppParser.ParseFile(headerFile, options);

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

                return 0;
            }

            CsCodeGenerator.Generate(compilation, "../../../../HexaEngine.ImPlot/Generated");

            return 0;
        }

        private static int GenerateImNodes()
        {
            CsCodeGeneratorSettings.Load("cimnodes/generator.json");
            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom("cimgui");

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    CsCodeGeneratorSettings.Default.IgnoredFunctions.Add(functionDefinition.Overloads[j].ExportedName);
                }
            }
            for (int i = 0; i < imguiDefinitions.Enums.Length; i++)
            {
                var enumDefinition = imguiDefinitions.Enums[i];
                CsCodeGeneratorSettings.Default.IgnoredEnums.Add(enumDefinition.Names[0]);
            }
            for (int i = 0; i < imguiDefinitions.Types.Length; i++)
            {
                var typeDefinition = imguiDefinitions.Types[i];
                CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typeDefinition.Name);
            }
            for (int i = 0; i < imguiDefinitions.Typedefs.Length; i++)
            {
                var typedefDefinition = imguiDefinitions.Typedefs[i];
                CsCodeGeneratorSettings.Default.IgnoredTypedefs.Add(typedefDefinition.Name);
                if (typedefDefinition.IsStruct)
                {
                    CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typedefDefinition.Name);
                }
            }

            ImguiDefinitions imnodesDefinitions = new();
            imnodesDefinitions.LoadFrom("cimnodes");

            for (int i = 0; i < imnodesDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imnodesDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    var overload = functionDefinition.Overloads[j];
                    CsCodeGeneratorSettings.Default.FunctionMappings.Add(new(overload.ExportedName, overload.FriendlyName, overload.DefaultValues));

                    if (overload.IsMemberFunction && !overload.IsConstructor)
                    {
                        if (CsCodeGeneratorSettings.Default.KnownMemberFunctions.TryGetValue(overload.StructName, out var knownFunctions))
                        {
                            if (!knownFunctions.Contains(overload.ExportedName))
                                knownFunctions.Add(overload.ExportedName);
                        }
                        else
                        {
                            CsCodeGeneratorSettings.Default.KnownMemberFunctions.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                }
            }

            string headerFile = "cimnodes/cimnodes.h";

            var options = new CppParserOptions
            {
                ParseMacros = true,
            };

            var compilation = CppParser.ParseFile(headerFile, options);

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

                return 0;
            }

            CsCodeGenerator.Generate(compilation, "../../../../HexaEngine.ImNodes/Generated");

            return 0;
        }

        private static int GenerateImGuizmo()
        {
            CsCodeGeneratorSettings.Load("cimguizmo/generator.json");
            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom("cimgui");

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    CsCodeGeneratorSettings.Default.IgnoredFunctions.Add(functionDefinition.Overloads[j].ExportedName);
                }
            }
            for (int i = 0; i < imguiDefinitions.Enums.Length; i++)
            {
                var enumDefinition = imguiDefinitions.Enums[i];
                CsCodeGeneratorSettings.Default.IgnoredEnums.Add(enumDefinition.Names[0]);
            }
            for (int i = 0; i < imguiDefinitions.Types.Length; i++)
            {
                var typeDefinition = imguiDefinitions.Types[i];
                CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typeDefinition.Name);
            }
            for (int i = 0; i < imguiDefinitions.Typedefs.Length; i++)
            {
                var typedefDefinition = imguiDefinitions.Typedefs[i];
                CsCodeGeneratorSettings.Default.IgnoredTypedefs.Add(typedefDefinition.Name);
                if (typedefDefinition.IsStruct)
                {
                    CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typedefDefinition.Name);
                }
            }

            ImguiDefinitions imguizmoDefinitions = new();
            imguizmoDefinitions.LoadFrom("cimguizmo");

            for (int i = 0; i < imguizmoDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguizmoDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    var overload = functionDefinition.Overloads[j];
                    CsCodeGeneratorSettings.Default.FunctionMappings.Add(new(overload.ExportedName, overload.FriendlyName, overload.DefaultValues));

                    if (overload.IsMemberFunction && !overload.IsConstructor)
                    {
                        if (CsCodeGeneratorSettings.Default.KnownMemberFunctions.TryGetValue(overload.StructName, out var knownFunctions))
                        {
                            if (!knownFunctions.Contains(overload.ExportedName))
                                knownFunctions.Add(overload.ExportedName);
                        }
                        else
                        {
                            CsCodeGeneratorSettings.Default.KnownMemberFunctions.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                }
            }

            string headerFile = "cimguizmo/cimguizmo.h";

            var options = new CppParserOptions
            {
                ParseMacros = true,
            };

            var compilation = CppParser.ParseFile(headerFile, options);

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

                return 0;
            }

            CsCodeGenerator.Generate(compilation, "../../../../HexaEngine.ImGuizmo/Generated");

            return 0;
        }

        private static int GenerateImGui()
        {
            CsCodeGeneratorSettings.Load("cimgui/generator.json");
            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom("cimgui");

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    var overload = functionDefinition.Overloads[j];
                    CsCodeGeneratorSettings.Default.FunctionMappings.Add(new(overload.ExportedName, overload.FriendlyName, overload.DefaultValues));

                    if (overload.IsMemberFunction && !overload.IsConstructor)
                    {
                        if (CsCodeGeneratorSettings.Default.KnownMemberFunctions.TryGetValue(overload.StructName, out var knownFunctions))
                        {
                            if (!knownFunctions.Contains(overload.ExportedName))
                                knownFunctions.Add(overload.ExportedName);
                        }
                        else
                        {
                            CsCodeGeneratorSettings.Default.KnownMemberFunctions.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                }
            }

            string headerFile = "cimgui/cimgui.h";

            var options = new CppParserOptions
            {
                ParseMacros = true,
            };

            var compilation = CppParser.ParseFile(headerFile, options);

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

                return 0;
            }

            CsCodeGenerator.Generate(compilation, "../../../../HexaEngine.ImGui/Generated");

            return 0;
        }
    }
}