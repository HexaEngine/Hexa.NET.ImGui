namespace HexaEngine.Core.Graphics
{
    public enum DepthStencilViewDimension : int
    {
        Unknown = unchecked(0),
        Texture1D = unchecked(1),
        Texture1DArray = unchecked(2),
        Texture2D = unchecked(3),
        Texture2DArray = unchecked(4),
        Texture2DMultisampled = unchecked(5),
        Texture2DMultisampledArray = unchecked(6)
    }
}