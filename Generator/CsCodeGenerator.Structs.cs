namespace Generator
{
    using ClangSharp;
    using CppAst;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static partial class CsCodeGenerator
    {
        public static readonly HashSet<string> DefinedTypes = new();

        private static readonly Dictionary<string, string> WrappedPointers = new();

        private static void GenerateStructAndUnions(CppCompilation compilation, string outputPath)
        {
            string[] usings = { "System", "System.Diagnostics", "System.Runtime.CompilerServices", "System.Runtime.InteropServices" };

            // Generate Structures
            using var writer = new CodeWriter(Path.Combine(outputPath, "Structures.cs"), usings.Concat(CsCodeGeneratorSettings.Default.Usings).ToArray());

            // Print All classes, structs
            for (int i = 0; i < compilation.Classes.Count; i++)
            {
                CppClass? cppClass = compilation.Classes[i];
                if (CsCodeGeneratorSettings.Default.AllowedTypes.Count != 0 && !CsCodeGeneratorSettings.Default.AllowedTypes.Contains(cppClass.Name))
                {
                    continue;
                }

                if (CsCodeGeneratorSettings.Default.IgnoredTypes.Contains(cppClass.Name))
                {
                    continue;
                }

                if (DefinedTypes.Contains(cppClass.Name))
                {
                    continue;
                }

                DefinedTypes.Add(cppClass.Name);

                string csName = GetCsCleanName(cppClass.Name);
                WriteClass(writer, compilation, cppClass, csName);
                if (IsUsedAsPointer(cppClass, compilation, out var depths))
                {
                    for (int j = 0; j < depths.Count; j++)
                    {
                        int depth = depths[j];
                        Console.WriteLine(cppClass.Name + " : " + depth);
                        StringBuilder sb1 = new();
                        StringBuilder sb2 = new();
                        sb1.Append(csName);
                        sb2.Append(csName);
                        for (int jj = 0; jj < depth; jj++)
                        {
                            sb1.Append("Ptr");
                            sb2.Append('*');
                        }

                        WriteStructHandle(writer, compilation, cppClass, sb1.ToString(), sb2.ToString());
                        WrappedPointers.Add(sb2.ToString(), sb1.ToString());
                    }
                }
            }
        }

        private static void WriteStructHandle(CodeWriter writer, CppCompilation compilation, CppClass cppClass, string csName, string handleType)
        {
            WriteCsSummary(cppClass.Comment, writer);
            writer.WriteLine($"[DebuggerDisplay(\"{{DebuggerDisplay,nq}}\")]");
            using (writer.PushBlock($"public unsafe struct {csName} : IEquatable<{csName}>"))
            {
                string nullValue = "null";

                writer.WriteLine($"public {csName}({handleType} handle) {{ Handle = handle; }}");
                writer.WriteLine();
                writer.WriteLine($"public {handleType} Handle;");
                writer.WriteLine();
                writer.WriteLine($"public bool IsNull => Handle == null;");
                writer.WriteLine();
                writer.WriteLine($"public static {csName} Null => new {csName}({nullValue});");
                writer.WriteLine();
                writer.WriteLine($"public static implicit operator {csName}({handleType} handle) => new {csName}(handle);");
                writer.WriteLine();
                writer.WriteLine($"public static implicit operator {handleType}({csName} handle) => handle.Handle;");
                writer.WriteLine();
                writer.WriteLine($"public static bool operator ==({csName} left, {csName} right) => left.Handle == right.Handle;");
                writer.WriteLine();
                writer.WriteLine($"public static bool operator !=({csName} left, {csName} right) => left.Handle != right.Handle;");
                writer.WriteLine();
                writer.WriteLine($"public static bool operator ==({csName} left, {handleType} right) => left.Handle == right;");
                writer.WriteLine();
                writer.WriteLine($"public static bool operator !=({csName} left, {handleType} right) => left.Handle != right;");
                writer.WriteLine();
                writer.WriteLine($"public bool Equals({csName} other) => Handle == other.Handle;");
                writer.WriteLine();
                writer.WriteLine("/// <inheritdoc/>");
                writer.WriteLine($"public override bool Equals(object obj) => obj is {csName} handle && Equals(handle);");
                writer.WriteLine();
                writer.WriteLine("/// <inheritdoc/>");
                writer.WriteLine($"public override int GetHashCode() => ((nuint)Handle).GetHashCode();");
                writer.WriteLine();
                writer.WriteLine($"private string DebuggerDisplay => string.Format(\"{csName} [0x{{0}}]\", ((nuint)Handle).ToString(\"X\"));");
                var pCount = handleType.Count(x => x == '*');
                if (pCount == 1)
                {
                    for (int j = 0; j < cppClass.Fields.Count; j++)
                    {
                        CppField cppField = cppClass.Fields[j];

                        WriteProperty(writer, cppClass, cppField, false);
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
                            GenerateVariations(cppFunction.Parameters, overload, true);

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
                            {
                                WriteMethods(writer, definedFunctions, function, overload, false, true, "public unsafe");
                            }
                        }
                    }
                }
                else
                {
                    using (writer.PushBlock($"public {csName[..^3]} this[int index]"))
                    {
                        writer.WriteLine($"get => Handle[index]; set => Handle[index] = value;");
                    }
                }
            }
            writer.WriteLine();
        }

        public static bool IsUsedAsPointer(CppClass cppClass, CppCompilation compilation, out List<int> depths)
        {
            depths = new List<int>();
            int depth = 0;
            for (int i = 0; i < compilation.Functions.Count; i++)
            {
                depth = 0;
                var func = compilation.Functions[i];
                if (IsPointerOf(cppClass, func.ReturnType, ref depth))
                {
                    if (!depths.Contains(depth))
                        depths.Add(depth);
                }

                for (int j = 0; j < func.Parameters.Count; j++)
                {
                    depth = 0;
                    var param = func.Parameters[j];
                    if (IsPointerOf(cppClass, param.Type, ref depth))
                    {
                        if (!depths.Contains(depth))
                            depths.Add(depth);
                    }
                }
            }

            for (int i = 0; i < compilation.Classes.Count; i++)
            {
                var cl = compilation.Classes[i];
                for (int j = 0; j < cl.Fields.Count; j++)
                {
                    depth = 0;
                    var field = cl.Fields[j];
                    if (IsPointerOf(cppClass, field.Type, ref depth))
                    {
                        if (!depths.Contains(depth))
                            depths.Add(depth);
                    }
                }
            }

            return depths.Count > 0;
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
                        returnCsName = returnCsName.Replace("bool", "byte");
                        if (CsCodeGeneratorSettings.Default.DelegatesAsVoidPointer)
                        {
                            writer.WriteLine($"public unsafe void* {csFieldName};");
                        }
                        else
                        {
                            writer.WriteLine($"public unsafe delegate* unmanaged[{GetCallingConventionDelegate(cppFunctionType.CallingConvention)}]<{signature}, {returnCsName}> {csFieldName};");
                        }
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
                        WriteProperty(writer, cppField);
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
                        GenerateVariations(cppFunction.Parameters, overload, true);

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
                        {
                            WriteMethods(writer, definedFunctions, function, overload, useThis || useThisRef, false, "public unsafe");
                        }
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

                if (arrayType.ElementType is CppTypedef typedef && IsPrimitive(typedef, out var primitive))
                {
                    csFieldType = GetCsTypeName(primitive, false);
                }

                string unsafePrefix = string.Empty;

                if (csFieldType.EndsWith('*'))
                {
                    unsafePrefix = "unsafe ";
                }

                for (int i = 0; i < arrayType.Size; i++)
                {
                    if (isUnion && i != 0)
                    {
                        writer.WriteLine($"[FieldOffset({arrayType.SizeOf * i})]");
                    }
                    writer.WriteLine($"public {unsafePrefix}{csFieldType} {csFieldName}_{i};");
                }
            }
            else
            {
                string csFieldType = GetCsTypeName(field.Type, false);
                string fieldPrefix = isReadOnly ? "readonly " : string.Empty;

                if (csFieldType == "bool")
                    csFieldType = "byte";

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
                    returnCsName = returnCsName.Replace("bool", "byte");
                    builder.Append(returnCsName);

                    if (CsCodeGeneratorSettings.Default.DelegatesAsVoidPointer)
                    {
                        writer.WriteLine($"public {fieldPrefix}unsafe void* {csFieldName};");
                    }
                    else
                    {
                        writer.WriteLine($"public {fieldPrefix}unsafe delegate* unmanaged[{GetCallingConventionDelegate(functionType.CallingConvention)}]<{builder}> {csFieldName};");
                    }

                    return;
                }

                if (csFieldType.EndsWith('*'))
                {
                    fieldPrefix += "unsafe ";
                }

                writer.WriteLine($"public {fieldPrefix}{csFieldType} {csFieldName};");
            }
        }

        private static void WriteProperty(CodeWriter writer, CppClass cppClass, CppField field, bool isReadOnly = false)
        {
            string csFieldName = NormalizeFieldName(field.Name);
            WriteCsSummary(field.Comment, writer);

            if (field.Type is CppArrayType arrayType)
            {
                string csFieldType = GetCsTypeName(arrayType.ElementType, false);

                if (arrayType.ElementType is CppTypedef typedef && IsPrimitive(typedef, out var primitive))
                {
                    csFieldType = GetCsTypeName(primitive, false);
                }

                if (csFieldType.EndsWith('*'))
                {
                    return;
                }

                writer.WriteLine($"public unsafe Span<{csFieldType}> {csFieldName}");
                using (writer.PushBlock(""))
                {
                    using (writer.PushBlock("get"))
                    {
                        writer.WriteLine($"return new Span<{csFieldType}>(&Handle->{csFieldName}_0, {arrayType.Size});");
                    }
                }
            }
            else
            {
                string csFieldType;

                if (field.Type is CppClass subClass)
                {
                    if (string.IsNullOrEmpty(subClass.Name))
                    {
                        string csName = GetCsCleanName(cppClass.Name);
                        csFieldType = csName + "Union";
                        csFieldName = csFieldType;
                    }
                    else
                    {
                        csFieldType = GetCsCleanName(subClass.Name);
                    }
                }
                else
                {
                    csFieldType = GetCsTypeName(field.Type, false);
                }

                if (IsDelegate(field.Type, out var functionType))
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
                    returnCsName = returnCsName.Replace("bool", "byte");
                    builder.Append(returnCsName);

                    if (CsCodeGeneratorSettings.Default.DelegatesAsVoidPointer)
                    {
                        if (isReadOnly)
                        {
                            writer.WriteLine($"public void* {csFieldName} {{ get => Handle->{csFieldName}; }}");
                        }
                        else
                        {
                            writer.WriteLine($"public void* {csFieldName} {{ get => Handle->{csFieldName}; set => Handle->{csFieldName} = value; }}");
                        }
                    }
                    else
                    {
                        if (isReadOnly)
                        {
                            writer.WriteLine($"public delegate* unmanaged[{GetCallingConventionDelegate(functionType.CallingConvention)}]<{builder}> {csFieldName} {{ get => Handle->{csFieldName}; }}");
                        }
                        else
                        {
                            writer.WriteLine($"public delegate* unmanaged[{GetCallingConventionDelegate(functionType.CallingConvention)}]<{builder}> {csFieldName} {{ get => Handle->{csFieldName}; set => Handle->{csFieldName} = value; }}");
                        }
                    }

                    return;
                }

                if (csFieldType.EndsWith('*') && !CsType.IsKnownPrimitive(csFieldType))
                {
                    StringBuilder sb = new();
                    int x = 0;
                    while (csFieldType.EndsWith('*'))
                    {
                        csFieldType = csFieldType[..^1];
                        x++;
                    }
                    sb.Append(csFieldType);
                    for (int j = 0; j < x; j++)
                    {
                        sb.Append("Ptr");
                    }
                    csFieldType = sb.ToString();
                }

                if (csFieldType.EndsWith('*'))
                {
                    if (isReadOnly)
                    {
                        writer.WriteLine($"public {csFieldType} {csFieldName} => Handle->{csFieldName};");
                    }
                    else
                    {
                        writer.WriteLine($"public {csFieldType} {csFieldName} {{ get => Handle->{csFieldName}; set => Handle->{csFieldName} = value; }}");
                    }
                }
                else
                {
                    if (isReadOnly)
                    {
                        writer.WriteLine($"public {csFieldType} {csFieldName} => Handle->{csFieldName};");
                    }
                    else
                    {
                        writer.WriteLine($"public ref {csFieldType} {csFieldName} => ref Unsafe.AsRef<{csFieldType}>(&Handle->{csFieldName});");
                    }
                }
            }
        }

        private static void WriteProperty(CodeWriter writer, CppField field)
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

        public static bool IsPointerOf(CppType type, CppType pointer, ref int depth)
        {
            if (pointer is CppPointerType pointerType)
            {
                if (pointerType.ElementType is CppPointerType cppPointer)
                {
                    depth++;
                    return IsPointerOf(type, cppPointer, ref depth);
                }
                depth++;
                if (pointerType.ElementType is CppQualifiedType qualifiedType && qualifiedType.Qualifier == CppTypeQualifier.Const)
                    return qualifiedType.ElementType.GetDisplayName() == type.GetDisplayName();
                else
                    return pointerType.ElementType.GetDisplayName() == type.GetDisplayName();
            }
            return false;
        }

        public static bool IsType(CppType a, CppType b)
        {
            return a.GetDisplayName() == b.GetDisplayName();
        }

        public static bool IsPrimitive(CppType cppType, out CppPrimitiveType primitive)
        {
            if (cppType is CppPrimitiveType cppPrimitive)
            {
                primitive = cppPrimitive;
                return true;
            }

            if (cppType is CppTypedef cppTypedef)
            {
                return IsPrimitive(cppTypedef.ElementType, out primitive);
            }

            if (cppType is CppPointerType cppPointerType)
            {
                return IsPrimitive(cppPointerType.ElementType, out primitive);
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
            {
                return "@" + name;
            }

            return name;
        }
    }
}