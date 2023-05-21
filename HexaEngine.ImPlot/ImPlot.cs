namespace HexaEngine.ImPlot
{
    public static unsafe partial class ImPlot
    {
        static ImPlot()
        {
            LibraryLoader.SetImportResolver();
        }
    }
}