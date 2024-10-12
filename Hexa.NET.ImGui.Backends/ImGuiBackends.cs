namespace Hexa.NET.ImGui.Backends
{
    public static partial class ImGuiBackends
    {
        static ImGuiBackends()
        {
            InitApi();
        }

        public static string GetLibraryName()
        {
            return "cimgui_impl";
        }
    }
}