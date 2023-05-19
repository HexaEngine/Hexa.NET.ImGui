namespace HexaEngine
{
    using HexaEngine.Core;
    using HexaEngine.Core.Debugging;
    using HexaEngine.D3D11;

    public static class Platform
    {
        public static void Init()
        {
            CrashLogger.Initialize();
            DXGIAdapterD3D11.Init();

#if DEBUG
            Application.GraphicsDebugging = true;
#endif
        }
    }
}