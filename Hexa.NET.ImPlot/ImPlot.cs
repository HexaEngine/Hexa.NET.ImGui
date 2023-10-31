namespace Hexa.NET.ImPlot
{
    public static unsafe partial class ImPlot
    {
        static ImPlot()
        {
            LibraryLoader.SetImportResolver();
        }
    }
}