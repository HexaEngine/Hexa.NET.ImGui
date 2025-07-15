namespace ExampleD3D11
{
    using ExampleFramework;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D.Compilers;
    using Silk.NET.Direct3D11;
    using Silk.NET.DXGI;
    using Silk.NET.SDL;
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using Window = Silk.NET.SDL.Window;

    public unsafe class D3D11Manager : IDisposable
    {
        internal readonly DXGI DXGI;

        internal ComPtr<IDXGIFactory2> IDXGIFactory;
        internal ComPtr<IDXGIAdapter1> IDXGIAdapter;

        internal readonly D3D11 D3D11;

        internal ComPtr<ID3D11Device1> Device;
        internal ComPtr<ID3D11DeviceContext1> DeviceContext;

        private ComPtr<IDXGISwapChain1> swapChain;
        private SwapChainDesc1 swapChainDesc;
        private ComPtr<ID3D11Texture2D> swapChainBackbuffer;

        private ComPtr<ID3D11RenderTargetView> swapChainRTV;

        public D3D11Manager(Window* window, bool debug)
        {
            DXGI = DXGI.GetApi();

            DXGI.CreateDXGIFactory2(0, out IDXGIFactory);

            IDXGIAdapter = GetHardwareAdapter();

            D3D11 = D3D11.GetApi();
            D3DFeatureLevel[] levelsArr =
            [
                D3DFeatureLevel.Level111,
                D3DFeatureLevel.Level110
            ];

            CreateDeviceFlag flags = CreateDeviceFlag.BgraSupport;

            if (debug)
            {
                flags |= CreateDeviceFlag.Debug;
            }

            ID3D11Device* tempDevice;
            ID3D11DeviceContext* tempContext;

            D3DFeatureLevel level = 0;
            D3DFeatureLevel* levels = (D3DFeatureLevel*)Unsafe.AsPointer(ref levelsArr[0]);

            D3D11.CreateDevice((IDXGIAdapter*)IDXGIAdapter.Handle, D3DDriverType.Unknown, IntPtr.Zero, (uint)flags, levels, (uint)levelsArr.Length, D3D11.SdkVersion, &tempDevice, &level, &tempContext).ThrowHResult();
            Level = level;

            tempDevice->QueryInterface(out Device);
            tempContext->QueryInterface(out DeviceContext);

            tempDevice->Release();
            tempContext->Release();

            CreateSwapChain(window);
        }

        private void CreateSwapChain(Window* window)
        {
            Sdl sdl = App.sdl;
            SysWMInfo info;
            sdl.GetVersion(&info.Version);
            sdl.GetWindowWMInfo(window, &info);

            int width = 0;
            int height = 0;

            sdl.GetWindowSize(window, &width, &height);

            var Hwnd = info.Info.Win.Hwnd;

            SwapChainDesc1 desc = new()
            {
                Width = (uint)width,
                Height = (uint)height,
                Format = Silk.NET.DXGI.Format.FormatB8G8R8A8Unorm,
                BufferCount = 3,
                BufferUsage = DXGI.UsageRenderTargetOutput,
                SampleDesc = new(1, 0),
                Scaling = Scaling.Stretch,
                SwapEffect = SwapEffect.FlipSequential,
                Flags = (uint)(SwapChainFlag.AllowModeSwitch | SwapChainFlag.AllowTearing)
            };

            SwapChainFullscreenDesc fullscreenDesc = new()
            {
                Windowed = 1,
                RefreshRate = new Rational(0, 1),
                Scaling = ModeScaling.Unspecified,
                ScanlineOrdering = ModeScanlineOrder.Unspecified,
            };

            ComPtr<IDXGISwapChain1> swapChain;
            IDXGIFactory.CreateSwapChainForHwnd((IUnknown*)Device.Handle, Hwnd, &desc, &fullscreenDesc, (IDXGIOutput*)null, &swapChain.Handle);
            // IDXGIFactory.MakeWindowAssociation(Hwnd, 1 << 0);

            this.swapChain = swapChain;
            swapChainDesc = desc;
            swapChain.GetBuffer(0, out swapChainBackbuffer);
            ID3D11RenderTargetView* rtv;
            Device.CreateRenderTargetView((ID3D11Resource*)swapChainBackbuffer.Handle, (RenderTargetViewDesc*)null, &rtv);
            swapChainRTV.Handle = rtv;
            Width = width;
            Height = height;
            Viewport = new(0, 0, width, height);
        }

        public void Present(uint sync, uint flags)
        {
            swapChain.Present(sync, flags);
        }

        public void Clear(Vector4 color)
        {
            DeviceContext.ClearRenderTargetView(swapChainRTV.Handle, (float*)&color);
        }

        public void SetTarget()
        {
            ID3D11RenderTargetView* rtv = swapChainRTV.Handle;
            DeviceContext.OMSetRenderTargets(1, &rtv, (ID3D11DepthStencilView*)null);
            Viewport viewport = Viewport;
            DeviceContext.RSSetViewports(1, &viewport);
        }

        public void UnTarget()
        {
            ID3D11RenderTargetView* rtv = null;
            DeviceContext.OMSetRenderTargets(1, &rtv, (ID3D11DepthStencilView*)null);
            Viewport viewport = default;
            DeviceContext.RSSetViewports(1, &viewport);
        }

        public void Resize(int width, int height)
        {
            swapChainBackbuffer.Release();
            swapChainRTV.Dispose();

            swapChain.ResizeBuffers(swapChainDesc.BufferCount, (uint)width, (uint)height, swapChainDesc.Format, swapChainDesc.Flags);
            Width = width;
            Height = height;
            Viewport = new(0, 0, width, height);

            swapChain.GetBuffer(0, out swapChainBackbuffer);

            ID3D11RenderTargetView* rtv;
            Device.CreateRenderTargetView((ID3D11Resource*)swapChainBackbuffer.Handle, (RenderTargetViewDesc*)null, &rtv);
            swapChainRTV.Handle = rtv;
        }

        public D3DFeatureLevel Level { get; protected set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public Viewport Viewport { get; private set; }

        private IDXGIAdapter1* GetHardwareAdapter()
        {
            ComPtr<IDXGIAdapter1> adapter = null;
            ComPtr<IDXGIFactory6> factory6;
            IDXGIFactory.QueryInterface(out factory6);

            if (factory6.Handle != null)
            {
                for (uint adapterIndex = 0;
                    (ResultCode)factory6.EnumAdapterByGpuPreference(adapterIndex, GpuPreference.HighPerformance, out adapter) !=
                    ResultCode.DXGI_ERROR_NOT_FOUND;
                    adapterIndex++)
                {
                    AdapterDesc1 desc;
                    adapter.GetDesc1(&desc);
                    if (((AdapterFlag)desc.Flags & AdapterFlag.Software) != AdapterFlag.None)
                    {
                        // Don't select the Basic Render Driver adapter.
                        adapter.Release();
                        continue;
                    }

                    return adapter;
                }

                factory6.Release();
            }

            if (adapter.Handle == null)
            {
                for (uint adapterIndex = 0;
                    (ResultCode)IDXGIFactory.EnumAdapters1(adapterIndex, &adapter.Handle) != ResultCode.DXGI_ERROR_NOT_FOUND;
                    adapterIndex++)
                {
                    AdapterDesc1 desc;
                    adapter.GetDesc1(&desc);
                    string name = new(desc.Description);

                    Console.WriteLine($"Found Adapter {name}");

                    if (((AdapterFlag)desc.Flags & AdapterFlag.Software) != AdapterFlag.None)
                    {
                        // Don't select the Basic Render Driver adapter.
                        adapter.Release();
                        continue;
                    }

                    return adapter;
                }
            }

            return adapter;
        }

        public void Dispose()
        {
            swapChainRTV.Release();
            swapChainBackbuffer.Release();
            swapChain.Release();

            DeviceContext.Release();
            Device.Release();

            D3D11.Dispose();

            IDXGIAdapter.Release();
            IDXGIFactory.Release();

            DXGI.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}