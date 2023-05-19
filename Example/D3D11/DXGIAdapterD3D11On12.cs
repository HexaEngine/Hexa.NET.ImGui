namespace HexaEngine.D3D11
{
    using HexaEngine.Core.Debugging;
    using HexaEngine.Core.Graphics;
    using Silk.NET.DXGI;
    using System.Runtime.Versioning;

    public unsafe class DXGIAdapterD3D11On12 : DXGIAdapterD3D11
    {
        public DXGIAdapterD3D11On12()
        {
        }

        public new static void Init()
        {
            if (OperatingSystem.IsWindows())
            {
                GraphicsAdapter.Adapters.Add(new DXGIAdapterD3D11On12());
            }
        }

        public override GraphicsBackend Backend => GraphicsBackend.D3D11On12;

        public override int PlatformScore => 150;

        [SupportedOSPlatform("windows")]
        public override IGraphicsDevice CreateGraphicsDevice(bool debug)
        {
            AdapterDesc1 desc;
            IDXGIAdapter.GetDesc1(&desc);
            string name = new(desc.Description);

            ImGuiConsole.Log(LogSeverity.Info, "Backend: Using Graphics API: D3D11On12");
            ImGuiConsole.Log(LogSeverity.Info, $"Backend: Using Graphics Device: {name}");
            return new D3D11On12GraphicsDevice(this, debug);
        }
    }
}