namespace HexaEngine.Core.Graphics
{
    using HexaEngine.Core.Graphics.Textures;
    using System.Diagnostics;

    public static class FormatHelper
    {
        public static bool IsValid(Format fmt)
        {
            return (int)fmt >= 1 && (int)fmt <= 190;
        }

        public static bool IsCompressed(Format fmt)
        {
            return fmt switch
            {
                Format.BC1Typeless or Format.BC1UNorm or Format.BC1UNormSRGB or Format.BC2Typeless or Format.BC2UNorm or Format.BC2UNormSRGB or Format.BC3Typeless or Format.BC3UNorm or Format.BC3UNormSRGB or Format.BC4Typeless or Format.BC4UNorm or Format.BC4SNorm or Format.BC5Typeless or Format.BC5UNorm or Format.BC5SNorm or Format.BC6HTypeless or Format.BC6HUF16 or Format.BC6HSF16 or Format.BC7Typeless or Format.BC7UNorm or Format.BC7UNormSRGB => true,
                _ => false,
            };
        }

        public static bool IsPalettized(Format fmt)
        {
            return fmt switch
            {
                Format.AI44 or Format.IA44 or Format.P8 or Format.A8P8 => true,
                _ => false,
            };
        }

        public static bool IsSRGB(Format fmt)
        {
            return fmt switch
            {
                Format.R8G8B8A8UNormSRGB or Format.BC1UNormSRGB or Format.BC2UNormSRGB or Format.BC3UNormSRGB or Format.B8G8R8A8UNormSRGB or Format.B8G8R8X8UNormSRGB or Format.BC7UNormSRGB => true,
                _ => false,
            };
        }

        public static bool IsBGR(Format fmt)
        {
            return fmt switch
            {
                Format.B5G6R5UNorm or Format.B5G5R5A1UNorm or Format.B8G8R8A8UNorm or Format.B8G8R8X8UNorm or Format.B8G8R8A8Typeless or Format.B8G8R8A8UNormSRGB or Format.B8G8R8X8Typeless or Format.B8G8R8X8UNormSRGB or Format.B4G4R4A4UNorm => true,
                _ => false,
            };
        }

        public static bool IsPacked(Format fmt)
        {
            return fmt switch
            {
                Format.R8G8B8G8UNorm or Format.G8R8G8B8UNorm or Format.YUY2 or Format.Y210 or Format.Y216 => true,
                _ => false,
            };
        }

        public static bool IsVideo(Format fmt)
        {
            return fmt switch
            {
                Format.AYUV or Format.Y410 or Format.Y416 or Format.NV12 or Format.P010 or Format.P016 or Format.YUY2 or Format.Y210 or Format.Y216 or Format.NV11 or Format.Opaque420 or Format.AI44 or Format.IA44 or Format.P8 or Format.A8P8 => true,// These are limited use video formats not usable in any way by the 3D pipeline
                _ => false,
            };
        }

        public static bool IsPlanar(Format fmt)
        {
            return fmt switch
            {
                Format.NV12 or Format.P010 or Format.P016 or Format.Opaque420 or Format.NV11 => true,
                _ => false,
            };
        }

        public static bool IsDepthStencil(Format fmt)
        {
            return fmt switch
            {
                Format.R32G8X24Typeless or Format.D32FloatS8X24UInt or Format.R32FloatX8X24Typeless or Format.X32TypelessG8X24UInt or Format.D32Float or Format.R24G8Typeless or Format.D24UNormS8UInt or Format.R24UNormX8Typeless or Format.X24TypelessG8UInt or Format.D16UNorm => true,
                _ => false,
            };
        }

