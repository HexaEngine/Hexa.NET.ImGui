namespace HexaEngine.Core.Graphics
{
    public interface IGraphicsAdapter
    {
        GraphicsBackend Backend { get; }

        IGraphicsDevice CreateGraphicsDevice(bool debug);

        int PlatformScore { get; }
    }
}