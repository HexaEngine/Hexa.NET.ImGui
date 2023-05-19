namespace HexaEngine.Core.Graphics.Textures
{
    /// <summary>
    /// Matches DDS_ALPHA_MODE, encoded in MISC_FLAGS2
    /// </summary>
    public enum TexAlphaMode
    {
        Unknown = 0,
        Straight = 1,
        Premultiplied = 2,
        Opaque = 3,
        Custom = 4,
    }
}