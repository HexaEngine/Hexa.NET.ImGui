namespace HexaEngine.Core.Graphics
{
    public interface IUnorderedAccessView : IDeviceChild
    {
        public UnorderedAccessViewDescription Description { get; }
    }
}