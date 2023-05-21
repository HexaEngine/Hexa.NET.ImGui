namespace HexaEngine.ImPlotNET
{
    public static unsafe partial class ImPlot
    {
        static ImPlot()
        {
            LibraryLoader.SetImportResolver();
        }
    }
}