namespace HexaEngine.ImGuiNET
{
    public static unsafe partial class ImGui
    {
        static ImGui()
        {
            LibraryLoader.SetImportResolver();
        }
    }
}