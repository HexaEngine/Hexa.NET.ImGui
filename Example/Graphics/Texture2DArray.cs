namespace HexaEngine.Core.Graphics
{
    using HexaEngine.Mathematics;
    using System;
    using System.Runtime.CompilerServices;
    using static HexaEngine.Core.Utils;

    public unsafe class TextureArray : IDisposable
    {
        private bool disposedValue;

        public int Width { get; }

        public int Height { get; }

        public Viewport Viewport => new(Width, Height);

        public readonly uint Count;

        public readonly IShaderResourceView[] SRVs;
        public readonly IRenderTargetView[] RTVs;
        public readonly void** PSRVs;
        public readonly void** PRTVs;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextureArray(IGraphicsDevice device, int width, int height, uint count = 1, Format format = Format.R32G32B32A32Float)
        {
            Count = count;
            Width = width;
            Height = height;
            RTVs = new IRenderTargetView[count];
            SRVs = new IShaderResourceView[count];
            PSRVs = AllocArray(count);
            PRTVs = AllocArray(count);
            for (int i = 0; i < count; i++)
            {
                ITexture2D tex = device.CreateTexture2D(format, Width, Height, 1, 1, null, BindFlags.ShaderResource | BindFlags.RenderTarget, ResourceMiscFlag.None);
                SRVs[i] = device.CreateShaderResourceView(tex);
                RTVs[i] = device.CreateRenderTargetView(tex, new(Width, Height));
                PSRVs[i] = (void*)SRVs[i].NativePointer;
                PRTVs[i] = (void*)RTVs[i].NativePointer;
                tex.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextureArray(IGraphicsDevice device, int width, int height, Format[] formats)
        {
            Count = (uint)formats.Length;
            Width = width;
            Height = height;
            RTVs = new IRenderTargetView[formats.Length];
            SRVs = new IShaderResourceView[formats.Length];
            PSRVs = AllocArray((uint)formats.Length);
            PRTVs = AllocArray((uint)formats.Length);
            for (int i = 0; i < formats.Length; i++)
            {
                ITexture2D tex = device.CreateTexture2D(formats[i], Width, Height, 1, 1, null, BindFlags.ShaderResource | BindFlags.RenderTarget, ResourceMiscFlag.None);
                SRVs[i] = device.CreateShaderResourceView(tex);
                RTVs[i] = device.CreateRenderTargetView(tex, new(Width, Height));
                PSRVs[i] = (void*)SRVs[i].NativePointer;
                PRTVs[i] = (void*)RTVs[i].NativePointer;
                tex.Dispose();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                foreach (IRenderTargetView rtv in RTVs)
                {
                    rtv.Dispose();
                }

                foreach (IShaderResourceView srv in SRVs)
                {
                    srv.Dispose();
                }

                Free(PSRVs);
                Free(PRTVs);

                disposedValue = true;
            }
        }

        ~TextureArray()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}