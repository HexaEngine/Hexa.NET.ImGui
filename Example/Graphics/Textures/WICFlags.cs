namespace HexaEngine.Core.Graphics.Textures
{
    using System;

    [Flags]
    public enum WICFlags
    {
        None = 0x0,

        /// <summary>
        /// Loads DXGI 1.1 BGR formats as DXGI_FORMAT_R8G8B8A8_UNORM to avoid use of optional WDDM 1.1 formats
        /// </summary>
        ForceRGB = 0x1,

        /// <summary>
        /// Loads DXGI 1.1 X2 10:10:10:2 format as DXGI_FORMAT_R10G10B10A2_UNORM
        /// </summary>
        NoX2Bias = 0x2,

        /// <summary>
        /// Loads 565, 5551, and 4444 formats as 8888 to avoid use of optional WDDM 1.2 formats
        /// </summary>
        No16BPP = 0x4,

        /// <summary>
        /// Loads 1-bit monochrome (black and white) as R1_UNORM rather than 8-bit grayscale
        /// </summary>
        AllowMono = 0x8,

        /// <summary>
        /// Loads all images in a multi-frame file, converting/resizing to match the first frame as needed, defaults to 0th frame otherwise
        /// </summary>
        AllFrames = 0x10,

        /// <summary>
        /// Ignores sRGB metadata if present in the file
        /// </summary>
        IgnoreSRGB = 0x20,

        /// <summary>
        /// Writes sRGB metadata into the file reguardless of format
        /// </summary>
        ForceSRGB = 0x40,

        /// <summary>
        /// Writes linear gamma metadata into the file reguardless of format
        /// </summary>
        ForceLinear = 0x80,

        /// <summary>
        /// If no colorspace is specified, assume sRGB
        /// </summary>
        DefaultSRGB = 0x100,

        /// <summary>
        /// Use ordered 4x4 dithering for any required conversions
        /// </summary>
        Dither = 0x10000,

        /// <summary>
        /// Use error-diffusion dithering for any required conversions
        /// </summary>
        DitherDiffusion = 0x20000,

        /// <summary>
        /// Filtering mode to use for any required image resizing (only needed when loading arrays of differently sized images; defaults to Fant)
        /// </summary>
        FilterPoint = 0x100000,

        /// <summary>
        /// Filtering mode to use for any required image resizing (only needed when loading arrays of differently sized images; defaults to Fant)
        /// </summary>
        FilterLinear = 0x200000,

        /// <summary>
        /// Filtering mode to use for any required image resizing (only needed when loading arrays of differently sized images; defaults to Fant)
        /// </summary>
        FilterCubic = 0x300000,

        /// <summary>
        /// Filtering mode to use for any required image resizing (only needed when loading arrays of differently sized images; defaults to Fant)
        /// Combination of Linear and Box filter
        /// </summary>
        FilterFant = 0x400000
    }
}