        public static bool IsTypeless(Format fmt, bool partialTypeless)
        {
            return fmt switch
            {
                Format.R32G32B32A32Typeless or Format.R32G32B32Typeless or Format.R16G16B16A16Typeless or Format.R32G32Typeless or Format.R32G8X24Typeless or Format.R10G10B10A2Typeless or Format.R8G8B8A8Typeless or Format.R16G16Typeless or Format.R32Typeless or Format.R24G8Typeless or Format.R8G8Typeless or Format.R16Typeless or Format.R8Typeless or Format.BC1Typeless or Format.BC2Typeless or Format.BC3Typeless or Format.BC4Typeless or Format.BC5Typeless or Format.B8G8R8A8Typeless or Format.B8G8R8X8Typeless or Format.BC6HTypeless or Format.BC7Typeless => true,
                Format.R32FloatX8X24Typeless or Format.X32TypelessG8X24UInt or Format.R24UNormX8Typeless or Format.X24TypelessG8UInt => partialTypeless,
                _ => false,
            };
        }

        public static bool HasAlpha(Format fmt)
        {
            return fmt switch
            {
                Format.R32G32B32A32Typeless or Format.R32G32B32A32Float or Format.R32G32B32A32UInt or Format.R32G32B32A32SInt or Format.R16G16B16A16Typeless or Format.R16G16B16A16Float or Format.R16G16B16A16UNorm or Format.R16G16B16A16UInt or Format.R16G16B16A16SNorm or Format.R16G16B16A16Sint or Format.R10G10B10A2Typeless or Format.R10G10B10A2UNorm or Format.R10G10B10A2UInt or Format.R8G8B8A8Typeless or Format.R8G8B8A8UNorm or Format.R8G8B8A8UNormSRGB or Format.R8G8B8A8UInt or Format.R8G8B8A8SNorm or Format.R8G8B8A8SInt or Format.A8UNorm or Format.BC1Typeless or Format.BC1UNorm or Format.BC1UNormSRGB or Format.BC2Typeless or Format.BC2UNorm or Format.BC2UNormSRGB or Format.BC3Typeless or Format.BC3UNorm or Format.BC3UNormSRGB or Format.B5G5R5A1UNorm or Format.B8G8R8A8UNorm or Format.R10G10B10XRBiasA2UNorm or Format.B8G8R8A8Typeless or Format.B8G8R8A8UNormSRGB or Format.BC7Typeless or Format.BC7UNorm or Format.BC7UNormSRGB or Format.AYUV or Format.Y410 or Format.Y416 or Format.AI44 or Format.IA44 or Format.A8P8 or Format.B4G4R4A4UNorm => true,
                _ => false,
            };
        }

