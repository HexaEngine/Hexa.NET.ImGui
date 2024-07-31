namespace Hexa.NET.ImPlot
{
    public static unsafe partial class ImPlot
    {
        static ImPlot()
        {
            LibraryLoader.SetImportResolver();
            InitApi();
        }

        public static nint GetLibraryName()
        {
            return LibraryLoader.LoadLibrary();
        }
    }
}