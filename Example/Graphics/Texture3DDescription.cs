namespace HexaEngine.Core.Graphics
{
    public struct Texture3DDescription
    {
        public int Width;
        public int Height;
        public int Depth;
        public int MipLevels;
        public Format Format;
        public Usage Usage;
        public BindFlags BindFlags;
        public CpuAccessFlags CPUAccessFlags;
        public ResourceMiscFlag MiscFlags;

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture3DDescription"/> struct.
        /// </summary>
        /// <param name="width">Texture width (in texels).</param>
        /// <param name="height">Texture height (in texels).</param>
        /// <param name="depth">Texture depth (in texels).</param>
        /// <param name="format">Texture format.</param>
        /// <param name="mipLevels">The maximum number of mipmap levels in the texture.</param>
        /// <param name="bindFlags">The <see cref="Direct3D11.BindFlags"/> for binding to pipeline stages.</param>
        /// <param name="usage">Value that identifies how the texture is to be read from and written to.</param>
        /// <param name="cpuAccessFlags">The <see cref="CpuAccessFlags"/> to specify the types of CPU access allowed.</param>
        /// <param name="miscFlags">The <see cref="ResourceMiscFlag"/> that identify other, less common resource options. </param>
        public Texture3DDescription(
            Format format,
            int width,
            int height,
            int depth,
            int mipLevels = 0,
            BindFlags bindFlags = BindFlags.ShaderResource,
            Usage usage = Usage.Default,
            CpuAccessFlags cpuAccessFlags = CpuAccessFlags.None,
            ResourceMiscFlag miscFlags = ResourceMiscFlag.None)
        {
            if (format == Format.Unknown)
            {
                throw new ArgumentException($"format need to be valid", nameof(format));
            }

            if (width < 1 || width > IResource.MaximumTexture3DSize)
            {
                throw new ArgumentException($"Width need to be in range 1-{IResource.MaximumTexture3DSize}", nameof(width));
            }

            if (height < 1 || height > IResource.MaximumTexture3DSize)
            {
                throw new ArgumentException($"Height need to be in range 1-{IResource.MaximumTexture3DSize}", nameof(height));
            }

            if (depth < 1 || depth > IResource.MaximumTexture3DSize)
            {
                throw new ArgumentException($"Depth need to be in range 1-{IResource.MaximumTexture3DSize}", nameof(depth));
            }

            Width = width;
            Height = height;
            Depth = depth;
            MipLevels = mipLevels;
            Format = format;
            Usage = usage;
            BindFlags = bindFlags;
            CPUAccessFlags = cpuAccessFlags;
            MiscFlags = miscFlags;
        }
    }
}