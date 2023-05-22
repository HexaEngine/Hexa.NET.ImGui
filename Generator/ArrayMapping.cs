namespace Generator
{
    using CppAst;

    public class ArrayMapping
    {
        private CppPrimitiveKind primitive;
        private int size;
        private string name;

        public ArrayMapping(CppPrimitiveKind primitive, int size, string name)
        {
            this.primitive = primitive;
            this.size = size;
            this.name = name;
        }

        public CppPrimitiveKind Primitive { get => primitive; set => primitive = value; }

        public int Size { get => size; set => size = value; }

        public string Name { get => name; set => name = value; }
    }
}