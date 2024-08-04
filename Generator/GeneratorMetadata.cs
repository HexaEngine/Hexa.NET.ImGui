namespace Generator
{
    public class GeneratorMetadata
    {
        public List<string> Constants { get; } = [];

        public List<string> Enums { get; } = [];

        public List<string> Extensions { get; } = [];

        public List<string> Functions { get; } = [];

        public List<string> Typedefs { get; } = [];

        public List<string> Types { get; } = [];

        public List<string> Delegates { get; } = [];

        public int VTableLength { get; private set; }

        public void CopyTo(CsCodeGenerator generator)
        {
            generator.LibDefinedConstants.AddRange(Constants);
            generator.LibDefinedEnums.AddRange(Enums);
            generator.LibDefinedExtensions.AddRange(Extensions);
            generator.LibDefinedFunctions.AddRange(Functions);
            generator.LibDefinedTypedefs.AddRange(Typedefs);
            generator.LibDefinedTypes.AddRange(Types);
            generator.LibDefinedDelegates.AddRange(Delegates);
        }

        public void CopyFrom(CsCodeGenerator generator)
        {
            Constants.AddRange(generator.DefinedConstants);
            Enums.AddRange(generator.DefinedEnums);
            Extensions.AddRange(generator.DefinedExtensions);
            Functions.AddRange(generator.DefinedFunctions);
            Typedefs.AddRange(generator.DefinedTypedefs);
            Types.AddRange(generator.DefinedTypes);
            Delegates.AddRange(generator.DefinedDelegates);
            VTableLength = generator.VTableLength;
        }

        public void Clear()
        {
            Constants.Clear();
            Enums.Clear();
            Extensions.Clear();
            Functions.Clear();
            Typedefs.Clear();
            Types.Clear();
            Delegates.Clear();
        }
    }
}