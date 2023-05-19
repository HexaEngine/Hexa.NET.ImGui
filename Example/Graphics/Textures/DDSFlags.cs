namespace HexaEngine.Core.Graphics.Textures
{
    using System;

    [Flags]
    public enum DDSFlags
    {
        None = 0x0,

        /// <summary>
        /// Assume pitch is DWORD aligned instead of BYTE aligned (used by some legacy DDS files)
        /// </summary>
        LegacyDWORD = 0x1,

        /// <summary>
        /// Do not implicitly convert legacy formats that result in larger pixel sizes (24 bpp, 3:3:2, A8L8, A4L4, P8, A8P8)
        /// </summary>
        NoLegacyExpansion = 0x2,

        /// <summary>
        /// Do not use work-around for long-standing D3DX DDS file format issue which reversed the 10:10:10:2 color order masks
        /// </summary>
        NoR10B10G10A2Fixup = 0x4,

        /// <summary>
        /// Convert DXGI 1.1 BGR formats to DXGI_FORMAT_R8G8B8A8_UNORM to avoid use of optional WDDM 1.1 formats
        /// </summary>
        ForceRGB = 0x8,

        /// <summary>
        /// Conversions avoid use of 565, 5551, and 4444 formats and instead expand to 8888 to avoid use of optional WDDM 1.2 formats
        /// </summary>
        No16BPP = 0x10,

        /// <summary>
        /// When loading legacy luminance formats expand replicating the color channels rather than leaving them packed (L8, L16, A8L8)
        /// </summary>
        ExpandLuminance = 0x20,

        /// <summary>
        /// Some older DXTn DDS files incorrectly handle mipchain tails for blocks smaller than 4x4
        /// </summary>
        BadDXTNTails = 0x40,

        /// <summary>
        /// Always use the 'DX10' header extension for DDS writer (i.e. don't try to write DX9 compatible DDS files)
        /// </summary>
        ForceDX10Ext = 0x10000,

        /// <summary>
        /// FORCE_DX10_EXT including miscFlags2 information (result may not be compatible with D3DX10 or D3DX11)
        /// </summary>
        ForceDX10ExtMisc2 = 0x20000,

        /// <summary>
        /// Force use of legacy header for DDS writer (will fail if unable to write as such)
        /// </summary>
        ForceDX9Legacy = 0x40000,

        /// <summary>
        /// Enables the loader to read large dimension .dds files (i.e. greater than known hardware requirements)
        /// </summary>
        AllowLargeFiles = 0x1000000,
    }
}