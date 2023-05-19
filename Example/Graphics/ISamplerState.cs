namespace HexaEngine.Core.Graphics
{
    public interface ISamplerState : IDeviceChild
    {
        public SamplerDescription Description { get; }
    }
}