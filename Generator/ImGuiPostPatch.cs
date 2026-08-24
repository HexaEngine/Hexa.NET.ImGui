namespace Generator
{
    using HexaGen;
    using HexaGen.FunctionGeneration;
    using HexaGen.Metadata;
    using HexaGen.Patching;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ImGuiPostPatch : PostPatch
    {
        private const string CImGuiHeader = "cimgui/cimgui.h";
        private const string CImGuiManualConfig = "cimgui/generator.manual.json";

        private const string CImGuiInternalsConfig = "cimgui/generator.internals.json";
        private const string ImGuiInternalsOutputPath = "../../../../Hexa.NET.ImGui/Internals";

        private const string ImGuiManualOutputPath = "../../../../Hexa.NET.ImGui/Manual/";
        private const string ImGuiOutputPath = "../../../../Hexa.NET.ImGui/Generated/";

        public override void Apply(PatchContext context, CsCodeGeneratorMetadata metadata, List<string> files)
        {
            if (metadata.Settings.LibName != "cimgui")
            {
                return;
            }

            Generate(metadata, CImGuiHeader, CImGuiInternalsConfig, ImGuiInternalsOutputPath, InternalsGenerationType.OnlyInternals, out var internalsMetadata);
            metadata.Merge(internalsMetadata, new HexaGen.Metadata.MergeOptions() { MergeFunctionTable = true });
            File.Delete(Path.Combine(ImGuiOutputPath, "FunctionTable.cs")); // Delete base.
            File.Delete(Path.Combine(ImGuiInternalsOutputPath, "FunctionTable.cs")); // Delete intermediate.

            Generate(metadata, CImGuiHeader, CImGuiManualConfig, ImGuiManualOutputPath, InternalsGenerationType.BothOrDontCare, out _);
            File.Move(Path.Combine(ImGuiManualOutputPath, "FunctionTable.cs"), Path.Combine(ImGuiOutputPath, "FunctionTable.cs")); // Move latest to base.

            // Patch Functions
            {
                Regex regex = new("\\b(.*) = Utils.GetByteCountUTF8\\b\\(buf\\);");
                const string manualFunctions = ImGuiManualOutputPath + "Functions/";

                foreach (var file in Directory.EnumerateFiles(manualFunctions, "*.cs"))
                {
                    int indexOffset = 0;
                    var content = File.ReadAllText(file);
                    var matches = regex.Matches(content);
                    if (matches.Count > 0)
                    {
                        var builder = new StringBuilder(content);
                        foreach (Match match in matches)
                        {
                            var name = match.Groups[1].Value.Trim();
                            var replacement = $"{name} = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);";
                            var delta = replacement.Length - match.Value.Length;

                            builder.Replace(match.Value, replacement, match.Index + indexOffset, match.Length);
                            indexOffset += delta;
                        }
                        File.WriteAllText(file, builder.ToString());
                        Console.WriteLine($"Patched file: {file}");
                    }
                }
            }
        }

        private static void Generate(CsCodeGeneratorMetadata metadata, string header, string configPath, string output, InternalsGenerationType generationType, out CsCodeGeneratorMetadata meta)
        {
            GeneratorBuilder.Create<ImGuiCodeGenerator>(configPath)
                .WithPrePatch<ImVectorPatch>()
                .WithPrePatch<ImGuiDefinitionsPatch>(() => new(generationType))
                .WithPrePatch<ImGuiPrePatch>()
                .WithPrePatch<NamingPatch>(() => new(["ImGui", "ImGuizmo", "ImNodes", "ImPlot"], NamingPatchOptions.None))
                .WithFunctionTableEntires(metadata)
                .OnConditionalPostConfigure(configPath.Contains("manual"), (gen, _) =>
                {
                    gen.FunctionGenerator.OverwriteRule<FunctionGenRuleSpan>(new ManualFunctionGenRuleSpan());
                })
                .CopyFromMetadata(metadata)
                .Generate(header, output)
                .GetMetadata(out meta);
        }
    }

    public static class GeneratorBuilderExtensions
    {
        public delegate TOut FactoryCallback<TOut>();

        public delegate TOut FactoryCallback<TOut, TParam>(TParam param);

        public static GeneratorBuilder WithPrePatch<T>(this GeneratorBuilder builder) where T : IPrePatch, new()
        {
            return builder.WithPrePatch(new T());
        }

        public static GeneratorBuilder WithPrePatch<T>(this GeneratorBuilder builder, T patch) where T : IPrePatch
        {
            return builder.WithPrePatch(patch);
        }

        public static GeneratorBuilder WithPrePatch<T>(this GeneratorBuilder builder, FactoryCallback<T> factory) where T : IPrePatch
        {
            return builder.WithPrePatch(factory());
        }

        public static GeneratorBuilder WithPrePatch<T, TParam>(this GeneratorBuilder builder, TParam param, FactoryCallback<T, TParam> factory) where T : IPrePatch
        {
            return builder.WithPrePatch(factory(param));
        }

        public static GeneratorBuilder WithPostPatch<T>(this GeneratorBuilder builder) where T : IPostPatch, new()
        {
            return builder.WithPostPatch(new T());
        }

        public static GeneratorBuilder WithPostPatch<T>(this GeneratorBuilder builder, T patch) where T : IPostPatch
        {
            return builder.WithPostPatch(patch);
        }

        public static GeneratorBuilder WithPostPatch<T>(this GeneratorBuilder builder, FactoryCallback<T> factory) where T : IPostPatch
        {
            return builder.WithPostPatch(factory());
        }

        public static GeneratorBuilder WithPostPatch<T, TParam>(this GeneratorBuilder builder, TParam param, FactoryCallback<T, TParam> factory) where T : IPostPatch
        {
            return builder.WithPostPatch(factory(param));
        }

        public static GeneratorBuilder OnConditionalPostConfigure(this GeneratorBuilder builder, bool condition, GenEventHandler<CsCodeGenerator, CsCodeGeneratorConfig> callback)
        {
            if (condition)
            {
                builder.OnPostConfigure(callback);
            }
            return builder;
        }
    }
}