namespace HexaEngine.Core.Input
{
    using Silk.NET.SDL;

    public static class TouchDevices
    {
        private static readonly Sdl sdl = Sdl.GetApi();

        internal static void Init()
        {
            var touchdevCount = sdl.GetNumTouchDevices();
            for (int i = 0; i < touchdevCount; i++)
            {
                var id = sdl.GetTouchDevice(i);
            }
        }
    }
}