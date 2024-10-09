namespace Hexa.NET.ImGuizmo
{
    public static unsafe partial class ImGuizmo
    {
        static ImGuizmo()
        {
            InitApi();
        }

        public static string GetLibarayName()
        {
            return "cimguizmo";
        }
    }
}