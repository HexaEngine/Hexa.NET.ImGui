namespace HexaEngine.Core.Graphics
{
    [Flags]
    public enum BufferUnorderedAccessViewFlags : int
    {
        Raw = unchecked(1),
        Append = unchecked(2),
        Counter = unchecked(4),
        None = unchecked(0)
    }
}