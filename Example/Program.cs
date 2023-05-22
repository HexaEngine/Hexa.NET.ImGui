namespace Example
{
    using HexaEngine;
    using HexaEngine.Core;
    using HexaEngine.ImGuiNET;
    using HexaEngine.Windows;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Platform.Init();
            Application.Run(new Window());
            ImGuiIOPtr io = ImGui.GetIO();
        }
    }
}