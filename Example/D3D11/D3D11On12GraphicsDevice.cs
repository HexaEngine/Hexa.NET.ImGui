namespace HexaEngine.D3D11
{
    using HexaEngine.Core;
    using HexaEngine.Core.Graphics;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;
    using Silk.NET.Direct3D11.Extensions.D3D11On12;
    using Silk.NET.Direct3D12;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;

    public unsafe partial class D3D11On12GraphicsDevice : D3D11GraphicsDevice
    {
        internal readonly D3D12 D3D12;
        internal readonly D3D11On12 D3D11On12;

        public new static readonly ShaderCompiler Compiler;

        public ComPtr<ID3D12Device> D3D12Device;

        static D3D11On12GraphicsDevice()
        {
            Compiler = new();
            ShaderCompilers.Register(GraphicsBackend.D3D11On12, Compiler);
        }

        [SupportedOSPlatform("windows")]
        public D3D11On12GraphicsDevice(DXGIAdapterD3D11 adapter, bool debug) : base(adapter)
        {
            D3D12 = D3D12.GetApi();
            D3D11On12 = new(D3D11.Context);

            D3DFeatureLevel[] levelsArr = new D3DFeatureLevel[]
            {
                D3DFeatureLevel.Level122,
                D3DFeatureLevel.Level121,
                D3DFeatureLevel.Level120,
                D3DFeatureLevel.Level111,
                D3DFeatureLevel.Level110
            };

            CreateDeviceFlag flags = CreateDeviceFlag.BgraSupport;

            if (debug)
            {
                flags |= CreateDeviceFlag.Debug;
            }

            ID3D11Device* tempDevice;
            ID3D11DeviceContext* tempContext;

            D3DFeatureLevel level = 0;
            D3DFeatureLevel* levels = (D3DFeatureLevel*)Unsafe.AsPointer(ref levelsArr[0]);

            D3D12.CreateDevice(adapter.IDXGIAdapter, D3DFeatureLevel.Level110, out D3D12Device).ThrowHResult();
            D3D11On12.On12CreateDevice((IUnknown*)D3D12Device.Handle, (uint)flags, levels, (uint)levelsArr.Length, null, 0, 0, &tempDevice, &tempContext, &level).ThrowHResult();

            Level = level;

            tempDevice->QueryInterface(out Device);
            tempContext->QueryInterface(out DeviceContext);
            tempDevice->Release();
            tempContext->Release();

            NativePointer = new(Device);

#if DEBUG
            if (debug)
            {
                Device.QueryInterface(out DebugDevice);
            }
#endif

            Context = new D3D11GraphicsContext(this);
        }

        public override GraphicsBackend Backend => GraphicsBackend.D3D11On12;

        public new event EventHandler? OnDisposed;

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                OnDisposed?.Invoke(this, EventArgs.Empty);

                Context.Dispose();
                Device.Release();
                D3D12Device.Release();

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

                if (DebugDevice.Handle != null)
                {
                    DebugDevice.ReportLiveDeviceObjects(Silk.NET.Direct3D11.RldoFlags.Detail | Silk.NET.Direct3D11.RldoFlags.IgnoreInternal);
                    DebugDevice.Release();
                }

                LeakTracer.ReportLiveInstances();

                D3D11.Dispose();

                disposedValue = true;
            }
        }

        ~D3D11On12GraphicsDevice()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }
    }
}