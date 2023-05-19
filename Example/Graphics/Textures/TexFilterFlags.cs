namespace HexaEngine.Core.Graphics.Textures
{
    using System;

    [Flags]
    public enum TexFilterFlags
    {
        Default = 0,

        /// <summary>
        /// Wrap vs. Mirror vs. Clamp filtering options
        /// </summary>
        WrapU = 0x1,

        /// <summary>
        /// Wrap vs. Mirror vs. Clamp filtering options
        /// </summary>
        WrapV = 0x2,

        /// <summary>
        /// Wrap vs. Mirror vs. Clamp filtering options
        /// </summary>
        WrapW = 0x4,

        /// <summary>
        /// Wrap vs. Mirror vs. Clamp filtering options
        /// </summary>
        Wrap = WrapU | WrapV | WrapW,

        MirrorU = 0x10,

        /// <summary>
        /// Wrap vs. Mirror vs. Clamp filtering options
        /// </summary>
        MirrorV = 0x20,

        /// <summary>
        /// Wrap vs. Mirror vs. Clamp filtering options
        /// </summary>
        MirrorW = 0x40,

        /// <summary>
        /// Wrap vs. Mirror vs. Clamp filtering options
        /// </summary>
        Mirror = MirrorU | MirrorV | MirrorW,

        /// <summary>
        /// Resize color and alpha channel independently
        /// </summary>
        SeparateAlpha = 0x100,

        /// <summary>
        /// Enable *2 - 1 conversion cases for unorm to/from float and positive-only float formats
        /// </summary>
        FloatX2Bias = 0x200,

        /// <summary>
        /// When converting RGB to R, defaults to using grayscale. These flags indicate copying a specific channel instead
        /// When converting RGB to RG, defaults to copying RED | GREEN. These flags control which channels are selected instead.
        /// </summary>
        RGBCopyRed = 0x1000,

        /// <summary>
        /// When converting RGB to R, defaults to using grayscale. These flags indicate copying a specific channel instead
        /// When converting RGB to RG, defaults to copying RED | GREEN. These flags control which channels are selected instead.
        /// </summary>
        RGBCopyGreen = 0x2000,

        /// <summary>
        /// When converting RGB to R, defaults to using grayscale. These flags indicate copying a specific channel instead
        /// When converting RGB to RG, defaults to copying RED | GREEN. These flags control which channels are selected instead.
        /// </summary>
        RGBCopyBlue = 0x4000,

        /// <summary>
        /// Use ordered 4x4 dithering for any required conversions
        /// </summary>
        Dither = 0x10000,

        /// <summary>
        /// Use error-diffusion dithering for any required conversions
        /// </summary>
        DitherDiffusion = 0x20000,

        /// <summary>
        /// Filtering mode to use for any required image resizing
        /// </summary>
        Point = 0x100000,

        /// <summary>
        /// Filtering mode to use for any required image resizing
        /// </summary>
        Linear = 0x200000,

        /// <summary>
        /// Filtering mode to use for any required image resizing
        /// </summary>
        Cubic = 0x300000,

        /// <summary>
        /// Filtering mode to use for any required image resizing
        /// </summary>
        Box = 0x400000,

        /// <summary>
        /// Filtering mode to use for any required image resizing
        /// Equiv to Box filtering for mipmap generation
        /// </summary>
        Fant = 0x400000,

        /// <summary>
        /// Filtering mode to use for any required image resizing
        /// </summary>
        Triangle = 0x500000,

        SRGBIn = 0x1000000,
        SRGBOut = 0x2000000,

        /// <summary>
        /// sRGB to/from RGB for use in conversion operations
        /// if the input format type is IsSRGB(), then SRGB_IN is on by default
        /// if the output format type is IsSRGB(), then SRGB_OUT is on by default
        /// </summary>
        SRGB = SRGBIn | SRGBOut,

        /// <summary>
        /// Forces use of the non-WIC path when both are an option
        /// </summary>
        ForceNonWIC = 0x10000000,

        /// <summary>
        /// Forces use of the WIC path even when logic would have picked a non-WIC path when both are an option
        /// </summary>
        ForceWIC = 0x20000000
    }
}