        /// <summary>
        /// Returns bits-per-pixel for a given DXGI format, or 0 on failure
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static int BitsPerPixel(Format fmt)
        {
            return fmt switch
            {
                Format.R32G32B32A32Typeless or Format.R32G32B32A32Float or Format.R32G32B32A32UInt or Format.R32G32B32A32SInt => 128,
                Format.R32G32B32Typeless or Format.R32G32B32Float or Format.R32G32B32UInt or Format.R32G32B32SInt => 96,
                Format.R16G16B16A16Typeless or Format.R16G16B16A16Float or Format.R16G16B16A16UNorm or Format.R16G16B16A16UInt or Format.R16G16B16A16SNorm or Format.R16G16B16A16Sint or Format.R32G32Typeless or Format.R32G32Float or Format.R32G32UInt or Format.R32G32SInt or Format.R32G8X24Typeless or Format.D32FloatS8X24UInt or Format.R32FloatX8X24Typeless or Format.X32TypelessG8X24UInt or Format.Y416 or Format.Y210 or Format.Y216 => 64,
                Format.R10G10B10A2Typeless or Format.R10G10B10A2UNorm or Format.R10G10B10A2UInt or Format.R11G11B10Float or Format.R8G8B8A8Typeless or Format.R8G8B8A8UNorm or Format.R8G8B8A8UNormSRGB or Format.R8G8B8A8UInt or Format.R8G8B8A8SNorm or Format.R8G8B8A8SInt or Format.R16G16Typeless or Format.R16G16Float or Format.R16G16UNorm or Format.R16G16UInt or Format.R16G16SNorm or Format.R16G16Sint or Format.R32Typeless or Format.D32Float or Format.R32Float or Format.R32UInt or Format.R32SInt or Format.R24G8Typeless or Format.D24UNormS8UInt or Format.R24UNormX8Typeless or Format.X24TypelessG8UInt or Format.R9G9B9E5SharedExp or Format.R8G8B8G8UNorm or Format.G8R8G8B8UNorm or Format.B8G8R8A8UNorm or Format.B8G8R8X8UNorm or Format.R10G10B10XRBiasA2UNorm or Format.B8G8R8A8Typeless or Format.B8G8R8A8UNormSRGB or Format.B8G8R8X8Typeless or Format.B8G8R8X8UNormSRGB or Format.AYUV or Format.Y410 or Format.YUY2 => 32,
                Format.P010 or Format.P016 => 24,
                Format.R8G8Typeless or Format.R8G8UNorm or Format.R8G8UInt or Format.R8G8SNorm or Format.R8G8Sint or Format.R16Typeless or Format.R16Float or Format.D16UNorm or Format.R16UNorm or Format.R16UInt or Format.R16SNorm or Format.R16Sint or Format.B5G6R5UNorm or Format.B5G5R5A1UNorm or Format.A8P8 or Format.B4G4R4A4UNorm => 16,
                Format.NV12 or Format.Opaque420 or Format.NV11 => 12,
                Format.R8Typeless or Format.R8UNorm or Format.R8UInt or Format.R8SNorm or Format.R8SInt or Format.A8UNorm or Format.BC2Typeless or Format.BC2UNorm or Format.BC2UNormSRGB or Format.BC3Typeless or Format.BC3UNorm or Format.BC3UNormSRGB or Format.BC5Typeless or Format.BC5UNorm or Format.BC5SNorm or Format.BC6HTypeless or Format.BC6HUF16 or Format.BC6HSF16 or Format.BC7Typeless or Format.BC7UNorm or Format.BC7UNormSRGB or Format.AI44 or Format.IA44 or Format.P8 => 8,
                Format.R1UNorm => 1,
                Format.BC1Typeless or Format.BC1UNorm or Format.BC1UNormSRGB or Format.BC4Typeless or Format.BC4UNorm or Format.BC4SNorm => 4,
                _ => 0,
            };
        }

