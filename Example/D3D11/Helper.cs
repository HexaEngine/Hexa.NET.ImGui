namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using HexaEngine.Core.Graphics.Reflection;
    using HexaEngine.Core.Graphics.Textures;
    using HexaEngine.Mathematics;
    using Silk.NET.Core.Native;
    using System.Numerics;
    using System.Runtime.InteropServices;

    public static unsafe class Helper
    {
        public static DirectXTex.CPFlags Convert(CPFlags flags)
        {
            DirectXTex.CPFlags result = 0;
            if ((flags & CPFlags.None) != 0)
            {
                result |= DirectXTex.CPFlags.None;
            }

            if ((flags & CPFlags.LegacyDWORD) != 0)
            {
                result |= DirectXTex.CPFlags.LegacyDword;
            }

            if ((flags & CPFlags.Paragraph) != 0)
            {
                result |= DirectXTex.CPFlags.Paragraph;
            }

            if ((flags & CPFlags.YMM) != 0)
            {
                result |= DirectXTex.CPFlags.Ymm;
            }

            if ((flags & CPFlags.ZMM) != 0)
            {
                result |= DirectXTex.CPFlags.Zmm;
            }

            if ((flags & CPFlags.Page4K) != 0)
            {
                result |= DirectXTex.CPFlags.Page4K;
            }

            if ((flags & CPFlags.BadDXTNTails) != 0)
            {
                result |= DirectXTex.CPFlags.BadDxtnTails;
            }

            if ((flags & CPFlags.BPP24) != 0)
            {
                result |= DirectXTex.CPFlags.Flags24Bpp;
            }

            if ((flags & CPFlags.BPP16) != 0)
            {
                result |= DirectXTex.CPFlags.Flags16Bpp;
            }

            if ((flags & CPFlags.BPP8) != 0)
            {
                result |= DirectXTex.CPFlags.Flags8Bpp;
            }

            return result;
        }

        public static DirectXTex.WICCodecs Convert(TexFileFormat format)
        {
            return format switch
            {
                TexFileFormat.DDS => throw new NotSupportedException(),
                TexFileFormat.TGA => throw new NotSupportedException(),
                TexFileFormat.HDR => throw new NotSupportedException(),
                TexFileFormat.BMP => DirectXTex.WICCodecs.WicCodecBmp,
                TexFileFormat.JPEG => DirectXTex.WICCodecs.WicCodecJpeg,
                TexFileFormat.PNG => DirectXTex.WICCodecs.WicCodecPng,
                TexFileFormat.TIFF => DirectXTex.WICCodecs.WicCodecTiff,
                TexFileFormat.GIF => DirectXTex.WICCodecs.WicCodecGif,
                TexFileFormat.WMP => DirectXTex.WICCodecs.WicCodecWmp,
                TexFileFormat.ICO => DirectXTex.WICCodecs.WicCodecIco,
                _ => throw new NotSupportedException(),
            };
        }

        public static DirectXTex.TexFilterFlags Convert(TexFilterFlags flags)
        {
            DirectXTex.TexFilterFlags result = 0;
            if ((flags & TexFilterFlags.Default) != 0)
            {
                result |= DirectXTex.TexFilterFlags.Default;
            }

            if ((flags & TexFilterFlags.WrapU) != 0)
            {
                result |= DirectXTex.TexFilterFlags.WrapU;
            }

            if ((flags & TexFilterFlags.WrapV) != 0)
            {
                result |= DirectXTex.TexFilterFlags.WrapV;
            }

            if ((flags & TexFilterFlags.WrapW) != 0)
            {
                result |= DirectXTex.TexFilterFlags.WrapW;
            }

            if ((flags & TexFilterFlags.Wrap) != 0)
            {
                result |= DirectXTex.TexFilterFlags.Wrap;
            }

            if ((flags & TexFilterFlags.MirrorU) != 0)
            {
                result |= DirectXTex.TexFilterFlags.MirrorU;
            }

            if ((flags & TexFilterFlags.MirrorV) != 0)
            {
                result |= DirectXTex.TexFilterFlags.MirrorV;
            }

            if ((flags & TexFilterFlags.MirrorW) != 0)
            {
                result |= DirectXTex.TexFilterFlags.MirrorW;
            }

            if ((flags & TexFilterFlags.Mirror) != 0)
            {
                result |= DirectXTex.TexFilterFlags.Mirror;
            }

            if ((flags & TexFilterFlags.SeparateAlpha) != 0)
            {
                result |= DirectXTex.TexFilterFlags.SeparateAlpha;
            }

            if ((flags & TexFilterFlags.FloatX2Bias) != 0)
            {
                result |= DirectXTex.TexFilterFlags.FloatX2Bias;
            }

            if ((flags & TexFilterFlags.RGBCopyRed) != 0)
            {
                result |= DirectXTex.TexFilterFlags.RgbCopyRed;
            }

            if ((flags & TexFilterFlags.RGBCopyGreen) != 0)
            {
                result |= DirectXTex.TexFilterFlags.RgbCopyGreen;
            }

            if ((flags & TexFilterFlags.RGBCopyBlue) != 0)
            {
                result |= DirectXTex.TexFilterFlags.RgbCopyBlue;
            }

            if ((flags & TexFilterFlags.Dither) != 0)
            {
                result |= DirectXTex.TexFilterFlags.Dither;
            }

            if ((flags & TexFilterFlags.DitherDiffusion) != 0)
            {
                result |= DirectXTex.TexFilterFlags.DitherDiffusion;
            }

            if ((flags & TexFilterFlags.Point) != 0)
            {
                result |= DirectXTex.TexFilterFlags.Point;
            }

            if ((flags & TexFilterFlags.Linear) != 0)
            {
                result |= DirectXTex.TexFilterFlags.Linear;
            }

            if ((flags & TexFilterFlags.Cubic) != 0)
            {
                result |= DirectXTex.TexFilterFlags.Cubic;
            }

            if ((flags & TexFilterFlags.Box) != 0)
            {
                result |= DirectXTex.TexFilterFlags.Box;
            }

            if ((flags & TexFilterFlags.Triangle) != 0)
            {
                result |= DirectXTex.TexFilterFlags.Triangle;
            }

            if ((flags & TexFilterFlags.SRGBIn) != 0)
            {
                result |= DirectXTex.TexFilterFlags.SrgbIn;
            }

            if ((flags & TexFilterFlags.SRGBOut) != 0)
            {
                result |= DirectXTex.TexFilterFlags.SrgbOut;
            }

            if ((flags & TexFilterFlags.SRGB) != 0)
            {
                result |= DirectXTex.TexFilterFlags.Srgb;
            }

            if ((flags & TexFilterFlags.ForceNonWIC) != 0)
            {
                result |= DirectXTex.TexFilterFlags.ForceNonWic;
            }

            if ((flags & TexFilterFlags.ForceWIC) != 0)
            {
                result |= DirectXTex.TexFilterFlags.ForceWic;
            }

            return result;
        }

        public static DirectXTex.TexCompressFlags Convert(TexCompressFlags flags)
        {
            DirectXTex.TexCompressFlags result = 0;
            if ((flags & TexCompressFlags.Default) != 0)
            {
                result |= DirectXTex.TexCompressFlags.Default;
            }

            if ((flags & TexCompressFlags.DitherRGB) != 0)
            {
                result |= DirectXTex.TexCompressFlags.RgbDither;
            }

            if ((flags & TexCompressFlags.DitherA) != 0)
            {
                result |= DirectXTex.TexCompressFlags.ADither;
            }

            if ((flags & TexCompressFlags.Dither) != 0)
            {
                result |= DirectXTex.TexCompressFlags.Dither;
            }

            if ((flags & TexCompressFlags.Uniform) != 0)
            {
                result |= DirectXTex.TexCompressFlags.Uniform;
            }

            if ((flags & TexCompressFlags.BC7Use3Sunsets) != 0)
            {
                result |= DirectXTex.TexCompressFlags.Bc7Use3Subsets;
            }

            if ((flags & TexCompressFlags.BC7Quick) != 0)
            {
                result |= DirectXTex.TexCompressFlags.Bc7Quick;
            }

            if ((flags & TexCompressFlags.SRGBIn) != 0)
            {
                result |= DirectXTex.TexCompressFlags.SrgbIn;
            }

            if ((flags & TexCompressFlags.SRGBOut) != 0)
            {
                result |= DirectXTex.TexCompressFlags.SrgbOut;
            }

            if ((flags & TexCompressFlags.Parallel) != 0)
            {
                result |= DirectXTex.TexCompressFlags.Parallel;
            }

            return result;
        }

        public static Silk.NET.Direct3D11.Map Convert(Map map)
        {
            return map switch
            {
                Map.Read => Silk.NET.Direct3D11.Map.Read,
                Map.Write => Silk.NET.Direct3D11.Map.Write,
                Map.ReadWrite => Silk.NET.Direct3D11.Map.ReadWrite,
                Map.WriteDiscard => Silk.NET.Direct3D11.Map.WriteDiscard,
                Map.WriteNoOverwrite => Silk.NET.Direct3D11.Map.WriteNoOverwrite,
                _ => throw new NotImplementedException(),
            };
        }

        public static Silk.NET.Direct3D11.Viewport Convert(Viewport viewport)
        {
            return new()
            {
                Height = viewport.Height,
                MaxDepth = viewport.MaxDepth,
                MinDepth = viewport.MinDepth,
                TopLeftX = viewport.X,
                TopLeftY = viewport.Y,
                Width = viewport.Width
            };
        }

        private static float ClampAndRound(float value, float min, float max)
        {
            if (float.IsNaN(value))
            {
                return 0.0f;
            }

            if (float.IsInfinity(value))
            {
                return float.IsNegativeInfinity(value) ? min : max;
            }

            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return MathF.Round(value);
        }

        public static uint PackUNorm(float bitmask, float value)
        {
            value *= bitmask;
            return (uint)ClampAndRound(value, 0.0f, bitmask);
        }

        public static uint PackRGBA(float x, float y, float z, float w)
        {
            uint red = PackUNorm(255.0f, x);
            uint green = PackUNorm(255.0f, y) << 8;
            uint blue = PackUNorm(255.0f, z) << 16;
            uint alpha = PackUNorm(255.0f, w) << 24;
            return red | green | blue | alpha;
        }

        public static uint PackRGBA(Vector4 color) => PackRGBA(color.X, color.Y, color.Z, color.W);

        public static Silk.NET.Core.Native.D3DPrimitiveTopology Convert(PrimitiveTopology topology)
        {
            return topology switch
            {
                PrimitiveTopology.Undefined => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopologyUndefined,
                PrimitiveTopology.PointList => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopologyPointlist,
                PrimitiveTopology.LineList => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopologyLinelist,
                PrimitiveTopology.LineStrip => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopologyLinestrip,
                PrimitiveTopology.TriangleList => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopologyTrianglelist,
                PrimitiveTopology.TriangleStrip => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopologyTrianglestrip,
                PrimitiveTopology.LineListAdjacency => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopologyLinelistAdj,
                PrimitiveTopology.LineStripAdjacency => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopologyLinestripAdj,
                PrimitiveTopology.TriangleListAdjacency => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopologyTrianglelistAdj,
                PrimitiveTopology.TriangleStripAdjacency => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopologyTrianglestripAdj,
                PrimitiveTopology.PatchListWith1ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology1ControlPointPatchlist,
                PrimitiveTopology.PatchListWith2ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology2ControlPointPatchlist,
                PrimitiveTopology.PatchListWith3ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology3ControlPointPatchlist,
                PrimitiveTopology.PatchListWith4ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology4ControlPointPatchlist,
                PrimitiveTopology.PatchListWith5ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology5ControlPointPatchlist,
                PrimitiveTopology.PatchListWith6ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology6ControlPointPatchlist,
                PrimitiveTopology.PatchListWith7ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology7ControlPointPatchlist,
                PrimitiveTopology.PatchListWith8ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology8ControlPointPatchlist,
                PrimitiveTopology.PatchListWith9ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology9ControlPointPatchlist,
                PrimitiveTopology.PatchListWith10ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology10ControlPointPatchlist,
                PrimitiveTopology.PatchListWith11ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology11ControlPointPatchlist,
                PrimitiveTopology.PatchListWith12ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology12ControlPointPatchlist,
                PrimitiveTopology.PatchListWith13ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology13ControlPointPatchlist,
                PrimitiveTopology.PatchListWith14ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology14ControlPointPatchlist,
                PrimitiveTopology.PatchListWith15ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology15ControlPointPatchlist,
                PrimitiveTopology.PatchListWith16ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology16ControlPointPatchlist,
                PrimitiveTopology.PatchListWith17ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology17ControlPointPatchlist,
                PrimitiveTopology.PatchListWith18ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology18ControlPointPatchlist,
                PrimitiveTopology.PatchListWith19ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology19ControlPointPatchlist,
                PrimitiveTopology.PatchListWith20ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology20ControlPointPatchlist,
                PrimitiveTopology.PatchListWith21ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology21ControlPointPatchlist,
                PrimitiveTopology.PatchListWith22ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology22ControlPointPatchlist,
                PrimitiveTopology.PatchListWith23ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology23ControlPointPatchlist,
                PrimitiveTopology.PatchListWith24ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology24ControlPointPatchlist,
                PrimitiveTopology.PatchListWith25ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology25ControlPointPatchlist,
                PrimitiveTopology.PatchListWith26ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology26ControlPointPatchlist,
                PrimitiveTopology.PatchListWith27ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology27ControlPointPatchlist,
                PrimitiveTopology.PatchListWith28ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology28ControlPointPatchlist,
                PrimitiveTopology.PatchListWith29ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology29ControlPointPatchlist,
                PrimitiveTopology.PatchListWith30ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology30ControlPointPatchlist,
                PrimitiveTopology.PatchListWith31ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology31ControlPointPatchlist,
                PrimitiveTopology.PatchListWith32ControlPoints => Silk.NET.Core.Native.D3DPrimitiveTopology.D3DPrimitiveTopology32ControlPointPatchlist,
                _ => throw new ArgumentOutOfRangeException(nameof(topology)),
            };
        }

        public static Silk.NET.Direct3D11.MapFlag Convert(MapFlags flags)
        {
            return flags switch
            {
                MapFlags.DoNotWait => Silk.NET.Direct3D11.MapFlag.DONotWait,
                MapFlags.None => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(flags)),
            };
        }

        public static Silk.NET.Direct3D11.Map Convert(MapMode mode)
        {
            return mode switch
            {
                MapMode.Read => Silk.NET.Direct3D11.Map.Read,
                MapMode.Write => Silk.NET.Direct3D11.Map.Write,
                MapMode.ReadWrite => Silk.NET.Direct3D11.Map.ReadWrite,
                MapMode.WriteDiscard => Silk.NET.Direct3D11.Map.WriteDiscard,
                MapMode.WriteNoOverwrite => Silk.NET.Direct3D11.Map.WriteNoOverwrite,
                _ => throw new ArgumentOutOfRangeException(nameof(mode)),
            };
        }

        public static Silk.NET.Direct3D11.ClearFlag Convert(DepthStencilClearFlags flags)
        {
            return flags switch
            {
                DepthStencilClearFlags.None => 0,
                DepthStencilClearFlags.Depth => Silk.NET.Direct3D11.ClearFlag.Depth,
                DepthStencilClearFlags.Stencil => Silk.NET.Direct3D11.ClearFlag.Stencil,
                DepthStencilClearFlags.All => Silk.NET.Direct3D11.ClearFlag.Depth | Silk.NET.Direct3D11.ClearFlag.Stencil,
                _ => throw new ArgumentOutOfRangeException(nameof(flags)),
            };
        }

        public static Texture1DDescription ConvertBack(Silk.NET.Direct3D11.Texture1DDesc desc)
        {
            return new()
            {
                Format = ConvertBack(desc.Format),
                ArraySize = (int)desc.ArraySize,
                BindFlags = ConvertBack((Silk.NET.Direct3D11.BindFlag)desc.BindFlags),
                CPUAccessFlags = ConvertBack((Silk.NET.Direct3D11.CpuAccessFlag)desc.CPUAccessFlags),
                MipLevels = (int)desc.MipLevels,
                MiscFlags = ConvertBack((Silk.NET.Direct3D11.ResourceMiscFlag)desc.MiscFlags),
                Usage = ConvertBack(desc.Usage),
                Width = (int)desc.Width
            };
        }

        public static Texture2DDescription ConvertBack(Silk.NET.Direct3D11.Texture2DDesc desc)
        {
            return new()
            {
                Format = ConvertBack(desc.Format),
                ArraySize = (int)desc.ArraySize,
                BindFlags = ConvertBack((Silk.NET.Direct3D11.BindFlag)desc.BindFlags),
                CPUAccessFlags = ConvertBack((Silk.NET.Direct3D11.CpuAccessFlag)desc.CPUAccessFlags),
                Height = (int)desc.Height,
                MipLevels = (int)desc.MipLevels,
                MiscFlags = ConvertBack((Silk.NET.Direct3D11.ResourceMiscFlag)desc.MiscFlags),
                SampleDescription = ConvertBack(desc.SampleDesc),
                Usage = ConvertBack(desc.Usage),
                Width = (int)desc.Width
            };
        }

        public static Texture3DDescription ConvertBack(Silk.NET.Direct3D11.Texture3DDesc desc)
        {
            return new()
            {
                Format = ConvertBack(desc.Format),
                Depth = (int)desc.Depth,
                BindFlags = ConvertBack((Silk.NET.Direct3D11.BindFlag)desc.BindFlags),
                CPUAccessFlags = ConvertBack((Silk.NET.Direct3D11.CpuAccessFlag)desc.CPUAccessFlags),
                Height = (int)desc.Height,
                MipLevels = (int)desc.MipLevels,
                MiscFlags = ConvertBack((Silk.NET.Direct3D11.ResourceMiscFlag)desc.MiscFlags),
                Usage = ConvertBack(desc.Usage),
                Width = (int)desc.Width
            };
        }

        public static CpuAccessFlags ConvertBack(Silk.NET.Direct3D11.CpuAccessFlag flags)
        {
            if (flags == Silk.NET.Direct3D11.CpuAccessFlag.Write)
            {
                return CpuAccessFlags.Write;
            }

            if (flags == Silk.NET.Direct3D11.CpuAccessFlag.Read)
            {
                return CpuAccessFlags.Read;
            }

            return CpuAccessFlags.None;
        }

        private static ResourceMiscFlag ConvertBack(Silk.NET.Direct3D11.ResourceMiscFlag flags)
        {
            ResourceMiscFlag result = 0;
            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.GenerateMips) != 0)
            {
                result |= ResourceMiscFlag.GenerateMips;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.Shared) != 0)
            {
                result |= ResourceMiscFlag.Shared;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.Texturecube) != 0)
            {
                result |= ResourceMiscFlag.TextureCube;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.DrawindirectArgs) != 0)
            {
                result |= ResourceMiscFlag.DrawIndirectArguments;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.BufferAllowRawViews) != 0)
            {
                result |= ResourceMiscFlag.BufferAllowRawViews;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.BufferStructured) != 0)
            {
                result |= ResourceMiscFlag.BufferStructured;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.ResourceClamp) != 0)
            {
                result |= ResourceMiscFlag.ResourceClamp;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.SharedKeyedmutex) != 0)
            {
                result |= ResourceMiscFlag.SharedKeyedMutex;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.GdiCompatible) != 0)
            {
                result |= ResourceMiscFlag.GdiCompatible;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.SharedNthandle) != 0)
            {
                result |= ResourceMiscFlag.SharedNTHandle;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.RestrictedContent) != 0)
            {
                result |= ResourceMiscFlag.RestrictedContent;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.RestrictSharedResource) != 0)
            {
                result |= ResourceMiscFlag.RestrictSharedResource;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.RestrictSharedResourceDriver) != 0)
            {
                result |= ResourceMiscFlag.RestrictSharedResourceDriver;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.Guarded) != 0)
            {
                result |= ResourceMiscFlag.Guarded;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.TilePool) != 0)
            {
                result |= ResourceMiscFlag.TilePool;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.Tiled) != 0)
            {
                result |= ResourceMiscFlag.Tiled;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.HWProtected) != 0)
            {
                result |= ResourceMiscFlag.HardwareProtected;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.SharedDisplayable) != 0)
            {
                result |= ResourceMiscFlag.SharedDisplayable;
            }

            if ((flags & Silk.NET.Direct3D11.ResourceMiscFlag.SharedExclusiveWriter) != 0)
            {
                result |= ResourceMiscFlag.SharedExclusiveWriter;
            }

            return result;
        }

        private static SampleDescription ConvertBack(Silk.NET.DXGI.SampleDesc sampleDesc)
        {
            return new()
            {
                Count = (int)sampleDesc.Count,
                Quality = (int)sampleDesc.Quality,
            };
        }

        private static Usage ConvertBack(Silk.NET.Direct3D11.Usage usage)
        {
            return usage switch
            {
                Silk.NET.Direct3D11.Usage.Default => Usage.Default,
                Silk.NET.Direct3D11.Usage.Immutable => Usage.Immutable,
                Silk.NET.Direct3D11.Usage.Dynamic => Usage.Dynamic,
                Silk.NET.Direct3D11.Usage.Staging => Usage.Staging,
                _ => throw new ArgumentOutOfRangeException(nameof(usage)),
            };
        }

        private static BindFlags ConvertBack(Silk.NET.Direct3D11.BindFlag flags)
        {
            BindFlags result = 0;
            if ((flags & Silk.NET.Direct3D11.BindFlag.VertexBuffer) != 0)
            {
                result |= BindFlags.VertexBuffer;
            }

            if ((flags & Silk.NET.Direct3D11.BindFlag.IndexBuffer) != 0)
            {
                result |= BindFlags.IndexBuffer;
            }

            if ((flags & Silk.NET.Direct3D11.BindFlag.ConstantBuffer) != 0)
            {
                result |= BindFlags.ConstantBuffer;
            }

            if ((flags & Silk.NET.Direct3D11.BindFlag.ShaderResource) != 0)
            {
                result |= BindFlags.ShaderResource;
            }

            if ((flags & Silk.NET.Direct3D11.BindFlag.StreamOutput) != 0)
            {
                result |= BindFlags.StreamOutput;
            }

            if ((flags & Silk.NET.Direct3D11.BindFlag.RenderTarget) != 0)
            {
                result |= BindFlags.RenderTarget;
            }

            if ((flags & Silk.NET.Direct3D11.BindFlag.UnorderedAccess) != 0)
            {
                result |= BindFlags.UnorderedAccess;
            }

            if ((flags & Silk.NET.Direct3D11.BindFlag.DepthStencil) != 0)
            {
                result |= BindFlags.DepthStencil;
            }

            if ((flags & Silk.NET.Direct3D11.BindFlag.Decoder) != 0)
            {
                result |= BindFlags.Decoder;
            }

            if ((flags & Silk.NET.Direct3D11.BindFlag.VideoEncoder) != 0)
            {
                result |= BindFlags.VideoEncoder;
            }

            return result;
        }

        public static Silk.NET.Direct3D11.SamplerDesc Convert(SamplerDescription description)
        {
            Silk.NET.Direct3D11.SamplerDesc result = new()
            {
                AddressU = Convert(description.AddressU),
                AddressV = Convert(description.AddressV),
                AddressW = Convert(description.AddressW),
                ComparisonFunc = Convert(description.ComparisonFunction),
                Filter = Convert(description.Filter),
                MaxAnisotropy = (uint)description.MaxAnisotropy,
                MaxLOD = description.MaxLOD,
                MinLOD = description.MinLOD,
                MipLODBias = description.MipLODBias
            };

            return result;
        }

        public static Silk.NET.Direct3D11.Filter Convert(Filter filter)
        {
            return filter switch
            {
                Filter.MinMagMipPoint => Silk.NET.Direct3D11.Filter.MinMagMipPoint,
                Filter.MinMagPointMipLinear => Silk.NET.Direct3D11.Filter.MinMagPointMipLinear,
                Filter.MinPointMagLinearMipPoint => Silk.NET.Direct3D11.Filter.MinPointMagLinearMipPoint,
                Filter.MinPointMagMipLinear => Silk.NET.Direct3D11.Filter.MinPointMagMipLinear,
                Filter.MinLinearMagMipPoint => Silk.NET.Direct3D11.Filter.MinLinearMagMipPoint,
                Filter.MinLinearMagPointMipLinear => Silk.NET.Direct3D11.Filter.MinLinearMagPointMipLinear,
                Filter.MinMagLinearMipPoint => Silk.NET.Direct3D11.Filter.MinMagLinearMipPoint,
                Filter.MinMagMipLinear => Silk.NET.Direct3D11.Filter.MinMagMipLinear,
                Filter.Anisotropic => Silk.NET.Direct3D11.Filter.Anisotropic,
                Filter.ComparisonMinMagMipPoint => Silk.NET.Direct3D11.Filter.ComparisonMinMagMipPoint,
                Filter.ComparisonMinMagPointMipLinear => Silk.NET.Direct3D11.Filter.ComparisonMinMagPointMipLinear,
                Filter.ComparisonMinPointMagLinearMipPoint => Silk.NET.Direct3D11.Filter.ComparisonMinPointMagLinearMipPoint,
                Filter.ComparisonMinPointMagMipLinear => Silk.NET.Direct3D11.Filter.ComparisonMinPointMagMipLinear,
                Filter.ComparisonMinLinearMagMipPoint => Silk.NET.Direct3D11.Filter.ComparisonMinLinearMagMipPoint,
                Filter.ComparisonMinLinearMagPointMipLinear => Silk.NET.Direct3D11.Filter.ComparisonMinLinearMagPointMipLinear,
                Filter.ComparisonMinMagLinearMipPoint => Silk.NET.Direct3D11.Filter.ComparisonMinMagLinearMipPoint,
                Filter.ComparisonMinMagMipLinear => Silk.NET.Direct3D11.Filter.ComparisonMinMagMipLinear,
                Filter.ComparisonAnisotropic => Silk.NET.Direct3D11.Filter.ComparisonAnisotropic,
                Filter.MinimumMinMagMipPoint => Silk.NET.Direct3D11.Filter.MinimumMinMagMipPoint,
                Filter.MinimumMinMagPointMipLinear => Silk.NET.Direct3D11.Filter.MinimumMinMagPointMipLinear,
                Filter.MinimumMinPointMagLinearMipPoint => Silk.NET.Direct3D11.Filter.MinimumMinPointMagLinearMipPoint,
                Filter.MinimumMinPointMagMipLinear => Silk.NET.Direct3D11.Filter.MinimumMinPointMagMipLinear,
                Filter.MinimumMinLinearMagMipPoint => Silk.NET.Direct3D11.Filter.MinimumMinLinearMagMipPoint,
                Filter.MinimumMinLinearMagPointMipLinear => Silk.NET.Direct3D11.Filter.MinimumMinLinearMagPointMipLinear,
                Filter.MinimumMinMagLinearMipPoint => Silk.NET.Direct3D11.Filter.MinimumMinMagLinearMipPoint,
                Filter.MinimumMinMagMipLinear => Silk.NET.Direct3D11.Filter.MinimumMinMagMipLinear,
                Filter.MinimumAnisotropic => Silk.NET.Direct3D11.Filter.MinimumAnisotropic,
                Filter.MaximumMinMagMipPoint => Silk.NET.Direct3D11.Filter.MaximumMinMagMipPoint,
                Filter.MaximumMinMagPointMipLinear => Silk.NET.Direct3D11.Filter.MaximumMinMagPointMipLinear,
                Filter.MaximumMinPointMagLinearMipPoint => Silk.NET.Direct3D11.Filter.MaximumMinPointMagLinearMipPoint,
                Filter.MaximumMinPointMagMipLinear => Silk.NET.Direct3D11.Filter.MaximumMinPointMagMipLinear,
                Filter.MaximumMinLinearMagMipPoint => Silk.NET.Direct3D11.Filter.MaximumMinLinearMagMipPoint,
                Filter.MaximumMinLinearMagPointMipLinear => Silk.NET.Direct3D11.Filter.MaximumMinLinearMagPointMipLinear,
                Filter.MaximumMinMagLinearMipPoint => Silk.NET.Direct3D11.Filter.MaximumMinMagLinearMipPoint,
                Filter.MaximumMinMagMipLinear => Silk.NET.Direct3D11.Filter.MaximumMinMagMipLinear,
                Filter.MaximumAnisotropic => Silk.NET.Direct3D11.Filter.MaximumAnisotropic,
                _ => throw new ArgumentOutOfRangeException(nameof(filter)),
            };
        }

        public static Silk.NET.Direct3D11.TextureAddressMode Convert(TextureAddressMode address)
        {
            return address switch
            {
                TextureAddressMode.Wrap => Silk.NET.Direct3D11.TextureAddressMode.Wrap,
                TextureAddressMode.Mirror => Silk.NET.Direct3D11.TextureAddressMode.Mirror,
                TextureAddressMode.Clamp => Silk.NET.Direct3D11.TextureAddressMode.Clamp,
                TextureAddressMode.Border => Silk.NET.Direct3D11.TextureAddressMode.Border,
                TextureAddressMode.MirrorOnce => Silk.NET.Direct3D11.TextureAddressMode.MirrorOnce,
                _ => throw new ArgumentOutOfRangeException(nameof(address)),
            };
        }

        public static Silk.NET.Direct3D11.RenderTargetViewDesc Convert(RenderTargetViewDescription description)
        {
            return new()
            {
                Anonymous = Convert(description, description.ViewDimension),
                Format = Convert(description.Format),
                ViewDimension = Convert(description.ViewDimension),
            };
        }

        public static Silk.NET.Direct3D11.RtvDimension Convert(RenderTargetViewDimension viewDimension)
        {
            return viewDimension switch
            {
                RenderTargetViewDimension.Buffer => Silk.NET.Direct3D11.RtvDimension.Buffer,
                RenderTargetViewDimension.Texture1D => Silk.NET.Direct3D11.RtvDimension.Texture1D,
                RenderTargetViewDimension.Texture1DArray => Silk.NET.Direct3D11.RtvDimension.Texture1Darray,
                RenderTargetViewDimension.Texture2D => Silk.NET.Direct3D11.RtvDimension.Texture2D,
                RenderTargetViewDimension.Texture2DArray => Silk.NET.Direct3D11.RtvDimension.Texture2Darray,
                RenderTargetViewDimension.Texture2DMultisampled => Silk.NET.Direct3D11.RtvDimension.Texture2Dms,
                RenderTargetViewDimension.Texture2DMultisampledArray => Silk.NET.Direct3D11.RtvDimension.Texture2Dmsarray,
                RenderTargetViewDimension.Texture3D => Silk.NET.Direct3D11.RtvDimension.Texture3D,
                _ => throw new NotImplementedException(),
            };
        }

        public static Silk.NET.Direct3D11.RenderTargetViewDescUnion Convert(RenderTargetViewDescription description, RenderTargetViewDimension viewDimension)
        {
            return viewDimension switch
            {
                RenderTargetViewDimension.Buffer => new Silk.NET.Direct3D11.RenderTargetViewDescUnion(buffer: Convert(description.Buffer)),
                RenderTargetViewDimension.Texture1D => new Silk.NET.Direct3D11.RenderTargetViewDescUnion(texture1D: Convert(description.Texture1D)),
                RenderTargetViewDimension.Texture1DArray => new Silk.NET.Direct3D11.RenderTargetViewDescUnion(texture1DArray: Convert(description.Texture1DArray)),
                RenderTargetViewDimension.Texture2D => new Silk.NET.Direct3D11.RenderTargetViewDescUnion(texture2D: Convert(description.Texture2D)),
                RenderTargetViewDimension.Texture2DArray => new Silk.NET.Direct3D11.RenderTargetViewDescUnion(texture2DArray: Convert(description.Texture2DArray)),
                RenderTargetViewDimension.Texture2DMultisampled => new Silk.NET.Direct3D11.RenderTargetViewDescUnion(texture2DMS: Convert(description.Texture2DMS)),
                RenderTargetViewDimension.Texture2DMultisampledArray => new Silk.NET.Direct3D11.RenderTargetViewDescUnion(texture2DMSArray: Convert(description.Texture2DMSArray)),
                RenderTargetViewDimension.Texture3D => new Silk.NET.Direct3D11.RenderTargetViewDescUnion(texture3D: Convert(description.Texture3D)),
                _ => throw new NotImplementedException(),
            };
        }

        public static Silk.NET.Direct3D11.Tex3DRtv Convert(Texture3DRenderTargetView texture3D)
        {
            return new Silk.NET.Direct3D11.Tex3DRtv((uint)texture3D.MipSlice, (uint)texture3D.FirstWSlice, (uint)texture3D.WSize);
        }

        public static Silk.NET.Direct3D11.Tex2DmsArrayRtv Convert(Texture2DMultisampledArrayRenderTargetView texture2DMSArray)
        {
            return new Silk.NET.Direct3D11.Tex2DmsArrayRtv((uint)texture2DMSArray.FirstArraySlice, (uint)texture2DMSArray.ArraySize);
        }

        public static Silk.NET.Direct3D11.Tex2DmsRtv Convert(Texture2DMultisampledRenderTargetView texture2DMultisampled)
        {
            return new Silk.NET.Direct3D11.Tex2DmsRtv((uint)texture2DMultisampled.UnusedFieldNothingToDefine);
        }

        public static Silk.NET.Direct3D11.Tex2DArrayRtv Convert(Texture2DArrayRenderTargetView texture2DArray)
        {
            return new Silk.NET.Direct3D11.Tex2DArrayRtv((uint)texture2DArray.MipSlice, (uint)texture2DArray.FirstArraySlice, (uint)texture2DArray.ArraySize);
        }

        public static Silk.NET.Direct3D11.Tex2DRtv Convert(Texture2DRenderTargetView texture2D)
        {
            return new Silk.NET.Direct3D11.Tex2DRtv((uint)texture2D.MipSlice);
        }

        public static Silk.NET.Direct3D11.Tex1DArrayRtv Convert(Texture1DArrayRenderTargetView texture1DArray)
        {
            return new Silk.NET.Direct3D11.Tex1DArrayRtv((uint)texture1DArray.MipSlice, (uint)texture1DArray.FirstArraySlice, (uint)texture1DArray.ArraySize);
        }

        public static Silk.NET.Direct3D11.Tex1DRtv Convert(Texture1DRenderTargetView texture1D)
        {
            return new Silk.NET.Direct3D11.Tex1DRtv((uint)texture1D.MipSlice);
        }

        public static Silk.NET.Direct3D11.BufferRtv Convert(BufferRenderTargetView buffer)
        {
            return new()
            {
                Anonymous1 = new()
                {
                    ElementOffset = (uint)buffer.ElementOffset,
                    FirstElement = (uint)buffer.FirstElement,
                },
                Anonymous2 = new()
                {
                    ElementWidth = (uint)buffer.ElementWidth,
                    NumElements = (uint)buffer.NumElements,
                }
            };
        }

        public static Silk.NET.Direct3D11.DepthStencilViewDesc Convert(DepthStencilViewDescription description)
        {
            return new()
            {
                Format = Convert(description.Format),
                Anonymous = Convert(description, description.ViewDimension),
                Flags = (uint)Convert(description.Flags),
                ViewDimension = Convert(description.ViewDimension)
            };
        }

        public static Silk.NET.Direct3D11.DsvDimension Convert(DepthStencilViewDimension viewDimension)
        {
            return viewDimension switch
            {
                DepthStencilViewDimension.Texture1D => Silk.NET.Direct3D11.DsvDimension.Texture1D,
                DepthStencilViewDimension.Texture1DArray => Silk.NET.Direct3D11.DsvDimension.Texture1Darray,
                DepthStencilViewDimension.Texture2D => Silk.NET.Direct3D11.DsvDimension.Texture2D,
                DepthStencilViewDimension.Texture2DArray => Silk.NET.Direct3D11.DsvDimension.Texture2Darray,
                DepthStencilViewDimension.Texture2DMultisampled => Silk.NET.Direct3D11.DsvDimension.Texture2Dms,
                DepthStencilViewDimension.Texture2DMultisampledArray => Silk.NET.Direct3D11.DsvDimension.Texture2Dmsarray,
                _ => throw new NotImplementedException(),
            };
        }

        public static Silk.NET.Direct3D11.DsvFlag Convert(DepthStencilViewFlags flags)
        {
            Silk.NET.Direct3D11.DsvFlag result = 0;
            if (flags == DepthStencilViewFlags.None)
            {
                return 0;
            }

            if ((flags & DepthStencilViewFlags.ReadOnlyDepth) != 0)
            {
                result |= Silk.NET.Direct3D11.DsvFlag.Depth;
            }

            if ((flags & DepthStencilViewFlags.ReadOnlyStencil) != 0)
            {
                result |= Silk.NET.Direct3D11.DsvFlag.Stencil;
            }

            return result;
        }

        public static Silk.NET.Direct3D11.DepthStencilViewDescUnion Convert(DepthStencilViewDescription description, DepthStencilViewDimension viewDimension)
        {
            return viewDimension switch
            {
                DepthStencilViewDimension.Texture1D => new Silk.NET.Direct3D11.DepthStencilViewDescUnion(texture1D: Convert(description.Texture1D)),
                DepthStencilViewDimension.Texture1DArray => new Silk.NET.Direct3D11.DepthStencilViewDescUnion(texture1DArray: Convert(description.Texture1DArray)),
                DepthStencilViewDimension.Texture2D => new Silk.NET.Direct3D11.DepthStencilViewDescUnion(texture2D: Convert(description.Texture2D)),
                DepthStencilViewDimension.Texture2DArray => new Silk.NET.Direct3D11.DepthStencilViewDescUnion(texture2DArray: Convert(description.Texture2DArray)),
                DepthStencilViewDimension.Texture2DMultisampled => new Silk.NET.Direct3D11.DepthStencilViewDescUnion(texture2DMS: Convert(description.Texture2DMS)),
                DepthStencilViewDimension.Texture2DMultisampledArray => new Silk.NET.Direct3D11.DepthStencilViewDescUnion(texture2DMSArray: Convert(description.Texture2DMSArray)),
                _ => throw new NotImplementedException(),
            };
        }

        public static Silk.NET.Direct3D11.Tex2DmsArrayDsv Convert(Texture2DMultisampledArrayDepthStencilView texture2DMSArray)
        {
            return new Silk.NET.Direct3D11.Tex2DmsArrayDsv((uint)texture2DMSArray.FirstArraySlice, (uint)texture2DMSArray.ArraySize);
        }

        public static Silk.NET.Direct3D11.Tex2DmsDsv Convert(Texture2DMultisampledDepthStencilView texture2DMS)
        {
            return new Silk.NET.Direct3D11.Tex2DmsDsv((uint)texture2DMS.UnusedFieldNothingToDefine);
        }

        public static Silk.NET.Direct3D11.Tex2DArrayDsv Convert(Texture2DArrayDepthStencilView texture2DArray)
        {
            return new Silk.NET.Direct3D11.Tex2DArrayDsv((uint)texture2DArray.MipSlice, (uint)texture2DArray.FirstArraySlice, (uint)texture2DArray.ArraySize);
        }

        public static Silk.NET.Direct3D11.Tex2DDsv Convert(Texture2DDepthStencilView texture2D)
        {
            return new Silk.NET.Direct3D11.Tex2DDsv((uint)texture2D.MipSlice);
        }

        public static Silk.NET.Direct3D11.Tex1DArrayDsv Convert(Texture1DArrayDepthStencilView texture1DArray)
        {
            return new Silk.NET.Direct3D11.Tex1DArrayDsv((uint)texture1DArray.MipSlice, (uint)texture1DArray.FirstArraySlice, (uint)texture1DArray.ArraySize);
        }

        public static Silk.NET.Direct3D11.Tex1DDsv Convert(Texture1DDepthStencilView texture1D)
        {
            return new Silk.NET.Direct3D11.Tex1DDsv((uint)texture1D.MipSlice);
        }

        public static Silk.NET.Direct3D11.ShaderResourceViewDesc Convert(ShaderResourceViewDescription description)
        {
            return new()
            {
                Format = Convert(description.Format),
                ViewDimension = Convert(description.ViewDimension),
                Anonymous = Convert(description, description.ViewDimension),
            };
        }

        public static Silk.NET.Direct3D11.ShaderResourceViewDescUnion Convert(ShaderResourceViewDescription description, ShaderResourceViewDimension dimension)
        {
            return dimension switch
            {
                ShaderResourceViewDimension.Buffer => new Silk.NET.Direct3D11.ShaderResourceViewDescUnion(buffer: Convert(description.Buffer)),
                ShaderResourceViewDimension.Texture1D => new Silk.NET.Direct3D11.ShaderResourceViewDescUnion(texture1D: Convert(description.Texture1D)),
                ShaderResourceViewDimension.Texture1DArray => new Silk.NET.Direct3D11.ShaderResourceViewDescUnion(texture1DArray: Convert(description.Texture1DArray)),
                ShaderResourceViewDimension.Texture2D => new Silk.NET.Direct3D11.ShaderResourceViewDescUnion(texture2D: Convert(description.Texture2D)),
                ShaderResourceViewDimension.Texture2DArray => new Silk.NET.Direct3D11.ShaderResourceViewDescUnion(texture2DArray: Convert(description.Texture2DArray)),
                ShaderResourceViewDimension.Texture2DMultisampled => new Silk.NET.Direct3D11.ShaderResourceViewDescUnion(texture2DMS: Convert(description.Texture2DMS)),
                ShaderResourceViewDimension.Texture2DMultisampledArray => new Silk.NET.Direct3D11.ShaderResourceViewDescUnion(texture2DMSArray: Convert(description.Texture2DMSArray)),
                ShaderResourceViewDimension.Texture3D => new Silk.NET.Direct3D11.ShaderResourceViewDescUnion(texture3D: Convert(description.Texture3D)),
                ShaderResourceViewDimension.TextureCube => new Silk.NET.Direct3D11.ShaderResourceViewDescUnion(textureCube: Convert(description.TextureCube)),
                ShaderResourceViewDimension.TextureCubeArray => new Silk.NET.Direct3D11.ShaderResourceViewDescUnion(textureCubeArray: Convert(description.TextureCubeArray)),
                ShaderResourceViewDimension.BufferExtended => new Silk.NET.Direct3D11.ShaderResourceViewDescUnion(bufferEx: Convert(description.BufferEx)),
                _ => throw new NotImplementedException(),
            };
        }

        public static Silk.NET.Direct3D11.BufferexSrv Convert(BufferExtendedShaderResourceView bufferEx)
        {
            return new Silk.NET.Direct3D11.BufferexSrv((uint)bufferEx.FirstElement, (uint)bufferEx.NumElements, (uint)Convert(bufferEx.Flags));
        }

        public static Silk.NET.Direct3D11.BufferexSrvFlag Convert(BufferExtendedShaderResourceViewFlags flags)
        {
            if ((flags & BufferExtendedShaderResourceViewFlags.Raw) != 0)
            {
                return Silk.NET.Direct3D11.BufferexSrvFlag.Raw;
            }
            else
            {
                return 0;
            }
        }

        public static Silk.NET.Direct3D11.TexcubeArraySrv Convert(TextureCubeArrayShaderResourceView textureCubeArray)
        {
            return new Silk.NET.Direct3D11.TexcubeArraySrv((uint?)textureCubeArray.MostDetailedMip, (uint?)textureCubeArray.MipLevels, (uint?)textureCubeArray.First2DArrayFace, (uint?)textureCubeArray.NumCubes);
        }

        public static Silk.NET.Direct3D11.TexcubeSrv Convert(TextureCubeShaderResourceView textureCube)
        {
            return new Silk.NET.Direct3D11.TexcubeSrv((uint)textureCube.MostDetailedMip, (uint)textureCube.MipLevels);
        }

        public static Silk.NET.Direct3D11.Tex3DSrv Convert(Texture3DShaderResourceView texture3D)
        {
            return new Silk.NET.Direct3D11.Tex3DSrv((uint)texture3D.MostDetailedMip, (uint)texture3D.MipLevels);
        }

        public static Silk.NET.Direct3D11.Tex2DmsArraySrv Convert(Texture2DMultisampledArrayShaderResourceView texture2DMSArray)
        {
            return new()
            {
                ArraySize = (uint)texture2DMSArray.ArraySize,
                FirstArraySlice = (uint)texture2DMSArray.FirstArraySlice
            };
        }

        public static Silk.NET.Direct3D11.Tex2DmsSrv Convert(Texture2DMultisampledShaderResourceView texture2DMS)
        {
            return new() { UnusedFieldNothingToDefine = (uint)texture2DMS.UnusedFieldNothingToDefine };
        }

        public static Silk.NET.Direct3D11.Tex2DArraySrv Convert(Texture2DArrayShaderResourceView texture2DArray)
        {
            return new()
            {
                ArraySize = (uint)texture2DArray.ArraySize,
                FirstArraySlice = (uint)texture2DArray.FirstArraySlice,
                MipLevels = (uint)texture2DArray.MipLevels,
                MostDetailedMip = (uint)texture2DArray.MostDetailedMip
            };
        }

        public static Silk.NET.Direct3D11.Tex2DSrv Convert(Texture2DShaderResourceView texture2D)
        {
            return new()
            {
                MipLevels = (uint)texture2D.MipLevels,
                MostDetailedMip = (uint)texture2D.MostDetailedMip
            };
        }

        public static Silk.NET.Direct3D11.Tex1DArraySrv Convert(Texture1DArrayShaderResourceView texture1DArray)
        {
            return new()
            {
                ArraySize = (uint)texture1DArray.ArraySize,
                FirstArraySlice = (uint)texture1DArray.FirstArraySlice,
                MipLevels = (uint)texture1DArray.MipLevels,
                MostDetailedMip = (uint)texture1DArray.MostDetailedMip
            };
        }

        public static Silk.NET.Direct3D11.Tex1DSrv Convert(Texture1DShaderResourceView texture1D)
        {
            return new()
            {
                MipLevels = (uint)texture1D.MipLevels,
                MostDetailedMip = (uint)texture1D.MostDetailedMip,
            };
        }

        public static Silk.NET.Direct3D11.BufferSrv Convert(BufferShaderResourceView buffer)
        {
            return new()
            {
                Anonymous1 = new()
                {
                    ElementOffset = (uint)buffer.ElementOffset,
                    FirstElement = (uint)buffer.FirstElement
                },
                Anonymous2 = new()
                {
                    NumElements = (uint)buffer.NumElements,
                    ElementWidth = (uint)buffer.ElementWidth
                }
            };
        }

        public static Silk.NET.Core.Native.D3DSrvDimension Convert(ShaderResourceViewDimension viewDimension)
        {
            return viewDimension switch
            {
                ShaderResourceViewDimension.Unknown => Silk.NET.Core.Native.D3DSrvDimension.D3D101SrvDimensionUnknown,
                ShaderResourceViewDimension.Buffer => Silk.NET.Core.Native.D3DSrvDimension.D3D101SrvDimensionBuffer,
                ShaderResourceViewDimension.Texture1D => Silk.NET.Core.Native.D3DSrvDimension.D3D101SrvDimensionTexture1D,
                ShaderResourceViewDimension.Texture1DArray => Silk.NET.Core.Native.D3DSrvDimension.D3D101SrvDimensionTexture1Darray,
                ShaderResourceViewDimension.Texture2D => Silk.NET.Core.Native.D3DSrvDimension.D3D101SrvDimensionTexture2D,
                ShaderResourceViewDimension.Texture2DArray => Silk.NET.Core.Native.D3DSrvDimension.D3D101SrvDimensionTexture2Darray,
                ShaderResourceViewDimension.Texture2DMultisampled => Silk.NET.Core.Native.D3DSrvDimension.D3D101SrvDimensionTexture2Dms,
                ShaderResourceViewDimension.Texture2DMultisampledArray => Silk.NET.Core.Native.D3DSrvDimension.D3D101SrvDimensionTexture2Dmsarray,
                ShaderResourceViewDimension.Texture3D => Silk.NET.Core.Native.D3DSrvDimension.D3D101SrvDimensionTexture3D,
                ShaderResourceViewDimension.TextureCube => Silk.NET.Core.Native.D3DSrvDimension.D3D101SrvDimensionTexturecube,
                ShaderResourceViewDimension.TextureCubeArray => Silk.NET.Core.Native.D3DSrvDimension.D3D101SrvDimensionTexturecubearray,
                ShaderResourceViewDimension.BufferExtended => Silk.NET.Core.Native.D3DSrvDimension.D3D11SrvDimensionBufferex,
                _ => 0
            };
        }

        public static void Convert(SubresourceData[] datas, Silk.NET.Direct3D11.SubresourceData* subresourceDatas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                subresourceDatas[i] = Convert(datas[i]);
            }
        }

        public static Silk.NET.Direct3D11.SubresourceData Convert(SubresourceData data)
        {
            return new()
            {
                PSysMem = (void*)data.DataPointer,
                SysMemPitch = (uint)data.RowPitch,
                SysMemSlicePitch = (uint)data.SlicePitch,
            };
        }

        public static Silk.NET.Direct3D11.Texture3DDesc Convert(Texture3DDescription description)
        {
            return new()
            {
                BindFlags = (uint)Convert(description.BindFlags),
                CPUAccessFlags = (uint)Convert(description.CPUAccessFlags),
                Format = Convert(description.Format),
                MipLevels = (uint)description.MipLevels,
                MiscFlags = (uint)Convert(description.MiscFlags),
                Usage = Convert(description.Usage),
                Width = (uint)description.Width,
                Height = (uint)description.Height,
                Depth = (uint)description.Depth
            };
        }

        public static Silk.NET.Direct3D11.Texture2DDesc Convert(Texture2DDescription description)
        {
            return new()
            {
                ArraySize = (uint)description.ArraySize,
                BindFlags = (uint)Convert(description.BindFlags),
                CPUAccessFlags = (uint)Convert(description.CPUAccessFlags),
                Format = Convert(description.Format),
                MipLevels = (uint)description.MipLevels,
                MiscFlags = (uint)Convert(description.MiscFlags),
                Usage = Convert(description.Usage),
                Width = (uint)description.Width,
                Height = (uint)description.Height,
                SampleDesc = Convert(description.SampleDescription)
            };
        }

        public static Silk.NET.DXGI.SampleDesc Convert(SampleDescription sampleDescription)
        {
            return new()
            {
                Count = (uint)sampleDescription.Count,
                Quality = (uint)sampleDescription.Quality
            };
        }

        public static Silk.NET.Direct3D11.Texture1DDesc Convert(Texture1DDescription description)
        {
            return new()
            {
                ArraySize = (uint)description.ArraySize,
                BindFlags = (uint)Convert(description.BindFlags),
                CPUAccessFlags = (uint)Convert(description.CPUAccessFlags),
                Format = Convert(description.Format),
                MipLevels = (uint)description.MipLevels,
                MiscFlags = (uint)Convert(description.MiscFlags),
                Usage = Convert(description.Usage),
                Width = (uint)description.Width,
            };
        }

        public static Silk.NET.Direct3D11.DepthStencilDesc Convert(DepthStencilDescription description)
        {
            return new()
            {
                DepthEnable = description.DepthEnable,
                DepthFunc = Convert(description.DepthFunc),
                BackFace = Convert(description.BackFace),
                DepthWriteMask = Convert(description.DepthWriteMask),
                FrontFace = Convert(description.FrontFace),
                StencilEnable = description.StencilEnable,
                StencilReadMask = description.StencilReadMask,
                StencilWriteMask = description.StencilWriteMask
            };
        }

        public static Silk.NET.Direct3D11.DepthStencilopDesc Convert(DepthStencilOperationDescription description)
        {
            return new()
            {
                StencilDepthFailOp = Convert(description.StencilDepthFailOp),
                StencilFailOp = Convert(description.StencilFailOp),
                StencilFunc = Convert(description.StencilFunc),
                StencilPassOp = Convert(description.StencilPassOp),
            };
        }

        internal static Silk.NET.Direct3D11.Query Convert(Query query)
        {
            return query switch
            {
                Query.Event => Silk.NET.Direct3D11.Query.Event,
                Query.Occlusion => Silk.NET.Direct3D11.Query.Occlusion,
                Query.Timestamp => Silk.NET.Direct3D11.Query.Timestamp,
                Query.TimestampDisjoint => Silk.NET.Direct3D11.Query.TimestampDisjoint,
                Query.PipelineStatistics => Silk.NET.Direct3D11.Query.PipelineStatistics,
                Query.OcclusionPredicate => Silk.NET.Direct3D11.Query.OcclusionPredicate,
                Query.SOStatistics => Silk.NET.Direct3D11.Query.SOStatistics,
                Query.SOOverflowPredicate => Silk.NET.Direct3D11.Query.SOOverflowPredicate,
                Query.SOStatisticsStream0 => Silk.NET.Direct3D11.Query.SOStatisticsStream0,
                Query.SOOverflowPredicateStream0 => Silk.NET.Direct3D11.Query.SOOverflowPredicateStream0,
                Query.SOStatisticsStream1 => Silk.NET.Direct3D11.Query.SOStatisticsStream1,
                Query.SOOverflowPredicateStream1 => Silk.NET.Direct3D11.Query.SOOverflowPredicateStream1,
                Query.SOStatisticsStream2 => Silk.NET.Direct3D11.Query.SOStatisticsStream2,
                Query.SOOverflowPredicateStream2 => Silk.NET.Direct3D11.Query.SOOverflowPredicateStream2,
                Query.SOStatisticsStream3 => Silk.NET.Direct3D11.Query.SOStatisticsStream3,
                Query.SOOverflowPredicateStream3 => Silk.NET.Direct3D11.Query.SOOverflowPredicateStream3,
                _ => throw new NotSupportedException()
            };
        }

        public static Silk.NET.Direct3D11.StencilOp Convert(StencilOperation operation)
        {
            return operation switch
            {
                StencilOperation.Keep => Silk.NET.Direct3D11.StencilOp.Keep,
                StencilOperation.Zero => Silk.NET.Direct3D11.StencilOp.Zero,
                StencilOperation.Replace => Silk.NET.Direct3D11.StencilOp.Replace,
                StencilOperation.IncrementSaturate => Silk.NET.Direct3D11.StencilOp.IncrSat,
                StencilOperation.DecrementSaturate => Silk.NET.Direct3D11.StencilOp.DecrSat,
                StencilOperation.Invert => Silk.NET.Direct3D11.StencilOp.Invert,
                StencilOperation.Increment => Silk.NET.Direct3D11.StencilOp.Incr,
                StencilOperation.Decrement => Silk.NET.Direct3D11.StencilOp.Decr,
                _ => 0,
            };
        }

        public static Silk.NET.Direct3D11.ComparisonFunc Convert(ComparisonFunction function)
        {
            return function switch
            {
                ComparisonFunction.Never => Silk.NET.Direct3D11.ComparisonFunc.Never,
                ComparisonFunction.Less => Silk.NET.Direct3D11.ComparisonFunc.Less,
                ComparisonFunction.Equal => Silk.NET.Direct3D11.ComparisonFunc.Equal,
                ComparisonFunction.LessEqual => Silk.NET.Direct3D11.ComparisonFunc.LessEqual,
                ComparisonFunction.Greater => Silk.NET.Direct3D11.ComparisonFunc.Greater,
                ComparisonFunction.NotEqual => Silk.NET.Direct3D11.ComparisonFunc.NotEqual,
                ComparisonFunction.GreaterEqual => Silk.NET.Direct3D11.ComparisonFunc.GreaterEqual,
                ComparisonFunction.Always => Silk.NET.Direct3D11.ComparisonFunc.Always,
                _ => 0
            };
        }

        public static Silk.NET.Direct3D11.DepthWriteMask Convert(DepthWriteMask mask)
        {
            return mask switch
            {
                DepthWriteMask.Zero => Silk.NET.Direct3D11.DepthWriteMask.Zero,
                DepthWriteMask.All => Silk.NET.Direct3D11.DepthWriteMask.All,
                _ => 0
            };
        }

        public static Silk.NET.Direct3D11.RasterizerDesc Convert(RasterizerDescription description)
        {
            return new()
            {
                AntialiasedLineEnable = description.AntialiasedLineEnable,
                CullMode = Convert(description.CullMode),
                DepthBias = description.DepthBias,
                DepthBiasClamp = description.DepthBiasClamp,
                DepthClipEnable = description.DepthClipEnable,
                FillMode = Convert(description.FillMode),
                FrontCounterClockwise = description.FrontCounterClockwise,
                MultisampleEnable = description.MultisampleEnable,
                ScissorEnable = description.ScissorEnable,
                SlopeScaledDepthBias = description.SlopeScaledDepthBias
            };
        }

        public static Silk.NET.Direct3D11.FillMode Convert(FillMode mode)
        {
            return mode switch
            {
                FillMode.Solid => Silk.NET.Direct3D11.FillMode.Solid,
                FillMode.Wireframe => Silk.NET.Direct3D11.FillMode.Wireframe,
                _ => 0
            };
        }

        public static Silk.NET.Direct3D11.CullMode Convert(CullMode mode)
        {
            return mode switch
            {
                CullMode.None => Silk.NET.Direct3D11.CullMode.None,
                CullMode.Front => Silk.NET.Direct3D11.CullMode.Front,
                CullMode.Back => Silk.NET.Direct3D11.CullMode.Back,
                _ => 0
            };
        }

        public static Silk.NET.Direct3D11.BlendDesc Convert(BlendDescription description)
        {
            return new()
            {
                AlphaToCoverageEnable = description.AlphaToCoverageEnable,
                IndependentBlendEnable = description.IndependentBlendEnable,
                RenderTarget = Convert(description.RenderTarget),
            };
        }

        public static Silk.NET.Direct3D11.BlendDesc.RenderTargetBuffer Convert(RenderTargetBlendDescription[] descriptions)
        {
            return new()
            {
                Element0 = Convert(descriptions[0]),
                Element1 = Convert(descriptions[1]),
                Element2 = Convert(descriptions[2]),
                Element3 = Convert(descriptions[3]),
                Element4 = Convert(descriptions[4]),
                Element5 = Convert(descriptions[5]),
                Element6 = Convert(descriptions[6]),
                Element7 = Convert(descriptions[7]),
            };
        }

        public static Silk.NET.Direct3D11.RenderTargetBlendDesc Convert(RenderTargetBlendDescription description)
        {
            return new()
            {
                BlendEnable = description.IsBlendEnabled,
                BlendOp = Convert(description.BlendOperation),
                BlendOpAlpha = Convert(description.BlendOperationAlpha),
                DestBlend = Convert(description.DestinationBlend),
                DestBlendAlpha = Convert(description.DestinationBlendAlpha),
                RenderTargetWriteMask = (byte)Convert(description.RenderTargetWriteMask),
                SrcBlend = Convert(description.SourceBlend),
                SrcBlendAlpha = Convert(description.SourceBlendAlpha),
            };
        }

        public static Silk.NET.Direct3D11.ColorWriteEnable Convert(ColorWriteEnable flags)
        {
            Silk.NET.Direct3D11.ColorWriteEnable result = 0;
            if (flags == ColorWriteEnable.All)
            {
                return Silk.NET.Direct3D11.ColorWriteEnable.All;
            }

            if ((flags & ColorWriteEnable.Red) != 0)
            {
                result |= Silk.NET.Direct3D11.ColorWriteEnable.Red;
            }

            if ((flags & ColorWriteEnable.Green) != 0)
            {
                result |= Silk.NET.Direct3D11.ColorWriteEnable.Green;
            }

            if ((flags & ColorWriteEnable.Blue) != 0)
            {
                result |= Silk.NET.Direct3D11.ColorWriteEnable.Blue;
            }

            if ((flags & ColorWriteEnable.Alpha) != 0)
            {
                result |= Silk.NET.Direct3D11.ColorWriteEnable.Alpha;
            }

            return result;
        }

        public static Silk.NET.Direct3D11.Blend Convert(Blend blend)
        {
            return blend switch
            {
                Blend.Zero => Silk.NET.Direct3D11.Blend.Zero,
                Blend.One => Silk.NET.Direct3D11.Blend.One,
                Blend.SourceColor => Silk.NET.Direct3D11.Blend.SrcColor,
                Blend.InverseSourceColor => Silk.NET.Direct3D11.Blend.InvSrcColor,
                Blend.SourceAlpha => Silk.NET.Direct3D11.Blend.SrcAlpha,
                Blend.InverseSourceAlpha => Silk.NET.Direct3D11.Blend.InvSrcAlpha,
                Blend.DestinationAlpha => Silk.NET.Direct3D11.Blend.DestAlpha,
                Blend.InverseDestinationAlpha => Silk.NET.Direct3D11.Blend.InvDestAlpha,
                Blend.DestinationColor => Silk.NET.Direct3D11.Blend.DestColor,
                Blend.InverseDestinationColor => Silk.NET.Direct3D11.Blend.InvDestColor,
                Blend.SourceAlphaSaturate => Silk.NET.Direct3D11.Blend.SrcAlphaSat,
                Blend.BlendFactor => Silk.NET.Direct3D11.Blend.BlendFactor,
                Blend.InverseBlendFactor => Silk.NET.Direct3D11.Blend.InvBlendFactor,
                Blend.Source1Color => Silk.NET.Direct3D11.Blend.Src1Color,
                Blend.InverseSource1Color => Silk.NET.Direct3D11.Blend.InvSrc1Color,
                Blend.Source1Alpha => Silk.NET.Direct3D11.Blend.Src1Alpha,
                Blend.InverseSource1Alpha => Silk.NET.Direct3D11.Blend.InvSrc1Alpha,
                _ => 0
            };
        }

        public static Silk.NET.Direct3D11.BlendOp Convert(BlendOperation operation)
        {
            return operation switch
            {
                BlendOperation.Add => Silk.NET.Direct3D11.BlendOp.Add,
                BlendOperation.Subtract => Silk.NET.Direct3D11.BlendOp.Subtract,
                BlendOperation.ReverseSubtract => Silk.NET.Direct3D11.BlendOp.RevSubtract,
                BlendOperation.Min => Silk.NET.Direct3D11.BlendOp.Min,
                BlendOperation.Max => Silk.NET.Direct3D11.BlendOp.Max,
                _ => 0
            };
        }

        public static Silk.NET.Direct3D11.BufferDesc Convert(BufferDescription description)
        {
            return new()
            {
                BindFlags = (uint)Convert(description.BindFlags),
                ByteWidth = (uint)description.ByteWidth,
                CPUAccessFlags = (uint)Convert(description.CPUAccessFlags),
                MiscFlags = (uint)Convert(description.MiscFlags),
                StructureByteStride = (uint)description.StructureByteStride,
                Usage = Convert(description.Usage)
            };
        }

        public static Silk.NET.Direct3D11.CpuAccessFlag Convert(CpuAccessFlags flags)
        {
            Silk.NET.Direct3D11.CpuAccessFlag result = 0;
            if ((flags & CpuAccessFlags.Write) != 0)
            {
                result |= Silk.NET.Direct3D11.CpuAccessFlag.Write;
            }

            if ((flags & CpuAccessFlags.Read) != 0)
            {
                result |= Silk.NET.Direct3D11.CpuAccessFlag.Read;
            }

            return result;
        }

        public static Silk.NET.Direct3D11.ResourceMiscFlag Convert(ResourceMiscFlag flags)
        {
            Silk.NET.Direct3D11.ResourceMiscFlag result = 0;
            if ((flags & ResourceMiscFlag.GenerateMips) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.GenerateMips;
            }

            if ((flags & ResourceMiscFlag.Shared) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.Shared;
            }

            if ((flags & ResourceMiscFlag.TextureCube) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.Texturecube;
            }

            if ((flags & ResourceMiscFlag.DrawIndirectArguments) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.DrawindirectArgs;
            }

            if ((flags & ResourceMiscFlag.BufferAllowRawViews) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.BufferAllowRawViews;
            }

            if ((flags & ResourceMiscFlag.BufferStructured) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.BufferStructured;
            }

            if ((flags & ResourceMiscFlag.ResourceClamp) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.ResourceClamp;
            }

            if ((flags & ResourceMiscFlag.SharedKeyedMutex) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.SharedKeyedmutex;
            }

            if ((flags & ResourceMiscFlag.GdiCompatible) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.GdiCompatible;
            }

            if ((flags & ResourceMiscFlag.SharedNTHandle) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.SharedNthandle;
            }

            if ((flags & ResourceMiscFlag.RestrictedContent) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.RestrictedContent;
            }

            if ((flags & ResourceMiscFlag.RestrictSharedResource) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.RestrictSharedResource;
            }

            if ((flags & ResourceMiscFlag.RestrictSharedResourceDriver) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.RestrictSharedResourceDriver;
            }

            if ((flags & ResourceMiscFlag.Guarded) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.Guarded;
            }

            if ((flags & ResourceMiscFlag.TilePool) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.TilePool;
            }

            if ((flags & ResourceMiscFlag.Tiled) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.Tiled;
            }

            if ((flags & ResourceMiscFlag.HardwareProtected) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.HWProtected;
            }

            if ((flags & ResourceMiscFlag.SharedDisplayable) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.SharedDisplayable;
            }

            if ((flags & ResourceMiscFlag.SharedExclusiveWriter) != 0)
            {
                result |= Silk.NET.Direct3D11.ResourceMiscFlag.SharedExclusiveWriter;
            }

            if ((flags & ResourceMiscFlag.None) != 0)
            {
                result |= 0;
            }

            return result;
        }

        public static Silk.NET.Direct3D11.Usage Convert(Usage usage)
        {
            return usage switch
            {
                Usage.Default => Silk.NET.Direct3D11.Usage.Default,
                Usage.Immutable => Silk.NET.Direct3D11.Usage.Immutable,
                Usage.Dynamic => Silk.NET.Direct3D11.Usage.Dynamic,
                Usage.Staging => Silk.NET.Direct3D11.Usage.Staging,
                _ => throw new ArgumentOutOfRangeException(nameof(usage))
            };
        }

        public static Silk.NET.Direct3D11.BindFlag Convert(BindFlags flags)
        {
            Silk.NET.Direct3D11.BindFlag result = 0;
            if ((flags & BindFlags.VertexBuffer) != 0)
            {
                result |= Silk.NET.Direct3D11.BindFlag.VertexBuffer;
            }

            if ((flags & BindFlags.IndexBuffer) != 0)
            {
                result |= Silk.NET.Direct3D11.BindFlag.IndexBuffer;
            }

            if ((flags & BindFlags.ConstantBuffer) != 0)
            {
                result |= Silk.NET.Direct3D11.BindFlag.ConstantBuffer;
            }

            if ((flags & BindFlags.ShaderResource) != 0)
            {
                result |= Silk.NET.Direct3D11.BindFlag.ShaderResource;
            }

            if ((flags & BindFlags.StreamOutput) != 0)
            {
                result |= Silk.NET.Direct3D11.BindFlag.StreamOutput;
            }

            if ((flags & BindFlags.RenderTarget) != 0)
            {
                result |= Silk.NET.Direct3D11.BindFlag.RenderTarget;
            }

            if ((flags & BindFlags.DepthStencil) != 0)
            {
                result |= Silk.NET.Direct3D11.BindFlag.DepthStencil;
            }

            if ((flags & BindFlags.UnorderedAccess) != 0)
            {
                result |= Silk.NET.Direct3D11.BindFlag.UnorderedAccess;
            }

            if ((flags & BindFlags.Decoder) != 0)
            {
                result |= Silk.NET.Direct3D11.BindFlag.Decoder;
            }

            if ((flags & BindFlags.VideoEncoder) != 0)
            {
                result |= Silk.NET.Direct3D11.BindFlag.VideoEncoder;
            }

            return result;
        }

        public static void Convert(InputElementDescription[] inputElements, Silk.NET.Direct3D11.InputElementDesc* descs)
        {
            for (int i = 0; i < inputElements.Length; i++)
            {
                descs[i] = Convert(inputElements[i]);
            }
        }

        public static void Free(Silk.NET.Direct3D11.InputElementDesc* descs, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Marshal.FreeHGlobal((nint)descs[i].SemanticName);
            }
        }

        public static Silk.NET.Direct3D11.InputElementDesc Convert(InputElementDescription description)
        {
            return new()
            {
                AlignedByteOffset = (uint)description.AlignedByteOffset,
                Format = Convert(description.Format),
                InputSlot = (uint)description.Slot,
                InputSlotClass = Convert(description.Classification),
                InstanceDataStepRate = (uint)description.InstanceDataStepRate,
                SemanticIndex = (uint)description.SemanticIndex,
                SemanticName = description.SemanticName.ToBytes()
            };
        }

        public static Silk.NET.Direct3D11.InputClassification Convert(InputClassification classification)
        {
            return classification switch
            {
                InputClassification.PerVertexData => Silk.NET.Direct3D11.InputClassification.PerVertexData,
                InputClassification.PerInstanceData => Silk.NET.Direct3D11.InputClassification.PerInstanceData,
                _ => throw new NotImplementedException()
            };
        }

        public static Silk.NET.DXGI.Format Convert(Format format)
        {
            return format switch
            {
                Format.Unknown => Silk.NET.DXGI.Format.FormatUnknown,
                Format.R32G32B32A32Typeless => Silk.NET.DXGI.Format.FormatR32G32B32A32Typeless,
                Format.R32G32B32A32Float => Silk.NET.DXGI.Format.FormatR32G32B32A32Float,
                Format.R32G32B32A32UInt => Silk.NET.DXGI.Format.FormatR32G32B32A32Uint,
                Format.R32G32B32A32SInt => Silk.NET.DXGI.Format.FormatR32G32B32A32Sint,
                Format.R32G32B32Typeless => Silk.NET.DXGI.Format.FormatR32G32B32Typeless,
                Format.R32G32B32Float => Silk.NET.DXGI.Format.FormatR32G32B32Float,
                Format.R32G32B32UInt => Silk.NET.DXGI.Format.FormatR32G32B32Uint,
                Format.R32G32B32SInt => Silk.NET.DXGI.Format.FormatR32G32B32Sint,
                Format.R16G16B16A16Typeless => Silk.NET.DXGI.Format.FormatR16G16B16A16Typeless,
                Format.R16G16B16A16Float => Silk.NET.DXGI.Format.FormatR16G16B16A16Float,
                Format.R16G16B16A16UNorm => Silk.NET.DXGI.Format.FormatR16G16B16A16Unorm,
                Format.R16G16B16A16UInt => Silk.NET.DXGI.Format.FormatR16G16B16A16Uint,
                Format.R16G16B16A16SNorm => Silk.NET.DXGI.Format.FormatR16G16B16A16SNorm,
                Format.R16G16B16A16Sint => Silk.NET.DXGI.Format.FormatR16G16B16A16Sint,
                Format.R32G32Typeless => Silk.NET.DXGI.Format.FormatR32G32Typeless,
                Format.R32G32Float => Silk.NET.DXGI.Format.FormatR32G32Float,
                Format.R32G32UInt => Silk.NET.DXGI.Format.FormatR32G32Uint,
                Format.R32G32SInt => Silk.NET.DXGI.Format.FormatR32G32Sint,
                Format.R32G8X24Typeless => Silk.NET.DXGI.Format.FormatR32G8X24Typeless,
                Format.D32FloatS8X24UInt => Silk.NET.DXGI.Format.FormatD32FloatS8X24Uint,
                Format.R32FloatX8X24Typeless => Silk.NET.DXGI.Format.FormatR32FloatX8X24Typeless,
                Format.X32TypelessG8X24UInt => Silk.NET.DXGI.Format.FormatX32TypelessG8X24Uint,
                Format.R10G10B10A2Typeless => Silk.NET.DXGI.Format.FormatR10G10B10A2Typeless,
                Format.R10G10B10A2UNorm => Silk.NET.DXGI.Format.FormatR10G10B10A2Unorm,
                Format.R10G10B10A2UInt => Silk.NET.DXGI.Format.FormatR10G10B10A2Uint,
                Format.R11G11B10Float => Silk.NET.DXGI.Format.FormatR11G11B10Float,
                Format.R8G8B8A8Typeless => Silk.NET.DXGI.Format.FormatR8G8B8A8Typeless,
                Format.R8G8B8A8UNorm => Silk.NET.DXGI.Format.FormatR8G8B8A8Unorm,
                Format.R8G8B8A8UNormSRGB => Silk.NET.DXGI.Format.FormatR8G8B8A8UnormSrgb,
                Format.R8G8B8A8UInt => Silk.NET.DXGI.Format.FormatR8G8B8A8Uint,
                Format.R8G8B8A8SNorm => Silk.NET.DXGI.Format.FormatR8G8B8A8SNorm,
                Format.R8G8B8A8SInt => Silk.NET.DXGI.Format.FormatR8G8B8A8Sint,
                Format.R16G16Typeless => Silk.NET.DXGI.Format.FormatR16G16Typeless,
                Format.R16G16Float => Silk.NET.DXGI.Format.FormatR16G16Float,
                Format.R16G16UNorm => Silk.NET.DXGI.Format.FormatR16G16Unorm,
                Format.R16G16UInt => Silk.NET.DXGI.Format.FormatR16G16Uint,
                Format.R16G16SNorm => Silk.NET.DXGI.Format.FormatR16G16SNorm,
                Format.R16G16Sint => Silk.NET.DXGI.Format.FormatR16G16Sint,
                Format.R32Typeless => Silk.NET.DXGI.Format.FormatR32Typeless,
                Format.D32Float => Silk.NET.DXGI.Format.FormatD32Float,
                Format.R32Float => Silk.NET.DXGI.Format.FormatR32Float,
                Format.R32UInt => Silk.NET.DXGI.Format.FormatR32Uint,
                Format.R32SInt => Silk.NET.DXGI.Format.FormatR32Sint,
                Format.R24G8Typeless => Silk.NET.DXGI.Format.FormatR24G8Typeless,
                Format.D24UNormS8UInt => Silk.NET.DXGI.Format.FormatD24UnormS8Uint,
                Format.R24UNormX8Typeless => Silk.NET.DXGI.Format.FormatR24UnormX8Typeless,
                Format.X24TypelessG8UInt => Silk.NET.DXGI.Format.FormatX24TypelessG8Uint,
                Format.R8G8Typeless => Silk.NET.DXGI.Format.FormatR8G8Typeless,
                Format.R8G8UNorm => Silk.NET.DXGI.Format.FormatR8G8Unorm,
                Format.R8G8UInt => Silk.NET.DXGI.Format.FormatR8G8Uint,
                Format.R8G8SNorm => Silk.NET.DXGI.Format.FormatR8G8SNorm,
                Format.R8G8Sint => Silk.NET.DXGI.Format.FormatR8G8Sint,
                Format.R16Typeless => Silk.NET.DXGI.Format.FormatR16Typeless,
                Format.R16Float => Silk.NET.DXGI.Format.FormatR16Float,
                Format.D16UNorm => Silk.NET.DXGI.Format.FormatD16Unorm,
                Format.R16UNorm => Silk.NET.DXGI.Format.FormatR16Unorm,
                Format.R16UInt => Silk.NET.DXGI.Format.FormatR16Uint,
                Format.R16SNorm => Silk.NET.DXGI.Format.FormatR16SNorm,
                Format.R16Sint => Silk.NET.DXGI.Format.FormatR16Sint,
                Format.R8Typeless => Silk.NET.DXGI.Format.FormatR8Typeless,
                Format.R8UNorm => Silk.NET.DXGI.Format.FormatR8Unorm,
                Format.R8UInt => Silk.NET.DXGI.Format.FormatR8Uint,
                Format.R8SNorm => Silk.NET.DXGI.Format.FormatR8SNorm,
                Format.R8SInt => Silk.NET.DXGI.Format.FormatR8Sint,
                Format.A8UNorm => Silk.NET.DXGI.Format.FormatA8Unorm,
                Format.R1UNorm => Silk.NET.DXGI.Format.FormatR1Unorm,
                Format.R9G9B9E5SharedExp => Silk.NET.DXGI.Format.FormatR9G9B9E5Sharedexp,
                Format.R8G8B8G8UNorm => Silk.NET.DXGI.Format.FormatR8G8B8G8Unorm,
                Format.G8R8G8B8UNorm => Silk.NET.DXGI.Format.FormatG8R8G8B8Unorm,
                Format.BC1Typeless => Silk.NET.DXGI.Format.FormatBC1Typeless,
                Format.BC1UNorm => Silk.NET.DXGI.Format.FormatBC1Unorm,
                Format.BC1UNormSRGB => Silk.NET.DXGI.Format.FormatBC1UnormSrgb,
                Format.BC2Typeless => Silk.NET.DXGI.Format.FormatBC2Typeless,
                Format.BC2UNorm => Silk.NET.DXGI.Format.FormatBC2Unorm,
                Format.BC2UNormSRGB => Silk.NET.DXGI.Format.FormatBC2UnormSrgb,
                Format.BC3Typeless => Silk.NET.DXGI.Format.FormatBC3Typeless,
                Format.BC3UNorm => Silk.NET.DXGI.Format.FormatBC3Unorm,
                Format.BC3UNormSRGB => Silk.NET.DXGI.Format.FormatBC3UnormSrgb,
                Format.BC4Typeless => Silk.NET.DXGI.Format.FormatBC4Typeless,
                Format.BC4UNorm => Silk.NET.DXGI.Format.FormatBC4Unorm,
                Format.BC4SNorm => Silk.NET.DXGI.Format.FormatBC4SNorm,
                Format.BC5Typeless => Silk.NET.DXGI.Format.FormatBC5Typeless,
                Format.BC5UNorm => Silk.NET.DXGI.Format.FormatBC5Unorm,
                Format.BC5SNorm => Silk.NET.DXGI.Format.FormatBC5SNorm,
                Format.B5G6R5UNorm => Silk.NET.DXGI.Format.FormatB5G6R5Unorm,
                Format.B5G5R5A1UNorm => Silk.NET.DXGI.Format.FormatB5G5R5A1Unorm,
                Format.B8G8R8A8UNorm => Silk.NET.DXGI.Format.FormatB8G8R8A8Unorm,
                Format.B8G8R8X8UNorm => Silk.NET.DXGI.Format.FormatB8G8R8X8Unorm,
                Format.R10G10B10XRBiasA2UNorm => Silk.NET.DXGI.Format.FormatR10G10B10XRBiasA2Unorm,
                Format.B8G8R8A8Typeless => Silk.NET.DXGI.Format.FormatB8G8R8A8Typeless,
                Format.B8G8R8A8UNormSRGB => Silk.NET.DXGI.Format.FormatB8G8R8A8UnormSrgb,
                Format.B8G8R8X8Typeless => Silk.NET.DXGI.Format.FormatB8G8R8X8Typeless,
                Format.B8G8R8X8UNormSRGB => Silk.NET.DXGI.Format.FormatB8G8R8X8UnormSrgb,
                Format.BC6HTypeless => Silk.NET.DXGI.Format.FormatBC6HTypeless,
                Format.BC6HUF16 => Silk.NET.DXGI.Format.FormatBC6HUF16,
                Format.BC6HSF16 => Silk.NET.DXGI.Format.FormatBC6HSF16,
                Format.BC7Typeless => Silk.NET.DXGI.Format.FormatBC7Typeless,
                Format.BC7UNorm => Silk.NET.DXGI.Format.FormatBC7Unorm,
                Format.BC7UNormSRGB => Silk.NET.DXGI.Format.FormatBC7UnormSrgb,
                Format.AYUV => Silk.NET.DXGI.Format.FormatAyuv,
                Format.Y410 => Silk.NET.DXGI.Format.FormatY410,
                Format.Y416 => Silk.NET.DXGI.Format.FormatY416,
                Format.NV12 => Silk.NET.DXGI.Format.FormatNV12,
                Format.P010 => Silk.NET.DXGI.Format.FormatP010,
                Format.P016 => Silk.NET.DXGI.Format.FormatP016,
                Format.Opaque420 => Silk.NET.DXGI.Format.Format420Opaque,
                Format.YUY2 => Silk.NET.DXGI.Format.FormatYuy2,
                Format.Y210 => Silk.NET.DXGI.Format.FormatY210,
                Format.Y216 => Silk.NET.DXGI.Format.FormatY216,
                Format.NV11 => Silk.NET.DXGI.Format.FormatNV11,
                Format.AI44 => Silk.NET.DXGI.Format.FormatAI44,
                Format.IA44 => Silk.NET.DXGI.Format.FormatIA44,
                Format.P8 => Silk.NET.DXGI.Format.FormatP8,
                Format.A8P8 => Silk.NET.DXGI.Format.FormatA8P8,
                Format.B4G4R4A4UNorm => Silk.NET.DXGI.Format.FormatB4G4R4A4Unorm,
                Format.P208 => Silk.NET.DXGI.Format.FormatP208,
                Format.V208 => Silk.NET.DXGI.Format.FormatV208,
                Format.V408 => Silk.NET.DXGI.Format.FormatV408,
                Format.SamplerFeedbackMinMipOpaque => Silk.NET.DXGI.Format.FormatSamplerFeedbackMinMipOpaque,
                Format.SamplerFeedbackMipRegionUsedOpaque => Silk.NET.DXGI.Format.FormatSamplerFeedbackMipRegionUsedOpaque,
                Format.ForceUInt => Silk.NET.DXGI.Format.FormatForceUint,
                _ => Silk.NET.DXGI.Format.FormatUnknown,
            };
        }

        public static Format ConvertBack(Silk.NET.DXGI.Format format)
        {
            return format switch
            {
                Silk.NET.DXGI.Format.FormatUnknown => Format.Unknown,
                Silk.NET.DXGI.Format.FormatR32G32B32A32Typeless => Format.R32G32B32A32Typeless,
                Silk.NET.DXGI.Format.FormatR32G32B32A32Float => Format.R32G32B32A32Float,
                Silk.NET.DXGI.Format.FormatR32G32B32A32Uint => Format.R32G32B32A32UInt,
                Silk.NET.DXGI.Format.FormatR32G32B32A32Sint => Format.R32G32B32A32SInt,
                Silk.NET.DXGI.Format.FormatR32G32B32Typeless => Format.R32G32B32Typeless,
                Silk.NET.DXGI.Format.FormatR32G32B32Float => Format.R32G32B32Float,
                Silk.NET.DXGI.Format.FormatR32G32B32Uint => Format.R32G32B32UInt,
                Silk.NET.DXGI.Format.FormatR32G32B32Sint => Format.R32G32B32SInt,
                Silk.NET.DXGI.Format.FormatR16G16B16A16Typeless => Format.R16G16B16A16Typeless,
                Silk.NET.DXGI.Format.FormatR16G16B16A16Float => Format.R16G16B16A16Float,
                Silk.NET.DXGI.Format.FormatR16G16B16A16Unorm => Format.R16G16B16A16UNorm,
                Silk.NET.DXGI.Format.FormatR16G16B16A16Uint => Format.R16G16B16A16UInt,
                Silk.NET.DXGI.Format.FormatR16G16B16A16SNorm => Format.R16G16B16A16SNorm,
                Silk.NET.DXGI.Format.FormatR16G16B16A16Sint => Format.R16G16B16A16Sint,
                Silk.NET.DXGI.Format.FormatR32G32Typeless => Format.R32G32Typeless,
                Silk.NET.DXGI.Format.FormatR32G32Float => Format.R32G32Float,
                Silk.NET.DXGI.Format.FormatR32G32Uint => Format.R32G32UInt,
                Silk.NET.DXGI.Format.FormatR32G32Sint => Format.R32G32SInt,
                Silk.NET.DXGI.Format.FormatR32G8X24Typeless => Format.R32G8X24Typeless,
                Silk.NET.DXGI.Format.FormatD32FloatS8X24Uint => Format.D32FloatS8X24UInt,
                Silk.NET.DXGI.Format.FormatR32FloatX8X24Typeless => Format.R32FloatX8X24Typeless,
                Silk.NET.DXGI.Format.FormatX32TypelessG8X24Uint => Format.X32TypelessG8X24UInt,
                Silk.NET.DXGI.Format.FormatR10G10B10A2Typeless => Format.R10G10B10A2Typeless,
                Silk.NET.DXGI.Format.FormatR10G10B10A2Unorm => Format.R10G10B10A2UNorm,
                Silk.NET.DXGI.Format.FormatR10G10B10A2Uint => Format.R10G10B10A2UInt,
                Silk.NET.DXGI.Format.FormatR11G11B10Float => Format.R11G11B10Float,
                Silk.NET.DXGI.Format.FormatR8G8B8A8Typeless => Format.R8G8B8A8Typeless,
                Silk.NET.DXGI.Format.FormatR8G8B8A8Unorm => Format.R8G8B8A8UNorm,
                Silk.NET.DXGI.Format.FormatR8G8B8A8UnormSrgb => Format.R8G8B8A8UNormSRGB,
                Silk.NET.DXGI.Format.FormatR8G8B8A8Uint => Format.R8G8B8A8UInt,
                Silk.NET.DXGI.Format.FormatR8G8B8A8SNorm => Format.R8G8B8A8SNorm,
                Silk.NET.DXGI.Format.FormatR8G8B8A8Sint => Format.R8G8B8A8SInt,
                Silk.NET.DXGI.Format.FormatR16G16Typeless => Format.R16G16Typeless,
                Silk.NET.DXGI.Format.FormatR16G16Float => Format.R16G16Float,
                Silk.NET.DXGI.Format.FormatR16G16Unorm => Format.R16G16UNorm,
                Silk.NET.DXGI.Format.FormatR16G16Uint => Format.R16G16UInt,
                Silk.NET.DXGI.Format.FormatR16G16SNorm => Format.R16G16SNorm,
                Silk.NET.DXGI.Format.FormatR16G16Sint => Format.R16G16Sint,
                Silk.NET.DXGI.Format.FormatR32Typeless => Format.R32Typeless,
                Silk.NET.DXGI.Format.FormatD32Float => Format.D32Float,
                Silk.NET.DXGI.Format.FormatR32Float => Format.R32Float,
                Silk.NET.DXGI.Format.FormatR32Uint => Format.R32UInt,
                Silk.NET.DXGI.Format.FormatR32Sint => Format.R32SInt,
                Silk.NET.DXGI.Format.FormatR24G8Typeless => Format.R24G8Typeless,
                Silk.NET.DXGI.Format.FormatD24UnormS8Uint => Format.D24UNormS8UInt,
                Silk.NET.DXGI.Format.FormatR24UnormX8Typeless => Format.R24UNormX8Typeless,
                Silk.NET.DXGI.Format.FormatX24TypelessG8Uint => Format.X24TypelessG8UInt,
                Silk.NET.DXGI.Format.FormatR8G8Typeless => Format.R8G8Typeless,
                Silk.NET.DXGI.Format.FormatR8G8Unorm => Format.R8G8UNorm,
                Silk.NET.DXGI.Format.FormatR8G8Uint => Format.R8G8UInt,
                Silk.NET.DXGI.Format.FormatR8G8SNorm => Format.R8G8SNorm,
                Silk.NET.DXGI.Format.FormatR8G8Sint => Format.R8G8Sint,
                Silk.NET.DXGI.Format.FormatR16Typeless => Format.R16Typeless,
                Silk.NET.DXGI.Format.FormatR16Float => Format.R16Float,
                Silk.NET.DXGI.Format.FormatD16Unorm => Format.D16UNorm,
                Silk.NET.DXGI.Format.FormatR16Unorm => Format.R16UNorm,
                Silk.NET.DXGI.Format.FormatR16Uint => Format.R16UInt,
                Silk.NET.DXGI.Format.FormatR16SNorm => Format.R16SNorm,
                Silk.NET.DXGI.Format.FormatR16Sint => Format.R16Sint,
                Silk.NET.DXGI.Format.FormatR8Typeless => Format.R8Typeless,
                Silk.NET.DXGI.Format.FormatR8Unorm => Format.R8UNorm,
                Silk.NET.DXGI.Format.FormatR8Uint => Format.R8UInt,
                Silk.NET.DXGI.Format.FormatR8SNorm => Format.R8SNorm,
                Silk.NET.DXGI.Format.FormatR8Sint => Format.R8SInt,
                Silk.NET.DXGI.Format.FormatA8Unorm => Format.A8UNorm,
                Silk.NET.DXGI.Format.FormatR1Unorm => Format.R1UNorm,
                Silk.NET.DXGI.Format.FormatR9G9B9E5Sharedexp => Format.R9G9B9E5SharedExp,
                Silk.NET.DXGI.Format.FormatR8G8B8G8Unorm => Format.R8G8B8G8UNorm,
                Silk.NET.DXGI.Format.FormatG8R8G8B8Unorm => Format.G8R8G8B8UNorm,
                Silk.NET.DXGI.Format.FormatBC1Typeless => Format.BC1Typeless,
                Silk.NET.DXGI.Format.FormatBC1Unorm => Format.BC1UNorm,
                Silk.NET.DXGI.Format.FormatBC1UnormSrgb => Format.BC1UNormSRGB,
                Silk.NET.DXGI.Format.FormatBC2Typeless => Format.BC2Typeless,
                Silk.NET.DXGI.Format.FormatBC2Unorm => Format.BC2UNorm,
                Silk.NET.DXGI.Format.FormatBC2UnormSrgb => Format.BC2UNormSRGB,
                Silk.NET.DXGI.Format.FormatBC3Typeless => Format.BC3Typeless,
                Silk.NET.DXGI.Format.FormatBC3Unorm => Format.BC3UNorm,
                Silk.NET.DXGI.Format.FormatBC3UnormSrgb => Format.BC3UNormSRGB,
                Silk.NET.DXGI.Format.FormatBC4Typeless => Format.BC4Typeless,
                Silk.NET.DXGI.Format.FormatBC4Unorm => Format.BC4UNorm,
                Silk.NET.DXGI.Format.FormatBC4SNorm => Format.BC4SNorm,
                Silk.NET.DXGI.Format.FormatBC5Typeless => Format.BC5Typeless,
                Silk.NET.DXGI.Format.FormatBC5Unorm => Format.BC5UNorm,
                Silk.NET.DXGI.Format.FormatBC5SNorm => Format.BC5SNorm,
                Silk.NET.DXGI.Format.FormatB5G6R5Unorm => Format.B5G6R5UNorm,
                Silk.NET.DXGI.Format.FormatB5G5R5A1Unorm => Format.B5G5R5A1UNorm,
                Silk.NET.DXGI.Format.FormatB8G8R8A8Unorm => Format.B8G8R8A8UNorm,
                Silk.NET.DXGI.Format.FormatB8G8R8X8Unorm => Format.B8G8R8X8UNorm,
                Silk.NET.DXGI.Format.FormatR10G10B10XRBiasA2Unorm => Format.R10G10B10XRBiasA2UNorm,
                Silk.NET.DXGI.Format.FormatB8G8R8A8Typeless => Format.B8G8R8A8Typeless,
                Silk.NET.DXGI.Format.FormatB8G8R8A8UnormSrgb => Format.B8G8R8A8UNormSRGB,
                Silk.NET.DXGI.Format.FormatB8G8R8X8Typeless => Format.B8G8R8X8Typeless,
                Silk.NET.DXGI.Format.FormatB8G8R8X8UnormSrgb => Format.B8G8R8X8UNormSRGB,
                Silk.NET.DXGI.Format.FormatBC6HTypeless => Format.BC6HTypeless,
                Silk.NET.DXGI.Format.FormatBC6HUF16 => Format.BC6HUF16,
                Silk.NET.DXGI.Format.FormatBC6HSF16 => Format.BC6HSF16,
                Silk.NET.DXGI.Format.FormatBC7Typeless => Format.BC7Typeless,
                Silk.NET.DXGI.Format.FormatBC7Unorm => Format.BC7UNorm,
                Silk.NET.DXGI.Format.FormatBC7UnormSrgb => Format.BC7UNormSRGB,
                Silk.NET.DXGI.Format.FormatAyuv => Format.AYUV,
                Silk.NET.DXGI.Format.FormatY410 => Format.Y410,
                Silk.NET.DXGI.Format.FormatY416 => Format.Y416,
                Silk.NET.DXGI.Format.FormatNV12 => Format.NV12,
                Silk.NET.DXGI.Format.FormatP010 => Format.P010,
                Silk.NET.DXGI.Format.FormatP016 => Format.P016,
                Silk.NET.DXGI.Format.Format420Opaque => Format.Opaque420,
                Silk.NET.DXGI.Format.FormatYuy2 => Format.YUY2,
                Silk.NET.DXGI.Format.FormatY210 => Format.Y210,
                Silk.NET.DXGI.Format.FormatY216 => Format.Y216,
                Silk.NET.DXGI.Format.FormatNV11 => Format.NV11,
                Silk.NET.DXGI.Format.FormatAI44 => Format.AI44,
                Silk.NET.DXGI.Format.FormatIA44 => Format.IA44,
                Silk.NET.DXGI.Format.FormatP8 => Format.P8,
                Silk.NET.DXGI.Format.FormatA8P8 => Format.A8P8,
                Silk.NET.DXGI.Format.FormatB4G4R4A4Unorm => Format.B4G4R4A4UNorm,
                Silk.NET.DXGI.Format.FormatP208 => Format.P208,
                Silk.NET.DXGI.Format.FormatV208 => Format.V208,
                Silk.NET.DXGI.Format.FormatV408 => Format.V408,
                Silk.NET.DXGI.Format.FormatSamplerFeedbackMinMipOpaque => Format.SamplerFeedbackMinMipOpaque,
                Silk.NET.DXGI.Format.FormatSamplerFeedbackMipRegionUsedOpaque => Format.SamplerFeedbackMipRegionUsedOpaque,
                Silk.NET.DXGI.Format.FormatForceUint => Format.ForceUInt,
                _ => Format.Unknown,
            };
        }

        internal static ShaderInputBindDescription Convert(Silk.NET.Direct3D11.ShaderInputBindDesc shaderInputDesc)
        {
            return new()
            {
                BindCount = shaderInputDesc.BindCount,
                BindPoint = shaderInputDesc.BindPoint,
                Dimension = Convert(shaderInputDesc.Dimension),
                Name = Utils.ToStr(shaderInputDesc.Name),
                NumSamples = shaderInputDesc.NumSamples,
                ReturnType = Convert(shaderInputDesc.ReturnType),
                Type = Convert(shaderInputDesc.Type),
                UFlags = shaderInputDesc.UFlags
            };
        }

        private static ShaderInputType Convert(D3DShaderInputType type)
        {
            return type switch
            {
                D3DShaderInputType.D3DSitCbuffer => ShaderInputType.SitCbuffer,
                D3DShaderInputType.D3DSitTbuffer => ShaderInputType.SitTbuffer,
                D3DShaderInputType.D3DSitTexture => ShaderInputType.SitTexture,
                D3DShaderInputType.D3DSitSampler => ShaderInputType.SitSampler,
                D3DShaderInputType.D3DSitUavRwtyped => ShaderInputType.SitUavRwtyped,
                D3DShaderInputType.D3DSitStructured => ShaderInputType.SitStructured,
                D3DShaderInputType.D3DSitUavRwstructured => ShaderInputType.SitUavRwstructured,
                D3DShaderInputType.D3DSitByteaddress => ShaderInputType.SitByteaddress,
                D3DShaderInputType.D3DSitUavRwbyteaddress => ShaderInputType.SitUavRwbyteaddress,
                D3DShaderInputType.D3DSitUavAppendStructured => ShaderInputType.SitUavAppendStructured,
                D3DShaderInputType.D3DSitUavConsumeStructured => ShaderInputType.SitUavConsumeStructured,
                D3DShaderInputType.D3DSitUavRwstructuredWithCounter => ShaderInputType.SitUavRwstructuredWithCounter,
                D3DShaderInputType.D3DSitRtaccelerationstructure => ShaderInputType.SitRtaccelerationstructure,
                D3DShaderInputType.D3DSitUavFeedbacktexture => ShaderInputType.SitUavFeedbacktexture,
                _ => throw new NotImplementedException(),
            };
        }

        private static ResourceReturnType Convert(D3DResourceReturnType returnType)
        {
            return returnType switch
            {
                D3DResourceReturnType.None => ResourceReturnType.None,
                D3DResourceReturnType.D3DReturnTypeUnorm => ResourceReturnType.Unorm,
                D3DResourceReturnType.D3DReturnTypeSNorm => ResourceReturnType.SNorm,
                D3DResourceReturnType.D3DReturnTypeSint => ResourceReturnType.Sint,
                D3DResourceReturnType.D3DReturnTypeUint => ResourceReturnType.Uint,
                D3DResourceReturnType.D3DReturnTypeFloat => ResourceReturnType.Float,
                D3DResourceReturnType.D3DReturnTypeMixed => ResourceReturnType.Mixed,
                D3DResourceReturnType.D3DReturnTypeDouble => ResourceReturnType.Double,
                D3DResourceReturnType.D3DReturnTypeContinued => ResourceReturnType.Continued,
                _ => throw new NotImplementedException(),
            };
        }

        private static SrvDimension Convert(D3DSrvDimension dimension)
        {
            return dimension switch
            {
                D3DSrvDimension.D3DSrvDimensionUnknown => SrvDimension.Unknown,
                D3DSrvDimension.D3DSrvDimensionBuffer => SrvDimension.Buffer,
                D3DSrvDimension.D3DSrvDimensionTexture1D => SrvDimension.Texture1D,
                D3DSrvDimension.D3DSrvDimensionTexture1Darray => SrvDimension.Texture1Darray,
                D3DSrvDimension.D3DSrvDimensionTexture2D => SrvDimension.Texture2D,
                D3DSrvDimension.D3DSrvDimensionTexture2Darray => SrvDimension.Texture2Darray,
                D3DSrvDimension.D3DSrvDimensionTexture2Dms => SrvDimension.Texture2Dms,
                D3DSrvDimension.D3DSrvDimensionTexture2Dmsarray => SrvDimension.Texture2Dmsarray,
                D3DSrvDimension.D3DSrvDimensionTexture3D => SrvDimension.Texture3D,
                D3DSrvDimension.D3DSrvDimensionTexturecube => SrvDimension.Texturecube,
                D3DSrvDimension.D3DSrvDimensionTexturecubearray => SrvDimension.Texturecubearray,
                D3DSrvDimension.D3DSrvDimensionBufferex => SrvDimension.Bufferex,
                _ => throw new NotSupportedException()
            };
        }

        internal static SignatureParameterDescription Convert(Silk.NET.Direct3D11.SignatureParameterDesc shaderInputDesc)
        {
            return new()
            {
                Mask = shaderInputDesc.Mask,
                ComponentType = Convert(shaderInputDesc.ComponentType),
                MinPrecision = Convert(shaderInputDesc.MinPrecision),
                ReadWriteMask = shaderInputDesc.ReadWriteMask,
                Register = shaderInputDesc.Register,
                SemanticIndex = shaderInputDesc.SemanticIndex,
                SemanticName = Utils.ToStr(shaderInputDesc.SemanticName),
                Stream = shaderInputDesc.Stream,
                SystemValueType = Convert(shaderInputDesc.SystemValueType)
            };
        }

        private static Name Convert(D3DName systemValueType)
        {
            return systemValueType switch
            {
                D3DName.D3DNameUndefined => Name.Undefined,
                D3DName.D3DNamePosition => Name.Position,
                D3DName.D3DNameClipDistance => Name.ClipDistance,
                D3DName.D3DNameCullDistance => Name.CullDistance,
                D3DName.D3DNameRenderTargetArrayIndex => Name.RenderTargetArrayIndex,
                D3DName.D3DNameViewportArrayIndex => Name.ViewportArrayIndex,
                D3DName.D3DNameVertexID => Name.VertexID,
                D3DName.D3DNamePrimitiveID => Name.PrimitiveID,
                D3DName.D3DNameInstanceID => Name.InstanceID,
                D3DName.D3DNameIsFrontFace => Name.IsFrontFace,
                D3DName.D3DNameSampleIndex => Name.SampleIndex,
                D3DName.D3DNameFinalQuadEdgeTessfactor => Name.FinalQuadEdgeTessfactor,
                D3DName.D3DNameFinalQuadInsideTessfactor => Name.FinalQuadInsideTessfactor,
                D3DName.D3DNameFinalTriEdgeTessfactor => Name.FinalTriEdgeTessfactor,
                D3DName.D3DNameFinalTriInsideTessfactor => Name.FinalTriInsideTessfactor,
                D3DName.D3DNameFinalLineDetailTessfactor => Name.FinalLineDetailTessfactor,
                D3DName.D3DNameFinalLineDensityTessfactor => Name.FinalLineDensityTessfactor,
                D3DName.D3DNameBarycentrics => Name.Barycentrics,
                D3DName.D3DNameShadingrate => Name.Shadingrate,
                D3DName.D3DNameCullprimitive => Name.Cullprimitive,
                D3DName.D3DNameTarget => Name.Target,
                D3DName.D3DNameDepth => Name.Depth,
                D3DName.D3DNameCoverage => Name.Coverage,
                D3DName.D3DNameDepthGreaterEqual => Name.DepthGreaterEqual,
                D3DName.D3DNameDepthLessEqual => Name.DepthLessEqual,
                D3DName.D3DNameStencilRef => Name.StencilRef,
                D3DName.D3DNameInnerCoverage => Name.InnerCoverage,
                _ => throw new NotImplementedException(),
            };
        }

        private static MinPrecision Convert(D3DMinPrecision minPrecision)
        {
            return minPrecision switch
            {
                D3DMinPrecision.Default => MinPrecision.Default,
                D3DMinPrecision.Float16 => MinPrecision.Float16,
                D3DMinPrecision.Float28 => MinPrecision.Float28,
                D3DMinPrecision.Reserved => MinPrecision.Reserved,
                D3DMinPrecision.Sint16 => MinPrecision.Sint16,
                D3DMinPrecision.Uint16 => MinPrecision.Uint16,
                D3DMinPrecision.Any16 => MinPrecision.Any16,
                D3DMinPrecision.Any10 => MinPrecision.Any10,
                _ => throw new NotImplementedException(),
            };
        }

        private static RegisterComponentType Convert(D3DRegisterComponentType componentType)
        {
            return componentType switch
            {
                0 => RegisterComponentType.Unknown,
                D3DRegisterComponentType.D3DRegisterComponentUint32 => RegisterComponentType.Uint32,
                D3DRegisterComponentType.D3DRegisterComponentSint32 => RegisterComponentType.Sint32,
                D3DRegisterComponentType.D3DRegisterComponentFloat32 => RegisterComponentType.Float32,
                _ => throw new NotImplementedException(),
            };
        }

        internal static Silk.NET.Direct3D11.UnorderedAccessViewDesc Convert(UnorderedAccessViewDescription description)
        {
            Silk.NET.Direct3D11.UnorderedAccessViewDesc result = new()
            {
                Format = Convert(description.Format),
                ViewDimension = Convert(description.ViewDimension),
                Anonymous = new(),
            };

            switch (description.ViewDimension)
            {
                case UnorderedAccessViewDimension.Unknown:
                    break;

                case UnorderedAccessViewDimension.Buffer:
                    result.Anonymous.Buffer = Convert(description.Buffer);
                    break;

                case UnorderedAccessViewDimension.Texture1D:
                    result.Anonymous.Texture1D = Convert(description.Texture1D);
                    break;

                case UnorderedAccessViewDimension.Texture1DArray:
                    result.Anonymous.Texture1DArray = Convert(description.Texture1DArray);
                    break;

                case UnorderedAccessViewDimension.Texture2D:
                    result.Anonymous.Texture2D = Convert(description.Texture2D);
                    break;

                case UnorderedAccessViewDimension.Texture2DArray:
                    result.Anonymous.Texture2DArray = Convert(description.Texture2DArray);
                    break;

                case UnorderedAccessViewDimension.Texture3D:
                    result.Anonymous.Texture3D = Convert(description.Texture3D);
                    break;
            }
            return result;
        }

        private static Silk.NET.Direct3D11.Tex3DUav Convert(Texture3DUnorderedAccessView texture3D)
        {
            return new()
            {
                FirstWSlice = (uint)texture3D.FirstWSlice,
                MipSlice = (uint)texture3D.MipSlice,
                WSize = (uint)texture3D.WSize,
            };
        }

        private static Silk.NET.Direct3D11.Tex2DArrayUav Convert(Texture2DArrayUnorderedAccessView texture2DArray)
        {
            return new()
            {
                ArraySize = (uint)texture2DArray.ArraySize,
                FirstArraySlice = (uint)texture2DArray.FirstArraySlice,
                MipSlice = (uint)texture2DArray.MipSlice,
            };
        }

        private static Silk.NET.Direct3D11.Tex2DUav Convert(Texture2DUnorderedAccessView texture2D)
        {
            return new()
            {
                MipSlice = (uint)texture2D.MipSlice
            };
        }

        private static Silk.NET.Direct3D11.Tex1DArrayUav Convert(Texture1DArrayUnorderedAccessView texture1DArray)
        {
            return new()
            {
                ArraySize = (uint)texture1DArray.ArraySize,
                FirstArraySlice = (uint)texture1DArray.FirstArraySlice,
                MipSlice = (uint)texture1DArray.MipSlice
            };
        }

        private static Silk.NET.Direct3D11.Tex1DUav Convert(Texture1DUnorderedAccessView texture1D)
        {
            return new()
            {
                MipSlice = (uint)texture1D.MipSlice
            };
        }

        private static Silk.NET.Direct3D11.BufferUav Convert(BufferUnorderedAccessView buffer)
        {
            return new()
            {
                FirstElement = (uint)buffer.FirstElement,
                Flags = (uint)Convert(buffer.Flags),
                NumElements = (uint)buffer.NumElements,
            };
        }

        private static Silk.NET.Direct3D11.BufferUavFlag Convert(BufferUnorderedAccessViewFlags flags)
        {
            return flags switch
            {
                BufferUnorderedAccessViewFlags.Raw => Silk.NET.Direct3D11.BufferUavFlag.Raw,
                BufferUnorderedAccessViewFlags.Append => Silk.NET.Direct3D11.BufferUavFlag.Append,
                BufferUnorderedAccessViewFlags.Counter => Silk.NET.Direct3D11.BufferUavFlag.Counter,
                BufferUnorderedAccessViewFlags.None => 0,
                _ => throw new NotImplementedException(),
            };
        }

        private static Silk.NET.Direct3D11.UavDimension Convert(UnorderedAccessViewDimension viewDimension)
        {
            return viewDimension switch
            {
                UnorderedAccessViewDimension.Unknown => Silk.NET.Direct3D11.UavDimension.Unknown,
                UnorderedAccessViewDimension.Buffer => Silk.NET.Direct3D11.UavDimension.Buffer,
                UnorderedAccessViewDimension.Texture1D => Silk.NET.Direct3D11.UavDimension.Texture1D,
                UnorderedAccessViewDimension.Texture1DArray => Silk.NET.Direct3D11.UavDimension.Texture1Darray,
                UnorderedAccessViewDimension.Texture2D => Silk.NET.Direct3D11.UavDimension.Texture2D,
                UnorderedAccessViewDimension.Texture2DArray => Silk.NET.Direct3D11.UavDimension.Texture2Darray,
                UnorderedAccessViewDimension.Texture3D => Silk.NET.Direct3D11.UavDimension.Texture3D,
                _ => throw new NotImplementedException(),
            };
        }

        public static DirectXTex.TexMetadata Convert(TexMetadata metadata)
        {
            DirectXTex.TexMetadata texMetadata;
            texMetadata.Format = (int)Convert(metadata.Format);
            texMetadata.ArraySize = (nuint)metadata.ArraySize;
            texMetadata.Width = (nuint)metadata.Width;
            texMetadata.Height = (nuint)metadata.Height;
            texMetadata.Depth = (nuint)metadata.Depth;
            texMetadata.MipLevels = (nuint)metadata.MipLevels;
            texMetadata.Dimension = Convert(metadata.Dimension);
            texMetadata.MiscFlags = (uint)Convert(metadata.MiscFlags);
            texMetadata.MiscFlags2 = 0;
            texMetadata.SetAlphaMode(Convert(metadata.AlphaMode));
            return texMetadata;
        }

        public static DirectXTex.TexAlphaMode Convert(TexAlphaMode mode)
        {
            return mode switch
            {
                TexAlphaMode.Unknown => DirectXTex.TexAlphaMode.Unknown,
                TexAlphaMode.Straight => DirectXTex.TexAlphaMode.Straight,
                TexAlphaMode.Premultiplied => DirectXTex.TexAlphaMode.Premultiplied,
                TexAlphaMode.Opaque => DirectXTex.TexAlphaMode.Opaque,
                TexAlphaMode.Custom => DirectXTex.TexAlphaMode.Custom,
                _ => throw new NotImplementedException(),
            };
        }

        public static DirectXTex.TexMiscFlag Convert(TexMiscFlags flags)
        {
            DirectXTex.TexMiscFlag result = 0;

            if ((flags & TexMiscFlags.TextureCube) != 0)
            {
                result |= DirectXTex.TexMiscFlag.Texturecube;
            }

            return result;
        }

        public static DirectXTex.TexDimension Convert(TexDimension dimension)
        {
            return dimension switch
            {
                TexDimension.Texture1D => DirectXTex.TexDimension.Texture1D,
                TexDimension.Texture2D => DirectXTex.TexDimension.Texture2D,
                TexDimension.Texture3D => DirectXTex.TexDimension.Texture3D,
                _ => throw new NotImplementedException(),
            };
        }

        public static TexMetadata ConvertBack(DirectXTex.TexMetadata metadata)
        {
            TexMetadata texMetadata;
            texMetadata.Format = ConvertBack((Silk.NET.DXGI.Format)metadata.Format);
            texMetadata.ArraySize = (int)metadata.ArraySize;
            texMetadata.Width = (int)metadata.Width;
            texMetadata.Height = (int)metadata.Height;
            texMetadata.Depth = (int)metadata.Depth;
            texMetadata.MipLevels = (int)metadata.MipLevels;
            texMetadata.Dimension = ConvertBack(metadata.Dimension);
            texMetadata.MiscFlags = ConvertBack((DirectXTex.TexMiscFlag)metadata.MiscFlags);
            texMetadata.AlphaMode = ConvertBack(metadata.GetAlphaMode());
            return texMetadata;
        }

        private static TexDimension ConvertBack(DirectXTex.TexDimension dimension)
        {
            return dimension switch
            {
                DirectXTex.TexDimension.Texture1D => TexDimension.Texture1D,
                DirectXTex.TexDimension.Texture2D => TexDimension.Texture2D,
                DirectXTex.TexDimension.Texture3D => TexDimension.Texture3D,
                _ => throw new NotImplementedException(),
            };
        }

        private static TexMiscFlags ConvertBack(DirectXTex.TexMiscFlag flags)
        {
            TexMiscFlags result = 0;

            if ((flags & DirectXTex.TexMiscFlag.Texturecube) != 0)
            {
                result |= TexMiscFlags.TextureCube;
            }

            return result;
        }

        private static TexAlphaMode ConvertBack(DirectXTex.TexAlphaMode alphaMode)
        {
            return alphaMode switch
            {
                DirectXTex.TexAlphaMode.Unknown => TexAlphaMode.Unknown,
                DirectXTex.TexAlphaMode.Straight => TexAlphaMode.Straight,
                DirectXTex.TexAlphaMode.Premultiplied => TexAlphaMode.Premultiplied,
                DirectXTex.TexAlphaMode.Opaque => TexAlphaMode.Opaque,
                DirectXTex.TexAlphaMode.Custom => TexAlphaMode.Custom,
                _ => throw new NotImplementedException(),
            };
        }
    }
}