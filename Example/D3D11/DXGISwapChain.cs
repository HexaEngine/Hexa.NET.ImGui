namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Graphics;
    using HexaEngine.Core.Windows.Events;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;
    using Silk.NET.DXGI;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public unsafe class DXGISwapChain : DeviceChildBase, ISwapChain
    {
        private ComPtr<IDXGISwapChain1> swapChain;
        private SwapChainFlag flags;
        private ComPtr<ID3D11Texture2D> backbuffer;
        private ITexture2D depthStencil;
        private long fpsStartTime;
        private long fpsFrameCount;
        private bool vSync;
        private bool limitFPS;
        private int targetFPS = 120;
        private bool active;

        internal DXGISwapChain(D3D11GraphicsDevice device, ComPtr<IDXGISwapChain1> swapChain, SwapChainFlag flags)
        {
            Device = device;
            this.swapChain = swapChain;
            this.flags = flags;

            swapChain.GetBuffer(0, out backbuffer);
            Texture2DDesc desc;
            backbuffer.GetDesc(&desc);
            Texture2DDescription description = Helper.ConvertBack(desc);

            Backbuffer = new D3D11Texture2D(backbuffer, description);
            BackbufferRTV = device.CreateRenderTargetView(Backbuffer, new(description.Width, description.Height));
            depthStencil = device.CreateTexture2D(Core.Graphics.Format.D32FloatS8X24UInt, description.Width, description.Height, 1, 1, null, BindFlags.DepthStencil);
            BackbufferDSV = device.CreateDepthStencilView(depthStencil);
            Width = description.Width;
            Height = description.Height;
            Viewport = new(0, 0, Width, Height);
        }

        public ITexture2D Backbuffer { get; private set; }

        public IRenderTargetView BackbufferRTV { get; private set; }

        public IDepthStencilView BackbufferDSV { get; private set; }

        public IGraphicsDevice Device { get; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public Mathematics.Viewport Viewport { get; private set; }

        public event EventHandler? Resizing;

        public event EventHandler<ResizedEventArgs>? Resized;

        public bool VSync { get => vSync; set => vSync = value; }

        public bool LimitFPS { get => limitFPS; set => limitFPS = value; }

        public int TargetFPS { get => targetFPS; set => targetFPS = value; }

        public bool Active { get => active; set => active = value; }

        public void Present(bool sync)
        {
            if (sync)
            {
                swapChain.Present(1, 0);
            }
            else
            {
                swapChain.Present(0, DXGI.PresentAllowTearing);
            }
        }

        public void Present()
        {
            if (!active)
            {
                swapChain.Present(4, 0);
            }
            else if (vSync)
            {
                swapChain.Present(1, 0);
            }
            else
            {
                swapChain.Present(0, DXGI.PresentAllowTearing);
            }
        }

        public void Wait()
        {
            if (!vSync && limitFPS)
            {
                LimitFrameRate();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void LimitFrameRate()
        {
            int fps = targetFPS;
            long freq = Stopwatch.Frequency;
            long frame = Stopwatch.GetTimestamp();
            while ((frame - fpsStartTime) * fps < freq * fpsFrameCount)
            {
                int sleepTime = (int)((fpsStartTime * fps + freq * fpsFrameCount - frame * fps) * 1000 / (freq * fps));
                if (sleepTime > 0)
                {
                    Thread.Sleep(sleepTime);
                }

                frame = Stopwatch.GetTimestamp();
            }
            if (++fpsFrameCount > fps)
            {
                fpsFrameCount = 0;
                fpsStartTime = frame;
            }
        }

        public void Resize(int width, int height)
        {
            var oldWidth = Width;
            var oldHeight = Height;
            Resizing?.Invoke(this, EventArgs.Empty);

            Backbuffer.Dispose();
            BackbufferRTV.Dispose();
            BackbufferDSV.Dispose();
            depthStencil.Dispose();

            swapChain.ResizeBuffers(2, (uint)width, (uint)height, Silk.NET.DXGI.Format.FormatB8G8R8A8Unorm, (uint)flags);
            Width = width;
            Height = height;
            Viewport = new(0, 0, Width, Height);

            swapChain.GetBuffer(0, out backbuffer);
            Texture2DDesc desc;
            backbuffer.GetDesc(&desc);
            Texture2DDescription description = Helper.ConvertBack(desc);

            Backbuffer = new D3D11Texture2D(backbuffer, description);
            BackbufferRTV = Device.CreateRenderTargetView(Backbuffer, new(description.Width, description.Height));
            depthStencil = Device.CreateTexture2D(Core.Graphics.Format.D32FloatS8X24UInt, description.Width, description.Height, 1, 1, null, BindFlags.DepthStencil);
            BackbufferDSV = Device.CreateDepthStencilView(depthStencil);

            Resized?.Invoke(this, new(oldWidth, oldHeight, width, height));
        }

        protected override void DisposeCore()
        {
            Backbuffer.Dispose();
            BackbufferRTV.Dispose();
            BackbufferDSV.Dispose();
            depthStencil.Dispose();
            swapChain.Release();
        }
    }
}