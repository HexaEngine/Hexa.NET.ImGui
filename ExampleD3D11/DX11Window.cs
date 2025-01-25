namespace ExampleD3D11
{
    using ExampleFramework;
    using ExampleFramework.ImGuiDemo;
    using ExampleFramework.ImGuizmoDemo;
    using ExampleFramework.ImNodesDemo;
    using ExampleFramework.ImPlotDemo;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImGui.Backends;
    using Hexa.NET.ImGui.Backends.D3D11;
    using Hexa.NET.ImGui.Backends.SDL2;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;
    using Silk.NET.SDL;
    using System;

    public unsafe class DX11Window : CoreWindow
    {
        private D3D11Manager d3d11Manager;

        private ImGuiManager imGuiManager;
        private ImGuiDemo imGuiDemo;
        private ImGuizmoDemo imGuizmoDemo;
        private ImNodesDemo imNodesDemo;
        private ImPlotDemo imPlotDemo;

        public override void InitGraphics()
        {
            d3d11Manager = new(SDLWindow, true);

            imGuiManager = new();
            imGuiManager.OnRenderDrawData += OnRenderDrawData;

            ComPtr<ID3D11Device1> device = d3d11Manager.Device;
            ComPtr<ID3D11DeviceContext1> deviceContext = d3d11Manager.DeviceContext;

            ImGuiImplSDL2.InitForD3D((SDLWindow*)SDLWindow);
            App.RegisterHook(ProcessEvent);

            ImGuiImplD3D11.SetCurrentContext(ImGui.GetCurrentContext());
            ImGuiImplD3D11.Init((Hexa.NET.ImGui.Backends.D3D11.ID3D11Device*)(void*)device.Handle, (Hexa.NET.ImGui.Backends.D3D11.ID3D11DeviceContext*)(void*)deviceContext.Handle);
            ImGuiImplD3D11.NewFrame();

            imGuiDemo = new();
            imGuizmoDemo = new();
            imNodesDemo = new();
            imPlotDemo = new();
        }

        private void OnRenderDrawData()
        {
            ImGuiImplD3D11.NewFrame();
            ImGuiImplD3D11.RenderDrawData(ImGui.GetDrawData());
        }

        protected override void OnResized(ResizedEventArgs resizedEventArgs)
        {
            d3d11Manager.Resize(resizedEventArgs.Width, resizedEventArgs.Height);
        }

        public override void Render()
        {
            if (disposed) return;
            imGuiManager.NewFrame();

            ImGui.ShowDemoWindow();
            imGuizmoDemo.Draw();
            imNodesDemo.Draw();
            imPlotDemo.Draw();

            d3d11Manager.Clear(default);
            d3d11Manager.SetTarget();

            imGuiManager.EndFrame();

            d3d11Manager.DeviceContext.ClearState();

            d3d11Manager.Present(1, 0);
        }

        private static bool ProcessEvent(Event @event)
        {
            return ImGuiImplSDL2.ProcessEvent((SDLEvent*)&@event);
        }

        private bool disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                App.RemoveHook(ProcessEvent);
                ImGuiImplD3D11.Shutdown();
                ImGuiImplSDL2.Shutdown();
                imGuiManager.Dispose();
                d3d11Manager.Dispose();
                base.Dispose(disposing);
                disposed = true;
            }
        }
    }
}