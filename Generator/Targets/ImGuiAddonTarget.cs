﻿namespace Generator.Targets
{
    using HexaGen.BuildSystems;
    using Microsoft.Extensions.Options;
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
            builder.AddTarget<ImGuizmoTarget, ImGuizmoTargetOptions>(options =>
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
            builder.AddTarget<ImNodesTarget, ImNodesTargetOptions>(options =>
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
            builder.AddTarget<ImPlotTarget, ImPlotTargetOptions>(options =>
            {
                options.Name = "implot";
                options.Headers = [CImPlotHeader];
                options.ConfigPath = configPath;
                options.OutputPath = outputPath;
            });
            return builder;
        }
    }

    public class ImPlotTarget(IOptions<ImPlotTargetOptions> options) : ImGuiAddonTarget(options)
    { }

    public class ImNodesTarget(IOptions<ImNodesTargetOptions> options) : ImGuiAddonTarget(options)
    { }

    public class ImGuizmoTarget(IOptions<ImGuizmoTargetOptions> options) : ImGuiAddonTarget(options)
    { }

    public class ImPlotTargetOptions() : ImGuiAddonTargetOptions()
    { }

    public class ImNodesTargetOptions() : ImGuiAddonTargetOptions()
    { }

    public class ImGuizmoTargetOptions() : ImGuiAddonTargetOptions()

    { }

    public class ImGuiAddonTarget : ImGuiBaseTarget
    {
        private readonly List<string> headers;
        private readonly string configPath;
        private readonly string outputPath;

        protected ImGuiAddonTarget(IOptions<ImGuiAddonTargetOptions> opt)
        {
            var options = opt.Value;
            Name = options.Name;
            headers = options.Headers;
            configPath = options.ConfigPath;
            outputPath = options.OutputPath;
        }

        protected ImGuiAddonTarget(string name, IEnumerable<string> headers, string configPath, string outputPath)
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