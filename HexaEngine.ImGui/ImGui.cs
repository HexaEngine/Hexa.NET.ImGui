namespace HexaEngine.ImGui
{
    public static unsafe partial class ImGui
    {
        static ImGui()
        {
            LibraryLoader.SetImportResolver();
        }
    }
}