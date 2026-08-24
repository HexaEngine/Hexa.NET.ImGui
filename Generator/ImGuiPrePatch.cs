namespace Generator
{
    using HexaGen;
    using HexaGen.Patching;
    using System.Collections.Generic;

    public class ImGuiPrePatch : PrePatch
    {
        public override void Apply(PatchContext context, CsCodeGeneratorConfig config, List<string> files, ParseResult result)
        {
            if (config.LibName != "cimgui")
            {
                return;
            }
            config.DelegateMappings.Add(new("PlatformGetWindowPos", "Vector2*", "Vector2* pos, ImGuiViewport* viewport"));
            config.DelegateMappings.Add(new("PlatformGetWindowSize", "Vector2*", "Vector2* size, ImGuiViewport* viewport"));
        }
    }
}