namespace HexaEngine.Core.Graphics
{
    public enum StencilOperation : int
    {
        Keep = unchecked(1),
        Zero = unchecked(2),
        Replace = unchecked(3),
        IncrementSaturate = unchecked(4),
        DecrementSaturate = unchecked(5),
        Invert = unchecked(6),
        Increment = unchecked(7),
        Decrement = unchecked(8)
    }
}