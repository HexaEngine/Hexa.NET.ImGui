namespace Hexa.NET.ImGui.Backends.SDL2
{
    using System.Runtime.InteropServices;

    public static partial class ImGuiImplSDL2
    {
        static ImGuiImplSDL2()
        {
            InitApi();
        }

        public static string GetLibraryName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "ImGuiImplSDL2";
            }
            return "libImGuiImplSDL2";
        }
    }
}