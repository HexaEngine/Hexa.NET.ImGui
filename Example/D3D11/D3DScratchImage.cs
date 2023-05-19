using HexaEngine.Core.Graphics.Textures;

namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using HexaEngine.DirectXTex;
    using HexaEngine.Mathematics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;
    using System.IO;
    using ResourceMiscFlag = Core.Graphics.ResourceMiscFlag;
    using TexCompressFlags = TexCompressFlags;
    using TexFilterFlags = TexFilterFlags;
    using Usage = Core.Graphics.Usage;

    public unsafe class D3DScratchImage : IScratchImage
    {
        private bool _disposed;
        private ScratchImage scImage;

        public D3DScratchImage(ScratchImage outScImage)
        {
            scImage = outScImage;
        }

        public Core.Graphics.Textures.TexMetadata Metadata => Helper.ConvertBack(scImage.GetMetadata());

        public int ImageCount => (int)scImage.GetImageCount();

        public IScratchImage Compress(IGraphicsDevice device, Format format, TexCompressFlags flags)
        {
            D3D11GraphicsDevice graphicsDevice = (D3D11GraphicsDevice)device;
            ScratchImage inScImage = scImage;
            ScratchImage outScImage = DirectXTex.CreateScratchImage();
            DirectXTex.Compress4((ID3D11Device*)graphicsDevice.Device.Handle, inScImage.GetImages(), inScImage.GetImageCount(), inScImage.GetMetadata(), (int)Helper.Convert(format), Helper.Convert(flags), 1, outScImage);
            return new D3DScratchImage(outScImage);
        }

        public IScratchImage Decompress(Format format)
        {
            ScratchImage inScImage = scImage;
            ScratchImage outScImage = DirectXTex.CreateScratchImage();
            DirectXTex.Decompress2(inScImage.GetImages(), inScImage.GetImageCount(), inScImage.GetMetadata(), (int)Helper.Convert(format), outScImage);
            return new D3DScratchImage(outScImage);
        }

        public IScratchImage Convert(Format format, TexFilterFlags flags)
        {
            ScratchImage inScImage = scImage;
            ScratchImage outScImage = DirectXTex.CreateScratchImage();
            DirectXTex.Convert2(inScImage.GetImages(), inScImage.GetImageCount(), inScImage.GetMetadata(), (int)Helper.Convert(format), Helper.Convert(flags), 0, outScImage);
            return new D3DScratchImage(outScImage);
        }

        public IScratchImage GenerateMipMaps(TexFilterFlags flags)
        {
            ScratchImage inScImage = scImage;
            ScratchImage outScImage = DirectXTex.CreateScratchImage();
            DirectXTex.GenerateMipMaps2(inScImage.GetImages(), inScImage.GetImageCount(), inScImage.GetMetadata(), Helper.Convert(flags), (nuint)MathUtil.ComputeMipLevels(Metadata.Width, Metadata.Height), outScImage);
            return new D3DScratchImage(outScImage);
        }

        public IScratchImage Resize(float scale, TexFilterFlags flags)
        {
            ScratchImage inScImage = scImage;
            ScratchImage outScImage = DirectXTex.CreateScratchImage();
            DirectXTex.Resize2(inScImage.GetImages(), inScImage.GetImageCount(), inScImage.GetMetadata(), (nuint)(Metadata.Width * scale), (nuint)(Metadata.Height * scale), Helper.Convert(flags), outScImage);
            return new D3DScratchImage(outScImage);
        }

        public IScratchImage Resize(int width, int height, TexFilterFlags flags)
        {
            ScratchImage inScImage = scImage;
            ScratchImage outScImage = DirectXTex.CreateScratchImage();
            DirectXTex.Resize2(inScImage.GetImages(), inScImage.GetImageCount(), inScImage.GetMetadata(), (nuint)width, (nuint)height, Helper.Convert(flags), outScImage);
            return new D3DScratchImage(outScImage);
        }

        public bool OverwriteFormat(Format format)
        {
            return scImage.OverrideFormat((int)Helper.Convert(format));
        }

        public ITexture1D CreateTexture1D(IGraphicsDevice device, Usage usage, BindFlags bindFlags, CpuAccessFlags accessFlags, ResourceMiscFlag miscFlag)
        {
            D3D11GraphicsDevice graphicsDevice = (D3D11GraphicsDevice)device;
            ComPtr<ID3D11Texture1D> resource;
            var images = scImage.GetImages();
            var nimages = scImage.GetImageCount();
            var metadata = scImage.GetMetadata();
            metadata.MiscFlags = (uint)miscFlag;
            DirectXTex.CreateTextureEx((ID3D11Device*)graphicsDevice.Device.Handle, images, nimages, metadata, Helper.Convert(usage), (uint)Helper.Convert(bindFlags), (uint)Helper.Convert(accessFlags), (uint)Helper.Convert(miscFlag), CreateTexFlags.Default, (ID3D11Resource**)&resource.Handle);
            Texture1DDesc desc = default;
            resource.GetDesc(ref desc);
            Texture1DDescription texture = Helper.ConvertBack(desc);

            return new D3D11Texture1D(resource, texture);
        }

        public ITexture2D CreateTexture2D(IGraphicsDevice device, Usage usage, BindFlags bindFlags, CpuAccessFlags accessFlags, ResourceMiscFlag miscFlag)
        {
            D3D11GraphicsDevice graphicsDevice = (D3D11GraphicsDevice)device;
            ComPtr<ID3D11Texture2D> resource;
            var images = scImage.GetImages();
            var nimages = scImage.GetImageCount();
            var metadata = scImage.GetMetadata();
            metadata.MiscFlags = (uint)miscFlag;
            DirectXTex.CreateTextureEx((ID3D11Device*)graphicsDevice.Device.Handle, images, nimages, metadata, Helper.Convert(usage), (uint)Helper.Convert(bindFlags), (uint)Helper.Convert(accessFlags), (uint)Helper.Convert(miscFlag), CreateTexFlags.Default, (ID3D11Resource**)&resource.Handle);
            Texture2DDesc desc = default;
            resource.GetDesc(ref desc);
            Texture2DDescription texture = Helper.ConvertBack(desc);

            return new D3D11Texture2D(resource, texture);
        }

        public ITexture2D CreateTexture2D(IGraphicsDevice device, int index, Usage usage, BindFlags bindFlags, CpuAccessFlags accessFlags, ResourceMiscFlag miscFlag)
        {
            D3D11GraphicsDevice graphicsDevice = (D3D11GraphicsDevice)device;
            ComPtr<ID3D11Texture2D> resource;
            var images = scImage.GetImages();
            var metadata = scImage.GetMetadata();
            var image = images[index];
            metadata.Width = image.Width;
            metadata.Height = image.Height;
            metadata.ArraySize = 1;
            metadata.MipLevels = 1;
            metadata.MiscFlags = (uint)miscFlag;
            DirectXTex.CreateTextureEx((ID3D11Device*)graphicsDevice.Device.Handle, &image, 1, metadata, Helper.Convert(usage), (uint)Helper.Convert(bindFlags), (uint)Helper.Convert(accessFlags), (uint)Helper.Convert(miscFlag), CreateTexFlags.Default, (ID3D11Resource**)&resource.Handle);
            Texture2DDesc desc = default;
            resource.GetDesc(ref desc);
            Texture2DDescription texture = Helper.ConvertBack(desc);

            return new D3D11Texture2D(resource, texture);
        }

        public ITexture3D CreateTexture3D(IGraphicsDevice device, Usage usage, BindFlags bindFlags, CpuAccessFlags accessFlags, ResourceMiscFlag miscFlag)
        {
            ScratchImage inScImage = scImage;
            D3D11GraphicsDevice graphicsDevice = (D3D11GraphicsDevice)device;
            ComPtr<ID3D11Texture3D> resource;
            var images = scImage.GetImages();
            var nimages = scImage.GetImageCount();
            var metadata = scImage.GetMetadata();
            metadata.MiscFlags = (uint)miscFlag;
            DirectXTex.CreateTextureEx((ID3D11Device*)graphicsDevice.Device.Handle, images, nimages, metadata, Helper.Convert(usage), (uint)Helper.Convert(bindFlags), (uint)Helper.Convert(accessFlags), (uint)Helper.Convert(miscFlag), CreateTexFlags.Default, (ID3D11Resource**)&resource.Handle);
            Texture3DDesc desc = default;
            resource.GetDesc(ref desc);
            Texture3DDescription texture = Helper.ConvertBack(desc);

            return new D3D11Texture3D(resource, texture);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                scImage.Release();
                scImage = default;
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        public void SaveToFile(string path, TexFileFormat format, int flags)
        {
            ScratchImage image = scImage;
            var meta = image.GetMetadata();
            switch (format)
            {
                case TexFileFormat.Auto:
                    var ext = Path.GetExtension(path);
                    if (ext == ".dds")
                    {
                        SaveToFile(path, TexFileFormat.DDS, flags);
                    }
                    else if (ext == ".tga")
                    {
                        SaveToFile(path, TexFileFormat.TGA, flags);
                    }
                    else if (ext == ".hdr")
                    {
                        SaveToFile(path, TexFileFormat.HDR, flags);
                    }
                    else if (ext == ".bmp")
                    {
                        SaveToFile(path, TexFileFormat.BMP, flags);
                    }
                    else if (ext == ".jpg")
                    {
                        SaveToFile(path, TexFileFormat.JPEG, flags);
                    }
                    else if (ext == ".jpeg")
                    {
                        SaveToFile(path, TexFileFormat.JPEG, flags);
                    }
                    else if (ext == ".png")
                    {
                        SaveToFile(path, TexFileFormat.PNG, flags);
                    }
                    else if (ext == ".tiff")
                    {
                        SaveToFile(path, TexFileFormat.TIFF, flags);
                    }
                    else if (ext == ".gif")
                    {
                        SaveToFile(path, TexFileFormat.GIF, flags);
                    }
                    else if (ext == ".wmp")
                    {
                        SaveToFile(path, TexFileFormat.WMP, flags);
                    }
                    else if (ext == ".ico")
                    {
                        SaveToFile(path, TexFileFormat.ICO, flags);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    break;

                case TexFileFormat.DDS:
                    DirectXTex.SaveToDDSFile2(image.GetImages(), image.GetImageCount(), meta, (DDSFlags)flags, path);
                    break;

                case TexFileFormat.TGA:
                    DirectXTex.SaveToTGAFile(image.GetImages()[0], (TGAFlags)flags, path, ref meta);
                    break;

                case TexFileFormat.HDR:
                    DirectXTex.SaveToHDRFile(image.GetImages()[0], path);
                    break;

                default:
                    DirectXTex.SaveToWICFile2(image.GetImages(), image.GetImageCount(), (WICFlags)flags, DirectXTex.GetWICCodec(Helper.Convert(format)), path, null, default);
                    break;
            }
        }

        public void SaveToMemory(Stream stream, TexFileFormat format, int flags)
        {
            ScratchImage image = scImage;
            HexaEngine.DirectXTex.Blob blob = DirectXTex.CreateBlob();

            var meta = image.GetMetadata();
            switch (format)
            {
                case TexFileFormat.DDS:
                    DirectXTex.SaveToDDSMemory2(image.GetImages(), image.GetImageCount(), meta, (DDSFlags)flags, blob);
                    break;

                case TexFileFormat.TGA:
                    DirectXTex.SaveToTGAMemory(image.GetImages()[0], (TGAFlags)flags, blob, ref meta);
                    break;

                case TexFileFormat.HDR:
                    DirectXTex.SaveToHDRMemory(image.GetImages()[0], blob);
                    break;

                default:
                    DirectXTex.SaveToWICMemory2(image.GetImages(), image.GetImageCount(), (WICFlags)flags, DirectXTex.GetWICCodec(Helper.Convert(format)), blob, null, default);
                    break;
            }

            Span<byte> buffer = new(blob.GetBufferPointer(), (int)blob.GetBufferSize());
            stream.Write(buffer);
            blob.Release();
        }

        public IImage[] GetImages()
        {
            IImage[] images = new IImage[ImageCount];
            var pImages = scImage.GetImages();
            for (int i = 0; i < images.Length; i++)
            {
                images[i] = new D3DImage(pImages[i]);
            }
            return images;
        }

        public void CopyTo(IScratchImage scratchImage)
        {
            var a = this;
            var b = (D3DScratchImage)scratchImage;
            var metaA = a.Metadata;
            var metaB = b.Metadata;
            if (metaA.Format != metaB.Format)
            {
                throw new ArgumentOutOfRangeException(nameof(scratchImage), $"Format {metaA.Format} doesn't match with {metaB.Format}");
            }

            if (metaA.Width != metaB.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(scratchImage), $"Width {metaA.Width} doesn't match with {metaB.Width}");
            }

            if (metaA.Height != metaB.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(scratchImage), $"Height {metaA.Height} doesn't match with {metaB.Height}");
            }

            if (metaA.Depth != metaB.Depth)
            {
                throw new ArgumentOutOfRangeException(nameof(scratchImage), $"Depth {metaA.Depth} doesn't match with {metaB.Depth}");
            }

            if (metaA.ArraySize != metaB.ArraySize)
            {
                throw new ArgumentOutOfRangeException(nameof(scratchImage), $"ArraySize {metaA.ArraySize} doesn't match with {metaB.ArraySize}");
            }

            if (metaA.MipLevels != metaB.MipLevels)
            {
                throw new ArgumentOutOfRangeException(nameof(scratchImage), $"MipLevels {metaA.MipLevels} doesn't match with {metaB.MipLevels}");
            }

            if (metaA.Dimension != metaB.Dimension)
            {
                throw new ArgumentOutOfRangeException(nameof(scratchImage), $"Dimension {metaA.Dimension} doesn't match with {metaB.Dimension}");
            }

            if (metaA.AlphaMode != metaB.AlphaMode)
            {
                throw new ArgumentOutOfRangeException(nameof(scratchImage), $"AlphaMode {metaA.AlphaMode} doesn't match with {metaB.AlphaMode}");
            }

            var count = a.ImageCount;
            var aImages = a.GetImages();
            var bImages = b.GetImages();
            for (int i = 0; i < count; i++)
            {
                var aImage = aImages[i];
                var bImage = bImages[i];
                Memcpy(aImage.Pixels, bImage.Pixels, (long)aImage.RowPitch * aImage.Height, (long)bImage.RowPitch * bImage.Height);
            }
        }
    }
}