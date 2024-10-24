namespace Generator
{
    using HexaGen;
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

        private CsCodeGeneratorMetadata patchMetadata;

        public override void Apply(PatchContext context, CsCodeGeneratorMetadata metadata, List<string> files)
        {
            if (metadata.Settings.LibName != "cimgui")
            {
                return;
            }

            Generate(metadata, CImGuiHeader, CImGuiInternalsConfig, ImGuiInternalsOutputPath, InternalsGenerationType.OnlyInternals, out var internalsMetadata);
            metadata.Merge(internalsMetadata, true);
            File.Delete(Path.Combine(ImGuiOutputPath, "FunctionTable.cs")); // Delete base.
            File.Delete(Path.Combine(ImGuiInternalsOutputPath, "FunctionTable.cs")); // Delete intermediate.

            Generate(metadata, CImGuiHeader, CImGuiManualConfig, ImGuiManualOutputPath, InternalsGenerationType.BothOrDontCare, out patchMetadata);
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

        private static void Generate(CsCodeGeneratorMetadata metadata, string header, string config, string output, InternalsGenerationType generationType, out CsCodeGeneratorMetadata meta)
        {
            var settingsManual = CsCodeGeneratorConfig.Load(config);

            settingsManual.FunctionTableEntries = metadata.FunctionTable.Entries;

            ImGuiCodeGenerator generator = new(settingsManual);
            generator.PatchEngine.RegisterPrePatch(new ImVectorPatch());
            generator.PatchEngine.RegisterPrePatch(new ImGuiDefinitionsPatch(generationType));
            generator.PatchEngine.RegisterPrePatch(new ImGuiPrePatch());
            generator.PatchEngine.RegisterPrePatch(new NamingPatch(["ImGui", "ImGuizmo", "ImNodes", "ImPlot"], NamingPatchOptions.None));
            generator.LogToConsole();
            generator.CopyFrom(metadata);
            generator.Generate(header, output);
            meta = generator.GetMetadata();
        }
    }
}