        /// <summary>
        /// Returns bits-per-color-channel for a given DXGI format, or 0 on failure
        /// For mixed formats, it returns the largest color-depth in the format
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static int BitsPerColor(Format fmt)
        {
            return fmt switch
            {
                Format.R32G32B32A32Typeless or Format.R32G32B32A32Float or Format.R32G32B32A32UInt or Format.R32G32B32A32SInt or Format.R32G32B32Typeless or Format.R32G32B32Float or Format.R32G32B32UInt or Format.R32G32B32SInt or Format.R32G32Typeless or Format.R32G32Float or Format.R32G32UInt or Format.R32G32SInt or Format.R32G8X24Typeless or Format.D32FloatS8X24UInt or Format.R32FloatX8X24Typeless or Format.X32TypelessG8X24UInt or Format.R32Typeless or Format.D32Float or Format.R32Float or Format.R32UInt or Format.R32SInt => 32,
                Format.R24G8Typeless or Format.D24UNormS8UInt or Format.R24UNormX8Typeless or Format.X24TypelessG8UInt => 24,
                Format.R16G16B16A16Typeless or Format.R16G16B16A16Float or Format.R16G16B16A16UNorm or Format.R16G16B16A16UInt or Format.R16G16B16A16SNorm or Format.R16G16B16A16Sint or Format.R16G16Typeless or Format.R16G16Float or Format.R16G16UNorm or Format.R16G16UInt or Format.R16G16SNorm or Format.R16G16Sint or Format.R16Typeless or Format.R16Float or Format.D16UNorm or Format.R16UNorm or Format.R16UInt or Format.R16SNorm or Format.R16Sint or Format.BC6HTypeless or Format.BC6HUF16 or Format.BC6HSF16 or Format.Y416 or Format.P016 or Format.Y216 => 16,
                Format.R9G9B9E5SharedExp => 14,
                Format.R11G11B10Float => 11,
                Format.R10G10B10A2Typeless or Format.R10G10B10A2UNorm or Format.R10G10B10A2UInt or Format.R10G10B10XRBiasA2UNorm or Format.Y410 or Format.P010 or Format.Y210 => 10,
                Format.R8G8B8A8Typeless or Format.R8G8B8A8UNorm or Format.R8G8B8A8UNormSRGB or Format.R8G8B8A8UInt or Format.R8G8B8A8SNorm or Format.R8G8B8A8SInt or Format.R8G8Typeless or Format.R8G8UNorm or Format.R8G8UInt or Format.R8G8SNorm or Format.R8G8Sint or Format.R8Typeless or Format.R8UNorm or Format.R8UInt or Format.R8SNorm or Format.R8SInt or Format.A8UNorm or Format.R8G8B8G8UNorm or Format.G8R8G8B8UNorm or Format.BC4Typeless or Format.BC4UNorm or Format.BC4SNorm or Format.BC5Typeless or Format.BC5UNorm or Format.BC5SNorm or Format.B8G8R8A8UNorm or Format.B8G8R8X8UNorm or Format.B8G8R8A8Typeless or Format.B8G8R8A8UNormSRGB or Format.B8G8R8X8Typeless or Format.B8G8R8X8UNormSRGB or Format.AYUV or Format.NV12 or Format.Opaque420 or Format.YUY2 or Format.NV11 => 8,
                Format.BC7Typeless or Format.BC7UNorm or Format.BC7UNormSRGB => 7,
                Format.BC1Typeless or Format.BC1UNorm or Format.BC1UNormSRGB or Format.BC2Typeless or Format.BC2UNorm or Format.BC2UNormSRGB or Format.BC3Typeless or Format.BC3UNorm or Format.BC3UNormSRGB or Format.B5G6R5UNorm => 6,
                Format.B5G5R5A1UNorm => 5,
                Format.B4G4R4A4UNorm => 4,
                Format.R1UNorm => 1,
                _ => 0,
            };
        }

