namespace HexaEngine.ImNodesNET
{
    public static unsafe partial class ImNodes
    {
        static ImNodes()
        {
            LibraryLoader.SetImportResolver();
        }
    }
}