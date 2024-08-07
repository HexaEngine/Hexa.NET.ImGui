namespace ExampleFramework
{
    using Silk.NET.SDL;
    using System;

    public unsafe class CoreWindow
    {
        protected static readonly Sdl sdl = App.sdl;
        private Silk.NET.SDL.Window* window;
        private uint id;
        private int width;
        private int height;

        public CoreWindow()
        {
            int width = 1280;
            int height = 720;
            int y = 100;
            int x = 100;

            WindowFlags flags = WindowFlags.Resizable | WindowFlags.Hidden | WindowFlags.AllowHighdpi;

            switch (App.Backend)
            {
                case Backend.OpenGL:
                    flags |= WindowFlags.Opengl;
                    break;

                case Backend.Vulkan:
                    flags |= WindowFlags.Vulkan;
                    break;
            }

            window = sdl.CreateWindow("", x, y, width, height, (uint)flags);
            id = sdl.GetWindowID(window);
        }

        public uint Id => id;

        public int Width => width;

        public int Height => height;

        public Silk.NET.SDL.Window* SDLWindow => window;

        public event EventHandler<ResizedEventArgs>? Resized;

        public void Show()
        {
            sdl.ShowWindow(window);
        }

        internal void Destroy()
        {
            if (window != null)
            {
                sdl.DestroyWindow(window);
                window = null;
                id = 0;
            }
        }

        internal void ProcessWindowEvent(WindowEvent windowEvent)
        {
            switch ((WindowEventID)windowEvent.Type)
            {
                case WindowEventID.Resized:
                    var oldWidth = this.width;
                    var oldHeight = this.height;
                    int width = windowEvent.Data1;
                    int height = windowEvent.Data2;
                    Resized?.Invoke(this, new ResizedEventArgs(width, height, oldWidth, oldHeight));
                    this.width = width;
                    this.height = height;
                    break;

                case WindowEventID.Close:
                    Destroy();
                    break;
            }
        }

        public virtual void InitGraphics()
        {
        }

        public virtual void Render()
        {
        }
    }
}