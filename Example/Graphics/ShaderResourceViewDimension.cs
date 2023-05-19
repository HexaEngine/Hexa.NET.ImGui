namespace HexaEngine.Core.Graphics
{
    public enum ShaderResourceViewDimension : int
    {
        Unknown = unchecked(0),
        Buffer = unchecked(1),
        Texture1D = unchecked(2),
        Texture1DArray = unchecked(3),
        Texture2D = unchecked(4),
        Texture2DArray = unchecked(5),
        Texture2DMultisampled = unchecked(6),
        Texture2DMultisampledArray = unchecked(7),
        Texture3D = unchecked(8),
        TextureCube = unchecked(9),
        TextureCubeArray = unchecked(10),
        BufferExtended = unchecked(11)
    }
}