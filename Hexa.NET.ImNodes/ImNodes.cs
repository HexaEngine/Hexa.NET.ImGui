namespace Hexa.NET.ImNodes
{
    public static unsafe partial class ImNodes
    {
        static ImNodes()
        {
            InitApi();
        }

        public static nint GetLibraryName()
        {
            return LibraryLoader.LoadLibrary();
        }
    }
}