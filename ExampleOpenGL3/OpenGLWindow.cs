namespace ExampleOpenGL3
{
    using ExampleFramework;
    using ExampleFramework.ImGuiDemo;
    using ExampleFramework.ImGuizmoDemo;
    using ExampleFramework.ImNodesDemo;
    using ExampleFramework.ImPlotDemo;
    using Hexa.NET.ImGui;
    using Silk.NET.Core.Contexts;
    using Silk.NET.OpenGL;

    public unsafe class OpenGLWindow : CoreWindow, IGLContextSource
    {
        private void* glcontext;
        private SDLGLContext context;
        private GL gl;

        private ImGuiManager imGuiManager;
        private ImGuiDemo imGuiDemo;
        private ImGuizmoDemo imGuizmoDemo;
        private ImNodesDemo imNodesDemo;
        private ImPlotDemo imPlotDemo;

        public IGLContext? GLContext { get; }

        public override void InitGraphics()
        {
            glcontext = sdl.GLCreateContext(SDLWindow);
            context = new(SDLWindow, glcontext, null);
            gl = GL.GetApi(context);

            imGuiManager = new();
            imGuiManager.OnRenderDrawData += OnRenderDrawData;
            ImGuiSDL2Platform.InitForOpenGL(SDLWindow, glcontext);
            ImGuiOpenGL3Renderer.Init(gl, null);

            imGuiDemo = new();
            imGuizmoDemo = new();
            imNodesDemo = new();
            imPlotDemo = new();
        }

        private void OnRenderDrawData()
        {
            ImGuiOpenGL3Renderer.RenderDrawData(ImGui.GetDrawData());
        }

        public override void Render()
        {
            imGuiManager.NewFrame();

            imGuiDemo.Draw();
            imGuizmoDemo.Draw();
            imNodesDemo.Draw();
            imPlotDemo.Draw();

            context.MakeCurrent();
            gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            gl.Clear((uint)ClearBufferMask.ColorBufferBit | (uint)ClearBufferMask.DepthBufferBit);

            imGuiManager.EndFrame();

            context.MakeCurrent();
            context.SwapBuffers();
        }
    }
}