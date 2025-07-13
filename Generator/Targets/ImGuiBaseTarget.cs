namespace Generator.Targets
{
    using HexaGen;
    using HexaGen.BuildSystems;
    using HexaGen.Metadata;
    using HexaGen.Patching;
    using Microsoft.CodeAnalysis;

    public abstract class ImGuiBaseTarget : BuildTarget
    {
        public static readonly string ImGuiMetadataKey = "ImGuiMetadata";

        protected static CsCodeGeneratorMetadata GetImGuiMetadata(BuildContext ctx) => ctx.GetVar<CsCodeGeneratorMetadata>(ImGuiMetadataKey);

        protected static bool Generate(IEnumerable<string> headers, string settingsPath, string output, CsCodeGeneratorMetadata? lib, out CsCodeGeneratorMetadata metadata, InternalsGenerationType type)
        {
            CsCodeGeneratorConfig settings = CsCodeGeneratorConfig.Load(settingsPath);
            settings.Defines.Add("IMGUI_USE_WCHAR32");
            settings.Defines.Add("IMGUI_ENABLE_FREETYPE");
            settings.WrapPointersAsHandle = true;

            ImGuiCodeGenerator generator = new(settings);
            generator.PatchEngine.RegisterPrePatch(new ImVectorPatch());
            generator.PatchEngine.RegisterPrePatch(new ImGuiDefinitionsPatch(type));
            generator.PatchEngine.RegisterPrePatch(new ImGuizmoPrePatch());
            generator.PatchEngine.RegisterPrePatch(new ImGuiPrePatch());
            generator.PatchEngine.RegisterPrePatch(new NamingPatch(["CImGui", "ImGui", "ImGuizmo", "ImNodes", "ImPlot", "ImplSDL2", "ImplSDL3", "ImplGlfw", "Impl"], NamingPatchOptions.MultiplePrefixes));
            generator.PatchEngine.RegisterPostPatch(new ImGuiPostPatch());
            generator.PatchEngine.RegisterPostPatch(new ImGuiBackendsPostPatch());
            generator.PatchEngine.RegisterPostPatch(new ImGuiImGuiDataTypePrivatePatch());

            generator.LogToConsole();

            if (lib != null)
            {
                generator.CopyFrom(lib);
            }

            bool result = generator.Generate([.. headers], output);
            metadata = generator.GetMetadata();

            File.WriteAllText(settings.LibName + ".log", string.Join(Environment.NewLine, generator.Messages.Select(x => x.ToString())));

            return result;
        }
    }
}