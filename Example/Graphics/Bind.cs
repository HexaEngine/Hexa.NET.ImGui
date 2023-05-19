namespace HexaEngine.Core.Graphics
{
    [Flags]
    public enum BindFlags : int
    {
        VertexBuffer = unchecked(1),
        IndexBuffer = unchecked(2),
        ConstantBuffer = unchecked(4),
        ShaderResource = unchecked(8),
        StreamOutput = unchecked(16),
        RenderTarget = unchecked(32),
        DepthStencil = unchecked(64),
        UnorderedAccess = unchecked(128),
        Decoder = unchecked(512),
        VideoEncoder = unchecked(1024),
        None = unchecked(0)
    }
}