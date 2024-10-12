namespace Generator
{
    using HexaGen.Metadata;
    using HexaGen.Patching;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public partial class ImGuiBackendsPostPatch : PostPatch
    {
        private const string ImGuiBackendsOutputPath = "../../../../Hexa.NET.ImGui.Backends/Generated";

        const string pattern = @"VkAllocationCallbacksPtr";
        const string replacement = @"VkAllocationCallbacks";

        Regex Regex = Pattern();

        public override void Apply(PatchContext context, CsCodeGeneratorMetadata metadata, List<string> files)
        {
            if (metadata.Settings.LibName != "cimgui_impl")
            {
                return;
            }

            var path = Path.Combine(ImGuiBackendsOutputPath, "Structs", "ImGuiImplVulkanInitInfo.cs");
            var text = File.ReadAllText(path);
            var matches = Regex.Matches(text);
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

        [GeneratedRegex(pattern)]
        private static partial Regex Pattern();
    }
}