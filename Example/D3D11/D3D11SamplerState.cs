namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;

    public unsafe class D3D11SamplerState : DeviceChildBase, ISamplerState
    {
        internal readonly ComPtr<ID3D11SamplerState> sampler;

        public D3D11SamplerState(ComPtr<ID3D11SamplerState> sampler, SamplerDescription description)
        {
            this.sampler = sampler;
            nativePointer = new(sampler);
            Description = description;
        }

        public SamplerDescription Description { get; }

        protected override void DisposeCore()
        {
            sampler.Release();
        }
    }
}