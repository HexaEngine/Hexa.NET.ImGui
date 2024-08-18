namespace Generator
{
    using CppAst;
    using HexaGen;
    using HexaGen.Patching;
    using System.Collections.Generic;

    public class ImGuiPrePatch : PrePatch
    {
        public override void Apply(PatchContext context, CsCodeGeneratorConfig config, List<string> files, CppCompilation compilation)
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