        /// <summary>
        /// Computes the image row pitch in bytes, and the slice ptich (size in bytes of the image)
        /// based on DXGI format, width, and height
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rowPitch"></param>
        /// <param name="slicePitch"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool ComputePitch(Format fmt, ulong width, ulong height, ref ulong rowPitch, ref ulong slicePitch, CPFlags flags)
        {
            ulong pitch;
            ulong slice;
            switch (fmt)
            {
                case Format.BC1Typeless:
                case Format.BC1UNorm:
                case Format.BC1UNormSRGB:
                case Format.BC4Typeless:
                case Format.BC4UNorm:
                case Format.BC4SNorm:
                    Trace.Assert(IsCompressed(fmt));
                    {
                        if ((flags & CPFlags.BadDXTNTails) != 0)
                        {
                            ulong nbw = width >> 2;
                            ulong nbh = height >> 2;
                            pitch = Math.Max(1u, (nbw) * 8u);
                            slice = Math.Max(1u, pitch * (nbh));
                        }
                        else
                        {
                            ulong nbw = Math.Max(1u, ((width) + 3u) / 4u);
                            ulong nbh = Math.Max(1u, ((height) + 3u) / 4u);
                            pitch = nbw * 8u;
                            slice = pitch * nbh;
                        }
                    }
                    break;

                case Format.BC2Typeless:
                case Format.BC2UNorm:
                case Format.BC2UNormSRGB:
                case Format.BC3Typeless:
                case Format.BC3UNorm:
                case Format.BC3UNormSRGB:
                case Format.BC5Typeless:
                case Format.BC5UNorm:
                case Format.BC5SNorm:
                case Format.BC6HTypeless:
                case Format.BC6HUF16:
                case Format.BC6HSF16:
                case Format.BC7Typeless:
                case Format.BC7UNorm:
                case Format.BC7UNormSRGB:
                    Debug.Assert(IsCompressed(fmt));
                    {
                        if ((flags & CPFlags.BadDXTNTails) != 0)
                        {
                            ulong nbw = width >> 2;
                            ulong nbh = height >> 2;
                            pitch = Math.Max(1u, (nbw) * 16u);
                            slice = Math.Max(1u, pitch * (nbh));
                        }
                        else
                        {
                            ulong nbw = Math.Max(1u, ((width) + 3u) / 4u);
                            ulong nbh = Math.Max(1u, ((height) + 3u) / 4u);
                            pitch = nbw * 16u;
                            slice = pitch * nbh;
                        }
                    }
                    break;

                case Format.R8G8B8G8UNorm:
                case Format.G8R8G8B8UNorm:
                case Format.YUY2:
                    Debug.Assert(IsPacked(fmt));
                    pitch = (((width) + 1u) >> 1) * 4u;
                    slice = pitch * (height);
                    break;

                case Format.Y210:
                case Format.Y216:
                    Debug.Assert(IsPacked(fmt));
                    pitch = (((width) + 1u) >> 1) * 8u;
                    slice = pitch * (height);
                    break;

                case Format.NV12:
                case Format.Opaque420:
                    if (height % 2 != 0)
                    {
                        // Requires a height alignment of 2.
                        return false;
                    }
                    Debug.Assert(IsPlanar(fmt));
                    pitch = (((width) + 1u) >> 1) * 2u;
                    slice = pitch * ((height) + (((height) + 1u) >> 1));
                    break;

                case Format.P010:
                case Format.P016:
                    if (height % 2 != 0)
                    {
                        // Requires a height alignment of 2.
                        return false;
                    }

                    goto case Format.NV11;

                case Format.NV11:
                    Debug.Assert(IsPlanar(fmt));
                    pitch = (((width) + 3u) >> 2) * 4u;
                    slice = pitch * (height) * 2u;
                    break;

                default:
                    Debug.Assert(!IsCompressed(fmt) && !IsPacked(fmt) && !IsPlanar(fmt));
                    {
                        ulong bpp;

                        if ((flags & CPFlags.BPP24) != 0)
                        {
                            bpp = 24;
                        }
                        else if ((flags & CPFlags.BPP16) != 0)
                        {
                            bpp = 16;
                        }
                        else if ((flags & CPFlags.BPP8) != 0)
                        {
                            bpp = 8;
                        }
                        else
                        {
                            bpp = (ulong)BitsPerPixel(fmt);
                        }

                        if (bpp == 0)
                        {
                            return false;
                        }

                        if ((flags & (CPFlags.LegacyDWORD | CPFlags.Paragraph | CPFlags.YMM | CPFlags.ZMM | CPFlags.Page4K)) != 0)
                        {
                            if ((flags & CPFlags.Page4K) != 0)
                            {
                                pitch = ((width) * bpp + 32767u) / 32768u * 4096u;
                                slice = pitch * (height);
                            }
                            else if ((flags & CPFlags.ZMM) != 0)
                            {
                                pitch = ((width) * bpp + 511u) / 512u * 64u;
                                slice = pitch * (height);
                            }
                            else if ((flags & CPFlags.YMM) != 0)
                            {
                                pitch = ((width) * bpp + 255u) / 256u * 32u;
                                slice = pitch * (height);
                            }
                            else if ((flags & CPFlags.Paragraph) != 0)
                            {
                                pitch = ((width) * bpp + 127u) / 128u * 16u;
                                slice = pitch * (height);
                            }
                            else // DWORD alignment
                            {
                                // Special computation for some incorrectly created DDS files based on
                                // legacy DirectDraw assumptions about pitch alignment
                                pitch = ((width) * bpp + 31u) / 32u * sizeof(uint);
                                slice = pitch * (height);
                            }
                        }
                        else
                        {
                            // Default byte alignment
                            pitch = ((width) * bpp + 7u) / 8u;
                            slice = pitch * (height);
                        }
                    }
                    break;
            }

            rowPitch = pitch;
            slicePitch = slice;

            return true;
        }

