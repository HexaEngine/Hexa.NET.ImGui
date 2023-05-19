namespace HexaEngine.Core.Graphics.Textures
{
    using System;

    [Flags]
    public enum TGAFlags : ulong
    {
        None = 0x0,

        /// <summary>
        /// 24bpp files are returned as BGRX; 32bpp files are returned as BGRA
        /// </summary>
        BGR = 0x1,

        /// <summary>
        /// If the loaded image has an all zero alpha channel, normally we assume it should be opaque. This flag leaves it alone.
        /// </summary>
        AllowAllZeroAlpha = 0x2,

        /// <summary>
        /// Ignores sRGB TGA 2.0 metadata if present in the file
        /// </summary>
        IgnoreSRGB = 0x10,

        /// <summary>
        /// Writes sRGB metadata into the file reguardless of format (TGA 2.0 only)
        /// </summary>
        ForceSRGB = 0x20,

        /// <summary>
        /// Writes linear gamma metadata into the file reguardless of format (TGA 2.0 only)
        /// </summary>
        ForceLinear = 0x40,

        /// <summary>
        /// If no colorspace is specified in TGA 2.0 metadata, assume sRGB
        /// </summary>
        DefaultSRGB = 0x80,
    }
}