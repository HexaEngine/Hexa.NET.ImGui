namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;

    public unsafe class D3D11UnorderedAccessView : DeviceChildBase, IUnorderedAccessView
    {
        private readonly ComPtr<ID3D11UnorderedAccessView> uva;

        public D3D11UnorderedAccessView(ComPtr<ID3D11UnorderedAccessView> uva, UnorderedAccessViewDescription description)
        {
            this.uva = uva;
            nativePointer = new(uva);
            Description = description;
        }

        public UnorderedAccessViewDescription Description { get; }

        protected override void DisposeCore()
        {
            uva.Release();
        }
    }
}