namespace Generator.Targets
{
    using HexaGen.BuildSystems;
    using System.Collections.Generic;

    public static class ImGuiTargetExtensions
    {
        public static BuildSystemBuilder AddImGui(this BuildSystemBuilder builder)
        {
            builder.AddTarget<ImGuiTarget>();
            return builder;
        }
    }

    public class ImGuiTarget : ImGuiBaseTarget
    {
        private const string CImGuiConfig = "cimgui/generator.json";
        private const string CImGuiHeader = "cimgui/cimgui.h";
        private const string ImGuiOutputPath = "../../../../Hexa.NET.ImGui/Generated";

        public override string Name { get; } = "imgui";

        public override IReadOnlyList<string> Dependencies { get; } = [];

        public override void Execute(BuildContext context)
        {
            // don't worry about "NoInternals" internals will be generated in a substep (post-patch) when generating. see ImGuiPostPatch.cs
            Generate([CImGuiHeader], CImGuiConfig, ImGuiOutputPath, null, out var metadata, InternalsGenerationType.NoInternals);
            context.SetVar(ImGuiMetadataKey, metadata);
        }
    }
}