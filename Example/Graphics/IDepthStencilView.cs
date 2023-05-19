namespace HexaEngine.Core.Graphics
{
    public interface IDepthStencilView : IDeviceChild
    {
        DepthStencilViewDescription Description { get; }
    }
}