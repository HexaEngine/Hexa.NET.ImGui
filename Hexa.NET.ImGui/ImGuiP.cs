#nullable disable

namespace Hexa.NET.ImGui
{
    using HexaGen.Runtime;

    public static unsafe partial class ImGuiP
    {
        internal static FunctionTable funcTable;

        static ImGuiP()
        {
            if (ImGui.funcTable == null)
            {
                ImGui.InitApi();
            }

            funcTable = ImGui.funcTable;
        }
    }
}