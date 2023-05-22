namespace Generator
{
    using CppAst;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    public static partial class CsCodeGenerator
    {
        public static void Generate(CppCompilation compilation, string outputPath)
        {
            GenerateConstants(compilation, outputPath);
            GenerateEnums(compilation, outputPath);
            GenerateHandles(compilation, outputPath);
            GenerateStructAndUnions(compilation, outputPath);
            GenerateCommands(compilation, outputPath);
        }

        public static void Reset()
        {
            DefinedConstants.Clear();
            DefinedEnums.Clear();
            DefinedExtensions.Clear();
            DefinedFunctions.Clear();
            DefinedTypedefs.Clear();
            DefinedTypes.Clear();
        }

        public static void CopyFrom(List<string> constants, List<string> enums, List<string> extensions, List<string> functions, List<string> typedefs, List<string> types)
        {
            for (int i = 0; i < constants.Count; i++)
            {
                DefinedConstants.Add(constants[i]);
            }
            for (int i = 0; i < enums.Count; i++)
            {
                DefinedEnums.Add(enums[i]);
            }
            for (int i = 0; i < extensions.Count; i++)
            {
                DefinedExtensions.Add(extensions[i]);
            }
            for (int i = 0; i < functions.Count; i++)
            {
                DefinedFunctions.Add(functions[i]);
            }
            for (int i = 0; i < typedefs.Count; i++)
            {
                DefinedTypedefs.Add(typedefs[i]);
            }
            for (int i = 0; i < types.Count; i++)
            {
                DefinedTypes.Add(types[i]);
            }
        }

        public static void AddCsMapping(string typeName, string csTypeName)
        {
            CsCodeGeneratorSettings.Default.NameMappings.Add(typeName, csTypeName);
        }

        private static string GetCsCleanName(string name)
        {
            if (CsCodeGeneratorSettings.Default.NameMappings.TryGetValue(name, out string? mappedName))
            {
                return mappedName;
            }
            else if (name.StartsWith("PFN"))
            {
                return "nint";
            }

            StringBuilder sb = new();
            bool wasLower = false;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (c == '_')
                {
                    wasLower = true;
                    continue;
                }
                if (i == 0)
                {
                    c = char.ToUpper(c);
                }

                if (wasLower)
                {
                    c = char.ToUpper(c);
                    wasLower = false;
                }
                sb.Append(c);
            }

            if (sb[^1] == 'T')
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        private static CppFunction FindFunction(CppCompilation compilation, string name)
        {
            for (int i = 0; i < compilation.Functions.Count; i++)
            {
                var function = compilation.Functions[i];
                if (function.Name == name)
                    return function;
            }
            return null;
        }

        private static string GetCsTypeName(CppType? type, bool isPointer = false)
        {
            if (type is CppPrimitiveType primitiveType)
            {
                return GetCsTypeName(primitiveType, isPointer);
            }

            if (type is CppQualifiedType qualifiedType)
            {
                return GetCsTypeName(qualifiedType.ElementType, isPointer);
            }

            if (type is CppEnum enumType)
            {
                var enumCsName = GetCsCleanName(enumType.Name);
                if (isPointer)
                    return enumCsName + "*";

                return enumCsName;
            }

            if (type is CppTypedef typedef)
            {
                var typeDefCsName = GetCsCleanName(typedef.Name);
                if (IsDelegate(typedef, out var _))
                {
                    return typeDefCsName;
                }

                if (isPointer)
                    return typeDefCsName + "*";

                return typeDefCsName;
            }

            if (type is CppClass @class)
            {
                var className = GetCsCleanName(@class.Name);
                if (isPointer)
                    return className + "*";

                return className;
            }

            if (type is CppPointerType pointerType)
            {
                var pointerName = GetCsTypeName(pointerType);
                if (isPointer)
                    return pointerName + "*";

                return pointerName;
            }

            if (type is CppArrayType arrayType)
            {
                var arrayName = GetCsTypeName(arrayType.ElementType, false);
                return arrayName + "*";
            }

            return string.Empty;
        }

        private static bool TryGetArrayMapping(CppArrayType arrayType, [NotNullWhen(true)] out string? mapping)
        {
            for (int i = 0; i < CsCodeGeneratorSettings.Default.ArrayMappings.Count; i++)
            {
                var map = CsCodeGeneratorSettings.Default.ArrayMappings[i];
                if (map.Primitive == GetPrimitiveKind(arrayType, false) && map.Size == arrayType.Size)
                {
                    mapping = map.Name;
                    return true;
                }
            }
            mapping = null;
            return false;
        }

        private static string GetCsTypeName(CppPrimitiveType primitiveType, bool isPointer)
        {
            switch (primitiveType.Kind)
            {
                case CppPrimitiveKind.Void:
                    return isPointer ? "void*" : "void";

                case CppPrimitiveKind.Char:
                    return isPointer ? "byte*" : "byte";

                case CppPrimitiveKind.Bool:
                    return isPointer ? "bool*" : "bool";

                case CppPrimitiveKind.WChar:
                    return isPointer ? "char*" : "char";

                case CppPrimitiveKind.Short:
                    return isPointer ? "short*" : "short";

                case CppPrimitiveKind.Int:
                    return isPointer ? "int*" : "int";

                case CppPrimitiveKind.LongLong:
                    return isPointer ? "long*" : "long";

                case CppPrimitiveKind.UnsignedChar:
                    return isPointer ? "byte*" : "byte";

                case CppPrimitiveKind.UnsignedShort:
                    return isPointer ? "ushort*" : "ushort";

                case CppPrimitiveKind.UnsignedInt:
                    return isPointer ? "uint*" : "uint";

                case CppPrimitiveKind.UnsignedLongLong:
                    return isPointer ? "ulong*" : "ulong";

                case CppPrimitiveKind.Float:
                    return isPointer ? "float*" : "float";

                case CppPrimitiveKind.Double:
                    return isPointer ? "double*" : "double";

                case CppPrimitiveKind.LongDouble:
                    break;

                default:
                    return string.Empty;
            }

            return string.Empty;
        }

        private static string GetCsTypeName(CppPointerType pointerType)
        {
            if (pointerType.ElementType is CppQualifiedType qualifiedType)
            {
                if (qualifiedType.ElementType is CppPrimitiveType primitiveType)
                {
                    return GetCsTypeName(primitiveType, true);
                }
                else if (qualifiedType.ElementType is CppClass @classType)
                {
                    return GetCsTypeName(@classType, true);
                }
                else if (qualifiedType.ElementType is CppPointerType subPointerType)
                {
                    return GetCsTypeName(subPointerType, true) + "*";
                }
                else if (qualifiedType.ElementType is CppTypedef typedef)
                {
                    return GetCsTypeName(typedef, true);
                }
                else if (qualifiedType.ElementType is CppEnum @enum)
                {
                    return GetCsTypeName(@enum, true);
                }

                return GetCsTypeName(qualifiedType.ElementType, true);
            }

            if (pointerType.ElementType is CppFunctionType functionType)
            {
                return $"delegate*<{GetNamelessParameterSignature(functionType.Parameters, false)}>";
            }

            if (pointerType.ElementType is CppPointerType subPointer)
            {
                return GetCsTypeName(subPointer) + "*";
            }

            return GetCsTypeName(pointerType.ElementType, true);
        }

        private static string GetCsWrapperTypeName(CppType? type, bool isPointer = false)
        {
            if (type is CppPrimitiveType primitiveType)
            {
                return GetCsWrapperTypeName(primitiveType, isPointer);
            }

            if (type is CppQualifiedType qualifiedType)
            {
                return GetCsWrapperTypeName(qualifiedType.ElementType, isPointer);
            }

            if (type is CppEnum enumType)
            {
                var enumCsName = GetCsCleanName(enumType.Name);
                if (isPointer)
                    return "ref " + enumCsName;

                return enumCsName;
            }

            if (type is CppTypedef typedef)
            {
                var typeDefCsName = GetCsCleanName(typedef.Name);
                if (IsDelegate(typedef, out var _))
                {
                    return typeDefCsName;
                }
                if (isPointer)
                    return "ref " + typeDefCsName;

                return typeDefCsName;
            }

            if (type is CppClass @class)
            {
                var className = GetCsCleanName(@class.Name);
                if (isPointer)
                    return "ref " + className;

                return className;
            }

            if (type is CppPointerType pointerType)
            {
                return GetCsWrapperTypeName(pointerType);
            }

            if (type is CppArrayType arrayType && arrayType.Size > 0)
            {
                if (TryGetArrayMapping(arrayType, out string? mapping))
                {
                    return mapping;
                }

                return GetCsWrapperTypeName(arrayType.ElementType, true);
            }
            else if (type is CppArrayType arrayType1 && arrayType1.Size < 0)
            {
                var arrayName = GetCsTypeName(arrayType1.ElementType, false);
                return arrayName + "*";
            }

            return string.Empty;
        }

        private static string GetCsWrapperTypeName(CppPrimitiveType primitiveType, bool isPointer)
        {
            switch (primitiveType.Kind)
            {
                case CppPrimitiveKind.Void:
                    return isPointer ? "void*" : "void";

                case CppPrimitiveKind.Char:
                    return isPointer ? "ref byte" : "byte";

                case CppPrimitiveKind.Bool:
                    return isPointer ? "ref bool" : "bool";

                case CppPrimitiveKind.WChar:
                    return isPointer ? "ref char" : "char";

                case CppPrimitiveKind.Short:
                    return isPointer ? "ref short" : "short";

                case CppPrimitiveKind.Int:
                    return isPointer ? "ref int" : "int";

                case CppPrimitiveKind.LongLong:
                    break;

                case CppPrimitiveKind.UnsignedChar:
                    return isPointer ? "ref byte" : "byte";

                case CppPrimitiveKind.UnsignedShort:
                    return isPointer ? "ref ushort" : "ushort";

                case CppPrimitiveKind.UnsignedInt:
                    return isPointer ? "ref uint" : "uint";

                case CppPrimitiveKind.UnsignedLongLong:
                    break;

                case CppPrimitiveKind.Float:
                    return isPointer ? "ref float" : "float";

                case CppPrimitiveKind.Double:
                    return isPointer ? "ref double" : "double";

                case CppPrimitiveKind.LongDouble:
                    break;

                default:
                    return string.Empty;
            }

            return string.Empty;
        }

        private static string GetCsWrapperTypeName(CppPointerType pointerType)
        {
            if (pointerType.ElementType is CppQualifiedType qualifiedType)
            {
                if (qualifiedType.ElementType is CppPrimitiveType primitiveType)
                {
                    return GetCsWrapperTypeName(primitiveType, true);
                }
                else if (qualifiedType.ElementType is CppClass @classType)
                {
                    return GetCsWrapperTypeName(@classType, true);
                }
                else if (qualifiedType.ElementType is CppPointerType subPointerType)
                {
                    return GetCsWrapperTypeName(subPointerType, true) + "*";
                }
                else if (qualifiedType.ElementType is CppTypedef typedef)
                {
                    return GetCsWrapperTypeName(typedef, true);
                }
                else if (qualifiedType.ElementType is CppEnum @enum)
                {
                    return GetCsWrapperTypeName(@enum, true);
                }

                return GetCsWrapperTypeName(qualifiedType.ElementType, true);
            }

            if (pointerType.ElementType is CppFunctionType functionType)
            {
                return $"delegate*<{GetNamelessParameterSignature(functionType.Parameters, false)}>";
            }

            if (pointerType.ElementType is CppPointerType subPointer)
            {
                return GetCsWrapperTypeName(subPointer) + "*";
            }

            return GetCsWrapperTypeName(pointerType.ElementType, true);
        }

        private static bool WriteCsSummary(CppComment? comment, CodeWriter writer)
        {
            if (comment is CppCommentFull full)
            {
                writer.WriteLine("/// <summary>");
                for (int i = 0; i < full.Children.Count; i++)
                {
                    WriteCsSummary(full.Children[i], writer);
                }
                writer.WriteLine("/// </summary>");
                return true;
            }
            if (comment is CppCommentParagraph paragraph)
            {
                for (int i = 0; i < paragraph.Children.Count; i++)
                {
                    WriteCsSummary(paragraph.Children[i], writer);
                }
                return true;
            }
            if (comment is CppCommentText text)
            {
                writer.WriteLine($"/// " + text.Text);
                return true;
            }

            if (comment == null || comment.Kind == CppCommentKind.Null)
            {
                return false;
            }

            throw new NotImplementedException();
        }

        private static void WriteCsSummary(CppComment? cppComment, out string? comment)
        {
            StringBuilder sb = new();
            if (cppComment is CppCommentFull full)
            {
                sb.AppendLine("/// <summary>");
                for (int i = 0; i < full.Children.Count; i++)
                {
                    WriteCsSummary(full.Children[i], out var subComment);
                    sb.Append(subComment);
                }
                sb.AppendLine("/// </summary>");
                comment = sb.ToString();
                return;
            }
            if (cppComment is CppCommentParagraph paragraph)
            {
                for (int i = 0; i < paragraph.Children.Count; i++)
                {
                    WriteCsSummary(paragraph.Children[i], out var subComment);
                    sb.Append(subComment);
                }
                comment = sb.ToString();
                return;
            }
            if (cppComment is CppCommentText text)
            {
                sb.AppendLine($"/// " + text.Text);
                comment = sb.ToString();
                return;
            }

            if (cppComment == null || cppComment.Kind == CppCommentKind.Null)
            {
                comment = null;
                return;
            }

            throw new NotImplementedException();
        }
    }
}