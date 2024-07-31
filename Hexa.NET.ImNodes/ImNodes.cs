namespace Hexa.NET.ImNodes
{
    public static unsafe partial class ImNodes
    {
        static ImNodes()
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