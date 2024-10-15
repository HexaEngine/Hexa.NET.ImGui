namespace Hexa.NET.ImGui.Backends.GLFW
{
    using System.Runtime.InteropServices;

    public static partial class ImGuiImplGLFW
    {
        static ImGuiImplGLFW()
        {
            InitApi();
        }

        public static string GetLibraryName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "ImGuiImplGLFW";
            }
            return "libImGuiImplGLFW";
        }
    }
}