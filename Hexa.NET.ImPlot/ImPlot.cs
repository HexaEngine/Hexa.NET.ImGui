namespace Hexa.NET.ImPlot
{
    using HexaGen.Runtime;
    using System.Diagnostics;
    using System.Numerics;

    public static class ImPlotConfig
    {
        public static bool AotStaticLink;
    }

    public static unsafe partial class ImPlot
    {
        static ImPlot()
        {
            if (ImPlotConfig.AotStaticLink)
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
            return "cimplot";
        }

        public const int IMPLOT_AUTO = -1;

        public static readonly Vector4 IMPLOT_AUTO_COL = new(0, 0, 0, -1);
    }
}