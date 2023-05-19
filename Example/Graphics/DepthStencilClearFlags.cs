namespace HexaEngine.Core.Graphics
{
    public enum DepthStencilClearFlags
    {
        None = 0,
        Depth = 1,
        Stencil = 2,
        All = Depth | Stencil,
    }
}