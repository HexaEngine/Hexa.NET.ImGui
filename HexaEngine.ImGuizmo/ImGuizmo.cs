namespace HexaEngine.ImGuizmoNET
{
    public static unsafe partial class ImGuizmo
    {
        static ImGuizmo()
        {
            LibraryLoader.SetImportResolver();
        }
    }
}