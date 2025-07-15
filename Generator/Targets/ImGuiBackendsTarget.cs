namespace Generator.Targets
{
    using HexaGen;
    using HexaGen.BuildSystems;
    using HexaGen.Metadata;
    using HexaGen.Patching;
    using System.Collections.Generic;

    public static class ImGuiBackendsTargetExtensions
    {
        public static BuildSystemBuilder AddImGuiBackends(this BuildSystemBuilder builder)
        {
            builder.AddTarget<ImGuiBackendsTarget>();
            return builder;
        }
    }

    public class ImGuiBackendsTarget : ImGuiBaseTarget
    {
        private const string ImGuiBackendsOutputPath = "../../../../Hexa.NET.ImGui.Backends/Generated";
        private const string ImGuiBackendsSDL2OutputPath = "../../../../Hexa.NET.ImGui.Backends.SDL2/Generated";
        private const string ImGuiBackendsSDL3OutputPath = "../../../../Hexa.NET.ImGui.Backends.SDL3/Generated";
        private const string ImGuiBackendsGLFWOutputPath = "../../../../Hexa.NET.ImGui.Backends.GLFW/Generated";
        private const string CImGuiBackendsHeaders = "backends/include";
        private const string CImGuiBackendsConfig = "backends/generator.json";
        private const string CImGuiBackendsSDL2Config = "backends/generator.sdl2.json";
        private const string CImGuiBackendsSDL3Config = "backends/generator.sdl3.json";
        private const string CImGuiBackendsGLFWConfig = "backends/generator.glfw.json";

        private static readonly string[] backends =
        [
            "OpenGL3",
            "OpenGL2",
            "D3D9",
            "D3D10",
            "D3D11",
            "D3D12",
            "Vulkan",
            "Win32",
            "OSX",
            "Metal",
            "Android"
        ];

        public override string Name { get; } = "backends";

        public override IReadOnlyList<string> Dependencies { get; } = ["imgui"];

        public override void Execute(BuildContext context)
        {
            var metadata = GetImGuiMetadata(context);

            metadata.CppDefinedFunctions.Clear();

            CsCodeGeneratorMetadata? metadataBackend = new()
            {
                FunctionTable = new() { Entries = [new(0, "igSetCurrentContext"), new(1, "igGetCurrentContext")] },
                Settings = new(),
                WrappedPointers = metadata.WrappedPointers.ToDictionary()
            };

            var headers = Directory.GetFiles(CImGuiBackendsHeaders, "*.h");

            foreach (string lib in backends)
            {
                GenerateBackend(headers, CImGuiBackendsConfig, ImGuiBackendsOutputPath, metadataBackend, out var libMetadata, lib);
                metadataBackend ??= libMetadata;
            }

            Generate(headers, CImGuiBackendsSDL2Config, ImGuiBackendsSDL2OutputPath, metadata, out _, InternalsGenerationType.BothOrDontCare);
            Generate(headers, CImGuiBackendsSDL3Config, ImGuiBackendsSDL3OutputPath, metadata, out _, InternalsGenerationType.BothOrDontCare);
            Generate(headers, CImGuiBackendsGLFWConfig, ImGuiBackendsGLFWOutputPath, metadata, out _, InternalsGenerationType.BothOrDontCare);
        }

        private static void GenerateBackend(string[] headers, string configPath, string output, CsCodeGeneratorMetadata? lib, out CsCodeGeneratorMetadata metadata, string name)
        {
            string originalNamespace = string.Empty;
            var originalOutput = output;
            output = Path.Combine(output, name == "Win32" ? "Windows" : name);

            GeneratorBuilder.Create<ImGuiCodeGenerator>(configPath)
                .WithPrePatch<ImVectorPatch>()
                .WithPrePatch<NamingPatch, string>(name, name =>
                {
                    string ignoreName = name switch
                    {
                        "SDL2" => "ImplSDL2",
                        "SDL3" => "ImplSDL3",
                        "GLFW" => "ImplGlfw",
                        "D3D9" => "ImplDX9",
                        "D3D10" => "ImplDX10",
                        "D3D11" => "ImplDX11",
                        "D3D12" => "ImplDX12",
                        _ => $"Impl{name}"
                    };
                    return new(["CImGui", "ImGui", "ImGuizmo", "ImNodes", "ImPlot", ignoreName], NamingPatchOptions.MultiplePrefixes);
                })
                .WithPostPatch<ImGuiBackendsPostPatch>()
                .WithMacros(builder =>
                    builder.WithPrefix("CIMGUI_USE_")
                        .WithSelector(name, (option, value, cond) => $"{value ?? option}={(cond ? "1" : "0")}", StringComparer.OrdinalIgnoreCase.Equals)
                            .Option("GLFW")
                            .Option("SDL2", ["SDL2", "SDL2Renderer"])
                            .Option("SDL3", ["SDL3", "SDL3Renderer", "SDLGPU3"])
                            .Option("OPENGL2")
                            .Option("OPENGL3")
                            .Option("D3D9")
                            .Option("D3D10")
                            .Option("D3D11")
                            .Option("D3D12")
                            .Option("VULKAN")
                            .Option("WIN32")
                            .Option("METAL")
                            .Option("OSX")
                            .Option("ANDROID")
                )
                .AlterConfig(config =>
                {
                    originalNamespace = config.Namespace;
                    config.Namespace += $".{name}";
                    config.ApiName = $"ImGuiImpl{name}";
                })
                .WithFunctionTableEntires(lib)
                .CopyFromMetadata(lib)
                .Generate([.. headers], output)
                .GetMetadata(out metadata)
                .GetConfig(out var config);

            lib?.Merge(metadata, true);

            string destPath = Path.Combine(originalOutput, "FunctionTable.cs");
            File.Move(Path.Combine(output, "FunctionTable.cs"), destPath, true);

            string text = File.ReadAllText(destPath);
            text = text.Replace(config.ApiName, "ImGuiImpl").Replace(config.Namespace, originalNamespace);
            File.WriteAllText(destPath, text);
        }
    }
}