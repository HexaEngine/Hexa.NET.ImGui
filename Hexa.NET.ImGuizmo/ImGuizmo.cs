namespace Hexa.NET.ImGuizmo
{
    public static unsafe partial class ImGuizmo
    {
        static ImGuizmo()
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