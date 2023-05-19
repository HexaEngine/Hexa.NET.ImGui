namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;
    using ResourceDimension = Core.Graphics.ResourceDimension;

    public unsafe class D3D11Texture2D : DeviceChildBase, ITexture2D
    {
        internal readonly ComPtr<ID3D11Texture2D> texture;

        public D3D11Texture2D(ComPtr<ID3D11Texture2D> texture, Texture2DDescription description)
        {
            this.texture = texture;
            nativePointer = new(texture);
            Description = description;
        }

        public Texture2DDescription Description { get; }

        public ResourceDimension Dimension => ResourceDimension.Texture2D;

        protected override void DisposeCore()
        {
            texture.Release();
            texture.Detach();
        }
    }
}