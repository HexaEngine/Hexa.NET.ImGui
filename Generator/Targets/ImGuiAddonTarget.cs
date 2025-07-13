namespace Generator.Targets
{
    using HexaGen.BuildSystems;
    using System.Collections.Generic;

    public class ImGuiAddonTargetOptions
    {
        public string Name { get; set; } = null!;

        public List<string> Headers { get; set; } = [];

        public string ConfigPath { get; set; } = null!;

        public string OutputPath { get; set; } = null!;
    }

    public static class ImGuiAddonTargetExtensions
    {
        private const string CImGuizmoConfig = "cimguizmo/generator.json";
        private const string CImNodesConfig = "cimnodes/generator.json";
        private const string CImPlotConfig = "cimplot/generator.json";

        private const string CImGuizmoHeader = "cimguizmo/cimguizmo.h";
        private const string CImNodesHeader = "cimnodes/cimnodes.h";
        private const string CImPlotHeader = "cimplot/cimplot.h";

        private const string ImGuizmoOutputPath = "../../../../Hexa.NET.ImGuizmo/Generated";
        private const string ImNodesOutputPath = "../../../../Hexa.NET.ImNodes/Generated";
        private const string ImPlotOutputPath = "../../../../Hexa.NET.ImPlot/Generated";

        public static BuildSystemBuilder AddImGuizmo(this BuildSystemBuilder builder, string configPath = CImGuizmoConfig, string outputPath = ImGuizmoOutputPath)
        {
            builder.AddTarget<ImGuiAddonTarget, ImGuiAddonTargetOptions>("imguizmo", options =>
            {
                options.Name = "imguizmo";
                options.Headers = [CImGuizmoHeader];
                options.ConfigPath = configPath;
                options.OutputPath = outputPath;
            });
            return builder;
        }

        public static BuildSystemBuilder AddImNodes(this BuildSystemBuilder builder, string configPath = CImNodesConfig, string outputPath = ImNodesOutputPath)
        {
            builder.AddTarget<ImGuiAddonTarget, ImGuiAddonTargetOptions>("imnodes", options =>
            {
                options.Name = "imnodes";
                options.Headers = [CImNodesHeader];
                options.ConfigPath = configPath;
                options.OutputPath = outputPath;
            });
            return builder;
        }

        public static BuildSystemBuilder AddImPlot(this BuildSystemBuilder builder, string configPath = CImPlotConfig, string outputPath = ImPlotOutputPath)
        {
            builder.AddTarget<ImGuiAddonTarget, ImGuiAddonTargetOptions>("implot", options =>
            {
                options.Name = "implot";
                options.Headers = [CImPlotHeader];
                options.ConfigPath = configPath;
                options.OutputPath = outputPath;
            });
            return builder;
        }
    }

    public class ImGuiAddonTarget : ImGuiBaseTarget
    {
        private readonly List<string> headers;
        private readonly string configPath;
        private readonly string outputPath;

        public ImGuiAddonTarget(ImGuiAddonTargetOptions options)
        {
            Name = options.Name;
            headers = options.Headers;
            configPath = options.ConfigPath;
            outputPath = options.OutputPath;
        }

        public ImGuiAddonTarget(string name, IEnumerable<string> headers, string configPath, string outputPath)
        {
            Name = name;
            this.headers = [.. headers];
            this.configPath = configPath;
            this.outputPath = outputPath;
        }

        public override string Name { get; }

        public override IReadOnlyList<string> Dependencies { get; } = ["imgui"];

        public override void Execute(BuildContext context)
        {
            var metadata = GetImGuiMetadata(context);
            Generate(headers, configPath, outputPath, metadata, out _, InternalsGenerationType.BothOrDontCare);
        }
    }
}