namespace HexaEngine.Core.Graphics
{
    [Flags]
    public enum DepthStencilViewFlags : int
    {
        ReadOnlyDepth = unchecked(1),
        ReadOnlyStencil = unchecked(2),
        None = unchecked(0)
    }
}