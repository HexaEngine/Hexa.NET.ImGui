namespace HexaEngine.Core
{
    using Silk.NET.Maths;
    using Silk.NET.SDL;

    public static unsafe class Display
    {
        private static readonly Sdl Sdl = Application.sdl;

        public static int NumVideoDisplays => Sdl.GetNumVideoDisplays();

        public static DisplayMode GetClosestDisplayMode(int displayIndex, DisplayMode mode)
        {
            DisplayMode closest;
            Sdl.GetClosestDisplayMode(displayIndex, (Silk.NET.SDL.DisplayMode*)&mode, (Silk.NET.SDL.DisplayMode*)&closest);
            return closest;
        }

        public static DisplayMode GetCurrentDisplayMode(int displayIndex)
        {
            DisplayMode mode;
            Sdl.GetCurrentDisplayMode(displayIndex, (Silk.NET.SDL.DisplayMode*)&mode);
            return mode;
        }

        public static DisplayMode GetDesktopDisplayMode(int displayIndex)
        {
            DisplayMode mode;
            Sdl.GetDesktopDisplayMode(displayIndex, (Silk.NET.SDL.DisplayMode*)&mode);
            return mode;
        }

        public static string GetDisplayName(int displayIndex)
        {
            return Sdl.GetDisplayNameS(displayIndex);
        }

        public static DisplayOrientation GetDisplayOrientation(int displayIndex)
        {
            return (DisplayOrientation)Sdl.GetDisplayOrientation(displayIndex);
        }

        public static void GetDisplayDPI(int displayIndex, ref float ddpi, ref float hdpi, ref float vdpi)
        {
            Sdl.GetDisplayDPI(displayIndex, ref ddpi, ref hdpi, ref vdpi);
        }

        public static void GetDisplayBounds(int displayIndex, ref int x, ref int y, ref int width, ref int height)
        {
            Rectangle<int> rectangle;
            Sdl.GetDisplayBounds(displayIndex, &rectangle);
            x = rectangle.Origin.X;
            y = rectangle.Origin.Y;
            width = rectangle.Size.X;
            height = rectangle.Size.Y;
        }

        public static void GetDisplayUsableBounds(int displayIndex, ref int x, ref int y, ref int width, ref int height)
        {
            Rectangle<int> rectangle;
            Sdl.GetDisplayUsableBounds(displayIndex, &rectangle);
            x = rectangle.Origin.X;
            y = rectangle.Origin.Y;
            width = rectangle.Size.X;
            height = rectangle.Size.Y;
        }

        public static DisplayMode GetDisplayMode(int displayIndex, int modeIndex)
        {
            DisplayMode mode;
            Sdl.GetDisplayMode(displayIndex, modeIndex, (Silk.NET.SDL.DisplayMode*)&mode);
            return mode;
        }

        public static int GetNumDisplayModes(int displayIndex)
        {
            return Sdl.GetNumDisplayModes(displayIndex);
        }

        public static int GetPointDisplayIndex(int x, int y)
        {
            return Sdl.GetPointDisplayIndex(new Point(x, y));
        }

        public static int GetRectDisplayIndex(int x, int y, int width, int height)
        {
            return Sdl.GetRectDisplayIndex(new Rectangle<int>(x, y, width, height));
        }

        public static int GetWindowDisplayIndex(Windows.SdlWindow window)
        {
            return Sdl.GetWindowDisplayIndex(window.GetWindow());
        }

        public static DisplayMode GetWindowDisplayMode(Windows.SdlWindow window)
        {
            DisplayMode mode;
            Sdl.GetWindowDisplayMode(window.GetWindow(), (Silk.NET.SDL.DisplayMode*)&mode);
            return mode;
        }
    }
}