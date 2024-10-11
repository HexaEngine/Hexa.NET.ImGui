namespace Generator
{
    using HexaGen;
    using HexaGen.Core.Logging;
    using HexaGen.Metadata;
    using HexaGen.Patching;

    internal unsafe class Program
    {
        private const string CImGuiConfig = "cimgui/generator.json";

        private const string CImGuizmoConfig = "cimguizmo/generator.json";
        private const string CImNodesConfig = "cimnodes/generator.json";
        private const string CImPlotConfig = "cimplot/generator.json";

        private const string CImGuiHeader = "cimgui/cimgui.h";
        private const string CImGuizmoHeader = "cimguizmo/cimguizmo.h";
        private const string CImNodesHeader = "cimnodes/cimnodes.h";
        private const string CImPlotHeader = "cimplot/cimplot.h";

        private const string ImGuiOutputPath = "../../../../Hexa.NET.ImGui/Generated";

        private const string ImGuizmoOutputPath = "../../../../Hexa.NET.ImGuizmo/Generated";
        private const string ImNodesOutputPath = "../../../../Hexa.NET.ImNodes/Generated";
        private const string ImPlotOutputPath = "../../../../Hexa.NET.ImPlot/Generated";

        private static void Main(string[] args)
        {
            if (Directory.Exists("patches"))
            {
                Directory.Delete("patches", true);
            }

            Directory.CreateDirectory("./patches");
            // don't worry about "NoInternals" internals will be generated in a substep (post-patch) when generating. see ImGuiPostPatch.cs
            Generate(CImGuiHeader, CImGuiConfig, ImGuiOutputPath, null, out var metadata, InternalsGenerationType.NoInternals);

            Generate(CImGuizmoHeader, CImGuizmoConfig, ImGuizmoOutputPath, metadata, out _, InternalsGenerationType.BothOrDontCare);
            Generate(CImPlotHeader, CImPlotConfig, ImPlotOutputPath, metadata, out var imPlotMetadata, InternalsGenerationType.BothOrDontCare);
            Generate(CImNodesHeader, CImNodesConfig, ImNodesOutputPath, metadata, out _, InternalsGenerationType.BothOrDontCare);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("All Done!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static bool Generate(string header, string settingsPath, string output, CsCodeGeneratorMetadata? lib, out CsCodeGeneratorMetadata metadata, InternalsGenerationType type)
        {
            CsCodeGeneratorConfig settings = CsCodeGeneratorConfig.Load(settingsPath);
            settings.WrapPointersAsHandle = true;
            ImGuiCodeGenerator generator = new(settings);
            generator.PatchEngine.RegisterPrePatch(new ImVectorPatch());
            generator.PatchEngine.RegisterPrePatch(new ImGuiDefinitionsPatch(type));
            generator.PatchEngine.RegisterPrePatch(new ImGuizmoPrePatch());
            generator.PatchEngine.RegisterPrePatch(new ImGuiPrePatch());
            generator.PatchEngine.RegisterPrePatch(new NamingPatch(["ImGui", "ImGuizmo", "ImNodes", "ImPlot"], NamingPatchOptions.None));
            generator.PatchEngine.RegisterPostPatch(new ImGuiPostPatch());

            generator.LogEvent += GeneratorLogEvent;

            if (lib != null)
            {
                generator.CopyFrom(lib);
            }

            bool result = generator.Generate(header, output);
            metadata = generator.GetMetadata();

            generator.LogEvent -= GeneratorLogEvent;

            File.WriteAllText(settings.LibName + ".log", string.Join(Environment.NewLine, generator.Messages.Select(x => x.ToString())));

            return result;
        }

        private static void GeneratorLogEvent(LogSeverity severity, string message)
        {
            switch (severity)
            {
                case LogSeverity.Trace:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case LogSeverity.Information:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}