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

        public bool IsSpan { get; set; }

        public bool IsString { get; set; }

        public bool IsPrimitive { get; set; }

        public bool IsVoid { get; set; }

        public bool IsBool { get; set; }

        public bool IsArray { get; set; }

        public CsStringType StringType { get; set; }

        public CsPrimitiveType PrimitiveType { get; set; }

        public static bool IsKnownPrimitive(string name)
        {
            if (name.StartsWith("void"))
                return true;
            if (name.StartsWith("bool"))
                return true;
            if (name.StartsWith("byte"))
                return true;
            if (name.StartsWith("sbyte"))
                return true;
            if (name.StartsWith("char"))
                return true;
            if (name.StartsWith("short"))
                return true;
            if (name.StartsWith("ushort"))
                return true;
            if (name.StartsWith("int"))
                return true;
            if (name.StartsWith("uint"))
                return true;
            if (name.StartsWith("long"))
                return true;
            if (name.StartsWith("ulong"))
                return true;
            if (name.StartsWith("float"))
                return true;
            if (name.StartsWith("double"))
                return true;
            if (name.StartsWith("Vector2"))
                return true;
            if (name.StartsWith("Vector3"))
                return true;
            if (name.StartsWith("Vector4"))
                return true;
            return false;
        }

        public void Classify()
        {
            IsSpan = Name.StartsWith("ReadOnlySpan<") || Name.StartsWith("Span<");
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

            if (IsRef)
            {
                CleanName = Name.Replace("ref ", string.Empty);
            }
            else if (IsSpan)
            {
                CleanName = Name.Replace("ReadOnlySpan<", string.Empty).Replace("Span<", string.Empty).Replace(">", string.Empty);
            }
            else if (IsArray)
            {
                CleanName = Name.Replace("[]", string.Empty);
            }
            else if (IsPointer)
            {
                CleanName = Name.Replace("*", string.Empty);
            }
            else
            {
                CleanName = Name;
            }
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