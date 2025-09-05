namespace Hexa.NET.ImPlot3D
{
    using HexaGen.Runtime;
    using System.Diagnostics;

    public static class ImPlot3DConfig
    {
        public static bool AotStaticLink;
    }

    public static unsafe partial class ImPlot3D
    {
        static ImPlot3D()
        {
            if (ImPlot3DConfig.AotStaticLink)
            {
                InitApi(new NativeLibraryContext(Process.GetCurrentProcess().MainModule!.BaseAddress));
            }
            else
            {
                InitApi(new NativeLibraryContext(LibraryLoader.LoadLibrary(GetLibraryName, null)));
            }
        }

        public static string GetLibraryName()
        {
            return "cimplot3d";
        }
    }
}