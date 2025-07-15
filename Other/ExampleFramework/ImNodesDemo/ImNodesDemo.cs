namespace ExampleFramework.ImNodesDemo
{
    using ExampleFramework.NodeEditor;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImNodes;

    public class ImNodesDemo
    {
        private NodeEditor editor = new();

        public ImNodesDemo()
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

        public void Draw()
        {
            if (!ImGui.Begin("Demo ImNodes", ImGuiWindowFlags.MenuBar))
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