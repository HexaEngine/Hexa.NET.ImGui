namespace Example.ImNodesDemo
{
    using Example.NodeEditor;
    using HexaEngine.ImGuiNET;
    using HexaEngine.ImNodesNET;

    public class ImNodesDemo
    {
        private static NodeEditor editor = new();

        static ImNodesDemo()
        {
            editor.Initialize();
            var node1 = editor.CreateNode("Node");
            node1.CreatePin(editor, "In", PinKind.Input, PinType.DontCare, ImNodesPinShape.Circle);
            var out1 = node1.CreatePin(editor, "Out", PinKind.Output, PinType.DontCare, ImNodesPinShape.Circle);
            var node2 = editor.CreateNode("Node");
            var in2 = node2.CreatePin(editor, "In", PinKind.Input, PinType.DontCare, ImNodesPinShape.Circle);
            var out2 = node2.CreatePin(editor, "Out", PinKind.Output, PinType.DontCare, ImNodesPinShape.Circle);
            var node3 = editor.CreateNode("Node");
            var in3 = node3.CreatePin(editor, "In", PinKind.Input, PinType.DontCare, ImNodesPinShape.Circle);
            node3.CreatePin(editor, "Out", PinKind.Output, PinType.DontCare, ImNodesPinShape.Circle);
            editor.CreateLink(out1, in2);
            editor.CreateLink(out1, in3);
            editor.CreateLink(out2, in3);
        }

        public static unsafe void Draw()
        {
            if (!ImGui.Begin("Demo ImNodes", null, ImGuiWindowFlags.MenuBar))
            {
                ImGui.End();
                return;
            }

            if (ImGui.BeginMenuBar())
            {
                if (ImGui.MenuItem("New Node"))
                {
                    var node = editor.CreateNode("Node");
                    node.CreatePin(editor, "In", PinKind.Input, PinType.DontCare, ImNodesPinShape.Circle);
                    node.CreatePin(editor, "Out", PinKind.Output, PinType.DontCare, ImNodesPinShape.Circle);
                }
                ImGui.EndMenuBar();
            }

            editor.Draw();

            ImGui.End();
        }
    }
}