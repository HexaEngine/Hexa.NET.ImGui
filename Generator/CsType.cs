namespace Generator
{
    using CppAst;

    public enum CsStringType
    {
        None,
        StringUTF8,
        StringUTF16
    }

    public enum CsPrimitiveType
    {
        Void,
        Bool,
        Byte,
        SByte,
        Char,
        UShort,
        Short,
        UInt,
        Int,
        ULong,
        Long,
        Float,
        Double,
    }

    public class CsType
    {
        public CsType(string name, bool isPointer, bool isRef, bool isString, bool isPrimitive, bool isVoid, bool isArray, CsPrimitiveType primitiveType)
        {
            Name = name;
            IsPointer = isPointer;
            IsRef = isRef;
            IsString = isString;
            IsPrimitive = isPrimitive;
            IsVoid = isVoid;
            IsArray = isArray;
            PrimitiveType = primitiveType;
            Classify();
        }

        public CsType(string name, CsPrimitiveType primitiveType)
        {
            Name = name;
            PrimitiveType = primitiveType;
            Classify();
        }

        public CsType(string name, CppPrimitiveKind primitiveType)
        {
            Name = name;
            PrimitiveType = Map(primitiveType);
            Classify();
        }

        public string Name { get; set; }

        public string CleanName { get; set; }

        public bool IsPointer { get; set; }

        public bool IsRef { get; set; }

        public bool IsString { get; set; }

        public bool IsPrimitive { get; set; }

        public bool IsVoid { get; set; }

        public bool IsBool { get; set; }

        public bool IsArray { get; set; }

        public CsStringType StringType { get; set; }

        public CsPrimitiveType PrimitiveType { get; set; }

        public void Classify()
        {
            IsRef = Name.StartsWith("ref");
            IsArray = Name.Contains("[]");
            IsPointer = Name.Contains('*');
            IsBool = Name.Contains("bool");
            IsString = Name.Contains("string");
            IsVoid = Name.StartsWith("void");

            IsPrimitive = !IsRef && !IsArray && !IsPointer && !IsArray && !IsString;
            if (IsString)
            {
                if (PrimitiveType == CsPrimitiveType.Byte)
                {
                    StringType = CsStringType.StringUTF8;
                }
                if (PrimitiveType == CsPrimitiveType.Char)
                {
                    StringType = CsStringType.StringUTF16;
                }
            }

            CleanName = Name.Replace("ref ", string.Empty);
        }

        public static CsPrimitiveType Map(CppPrimitiveKind kind)
        {
            return kind switch
            {
                CppPrimitiveKind.Void => CsPrimitiveType.Void,
                CppPrimitiveKind.Bool => CsPrimitiveType.Bool,
                CppPrimitiveKind.WChar => CsPrimitiveType.Char,
                CppPrimitiveKind.Char => CsPrimitiveType.Byte,
                CppPrimitiveKind.Short => CsPrimitiveType.Short,
                CppPrimitiveKind.Int => CsPrimitiveType.Int,
                CppPrimitiveKind.LongLong => CsPrimitiveType.Long,
                CppPrimitiveKind.UnsignedChar => CsPrimitiveType.Byte,
                CppPrimitiveKind.UnsignedShort => CsPrimitiveType.UShort,
                CppPrimitiveKind.UnsignedInt => CsPrimitiveType.UInt,
                CppPrimitiveKind.UnsignedLongLong => CsPrimitiveType.ULong,
                CppPrimitiveKind.Float => CsPrimitiveType.Float,
                CppPrimitiveKind.Double => CsPrimitiveType.Double,
                CppPrimitiveKind.LongDouble => CsPrimitiveType.Double,
                _ => throw new NotImplementedException(),
            };
        }

        public override string ToString()
        {
            return Name;
        }
    }
}