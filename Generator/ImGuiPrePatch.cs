namespace Generator
{
    using CppAst;
    using HexaGen;
    using HexaGen.Patching;
    using System.Collections.Generic;

    public class ImGuiPrePatch : PrePatch
    {
        public override void Apply(PatchContext context, CsCodeGeneratorSettings settings, List<string> files, CppCompilation compilation)
        {
            if (settings.LibName != "cimgui")
            {
                return;
            }
            settings.DelegateMappings.Add(new("PlatformGetWindowPos", "Vector2*", "Vector2* pos, ImGuiViewport* viewport"));
            settings.DelegateMappings.Add(new("PlatformGetWindowSize", "Vector2*", "Vector2* size, ImGuiViewport* viewport"));
        }
    }
}