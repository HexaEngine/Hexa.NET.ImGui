namespace HexaEngine.Core.Graphics
{
    using HexaEngine.Mathematics;

    public interface IRenderTargetView : IDeviceChild
    {
        RenderTargetViewDescription Description { get; }

        public Viewport Viewport { get; }
    }
}