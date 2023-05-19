namespace HexaEngine.Core.Graphics.Textures
{
    /// <summary>
    /// Subset here matches D3D10_RESOURCE_MISC_FLAG and D3D11_RESOURCE_MISC_FLAG
    /// </summary>
    [Flags]
    public enum TexMiscFlags : uint
    {
        GenerateMips = unchecked(1),
        Shared = unchecked(2),
        TextureCube = unchecked(4),
        DrawIndirectArguments = unchecked(16),
        BufferAllowRawViews = unchecked(32),
        BufferStructured = unchecked(64),
        ResourceClamp = unchecked(128),
        SharedKeyedMutex = unchecked(256),
        GdiCompatible = unchecked(512),
        SharedNTHandle = unchecked(2048),
        RestrictedContent = unchecked(4096),
        RestrictSharedResource = unchecked(8192),
        RestrictSharedResourceDriver = unchecked(16384),
        Guarded = unchecked(32768),
        TilePool = unchecked(131072),
        Tiled = unchecked(262144),
        HardwareProtected = unchecked(524288),
        SharedDisplayable = unchecked(1048576),
        SharedExclusiveWriter = unchecked(2097152),
        None = unchecked(0)
    }
}