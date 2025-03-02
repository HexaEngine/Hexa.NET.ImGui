namespace ExampleFramework
{
    using Silk.NET.SDL;
    using System;

    public class ClosingEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }

    public class ClosedEventArgs : EventArgs
    {
    }

    public unsafe class CoreWindow : IDisposable
    {
        protected static readonly Sdl sdl = App.sdl;
        private Window* window;
        private uint id;
        private int width;
        private int height;
        private bool disposedValue;

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

        public Window* SDLWindow => window;

        public event EventHandler<ResizedEventArgs>? Resized;

        public event EventHandler<ClosingEventArgs>? Closing;

        public event EventHandler<ClosedEventArgs>? Closed;

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
            switch ((WindowEventID)windowEvent.Event)
            {
                case WindowEventID.Resized:
                    var oldWidth = this.width;
                    var oldHeight = this.height;
                    int width = windowEvent.Data1;
                    int height = windowEvent.Data2;
                    var resizedEventArgs = new ResizedEventArgs(width, height, oldWidth, oldHeight);
                    this.width = width;
                    this.height = height;
                    OnResized(resizedEventArgs);
                    if (!resizedEventArgs.Handled)
                    {
                        Resized?.Invoke(this, resizedEventArgs);
                    }
                    else
                    {
                        sdl.SetWindowSize(window, oldWidth, oldHeight);
                        this.width = oldWidth;
                        this.height = oldHeight;
                    }

                    break;

                case WindowEventID.Close:
                    ClosingEventArgs eventArgs = new();
                    Closing?.Invoke(this, eventArgs);
                    if (eventArgs.Handled)
                    {
                        return;
                    }
                    Dispose();
                    Closed?.Invoke(this, new());
                    break;
            }
        }

        protected virtual void OnResized(ResizedEventArgs resizedEventArgs)
        {
        }

        public virtual void InitGraphics()
        {
        }

        public virtual void Render()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Destroy();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}