        public static ulong ComputeScanlines(Format fmt, ulong height)
        {
            switch (fmt)
            {
                case Format.BC1Typeless:
                case Format.BC1UNorm:
                case Format.BC1UNormSRGB:
                case Format.BC2Typeless:
                case Format.BC2UNorm:
                case Format.BC2UNormSRGB:
                case Format.BC3Typeless:
                case Format.BC3UNorm:
                case Format.BC3UNormSRGB:
                case Format.BC4Typeless:
                case Format.BC4UNorm:
                case Format.BC4SNorm:
                case Format.BC5Typeless:
                case Format.BC5UNorm:
                case Format.BC5SNorm:
                case Format.BC6HTypeless:
                case Format.BC6HUF16:
                case Format.BC6HSF16:
                case Format.BC7Typeless:
                case Format.BC7UNorm:
                case Format.BC7UNormSRGB:
                    Debug.Assert(IsCompressed(fmt));
                    return Math.Max(1, (height + 3) / 4);

                case Format.NV11:

                    Debug.Assert(IsPlanar(fmt));
                    return height * 2;

                case Format.NV12:
                case Format.P010:
                case Format.P016:
                case Format.Opaque420:
                    Debug.Assert(IsPlanar(fmt));
                    return height + ((height + 1) >> 1);

                default:
                    Debug.Assert(IsValid(fmt));
                    Debug.Assert(!IsCompressed(fmt) && !IsPlanar(fmt));
                    return height;
            }
        }

        /// <summary>
        /// Converts to an SRGB equivalent type if available
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static Format MakeSRGB(Format fmt)
        {
            return fmt switch
            {
                Format.R8G8B8A8UNorm => Format.R8G8B8A8UNormSRGB,
                Format.BC1UNorm => Format.BC1UNormSRGB,
                Format.BC2UNorm => Format.BC2UNormSRGB,
                Format.BC3UNorm => Format.BC3UNormSRGB,
                Format.B8G8R8A8UNorm => Format.B8G8R8A8UNormSRGB,
                Format.B8G8R8X8UNorm => Format.B8G8R8X8UNormSRGB,
                Format.BC7UNorm => Format.BC7UNormSRGB,
                _ => fmt,
            };
        }

        /// <summary>
        /// Converts to an non-SRBG equivalent type
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static Format MakeLinear(Format fmt)
        {
            return fmt switch
            {
                Format.R8G8B8A8UNormSRGB => Format.R8G8B8A8UNorm,
                Format.BC1UNormSRGB => Format.BC1UNorm,
                Format.BC2UNormSRGB => Format.BC2UNorm,
                Format.BC3UNormSRGB => Format.BC3UNorm,
                Format.B8G8R8A8UNormSRGB => Format.B8G8R8A8UNorm,
                Format.B8G8R8X8UNormSRGB => Format.B8G8R8X8UNorm,
                Format.BC7UNormSRGB => Format.BC7UNorm,
                _ => fmt,
            };
        }

