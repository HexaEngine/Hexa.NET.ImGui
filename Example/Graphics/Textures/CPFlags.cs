namespace HexaEngine.Core.Graphics.Textures
{
    using System;

    [Flags]
    public enum CPFlags
    {
        /// <summary>
        /// Normal operation
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Assume pitch is DWORD aligned instead of BYTE aligned
        /// </summary>
        LegacyDWORD = 0x1,

        /// <summary>
        /// Assume pitch is 16-byte aligned instead of BYTE aligned
        /// </summary>
        Paragraph = 0x2,

        /// <summary>
        /// Assume pitch is 32-byte aligned instead of BYTE aligned
        /// </summary>
        YMM = 0x4,

        /// <summary>
        /// The ZMM
        /// </summary>
        ZMM = 0x8,

        /// <summary>
        /// Assume pitch is 4096-byte aligned instead of BYTE aligned
        /// </summary>
        Page4K = 0x200,

        /// <summary>
        /// BC formats with malformed mipchain blocks smaller than 4x4
        /// </summary>
        BadDXTNTails = 0x1000,

        /// <summary>
        /// Override with a legacy 24 bits-per-pixel format size
        /// </summary>
        BPP24 = 0x10000,

        /// <summary>
        /// Override with a legacy 16 bits-per-pixel format size
        /// </summary>
        BPP16 = 0x20000,

        /// <summary>
        /// Override with a legacy 8 bits-per-pixel format size
        /// </summary>
        BPP8 = 0x40000,
    }
}