namespace HexaEngine.Core.Graphics
{
    public enum UnorderedAccessViewDimension : int
    {
        Unknown = unchecked(0),
        Buffer = unchecked(1),
        Texture1D = unchecked(2),
        Texture1DArray = unchecked(3),
        Texture2D = unchecked(4),
        Texture2DArray = unchecked(5),
        Texture3D = unchecked(8)
    }
}