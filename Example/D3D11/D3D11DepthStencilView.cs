namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;

    public unsafe class D3D11DepthStencilView : DeviceChildBase, IDepthStencilView
    {
        internal readonly ComPtr<ID3D11DepthStencilView> dsv;

        public D3D11DepthStencilView(ComPtr<ID3D11DepthStencilView> dsv, DepthStencilViewDescription description)
        {
            this.dsv = dsv;
            nativePointer = new(dsv);
            Description = description;
        }

        public DepthStencilViewDescription Description { get; }

        protected override void DisposeCore()
        {
            dsv.Release();
        }
    }
}