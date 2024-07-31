#nullable disable
namespace Hexa.NET.ImGui
{
    using System.Numerics;
    using System.Runtime.InteropServices;

    public static unsafe partial class ImGui
    {
        static ImGui()
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