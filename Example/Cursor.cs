namespace HexaEngine.Core
{
    using Silk.NET.SDL;

    public static unsafe class CursorHelper
    {
        private static readonly Sdl Sdl = Sdl.GetApi();

        public static void SetCursor(SystemCursor cursor)
        {
            Sdl.SetCursor(Sdl.CreateSystemCursor(cursor));
        }

        public static void SetCursor(IntPtr ptr)
        {
            Sdl.SetCursor((Cursor*)ptr.ToPointer());
        }

        public static void SetCursor(Cursor* cursor)
        {
            Sdl.SetCursor(cursor);
        }

        public static Cursor* CreateSystemCursor(SystemCursor cursor)
        {
            return Sdl.CreateSystemCursor(cursor);
        }
    }
}