namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Debugging;
    using HexaEngine.Core.Graphics;
    using HexaEngine.Core.Windows;
    using Silk.NET.Core.Native;
    using Silk.NET.DXGI;
    using System.Diagnostics;
    using System.Runtime.Versioning;

    public unsafe class DXGIAdapterD3D11 : IGraphicsAdapter
    {
        internal readonly DXGI DXGI;

        internal ComPtr<IDXGIFactory2> IDXGIFactory;
        internal ComPtr<IDXGIAdapter1> IDXGIAdapter;

        public DXGIAdapterD3D11()
        {
            DXGI = DXGI.GetApi();

            IDXGIFactory2* factory;
            DXGI.CreateDXGIFactory2(0, Utils.Guid(IDXGIFactory2.Guid), (void**)&factory);
            IDXGIFactory = factory;

            IDXGIAdapter = GetHardwareAdapter();
        }

        public static void Init()
        {
            if (OperatingSystem.IsWindows())
            {
                GraphicsAdapter.Adapters.Add(new DXGIAdapterD3D11());
            }
        }

        public virtual GraphicsBackend Backend => GraphicsBackend.D3D11;

        public virtual int PlatformScore => 100;

        public virtual IGraphicsDevice CreateGraphicsDevice(bool debug)
        {
            AdapterDesc1 desc;
            IDXGIAdapter.GetDesc1(&desc);
            string name = new(desc.Description);

            ImGuiConsole.Log(LogSeverity.Info, "Backend: Using Graphics API: D3D11");
            ImGuiConsole.Log(LogSeverity.Info, $"Backend: Using Graphics Device: {name}");
            return new D3D11GraphicsDevice(this, debug);
        }

        internal ISwapChain CreateSwapChainForWindow(D3D11GraphicsDevice device, SdlWindow window)
        {
            SwapChainDesc1 desc = new()
            {
                Width = (uint)window.Width,
                Height = (uint)window.Height,
                Format = Silk.NET.DXGI.Format.FormatB8G8R8A8Unorm,
                BufferCount = 2,
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
            IntPtr hwnd = window.GetWin32HWND();
            IDXGIFactory.CreateSwapChainForHwnd((IUnknown*)device.Device.Handle, hwnd, &desc, &fullscreenDesc, (IDXGIOutput*)null, &swapChain.Handle);
            IDXGIFactory.MakeWindowAssociation(hwnd, 1 << 0);

            return new DXGISwapChain(device, swapChain, (SwapChainFlag)desc.Flags);
        }

        private IDXGIAdapter1* GetHardwareAdapter()
        {
            IDXGIAdapter1* adapter = null;
            Guid* adapterGuid = Utils.Guid(IDXGIAdapter1.Guid);
            IDXGIFactory6* factory6;
            IDXGIFactory.QueryInterface(Utils.Guid(IDXGIFactory6.Guid), (void**)&factory6);

            if (factory6 != null)
            {
                for (uint adapterIndex = 0;
                    (ResultCode)factory6->EnumAdapterByGpuPreference(adapterIndex, GpuPreference.HighPerformance, adapterGuid, (void**)&adapter) !=
                    ResultCode.DXGI_ERROR_NOT_FOUND;
                    adapterIndex++)
                {
                    AdapterDesc1 desc;
                    adapter->GetDesc1(&desc);
                    if (((AdapterFlag)desc.Flags & AdapterFlag.Software) != AdapterFlag.None)
                    {
                        // Don't select the Basic Render Driver adapter.
                        adapter->Release();
                        continue;
                    }

                    return adapter;
                }

                factory6->Release();
            }

            if (adapter == null)
            {
                for (uint adapterIndex = 0;
                    (ResultCode)IDXGIFactory.EnumAdapters1(adapterIndex, &adapter) != ResultCode.DXGI_ERROR_NOT_FOUND;
                    adapterIndex++)
                {
                    AdapterDesc1 desc;
                    adapter->GetDesc1(&desc);
                    string name = new(desc.Description);

                    Trace.WriteLine($"Found Adapter {name}");

                    if (((AdapterFlag)desc.Flags & AdapterFlag.Software) != AdapterFlag.None)
                    {
                        // Don't select the Basic Render Driver adapter.
                        adapter->Release();
                        continue;
                    }

                    return adapter;
                }
            }

            return adapter;
        }
    }
}