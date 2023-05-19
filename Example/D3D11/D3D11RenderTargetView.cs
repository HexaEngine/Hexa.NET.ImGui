namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;
    using Viewport = Mathematics.Viewport;

    public unsafe class D3D11RenderTargetView : DeviceChildBase, IRenderTargetView
    {
        internal readonly ComPtr<ID3D11RenderTargetView> rtv;

        public D3D11RenderTargetView(ComPtr<ID3D11RenderTargetView> rtv, RenderTargetViewDescription description, Viewport viewport)
        {
            this.rtv = rtv;
            nativePointer = new(rtv);
            Description = description;
            Viewport = viewport;
        }

        public RenderTargetViewDescription Description { get; }

        public Viewport Viewport { get; }

        protected override void DisposeCore()
        {
            rtv.Release();
        }
    }
}