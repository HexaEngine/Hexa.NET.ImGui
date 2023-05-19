namespace HexaEngine.Core.Graphics.Textures
{
    using HexaEngine.Core.Graphics;
    using System;

    public struct TexMetadata : IEquatable<TexMetadata>
    {
        public int Width;
        public int Height;     // Should be 1 for 1D textures
        public int Depth;      // Should be 1 for 1D or 2D textures
        public int ArraySize;  // For cubemap, this is a multiple of 6
        public int MipLevels;
        public TexMiscFlags MiscFlags;
        public TexAlphaMode AlphaMode;
        public Format Format;
        public TexDimension Dimension;

        public int ComputeIndex(int mip, int item, int slice)
        {
            if (mip >= MipLevels)
            {
                return -1;
            }

            switch (Dimension)
            {
                case TexDimension.Texture1D:
                case TexDimension.Texture2D:
                    if (slice > 0)
                    {
                        return -1;
                    }

                    if (item >= ArraySize)
                    {
                        return -1;
                    }

                    return item * MipLevels + mip;

                case TexDimension.Texture3D:
                    if (item > 0)
                    {
                        // No support for arrays of volumes
                        return -1;
                    }
                    else
                    {
                        int index = 0;
                        int d = Depth;

                        for (int level = 0; level < mip; ++level)
                        {
                            index += d;
                            if (d > 1)
                            {
                                d >>= 1;
                            }
                        }

                        if (slice >= d)
                        {
                            return -1;
                        }

                        index += slice;

                        return index;
                    }

                default:
                    return -1;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is TexMetadata metadata && Equals(metadata);
        }

        public bool Equals(TexMetadata other)
        {
            return Width == other.Width &&
                   Height == other.Height &&
                   Depth == other.Depth &&
                   ArraySize == other.ArraySize &&
                   MipLevels == other.MipLevels &&
                   MiscFlags == other.MiscFlags &&
                   AlphaMode == other.AlphaMode &&
                   Format == other.Format &&
                   Dimension == other.Dimension;
        }

        public bool IsCubemap()
        {
            return (MiscFlags & TexMiscFlags.TextureCube) != 0;
        }

        public bool IsPMAlpha()
        {
            return AlphaMode == TexAlphaMode.Premultiplied;
        }

        public bool IsVolumemap()
        {
            return Dimension == TexDimension.Texture3D;
        }

        public static bool operator ==(TexMetadata left, TexMetadata right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TexMetadata left, TexMetadata right)
        {
            return !(left == right);
        }
    }
}