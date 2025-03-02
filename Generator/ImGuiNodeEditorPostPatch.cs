namespace Generator
{
    using HexaGen.Metadata;
    using HexaGen.Patching;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public partial class ImGuiNodeEditorPostPatch : PostPatch
    {
        private const string ImGuiNodeEditorOutputPath = "../../../../Hexa.NET.ImGuiNodeEditor/Generated";

        const string pattern = @"ImVector";
        const string replacement = @"ImVector<float>";

        [GeneratedRegex(pattern)]
        private static partial Regex Pattern();

        private readonly Regex Regex = Pattern();

        public override void Apply(PatchContext context, CsCodeGeneratorMetadata metadata, List<string> files)
        {
            if (metadata.Settings.ApiName != "ImGuiNodeEditor")
            {
                return;
            }

            var path = Path.Combine(ImGuiNodeEditorOutputPath, "Structs", "Config.cs");
            var text = File.ReadAllText(path);
            int offset = 0;
            foreach (Match match in Regex.Matches(text))
            {
                text = text.Remove(match.Index + offset, match.Length);
                text = text.Insert(match.Index + offset, replacement);
                offset -= match.Length;
                offset += replacement.Length;
            }
            File.WriteAllText(path, text);
        }
    }
}