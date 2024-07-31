#nullable disable

namespace Hexa.NET.ImGui
{
    using System.Numerics;
    using System.Runtime.InteropServices;

    public static unsafe partial class ImGui
    {
        static ImGui()
        {
            InitApi();
            int length = 1420;
            vt.Resize(length + 10);
            vt.Load(length + 0, "igInputText");
            vt.Load(length + 1, "igInputTextMultiline");
            vt.Load(length + 2, "igInputTextWithHint");
            vt.Load(length + 3, "igImFormatString");
            vt.Load(length + 4, "igImFormatStringV");
            vt.Load(length + 5, "igImParseFormatTrimDecorations");
            vt.Load(length + 6, "igImTextStrToUtf8");
            vt.Load(length + 7, "igImTextStrFromUtf8");
            vt.Load(length + 8, "igGetKeyChordName");
            vt.Load(length + 9, "igDataTypeFormatString");
        }

        public static nint GetLibraryName()
        {
            return LibraryLoader.LoadLibrary();
        }
    }
}