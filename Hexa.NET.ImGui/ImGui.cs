#nullable disable

namespace Hexa.NET.ImGui
{
    public static unsafe partial class ImGui
    {
        static ImGui()
        {
            InitApi();
        }

        public static string GetLibarayName()
        {
            return "cimgui";
        }
    }
}