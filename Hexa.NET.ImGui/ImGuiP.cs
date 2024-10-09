#nullable disable

namespace Hexa.NET.ImGui
{
    using HexaGen.Runtime;

    public static unsafe partial class ImGuiP
    {
        internal static VTable vt;

        static ImGuiP()
        {
            if (ImGui.vt == null)
            {
                ImGui.InitApi();
            }

            vt = ImGui.vt;
        }
    }
}