namespace HexaEngine.Core.Graphics
{
    using HexaEngine.Core.Graphics.Textures;
    using System;

    [Flags]
    public enum TextureLoaderFlags
    {
        None = 0,
        GenerateMipMaps = 1,
        Scale = 2,
    }

    public interface ITextureLoader
    {
        public IGraphicsDevice Device { get; }

        public TextureLoaderFlags Flags { get; set; }

        public float ScalingFactor { get; set; }

        public IScratchImage LoadFormFile(string filename);

        public IScratchImage LoadFormAssets(string filename);

        public IScratchImage LoadFromMemory(string filename, Stream stream);

        ITexture1D LoadTexture1D(string path, Usage usage, BindFlags bind, CpuAccessFlags cpuAccess, ResourceMiscFlag misc);

        ITexture2D LoadTexture2D(string path, Usage usage, BindFlags bind, CpuAccessFlags cpuAccess, ResourceMiscFlag misc);

        ITexture3D LoadTexture3D(string path, Usage usage, BindFlags bind, CpuAccessFlags cpuAccess, ResourceMiscFlag misc);

        ITexture1D LoadTexture1D(string path);

        ITexture2D LoadTexture2D(string path);

        ITexture3D LoadTexture3D(string path);

        public IScratchImage CaptureTexture(IGraphicsContext context, IResource resource);

        public IScratchImage Initialize(TexMetadata metadata, CPFlags flags);

        public IScratchImage Initialize1D(Format fmt, int length, int arraySize, int mipLevels, CPFlags flags = CPFlags.None);

        public IScratchImage Initialize2D(Format fmt, int width, int height, int arraySize, int mipLevels, CPFlags flags = CPFlags.None);

        public IScratchImage Initialize3D(Format fmt, int width, int height, int depth, int mipLevels, CPFlags flags = CPFlags.None);

        public IScratchImage InitializeCube(Format fmt, int width, int height, int nCubes, int mipLevels, CPFlags flags = CPFlags.None);

        public IScratchImage InitializeFromImage(IImage image, bool allow1D = false, CPFlags flags = CPFlags.None);

        public IScratchImage InitializeArrayFromImages(IImage[] images, bool allow1D = false, CPFlags flags = CPFlags.None);

        public IScratchImage InitializeCubeFromImages(IImage[] images, CPFlags flags = CPFlags.None);

        public IScratchImage Initialize3DFromImages(IImage[] images, int depth, CPFlags flags = CPFlags.None);
    }

    public interface IScratchImage : IDisposable
    {
        public TexMetadata Metadata { get; }

        public int ImageCount { get; }

        public ITexture1D CreateTexture1D(IGraphicsDevice device, Usage usage, BindFlags bindFlags, CpuAccessFlags accessFlags, ResourceMiscFlag miscFlag);

        public ITexture2D CreateTexture2D(IGraphicsDevice device, Usage usage, BindFlags bindFlags, CpuAccessFlags accessFlags, ResourceMiscFlag miscFlag);

        public ITexture2D CreateTexture2D(IGraphicsDevice device, int imageIndex, Usage usage, BindFlags bindFlags, CpuAccessFlags accessFlags, ResourceMiscFlag miscFlag);

        public ITexture3D CreateTexture3D(IGraphicsDevice device, Usage usage, BindFlags bindFlags, CpuAccessFlags accessFlags, ResourceMiscFlag miscFlag);

        public void SaveToFile(string path, TexFileFormat format, int flags);

        public void SaveToMemory(Stream stream, TexFileFormat format, int flags);

        public IScratchImage Resize(float scale, TexFilterFlags flags);

        public IScratchImage Resize(int width, int height, TexFilterFlags flags);

        public IScratchImage GenerateMipMaps(TexFilterFlags flags);

        public IScratchImage Compress(IGraphicsDevice device, Format format, TexCompressFlags flags);

        public IScratchImage Convert(Format format, TexFilterFlags flags);

        public IScratchImage Decompress(Format format);

        bool OverwriteFormat(Format format);

        public IImage[] GetImages();
        void CopyTo(IScratchImage scratchImage);
    }
}