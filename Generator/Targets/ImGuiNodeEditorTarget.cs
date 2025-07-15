namespace Generator.Targets
{
    using HexaGen;
    using HexaGen.BuildSystems;
    using HexaGen.Metadata;
    using System.Collections.Generic;

    public static class ImGuiNodeEditorTargetExtensions
    {
        public static BuildSystemBuilder AddImGuiNodeEditor(this BuildSystemBuilder builder, bool enabled)
        {
            if (enabled)
            {
                builder.AddTarget<ImGuiNodeEditorTarget>();
            }

            return builder;
        }
    }

    public class ImGuiNodeEditorTarget : ImGuiBaseTarget
    {
        private const string ImGuiNodeEditorConfig = "imgui-node-editor/generator.json";
        private const string ImGuiNodeEditorHeader = "imgui-node-editor/imgui_node_editor.h";
        private const string ImGuiNodeEditorOutputPath = "../../../../Hexa.NET.ImGuiNodeEditor/Generated";

        public override string Name { get; } = "imgui-node-editor";

        public override IReadOnlyList<string> Dependencies { get; } = ["imgui"];

        public override void Execute(BuildContext context)
        {
            GenerateNodeEditor(ImGuiNodeEditorHeader, ImGuiNodeEditorConfig, ImGuiNodeEditorOutputPath, null, out _);
        }

        protected static void GenerateNodeEditor(string header, string configPath, string output, CsCodeGeneratorMetadata? lib, out CsCodeGeneratorMetadata metadata)
        {
            GeneratorBuilder.Create<ImGuiCodeGenerator>(configPath)
                .WithPrePatch<ImVectorPatch>()
                .WithPostPatch<ImGuiNodeEditorPostPatch>()
                .Generate(header, output)
                .GetMetadata(out metadata);
        }
    }
}