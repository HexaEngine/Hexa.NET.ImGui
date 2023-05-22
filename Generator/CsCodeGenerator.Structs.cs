namespace Generator
{
    using ClangSharp;
    using CppAst;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static partial class CsCodeGenerator
    {
        private static readonly HashSet<string> s_definedTypes = new();

        private static void GenerateStructAndUnions(CppCompilation compilation, string outputPath)
        {
            string[] usings = { "System", "System.Runtime.CompilerServices", "System.Runtime.InteropServices" };

            // Generate Structures
            using var writer = new CodeWriter(Path.Combine(outputPath, "Structures.cs"), usings.Concat(CsCodeGeneratorSettings.Default.Usings).ToArray());

            // Print All classes, structs
            for (int i = 0; i < compilation.Classes.Count; i++)
            {
                CppClass? cppClass = compilation.Classes[i];
                if (CsCodeGeneratorSettings.Default.AllowedTypes.Count != 0 && !CsCodeGeneratorSettings.Default.AllowedTypes.Contains(cppClass.Name))
                    continue;
                if (CsCodeGeneratorSettings.Default.IgnoredTypes.Contains(cppClass.Name))
                    continue;
                if (s_definedTypes.Contains(cppClass.Name))
                    continue;
                s_definedTypes.Add(cppClass.Name);

                string csName = GetCsCleanName(cppClass.Name);
                WriteClass(writer, compilation, cppClass, csName);
            }
        }

        public static void WriteClass(CodeWriter writer, CppCompilation compilation, CppClass cppClass, string csName)
        {
            if (cppClass.ClassKind == CppClassKind.Class || cppClass.Name.EndsWith("_T") || csName == "void")
            {
                return;
            }
            Dictionary<CppType, string> subClasses = new();
            for (int j = 0; j < cppClass.Classes.Count; j++)
            {
                var subClass = cppClass.Classes[j];
                string csSubName;
                if (string.IsNullOrEmpty(subClass.Name))
                {
                    string label = cppClass.Classes.Count == 1 ? "" : j.ToString();
                    csSubName = csName + "Union" + label;
                }
                else
                {
                    csSubName = GetCsCleanName(subClass.Name);
                }

                WriteClass(writer, compilation, subClass, csSubName);
                subClasses.Add(subClass, csSubName);
            }

            bool isUnion = cppClass.ClassKind == CppClassKind.Union;

            if (isUnion)
            {
                writer.WriteLine("[StructLayout(LayoutKind.Explicit)]");
            }
            else
            {
                writer.WriteLine("[StructLayout(LayoutKind.Sequential)]");
            }

            bool isReadOnly = false;
            string modifier = "partial";

            WriteCsSummary(cppClass.Comment, writer);
            using (writer.PushBlock($"public {modifier} struct {csName}"))
            {
                if (CsCodeGeneratorSettings.Default.GenerateSizeOfStructs && cppClass.SizeOf > 0)
                {
                    writer.WriteLine("/// <summary>");
                    writer.WriteLine($"/// The size of the <see cref=\"{csName}\"/> type, in bytes.");
                    writer.WriteLine("/// </summary>");
                    writer.WriteLine($"public static readonly int SizeInBytes = {cppClass.SizeOf};");
                    writer.WriteLine();
                }

                for (int j = 0; j < cppClass.Fields.Count; j++)
                {
                    CppField cppField = cppClass.Fields[j];
                    if (cppField.Type is CppClass cppClass1 && cppClass1.ClassKind == CppClassKind.Union)
                    {
                        var subClass = subClasses[cppClass1];
                        if (isUnion)
                        {
                            writer.WriteLine("[FieldOffset(0)]");
                        }

                        writer.WriteLine($"public {subClass} {subClass};");
                    }
                    else if (cppField.Type is CppPointerType cppPointer && IsDelegate(cppPointer, out var cppFunctionType))
                    {
                        string csFieldName = NormalizeFieldName(cppField.Name);
                        string returnCsName = GetCsTypeName(cppFunctionType.ReturnType, false);
                        string signature = GetNamelessParameterSignature(cppFunctionType.Parameters, false);

                        writer.WriteLine($"public unsafe delegate*<{signature}, {returnCsName}>* {csFieldName};");
                    }
                    else
                    {
                        WriteField(writer, cppField, isUnion, isReadOnly);
                    }
                }

                writer.WriteLine();

                for (int j = 0; j < cppClass.Fields.Count; j++)
                {
                    CppField cppField = cppClass.Fields[j];
                    if (cppField.Type.TypeKind == CppTypeKind.Array)
                    {
                        WriteProperty(writer, cppField, isReadOnly);
                    }
                }

                if (CsCodeGeneratorSettings.Default.KnownMemberFunctions.TryGetValue(cppClass.Name, out var functions))
                {
                    HashSet<string> definedFunctions = new();
                    writer.WriteLine();
                    List<CsFunction> commands = new();
                    for (int i = 0; i < functions.Count; i++)
                    {
                        CppFunction cppFunction = FindFunction(compilation, functions[i]);
                        var csFunctionName = GetPrettyCommandName(cppFunction.Name);
                        string returnCsName = GetCsTypeName(cppFunction.ReturnType, false);
                        CppPrimitiveKind returnKind = GetPrimitiveKind(cppFunction.ReturnType, false);

                        CsFunction? function = null;
                        for (int j = 0; j < commands.Count; j++)
                        {
                            if (commands[j].Name == csFunctionName)
                            {
                                function = commands[j];
                                break;
                            }
                        }

                        if (function == null)
                        {
                            WriteCsSummary(cppFunction.Comment, out string? comment);
                            function = new(csFunctionName, comment);
                            commands.Add(function);
                        }

                        CsFunctionOverload overload = new(cppFunction.Name, csFunctionName, "", false, false, false, new(returnCsName, returnKind));
                        for (int j = 0; j < cppFunction.Parameters.Count; j++)
                        {
                            var cppParameter = cppFunction.Parameters[j];
                            var paramCsTypeName = GetCsTypeName(cppParameter.Type, false);
                            var paramCsName = GetParameterName(cppParameter.Type, cppParameter.Name);
                            var direction = GetDirection(cppParameter.Type);
                            var kind = GetPrimitiveKind(cppParameter.Type, false);

                            CsType csType = new(paramCsTypeName, kind);

                            CsParameterInfo csParameter = new(paramCsName, csType, direction);

                            overload.Parameters.Add(csParameter);
                            if (TryGetDefaultValue(cppFunction.Name, cppParameter, false, out var defaultValue))
                            {
                                overload.DefaultValues.Add(paramCsName, defaultValue);
                            }
                        }

                        function.Overloads.Add(overload);
                        GenerateVariations(cppFunction.Parameters, overload, null);

                        bool useThisRef = false;
                        if (cppFunction.Parameters.Count > 0 && IsPointerOf(cppClass, cppFunction.Parameters[0].Type))
                        {
                            useThisRef = true;
                        }

                        bool useThis = false;
                        if (cppFunction.Parameters.Count > 0 && IsType(cppClass, cppFunction.Parameters[0].Type))
                        {
                            useThis = true;
                        }

                        if (useThis || useThisRef)
                            WriteMethods(writer, definedFunctions, function, overload, useThis || useThisRef, "public unsafe");
                    }
                }
            }

            writer.WriteLine();
        }

        private static void WriteField(CodeWriter writer, CppField field, bool isUnion = false, bool isReadOnly = false)
        {
            string csFieldName = NormalizeFieldName(field.Name);
            WriteCsSummary(field.Comment, writer);
            if (isUnion)
            {
                writer.WriteLine("[FieldOffset(0)]");
            }

            if (field.Type is CppArrayType arrayType)
            {
                string csFieldType = GetCsTypeName(arrayType.ElementType, false);
                bool canUseFixed = false;
                if (arrayType.ElementType is CppPrimitiveType)
                {
                    canUseFixed = true;
                }
                else if (arrayType.ElementType is CppTypedef typedef && IsPrimitive(typedef, out var primitive))
                {
                    csFieldType = GetCsTypeName(primitive, false);
                    canUseFixed = true;
                }

                if (canUseFixed)
                {
                    writer.WriteLine($"public unsafe fixed {csFieldType} {csFieldName}[{arrayType.Size}];");
                }
                else
                {
                    string unsafePrefix = string.Empty;

                    if (csFieldType.EndsWith('*'))
                    {
                        unsafePrefix = "unsafe ";
                    }

                    for (int i = 0; i < arrayType.Size; i++)
                    {
                        writer.WriteLine($"public {unsafePrefix}{csFieldType} {csFieldName}_{i};");
                    }
                }
            }
            else
            {
                string csFieldType = GetCsTypeName(field.Type, false);
                string fieldPrefix = isReadOnly ? "readonly " : string.Empty;

                if (field.Type is CppTypedef typedef &&
                    typedef.ElementType is CppPointerType pointerType &&
                    pointerType.ElementType is CppFunctionType functionType)
                {
                    StringBuilder builder = new();
                    for (int i = 0; i < functionType.Parameters.Count; i++)
                    {
                        CppParameter parameter = functionType.Parameters[i];
                        string paramCsType = GetCsTypeName(parameter.Type, false);
                        // Otherwise we get interop issues with non blittable types

                        builder.Append(paramCsType);

                        builder.Append(", ");
                    }

                    string returnCsName = GetCsTypeName(functionType.ReturnType, false);

                    builder.Append(returnCsName);

                    writer.WriteLine($"public {fieldPrefix}unsafe delegate*<{builder}> {csFieldName};");

                    return;
                }

                if (csFieldType.EndsWith('*'))
                {
                    fieldPrefix += "unsafe ";
                }

                writer.WriteLine($"public {fieldPrefix}{csFieldType} {csFieldName};");
            }
        }

        private static void WriteProperty(CodeWriter writer, CppField field, bool isReadOnly = false)
        {
            string csFieldName = NormalizeFieldName(field.Name);
            WriteCsSummary(field.Comment, writer);
            if (field.Type is CppArrayType arrayType)
            {
                string csFieldType = GetCsTypeName(arrayType.ElementType, false);
                bool canUseFixed = false;
                if (arrayType.ElementType is CppPrimitiveType)
                {
                    canUseFixed = true;
                }
                else if (arrayType.ElementType is CppTypedef typedef && IsPrimitive(typedef, out var primitive))
                {
                    csFieldType = GetCsTypeName(primitive, false);
                    canUseFixed = true;
                }

                if (canUseFixed)
                {
                }
                else
                {
                    if (csFieldType.EndsWith('*'))
                    {
                        return;
                    }

                    writer.WriteLine($"public unsafe Span<{csFieldType}> {csFieldName}");
                    using (writer.PushBlock(""))
                    {
                        using (writer.PushBlock("get"))
                        {
                            using (writer.PushBlock($"fixed ({csFieldType}* p = &this.{csFieldName}_0)"))
                            {
                                writer.WriteLine($"return new Span<{csFieldType}>(p, {arrayType.Size});");
                            }
                        }
                    }
                }
            }
        }

        public static bool IsPointer(CppType type)
        {
            if (type is CppPointerType)
            {
                return true;
            }

            if (type is CppQualifiedType qualifiedType)
            {
                return IsPointer(qualifiedType.ElementType);
            }

            return false;
        }

        public static bool IsPointerOf(CppType type, CppType pointer)
        {
            if (pointer is CppPointerType pointerType)
            {
                return pointerType.ElementType.GetDisplayName() == type.GetDisplayName();
            }
            return false;
        }

        public static bool IsType(CppType a, CppType b)
        {
            return a.GetDisplayName() == b.GetDisplayName();
        }

        public static bool IsPrimitive(CppTypedef cppTypedef, out CppPrimitiveType primitive)
        {
            if (cppTypedef.ElementType is CppPrimitiveType cppPrimitive)
            {
                primitive = cppPrimitive;
                return true;
            }

            if (cppTypedef.ElementType is CppTypedef cppSubTypedef)
            {
                return IsPrimitive(cppSubTypedef, out primitive);
            }

            primitive = null;

            return false;
        }

        public static Direction GetDirection(CppType type, bool isPointer = false)
        {
            if (type is CppPrimitiveType)
            {
                return isPointer ? Direction.InOut : Direction.In;
            }

            if (type is CppPointerType pointerType)
            {
                return GetDirection(pointerType.ElementType, true);
            }

            if (type is CppReferenceType)
            {
                return Direction.Out;
            }

            if (type is CppQualifiedType qualifiedType)
            {
                return qualifiedType.Qualifier != CppTypeQualifier.Const && isPointer ? Direction.InOut : Direction.In;
            }

            if (type is CppFunctionType)
            {
                return isPointer ? Direction.InOut : Direction.In;
            }

            if (type is CppTypedef)
            {
                return isPointer ? Direction.InOut : Direction.In;
            }

            if (type is CppClass)
            {
                return isPointer ? Direction.InOut : Direction.In;
            }

            if (type is CppEnum)
            {
                return isPointer ? Direction.InOut : Direction.In;
            }

            return isPointer ? Direction.InOut : Direction.In;
        }

        public static bool IsDelegate(CppPointerType cppPointer, out CppFunctionType cppFunction)
        {
            if (cppPointer.ElementType is CppFunctionType functionType)
            {
                cppFunction = functionType;
                return true;
            }
            cppFunction = null;
            return false;
        }

        public static bool IsDelegate(CppType cppType, out CppFunctionType cppFunction)
        {
            if (cppType is CppTypedef typedefType)
            {
                return IsDelegate(typedefType.ElementType, out cppFunction);
            }
            if (cppType is CppPointerType cppPointer)
            {
                return IsDelegate(cppPointer.ElementType, out cppFunction);
            }
            if (cppType is CppFunctionType functionType)
            {
                cppFunction = functionType;
                return true;
            }
            cppFunction = null;
            return false;
        }

        private static string NormalizeFieldName(string name)
        {
            var parts = name.Split('_', StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new();
            for (int i = 0; i < parts.Length; i++)
            {
                sb.Append(char.ToUpper(parts[i][0]));
                sb.Append(parts[i][1..]);
            }
            name = sb.ToString();
            if (CsCodeGeneratorSettings.Default.Keywords.Contains(name))
                return "@" + name;

            return name;
        }
    }
}