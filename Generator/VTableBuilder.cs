namespace Generator
{
    using System.Text;

    public class VTableBuilder
    {
        private readonly StringBuilder sb = new();
        private int index;
        public VTableBuilder()
        {
        }

        public VTableBuilder(int vTableStart)
        {
            this.index = vTableStart;
        }

        public int Add(string name)
        {
            int id = index;
            sb.AppendLine($"vt.Load({id}, \"{name}\");");
            index++;
            return id;
        }

        public string Finish(out int count)
        {
            count = index + 1;
            return sb.ToString();
        }
    }
}