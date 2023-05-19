namespace HexaEngine.Core.Graphics
{
    using HexaEngine.Core.Debugging;
    using System.Collections.Generic;

    public static class PipelineManager
    {
        private static readonly List<IGraphicsPipeline> graphicsPipelines = new();
        private static readonly List<IComputePipeline> computePipelines = new();
#nullable disable
        private static IGraphicsDevice device;
#nullable enable

        public static event Action? OnRecompile;

        public static IReadOnlyList<IGraphicsPipeline> GraphicsPipelines => graphicsPipelines;

        public static IReadOnlyList<IComputePipeline> ComputePipelines => computePipelines;

        public static void Initialize(IGraphicsDevice device)
        {
            PipelineManager.device = device;
        }

        public static void Recompile()
        {
            lock (graphicsPipelines)
            {
                OnRecompile?.Invoke();

                ImGuiConsole.Log(LogSeverity.Info, "recompiling graphics pipelines ...");
                for (int i = 0; i < graphicsPipelines.Count; i++)
                {
                    graphicsPipelines[i].Recompile();
                }
                ImGuiConsole.Log(LogSeverity.Info, "recompiling graphics pipelines ... done!");

                ImGuiConsole.Log(LogSeverity.Info, "recompiling compute pipelines ...");
                for (int i = 0; i < computePipelines.Count; i++)
                {
                    computePipelines[i].Recompile();
                }
                ImGuiConsole.Log(LogSeverity.Info, "recompiling compute pipelines ... done!");
            }
        }

        public static void Register(IGraphicsPipeline pipeline)
        {
            lock (graphicsPipelines)
            {
                graphicsPipelines.Add(pipeline);
            }
        }

        public static void Register(IComputePipeline pipeline)
        {
            lock (graphicsPipelines)
            {
                computePipelines.Add(pipeline);
            }
        }

        public static void Unregister(IGraphicsPipeline pipeline)
        {
            lock (graphicsPipelines)
            {
                graphicsPipelines.Remove(pipeline);
            }
        }

        public static void Unregister(IComputePipeline pipeline)
        {
            lock (graphicsPipelines)
            {
                computePipelines.Remove(pipeline);
            }
        }
    }
}