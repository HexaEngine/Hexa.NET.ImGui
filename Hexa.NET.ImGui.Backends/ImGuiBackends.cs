namespace Hexa.NET.ImGui.Backends
{
    using System.Runtime.InteropServices;

    public static partial class ImGuiBackends
    {
        static ImGuiBackends()
        {
            InitApi();
        }

        public static string GetLibraryName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "cimgui_impl";
            }
            return "libcimgui_impl";
        }
    }
}