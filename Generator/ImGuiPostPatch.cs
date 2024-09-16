namespace Generator
{
    using HexaGen;
    using HexaGen.Metadata;
    using HexaGen.Patching;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ImGuiPostPatch : PostPatch
    {
        private const string CImGuiHeader = "cimgui/cimgui.h";
        private const string CImGuiManualConfig = "cimgui/generator.manual.json";

        private const string manual = "../../../../Hexa.NET.ImGui/Manual/";

        private CsCodeGeneratorMetadata patchMetadata;

        public override void Apply(PatchContext context, CsCodeGeneratorMetadata metadata, List<string> files)
        {
            if (metadata.Settings.LibName != "cimgui")
            {
                return;
            }
            var settingsManual = CsCodeGeneratorConfig.Load(CImGuiManualConfig);

            settingsManual.VTableStart = metadata.VTableLength;

            ImGuiCodeGenerator generator = new(settingsManual);
            generator.PatchEngine.RegisterPrePatch(new ImVectorPatch());
            generator.PatchEngine.RegisterPrePatch(new ImGuiDefinitionsPatch());
            generator.PatchEngine.RegisterPrePatch(new ImGuiPrePatch());
            generator.CopyFrom(metadata);
            generator.Generate(CImGuiHeader, manual);
            patchMetadata = generator.GetMetadata();

            const string generated = "../../../../Hexa.NET.ImGui/Generated/";

            // Patch VTable
            {
                const string vtTargetFile = generated + "Functions.VT.cs";
                const string vtPatchFile = manual + "Functions.VT.cs";

                var vtContent = File.ReadAllText(vtTargetFile);
                var rootMatch = Regex.Match(vtContent, "vt = new VTable\\(LibraryLoader\\.LoadLibrary\\(\\), (.*)\\);");

                Regex loadPattern = new("vt.Load\\((.*), \"(.*)\"\\);", RegexOptions.Singleline);
                var matches = loadPattern.Matches(vtContent);
                var lastMatch = matches[^1];
                int start = lastMatch.Index + lastMatch.Length;

                var content = File.ReadAllText(vtPatchFile);
                File.Delete(vtPatchFile);

                StringBuilder builder = new(vtContent[..(start + 1)]);

                foreach (Match match in loadPattern.Matches(content))
                {
                    builder.AppendLine($"\t\t\t" + match.Value);
                }

                builder.Remove(rootMatch.Index, rootMatch.Length);
                builder.Insert(rootMatch.Index, $"vt = new VTable(LibraryLoader.LoadLibrary(), {patchMetadata.VTableLength});");

                builder.Append(vtContent.AsSpan(start));

                var newContent = builder.ToString();

                File.WriteAllText(vtTargetFile, newContent);
            }

            // Patch Functions
            {
                Regex regex = new("\\b(.*) = Utils.GetByteCountUTF8\\b\\(buf\\);");
                const string manualFunctions = manual + "Functions/";

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
    }
}