namespace HexaEngine.Core.Graphics.Textures
{
    using System;

    [Flags]
    public enum TexCompressFlags
    {
        Default = 0,

        /// <summary>
        /// Enables dithering RGB colors for BC1-3 compression
        /// </summary>
        DitherRGB = 0x10000,

        /// <summary>
        /// Enables dithering alpha for BC1-3 compression
        /// </summary>
        DitherA = 0x20000,

        /// <summary>
        /// Enables both RGB and alpha dithering for BC1-3 compression
        /// </summary>
        Dither = 0x30000,

        /// <summary>
        /// Uniform color weighting for BC1-3 compression; by default uses perceptual weighting
        /// </summary>
        Uniform = 0x40000,

        /// <summary>
        /// Enables exhaustive search for BC7 compress for mode 0 and 2; by default skips trying these modes
        /// </summary>
        BC7Use3Sunsets = 0x80000,

        /// <summary>
        /// Minimal modes (usually mode 6) for BC7 compression
        /// </summary>
        BC7Quick = 0x100000,

        SRGBIn = 0x1000000,
        SRGBOut = 0x2000000,

        /// <summary>
        /// if the input format type is IsSRGB(), then SRGB_IN is on by default
        /// if the output format type is IsSRGB(), then SRGB_OUT is on by default
        /// </summary>
        SRGB = SRGBIn | SRGBOut,

        /// <summary>
        /// Compress is free to use multithreading to improve performance (by default it does not use multithreading)
        /// </summary>
        Parallel = 0x10000000
    }
}