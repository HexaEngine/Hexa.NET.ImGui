namespace ExampleOpenGL3
{
    using ExampleFramework;
    using ExampleFramework.ImGuiDemo;
    using ExampleFramework.ImGuizmoDemo;
    using ExampleFramework.ImNodesDemo;
    using ExampleFramework.ImPlotDemo;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImGui.Backends.GLFW;
    using Hexa.NET.ImGui.Backends.OpenGL3;
    using Hexa.NET.ImGui.Backends.SDL2;
    using Hexa.NET.OpenGL;
    using HexaGen.Runtime;
    using Silk.NET.SDL;

    internal unsafe class BindingsContext : IGLContext
    {
        private Sdl sdl;
        private readonly Window* window;
        private  void* glContext;

        public BindingsContext(Sdl sdl, Window* window)
        {
            this.sdl = sdl;
            this.window = window;
            glContext = sdl.GLCreateContext(window);
        }

        public nint Handle => (nint)glContext;

        public bool IsCurrent => sdl.GLGetCurrentContext() == glContext;

        public void Dispose()
        {
            if (glContext != null)
            {
                sdl.GLDeleteContext(glContext);
                glContext = null;
            }
        }

        public nint GetProcAddress(string procName)
        {
            return (nint)sdl.GLGetProcAddress(procName);
        }

        public bool IsExtensionSupported(string extensionName)
        {
            return sdl.GLExtensionSupported(extensionName) != 0;
        }

        public void MakeCurrent()
        {
            sdl.GLMakeCurrent(window, glContext);
        }

        public void SwapBuffers()
        {
            sdl.GLSwapWindow(window);
        }

        public void SwapInterval(int interval)
        {
            sdl.GLSetSwapInterval(interval);
        }

        public bool TryGetProcAddress(string procName, out nint procAddress)
        {
            procAddress = (nint)sdl.GLGetProcAddress(procName);
            return procAddress != 0;
        }
    }

    public unsafe class OpenGLWindow : CoreWindow
    {
        private BindingsContext context;
        private GL gl;
        private ImGuiManager imGuiManager;
        private ImGuiDemo imGuiDemo;
        private ImGuizmoDemo imGuizmoDemo;
        private ImNodesDemo imNodesDemo;
        private ImPlotDemo imPlotDemo;

        public override void InitGraphics()
        {
            context = new BindingsContext(App.sdl, SDLWindow);
            gl = new GL(context);

            imGuiManager = new();
            imGuiManager.OnRenderDrawData += OnRenderDrawData;

            ImGuiImplSDL2.InitForOpenGL((SDLWindow*)SDLWindow, (void*)context.Handle);
            App.RegisterHook(ProcessEvent);

            ImGuiImplGLFW.SetCurrentContext(ImGui.GetCurrentContext());

            ImGuiImplOpenGL3.SetCurrentContext(ImGui.GetCurrentContext());
            ImGuiImplOpenGL3.Init((string)null!);
            ImGuiImplOpenGL3.NewFrame();

            imGuiDemo = new();
            imGuizmoDemo = new();
            imNodesDemo = new();
            imPlotDemo = new();
        }

        private void OnRenderDrawData()
        {
            ImGuiImplOpenGL3.NewFrame();
            ImGuiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());
        }

        public override void Render()
        {
            imGuiManager.NewFrame();
            ImGui.ShowDemoWindow();
            imGuizmoDemo.Draw();
            imNodesDemo.Draw();
            imPlotDemo.Draw();

            context.MakeCurrent();
            gl.BindFramebuffer(GLFramebufferTarget.Framebuffer, 0);
            gl.Clear(GLClearBufferMask.ColorBufferBit | GLClearBufferMask.DepthBufferBit);

            imGuiManager.EndFrame();

            context.MakeCurrent();
            context.SwapBuffers();
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
                ImGuiImplOpenGL3.Shutdown();
                ImGuiImplSDL2.Shutdown();
                imGuiManager.Dispose();
                context.Dispose();
                gl.Dispose();
                base.Dispose(disposing);
                disposed = true;
            }
        }
    }
}