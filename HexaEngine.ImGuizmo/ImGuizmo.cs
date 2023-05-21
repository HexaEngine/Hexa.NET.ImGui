namespace HexaEngine.ImGuizmo
{
    public static unsafe partial class ImGuizmo
    {
        static ImGuizmo()
        {
            LibraryLoader.SetImportResolver();
        }
    }
}