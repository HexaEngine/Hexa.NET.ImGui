namespace ExampleD3D11
{
    using ExampleFramework;
    using ExampleFramework.ImGuiDemo;
    using ExampleFramework.ImGuizmoDemo;
    using ExampleFramework.ImNodesDemo;
    using ExampleFramework.ImPlotDemo;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImGui.Backends;
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

            ImGuiBackends.ImplSDL2InitForD3D((SDLWindow*)SDLWindow);

            ImGuiBackends.ImplDX11Init((Hexa.NET.ImGui.Backends.ID3D11Device*)(void*)device.Handle, (Hexa.NET.ImGui.Backends.ID3D11DeviceContext*)(void*)deviceContext.Handle);
            ImGuiBackends.ImplDX11NewFrame();

            imGuiDemo = new();
            imGuizmoDemo = new();
            imNodesDemo = new();
            imPlotDemo = new();
        }

        private void OnRenderDrawData()
        {
            //ImGuiD3D11Renderer.RenderDrawData(ImGui.GetDrawData());
            ImGuiBackends.ImplDX11NewFrame();
            ImGuiBackends.ImplDX11RenderDrawData(ImGui.GetDrawData());
        }

        public override void Render()
        {
            imGuiManager.NewFrame();

            //imGuiDemo.Draw();
            imGuizmoDemo.Draw();
            imNodesDemo.Draw();
            imPlotDemo.Draw();

            d3d11Manager.Clear(default);
            d3d11Manager.SetTarget();

            imGuiManager.EndFrame();

            d3d11Manager.DeviceContext.ClearState();

            d3d11Manager.Present(1, 0);
        }
    }
}