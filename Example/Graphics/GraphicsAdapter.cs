namespace HexaEngine.Core.Graphics
{
    using System;
    using System.Linq;

    public static class GraphicsAdapter
    {
        public static List<IGraphicsAdapter> Adapters { get; } = new();

        public static IGraphicsDevice CreateGraphicsDevice(GraphicsBackend backend, bool debug)
        {
            if (backend == GraphicsBackend.Auto)
            {
                if (Adapters.Count == 1)
                {
                    return Adapters[0].CreateGraphicsDevice(debug);
                }
                else
                {
                    IGraphicsAdapter audioAdapter = Adapters[0];
                    for (int i = 0; i < Adapters.Count; i++)
                    {
                        if (Adapters[i].PlatformScore > audioAdapter.PlatformScore)
                        {
                            audioAdapter = Adapters[i];
                        }
                    }
                    return audioAdapter.CreateGraphicsDevice(debug);
                }
            }
            var adapter = Adapters.FirstOrDefault(x => x.Backend == backend) ?? throw new PlatformNotSupportedException();
            return adapter.CreateGraphicsDevice(debug);
        }
    }
}