        /// <summary>
        /// Converts to a format to an equivalent Typeless format if available
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static Format MakeTypeless(Format fmt)
        {
            return fmt switch
            {
                Format.R32G32B32A32Float or Format.R32G32B32A32UInt or Format.R32G32B32A32SInt => Format.R32G32B32A32Typeless,
                Format.R32G32B32Float or Format.R32G32B32UInt or Format.R32G32B32SInt => Format.R32G32B32Typeless,
                Format.R16G16B16A16Float or Format.R16G16B16A16UNorm or Format.R16G16B16A16UInt or Format.R16G16B16A16SNorm or Format.R16G16B16A16Sint => Format.R16G16B16A16Typeless,
                Format.R32G32Float or Format.R32G32UInt or Format.R32G32SInt => Format.R32G32Typeless,
                Format.R10G10B10A2UNorm or Format.R10G10B10A2UInt => Format.R10G10B10A2Typeless,
                Format.R8G8B8A8UNorm or Format.R8G8B8A8UNormSRGB or Format.R8G8B8A8UInt or Format.R8G8B8A8SNorm or Format.R8G8B8A8SInt => Format.R8G8B8A8Typeless,
                Format.R16G16Float or Format.R16G16UNorm or Format.R16G16UInt or Format.R16G16SNorm or Format.R16G16Sint => Format.R16G16Typeless,
                Format.D32Float or Format.R32Float or Format.R32UInt or Format.R32SInt => Format.R32Typeless,
                Format.R8G8UNorm or Format.R8G8UInt or Format.R8G8SNorm or Format.R8G8Sint => Format.R8G8Typeless,
                Format.R16Float or Format.D16UNorm or Format.R16UNorm or Format.R16UInt or Format.R16SNorm or Format.R16Sint => Format.R16Typeless,
                Format.R8UNorm or Format.R8UInt or Format.R8SNorm or Format.R8SInt => Format.R8Typeless,
                Format.BC1UNorm or Format.BC1UNormSRGB => Format.BC1Typeless,
                Format.BC2UNorm or Format.BC2UNormSRGB => Format.BC2Typeless,
                Format.BC3UNorm or Format.BC3UNormSRGB => Format.BC3Typeless,
                Format.BC4UNorm or Format.BC4SNorm => Format.BC4Typeless,
                Format.BC5UNorm or Format.BC5SNorm => Format.BC5Typeless,
                Format.B8G8R8A8UNorm or Format.B8G8R8A8UNormSRGB => Format.B8G8R8A8Typeless,
                Format.B8G8R8X8UNorm or Format.B8G8R8X8UNormSRGB => Format.B8G8R8X8Typeless,
                Format.BC6HUF16 or Format.BC6HSF16 => Format.BC6HTypeless,
                Format.BC7UNorm or Format.BC7UNormSRGB => Format.BC7Typeless,
                _ => fmt,
            };
        }

        /// <summary>
        /// Converts to a Typeless format to an equivalent UNorm format if available
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static Format MakeTypelessUNorm(Format fmt)
        {
            return fmt switch
            {
                Format.R16G16B16A16Typeless => Format.R16G16B16A16UNorm,
                Format.R10G10B10A2Typeless => Format.R10G10B10A2UNorm,
                Format.R8G8B8A8Typeless => Format.R8G8B8A8UNorm,
                Format.R16G16Typeless => Format.R16G16UNorm,
                Format.R8G8Typeless => Format.R8G8UNorm,
                Format.R16Typeless => Format.R16UNorm,
                Format.R8Typeless => Format.R8UNorm,
                Format.BC1Typeless => Format.BC1UNorm,
                Format.BC2Typeless => Format.BC2UNorm,
                Format.BC3Typeless => Format.BC3UNorm,
                Format.BC4Typeless => Format.BC4UNorm,
                Format.BC5Typeless => Format.BC5UNorm,
                Format.B8G8R8A8Typeless => Format.B8G8R8A8UNorm,
                Format.B8G8R8X8Typeless => Format.B8G8R8X8UNorm,
                Format.BC7Typeless => Format.BC7UNorm,
                _ => fmt,
            };
        }

        /// <summary>
        /// Converts to a Typeless format to an equivalent Float format if available
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static Format MakeTypelessFloat(Format fmt)
        {
            return fmt switch
            {
                Format.R32G32B32A32Typeless => Format.R32G32B32A32Float,
                Format.R32G32B32Typeless => Format.R32G32B32Float,
                Format.R16G16B16A16Typeless => Format.R16G16B16A16Float,
                Format.R32G32Typeless => Format.R32G32Float,
                Format.R16G16Typeless => Format.R16G16Float,
                Format.R32Typeless => Format.R32Float,
                Format.R16Typeless => Format.R16Float,
                _ => fmt,
            };
        }
    }
}