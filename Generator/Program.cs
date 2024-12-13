#define BackendsOnly

namespace Generator
{
    using HexaGen;
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

        private const string ImGuiBackendsOutputPath = "../../../../Hexa.NET.ImGui.Backends/Generated";
        private const string ImGuiBackendsSDL2OutputPath = "../../../../Hexa.NET.ImGui.Backends.SDL2/Generated";
        private const string ImGuiBackendsGLFWOutputPath = "../../../../Hexa.NET.ImGui.Backends.GLFW/Generated";
        private const string CImGuiBackendsHeader = "backends/cimgui_impl.h";
        private const string CImGuiBackendsConfig = "backends/generator.json";
        private const string CImGuiBackendsSDL2Config = "backends/generator.sdl2.json";
        private const string CImGuiBackendsGLFWConfig = "backends/generator.glfw.json";

        private static void Main(string[] args)
        {
            if (Directory.Exists("patches"))
            {
                Directory.Delete("patches", true);
            }

            Directory.CreateDirectory("./patches");

#if !BackendsOnly
            // don't worry about "NoInternals" internals will be generated in a substep (post-patch) when generating. see ImGuiPostPatch.cs
            Generate([CImGuiHeader], CImGuiConfig, ImGuiOutputPath, null, out var metadata, InternalsGenerationType.NoInternals);

            Generate([CImGuizmoHeader], CImGuizmoConfig, ImGuizmoOutputPath, metadata, out _, InternalsGenerationType.BothOrDontCare);
            Generate([CImPlotHeader], CImPlotConfig, ImPlotOutputPath, metadata, out var imPlotMetadata, InternalsGenerationType.BothOrDontCare);
            Generate([CImNodesHeader], CImNodesConfig, ImNodesOutputPath, metadata, out _, InternalsGenerationType.BothOrDontCare);
#else
            Generate([CImGuiHeader], CImGuiConfig, ImGuiOutputPath, null, out var metadata, InternalsGenerationType.NoInternals);
#endif

            string[] backends = ["OpenGL3", "OpenGL2", "D3D9", "D3D10", "D3D11", "D3D12", "Vulkan", "Win32", "OSX", "Metal", "Android"];

            metadata.CppDefinedFunctions.Clear();

            CsCodeGeneratorMetadata? metadataBackend = new()
            {
                FunctionTable = new() { Entries = [new(0, "igSetCurrentContext"), new(1, "igGetCurrentContext")] },
                Settings = new(),
                WrappedPointers = metadata.WrappedPointers.ToDictionary()
            };

            foreach (string lib in backends)
            {
                GenerateBackend(["backends/cimgui.h", CImGuiBackendsHeader], CImGuiBackendsConfig, ImGuiBackendsOutputPath, metadataBackend, out var libMetadata, lib);
                metadataBackend ??= libMetadata;
            }

            Generate(["backends/cimgui.h", CImGuiBackendsHeader], CImGuiBackendsSDL2Config, ImGuiBackendsSDL2OutputPath, metadata, out _, InternalsGenerationType.BothOrDontCare);
            Generate(["backends/cimgui.h", CImGuiBackendsHeader], CImGuiBackendsGLFWConfig, ImGuiBackendsGLFWOutputPath, metadata, out _, InternalsGenerationType.BothOrDontCare);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("All Done!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static bool Generate(string[] headers, string settingsPath, string output, CsCodeGeneratorMetadata? lib, out CsCodeGeneratorMetadata metadata, InternalsGenerationType type)
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
            generator.PatchEngine.RegisterPrePatch(new NamingPatch(["CImGui", "ImGui", "ImGuizmo", "ImNodes", "ImPlot", "ImplSDL2", "ImplGlfw", "Impl"], NamingPatchOptions.MultiplePrefixes));
            generator.PatchEngine.RegisterPostPatch(new ImGuiPostPatch());
            generator.PatchEngine.RegisterPostPatch(new ImGuiBackendsPostPatch());

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

        private static bool GenerateBackend(string[] headers, string settingsPath, string output, CsCodeGeneratorMetadata? lib, out CsCodeGeneratorMetadata metadata, string name)
        {
            CsCodeGeneratorConfig settings = CsCodeGeneratorConfig.Load(settingsPath);
            settings.Defines.Add("IMGUI_USE_WCHAR32");
            settings.Defines.Add("IMGUI_ENABLE_FREETYPE");
            settings.WrapPointersAsHandle = true;

            string[] strings =
                [
                $"CIMGUI_USE_GLFW={(name == "GLFW" ? "1":"0")}",
                $"CIMGUI_USE_SDL2={(name == "SDL2" ? "1":"0")}",
                $"CIMGUI_USE_SDL2Renderer={(name == "SDL2" ? "1":"0")}",
                $"CIMGUI_USE_OPENGL3={(name == "OpenGL3" ? "1":"0")}",
                $"CIMGUI_USE_OPENGL2={(name == "OpenGL2" ? "1":"0")}",
                $"CIMGUI_USE_D3D9={(name == "D3D9" ? "1":"0")}",
                $"CIMGUI_USE_D3D10={(name == "D3D10" ? "1":"0")}",
                $"CIMGUI_USE_D3D11={(name == "D3D11" ? "1":"0")}",
                $"CIMGUI_USE_D3D12={(name == "D3D12" ? "1":"0")}",
                $"CIMGUI_USE_VULKAN={(name == "Vulkan" ? "1":"0")}",
                $"CIMGUI_USE_WIN32={(name == "Win32" ? "1":"0")}",
                $"CIMGUI_USE_METAL={(name == "Metal" ? "1":"0")}",
                $"CIMGUI_USE_OSX={(name == "OSX" ? "1":"0")}",
                $"CIMGUI_USE_ANDROID={(name == "Android" ? "1":"0")}"];

            var originalNamespace = settings.Namespace;

            settings.Defines.AddRange(strings);
            settings.ApiName = $"ImGuiImpl{name}";
            settings.Namespace += $".{name}";

            string ignoreName = name switch
            {
                "SDL2" => "ImplSDL2",
                "GLFW" => "ImplGlfw",
                "D3D9" => "ImplDX9",
                "D3D10" => "ImplDX10",
                "D3D11" => "ImplDX11",
                "D3D12" => "ImplDX12",
                _ => $"Impl{name}"
            };

            ImGuiCodeGenerator generator = new(settings);
            generator.PatchEngine.RegisterPrePatch(new ImVectorPatch());
            generator.PatchEngine.RegisterPrePatch(new NamingPatch(["CImGui", "ImGui", "ImGuizmo", "ImNodes", "ImPlot", ignoreName], NamingPatchOptions.MultiplePrefixes));
            generator.PatchEngine.RegisterPostPatch(new ImGuiBackendsPostPatch());

            generator.LogToConsole();

            if (lib != null)
            {
                settings.FunctionTableEntries.AddRange(lib.FunctionTable.Entries);
                generator.CopyFrom(lib);
            }

            if (name == "Win32")
            {
                name = "Windows";
            }

            var originalOutput = output;
            output = Path.Combine(output, name);

            bool result = generator.Generate([.. headers], output);
            metadata = generator.GetMetadata();

            File.WriteAllText(settings.LibName + ".log", string.Join(Environment.NewLine, generator.Messages.Select(x => x.ToString())));

            lib?.Merge(metadata, true);

            string destPath = Path.Combine(originalOutput, "FunctionTable.cs");
            File.Move(Path.Combine(output, "FunctionTable.cs"), destPath, true);

            string text = File.ReadAllText(destPath);
            text = text.Replace(settings.ApiName, "ImGuiImpl").Replace(settings.Namespace, originalNamespace);
            File.WriteAllText(destPath, text);

            return result;
        }
    }
}