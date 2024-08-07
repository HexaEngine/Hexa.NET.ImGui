namespace ExampleFramework.NodeEditor
{
    using Hexa.NET.ImNodes;

    public class Link
    {
        private readonly Node outputNode;
        private readonly Pin output;
        private readonly Node inputNode;
        private readonly Pin input;

        private NodeEditor? editor;
        private int id;
        private int outputId;
        private int inputId;

        public Link(int id, Node outputNode, Pin output, Node inputNode, Pin input)
        {
            this.id = id;
            this.outputNode = outputNode;
            this.output = output;
            outputId = output.Id;
            this.inputNode = inputNode;
            this.input = input;
            inputId = input.Id;
        }

        public int Id => id;

        public Node OutputNode => outputNode;

        public Pin Output => output;

        public Node InputNode => inputNode;

        public Pin Input => input;

        public void Draw()
        {
            ImNodes.Link(id, outputId, inputId);
        }

        public virtual void Destroy()
        {
            if (editor == null) return;
            editor.RemoveLink(this);
            OutputNode.RemoveLink(this);
            Output.RemoveLink(this);
            InputNode.RemoveLink(this);
            Input.RemoveLink(this);
            editor = null;
        }

        public virtual void Initialize(NodeEditor editor)
        {
            this.editor = editor;

            outputId = output.Id;
            inputId = input.Id;

            OutputNode.AddLink(this);
            Output.AddLink(this);
            InputNode.AddLink(this);
            Input.AddLink(this);
        }
    }
}