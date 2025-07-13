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

        protected static bool GenerateNodeEditor(string header, string settingsPath, string output, CsCodeGeneratorMetadata? lib, out CsCodeGeneratorMetadata metadata)
        {
            CsCodeGeneratorConfig settings = CsCodeGeneratorConfig.Load(settingsPath);
            settings.SystemIncludeFolders.Add("C:/dev/imgui");
            settings.WrapPointersAsHandle = true;
            ImGuiCodeGenerator generator = new(settings);
            generator.PatchEngine.RegisterPrePatch(new ImVectorPatch());
            generator.PatchEngine.RegisterPostPatch(new ImGuiNodeEditorPostPatch());

            generator.LogToConsole();

            if (lib != null)
            {
                generator.CopyFrom(lib);
            }

            bool result = generator.Generate(header, output);
            metadata = generator.GetMetadata();

            return result;
        }
    }
}