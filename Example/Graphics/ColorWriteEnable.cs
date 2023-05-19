namespace HexaEngine.Core.Graphics
{
    [Flags]
    public enum ColorWriteEnable : byte
    {
        Red = unchecked(1),
        Green = unchecked(2),
        Blue = unchecked(4),
        Alpha = unchecked(8),
        All = unchecked(15),
        None = unchecked(0)
    }
}