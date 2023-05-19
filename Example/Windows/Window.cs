namespace HexaEngine.Windows
{
    using HexaEngine.Core;
    using HexaEngine.Core.Debugging;
    using HexaEngine.Core.Graphics;
    using HexaEngine.Core.Windows;
    using HexaEngine.Core.Windows.Events;
    using HexaEngine.Mathematics;
    using HexaEngine.Rendering;
    using System;
    using System.Numerics;
    using HexaEngine.ImGui;

    public class Window : SdlWindow, IRenderWindow
    {
        private bool firstFrame;

        private IGraphicsDevice graphicsDevice;
        private IGraphicsContext graphicsContext;
        private ISwapChain swapChain;

        private bool resize = false;
        private ImGuiRenderer? imGuiRenderer;

        public IGraphicsDevice Device => graphicsDevice;

        public IGraphicsContext Context => graphicsContext;

        public ISwapChain SwapChain => swapChain;

        public string? StartupScene;
        private Viewport renderViewport;

        public Viewport RenderViewport => renderViewport;

        public Window()
        {
        }

        public virtual void Initialize(IGraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            graphicsContext = graphicsDevice.Context;
            swapChain = graphicsDevice.CreateSwapChain(this) ?? throw new PlatformNotSupportedException();
            swapChain.Active = true;

            if (Application.MainWindow == this)
            {
                PipelineManager.Initialize(graphicsDevice);
            }

            imGuiRenderer = new(this, graphicsDevice, swapChain);

            OnRendererInitialize(graphicsDevice);
        }

        public unsafe void Render(IGraphicsContext context)
        {
            if (resize)
            {
                swapChain.Resize(Width, Height);
                resize = false;
            }

            if (firstFrame)
            {
                Time.Initialize();
                firstFrame = false;
            }

            context.ClearDepthStencilView(swapChain.BackbufferDSV, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1, 0);
            context.ClearRenderTargetView(swapChain.BackbufferRTV, Vector4.Zero);

            imGuiRenderer?.BeginDraw();

            OnRenderBegin(context);

            ImGuiConsole.Draw();

            ImGui.ShowAboutWindow(null);

            OnRender(context);

            imGuiRenderer?.EndDraw();

            swapChain.Present();

            swapChain.Wait();
        }

        public virtual void Uninitialize()
        {
            OnRendererDispose();

            if (imGuiRenderer is not null)
            {
                imGuiRenderer?.Dispose();
            }

            swapChain.Dispose();
            graphicsContext.Dispose();
            graphicsDevice.Dispose();
        }

        protected virtual void OnRendererInitialize(IGraphicsDevice device)
        {
        }

        protected virtual void OnRenderBegin(IGraphicsContext context)
        {
        }

        protected virtual void OnRender(IGraphicsContext context)
        {
        }

        protected virtual void OnRendererDispose()
        {
        }

        protected override void OnResized(ResizedEventArgs args)
        {
            resize = true;
            base.OnResized(args);
        }
    }
}