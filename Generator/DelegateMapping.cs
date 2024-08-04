namespace Generator
{
    public class DelegateMapping
    {
        public DelegateMapping(string name, string returnType, string signature)
        {
            Name = name;
            ReturnType = returnType;
            Signature = signature;
        }

        public string Name { get; set; }

        public string ReturnType { get; set; }

        public string Signature { get; set; }
    }
}