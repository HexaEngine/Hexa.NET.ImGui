namespace HexaEngine.Core.Graphics
{
    public unsafe interface IResource : IDeviceChild
    {
        public ResourceDimension Dimension { get; }

        public const int MaximumMipLevels = unchecked(15);
        public const int ResourceSizeInMegabytes = unchecked(128);
        public const int MaximumTexture1DArraySize = unchecked(2048);
        public const int MaximumTexture2DArraySize = unchecked(2048);
        public const int MaximumTexture1DSize = unchecked(16384);
        public const int MaximumTexture2DSize = unchecked(16384);
        public const int MaximumTexture3DSize = unchecked(2048);
        public const int MaximumTextureCubeSize = unchecked(16384);

        public static int CalculateSubresourceIndex(int mipSlice, int arraySlice, int mipLevels)
        {
            return mipSlice + (arraySlice * mipLevels);
        }
    }

    public enum ResourceDimension : int
    {
        Unknown = unchecked(0),
        Buffer = unchecked(1),
        Texture1D = unchecked(2),
        Texture2D = unchecked(3),
        Texture3D = unchecked(4)
    }

    public enum TextureDimension : int
    {
        Unknown = unchecked(0),
        Texture1D = unchecked(1),
        Texture2D = unchecked(2),
        Texture3D = unchecked(3),
        TextureCube = unchecked(4)
    }
}