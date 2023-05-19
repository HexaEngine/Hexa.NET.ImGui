namespace HexaEngine.Core.Graphics
{
    public enum TextureAddressMode : int
    {
        Wrap = unchecked(1),
        Mirror = unchecked(2),
        Clamp = unchecked(3),
        Border = unchecked(4),
        MirrorOnce = unchecked(5)
    }
}