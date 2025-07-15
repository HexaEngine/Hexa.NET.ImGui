namespace ExampleOpenGL3
{
    using ExampleFramework;
    using Silk.NET.Core.Contexts;
    using Silk.NET.SDL;
    using Window = Silk.NET.SDL.Window;

    public unsafe class SDLGLContext : IGLContext
    {
        private static readonly Sdl sdl = App.sdl;
        private readonly Window* window;
        private void* context;
        private readonly IGLContextSource? contextSource;

        public SDLGLContext(Window* window, void* context, IGLContextSource? contextSource)
        {
            this.window = window;
            this.context = context;
            this.contextSource = contextSource;
        }

        public nint Handle => (nint)context;

        public IGLContextSource? Source => contextSource;

        public bool IsCurrent => sdl.GLGetCurrentContext() == context;

        public void Clear()
        {
        }

        public void Dispose()
        {
            sdl.GLDeleteContext(context);
            context = null;
        }

        public nint GetProcAddress(string proc, int? slot = null)
        {
            return (nint)sdl.GLGetProcAddress(proc);
        }

        public void MakeCurrent()
        {
            sdl.GLMakeCurrent(window, context);
        }

        public void SwapBuffers()
        {
            sdl.GLSwapWindow(window);
        }

        public void SwapInterval(int interval)
        {
            sdl.GLSetSwapInterval(interval);
        }

        public bool TryGetProcAddress(string proc, out nint addr, int? slot = null)
        {
            addr = (nint)sdl.GLGetProcAddress(proc);
            return addr != 0;
        }
    }
}