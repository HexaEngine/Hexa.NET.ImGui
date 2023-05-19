namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;
    using ResourceDimension = Core.Graphics.ResourceDimension;

    public unsafe class D3D11Texture1D : DeviceChildBase, ITexture1D
    {
        internal readonly ComPtr<ID3D11Texture1D> texture;

        public D3D11Texture1D(ComPtr<ID3D11Texture1D> texture, Texture1DDescription description)
        {
            this.texture = texture;
            nativePointer = new(texture);
            Description = description;
        }

        public Texture1DDescription Description { get; }

        public ResourceDimension Dimension => ResourceDimension.Texture1D;

        protected override void DisposeCore()
        {
            texture.Release();
        }
    }
}