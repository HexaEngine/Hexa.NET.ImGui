namespace HexaEngine.Core
{
    using Silk.NET.SDL;

    public static unsafe class Clipboard
    {
        private static readonly Sdl Sdl = Application.sdl;

        public static char* GetClipboardText()
        {
            return (char*)Sdl.GetClipboardText();
        }

        public static string GetClipboardTextS()
        {
            return Sdl.GetClipboardTextS();
        }

        public static void SetClipboardText(char* text)
        {
            Sdl.SetClipboardText((byte*)text);
        }

        public static void SetClipboardText(string text)
        {
            Sdl.SetClipboardText(text);
        }

        public static void Free(char* text)
        {
            Sdl.Free(text);
        }
    }
}