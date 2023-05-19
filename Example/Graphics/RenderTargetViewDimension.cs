namespace HexaEngine.Core.Graphics
{
    public enum RenderTargetViewDimension : int
    {
        Unknown = 0,
        Buffer = 1,
        Texture1D = 2,
        Texture1DArray = 3,
        Texture2D = 4,
        Texture2DArray = 5,
        Texture2DMultisampled = 6,
        Texture2DMultisampledArray = 7,
        Texture3D = 8
    }
}