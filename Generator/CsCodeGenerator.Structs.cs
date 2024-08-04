namespace Generator
{
    using CppAst;
    using System.IO;
    using System.Linq;
    using System.Text;

    public partial class CsCodeGenerator
    {
        public readonly HashSet<string> DefinedTypes = new();
        public readonly HashSet<string> LibDefinedTypes = new();

        private static readonly Dictionary<string, string> WrappedPointers = new();

        private bool FilterStruct(CppClass cppClass)
        {
            if (settings.AllowedTypes.Count != 0 && !settings.AllowedTypes.Contains(cppClass.Name))
            {
                return true;
            }

            if (settings.IgnoredTypes.Contains(cppClass.Name))
            {
                return true;
            }

            if (LibDefinedTypes.Contains(cppClass.Name))
            {
                return true;
            }

            if (DefinedTypes.Contains(cppClass.Name))
            {
                return true;
            }

            DefinedTypes.Add(cppClass.Name);
            return false;
        }

        private void GenerateStructAndUnions(CppCompilation compilation, string outputPath)
        {
            string[] usings = ["System", "System.Diagnostics", "System.Runtime.CompilerServices", "System.Runtime.InteropServices", "HexaGen.Runtime"];

            string outDir = Path.Combine(outputPath, "Structs");
            string fileName = Path.Combine(outDir, "Structs.cs");

            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);

            // Generate Structures
            using var writer = new CodeWriter(fileName, settings.Namespace, usings.Concat(settings.Usings).ToArray());

            // Print All classes, structs
            for (int i = 0; i < compilation.Classes.Count; i++)
            {
                CppClass? cppClass = compilation.Classes[i];
                if (FilterStruct(cppClass))
                {
                    continue;
                }

                string csName = settings.GetCsCleanName(cppClass.Name);
                WriteClass(writer, compilation, cppClass, csName);
                if (cppClass.IsUsedAsPointer(compilation, out var depths))
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

        private void WriteStructHandle(ICodeWriter writer, CppCompilation compilation, CppClass cppClass, string csName, string handleType)
        {
            WriteCsSummary(cppClass.Comment, writer);
            writer.WriteLine("#if NET5_0_OR_GREATER");
            writer.WriteLine($"[DebuggerDisplay(\"{{DebuggerDisplay,nq}}\")]");
            writer.WriteLine("#endif");
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
                writer.WriteLine("#if NET5_0_OR_GREATER");
                writer.WriteLine($"private string DebuggerDisplay => string.Format(\"{csName} [0x{{0}}]\", ((nuint)Handle).ToString(\"X\"));");
                writer.WriteLine("#endif");
                var pCount = handleType.Count(x => x == '*');
                if (pCount == 1)
                {
                    for (int j = 0; j < cppClass.Fields.Count; j++)
                    {
                        CppField cppField = cppClass.Fields[j];

                        WriteProperty(writer, cppClass, cppField, false);
                    }

                    if (settings.KnownMemberFunctions.TryGetValue(cppClass.Name, out var functions))
                    {
                        HashSet<string> definedFunctions = new();
                        writer.WriteLine();
                        List<CsFunction> commands = new();
                        for (int i = 0; i < functions.Count; i++)
                        {
                            CppFunction cppFunction = compilation.FindFunction(functions[i]);
                            var csFunctionName = settings.GetPrettyCommandName(cppFunction.Name);
                            string returnCsName = settings.GetCsTypeName(cppFunction.ReturnType, false);
                            CppPrimitiveKind returnKind = cppFunction.ReturnType.GetPrimitiveKind(false);

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
                                var paramCsTypeName = settings.GetCsTypeName(cppParameter.Type, false);
                                var paramCsName = settings.GetParameterName(cppParameter.Type, cppParameter.Name);
                                var direction = cppParameter.Type.GetDirection();
                                var kind = cppParameter.Type.GetPrimitiveKind(false);

                                CsType csType = new(paramCsTypeName, kind);

                                CsParameterInfo csParameter = new(paramCsName, csType, direction);

                                overload.Parameters.Add(csParameter);
                                if (settings.TryGetDefaultValue(cppFunction.Name, cppParameter, false, out var defaultValue))
                                {
                                    overload.DefaultValues.Add(paramCsName, defaultValue);
                                }
                            }

                            function.Overloads.Add(overload);
                            GenerateVariations(cppFunction.Parameters, overload, true);

                            bool useThisRef = false;
                            if (cppFunction.Parameters.Count > 0 && cppClass.IsPointerOf(cppFunction.Parameters[0].Type))
                            {
                                useThisRef = true;
                            }

                            bool useThis = false;
                            if (cppFunction.Parameters.Count > 0 && cppClass.IsType(cppFunction.Parameters[0].Type))
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

        public void WriteClass(ICodeWriter writer, CppCompilation compilation, CppClass cppClass, string csName)
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
                    csSubName = settings.GetCsCleanName(subClass.Name);
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
                if (settings.GenerateSizeOfStructs && cppClass.SizeOf > 0)
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
                    else if (cppField.Type is CppPointerType cppPointer && cppPointer.IsDelegate(out var cppFunctionType))
                    {
                        string csFieldName = settings.NormalizeFieldName(cppField.Name);
                        string returnCsName = settings.GetCsTypeName(cppFunctionType.ReturnType, false);
                        string signature = settings.GetNamelessParameterSignature(cppFunctionType.Parameters, false);
                        returnCsName = returnCsName.Replace("bool", "byte");
                        if (settings.DelegatesAsVoidPointer)
                        {
                            writer.WriteLine($"public unsafe void* {csFieldName};");
                        }
                        else
                        {
                            writer.WriteLine($"public unsafe delegate* unmanaged[{cppFunctionType.CallingConvention.GetCallingConventionDelegate()}]<{signature}, {returnCsName}> {csFieldName};");
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

                if (settings.KnownMemberFunctions.TryGetValue(cppClass.Name, out var functions))
                {
                    HashSet<string> definedFunctions = new();
                    writer.WriteLine();
                    List<CsFunction> commands = new();
                    for (int i = 0; i < functions.Count; i++)
                    {
                        CppFunction cppFunction = compilation.FindFunction(functions[i]);
                        var csFunctionName = settings.GetPrettyCommandName(cppFunction.Name);
                        string returnCsName = settings.GetCsTypeName(cppFunction.ReturnType, false);
                        CppPrimitiveKind returnKind = cppFunction.ReturnType.GetPrimitiveKind(false);

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
                            var paramCsTypeName = settings.GetCsTypeName(cppParameter.Type, false);
                            var paramCsName = settings.GetParameterName(cppParameter.Type, cppParameter.Name);
                            var direction = cppParameter.Type.GetDirection();
                            var kind = cppParameter.Type.GetPrimitiveKind(false);

                            CsType csType = new(paramCsTypeName, kind);

                            CsParameterInfo csParameter = new(paramCsName, csType, direction);

                            overload.Parameters.Add(csParameter);
                            if (settings.TryGetDefaultValue(cppFunction.Name, cppParameter, false, out var defaultValue))
                            {
                                overload.DefaultValues.Add(paramCsName, defaultValue);
                            }
                        }

                        function.Overloads.Add(overload);
                        GenerateVariations(cppFunction.Parameters, overload, true);

                        bool useThisRef = false;
                        if (cppFunction.Parameters.Count > 0 && cppClass.IsPointerOf(cppFunction.Parameters[0].Type))
                        {
                            useThisRef = true;
                        }

                        bool useThis = false;
                        if (cppFunction.Parameters.Count > 0 && cppClass.IsType(cppFunction.Parameters[0].Type))
                        {
                            useThis = true;
                        }

                        if (useThis || useThisRef)
                        {
                            WriteMethods(writer, definedFunctions, function, overload, useThis || useThisRef, false, "public unsafe");
                        }
                    }
                }

                if (settings.GenerateConstructors)
                {
                }
            }

            writer.WriteLine();
        }

        private void WriteField(ICodeWriter writer, CppField field, bool isUnion = false, bool isReadOnly = false)
        {
            string csFieldName = settings.NormalizeFieldName(field.Name);
            WriteCsSummary(field.Comment, writer);
            if (isUnion)
            {
                writer.WriteLine("[FieldOffset(0)]");
            }

            if (field.Type is CppArrayType arrayType)
            {
                string csFieldType = settings.GetCsTypeName(arrayType.ElementType, false);

                if (arrayType.ElementType is CppTypedef typedef && typedef.IsPrimitive(out var primitive))
                {
                    csFieldType = settings.GetCsTypeName(primitive, false);
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
                string csFieldType = settings.GetCsTypeName(field.Type, false);
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
                        string paramCsType = settings.GetCsTypeName(parameter.Type, false);
                        // Otherwise we get interop issues with non blittable types

                        builder.Append(paramCsType);

                        builder.Append(", ");
                    }

                    string returnCsName = settings.GetCsTypeName(functionType.ReturnType, false);
                    returnCsName = returnCsName.Replace("bool", "byte");
                    builder.Append(returnCsName);

                    if (settings.DelegatesAsVoidPointer)
                    {
                        writer.WriteLine($"public {fieldPrefix}unsafe void* {csFieldName};");
                    }
                    else
                    {
                        writer.WriteLine($"public {fieldPrefix}unsafe delegate* unmanaged[{functionType.CallingConvention.GetCallingConventionDelegate()}]<{builder}> {csFieldName};");
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

        private void WriteProperty(ICodeWriter writer, CppClass cppClass, CppField field, bool isReadOnly = false)
        {
            string csFieldName = settings.NormalizeFieldName(field.Name);
            WriteCsSummary(field.Comment, writer);

            if (field.Type is CppArrayType arrayType)
            {
                string csFieldType = settings.GetCsTypeName(arrayType.ElementType, false);

                if (arrayType.ElementType is CppTypedef typedef && typedef.IsPrimitive(out var primitive))
                {
                    csFieldType = settings.GetCsTypeName(primitive, false);
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
                        string csName = settings.GetCsCleanName(cppClass.Name);
                        csFieldType = csName + "Union";
                        csFieldName = csFieldType;
                    }
                    else
                    {
                        csFieldType = settings.GetCsCleanName(subClass.Name);
                    }
                }
                else
                {
                    csFieldType = settings.GetCsTypeName(field.Type, false);
                }

                if (field.Type.IsDelegate(out var functionType))
                {
                    StringBuilder builder = new();
                    for (int i = 0; i < functionType.Parameters.Count; i++)
                    {
                        CppParameter parameter = functionType.Parameters[i];
                        string paramCsType = settings.GetCsTypeName(parameter.Type, false);
                        // Otherwise we get interop issues with non blittable types

                        builder.Append(paramCsType);

                        builder.Append(", ");
                    }

                    string returnCsName = settings.GetCsTypeName(functionType.ReturnType, false);
                    returnCsName = returnCsName.Replace("bool", "byte");
                    builder.Append(returnCsName);

                    if (settings.DelegatesAsVoidPointer)
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
                            writer.WriteLine($"public delegate* unmanaged[{functionType.CallingConvention.GetCallingConventionDelegate()}]<{builder}> {csFieldName} {{ get => Handle->{csFieldName}; }}");
                        }
                        else
                        {
                            writer.WriteLine($"public delegate* unmanaged[{functionType.CallingConvention.GetCallingConventionDelegate()}]<{builder}> {csFieldName} {{ get => Handle->{csFieldName}; set => Handle->{csFieldName} = value; }}");
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

        private void WriteProperty(ICodeWriter writer, CppField field)
        {
            string csFieldName = settings.NormalizeFieldName(field.Name);
            WriteCsSummary(field.Comment, writer);
            if (field.Type is CppArrayType arrayType)
            {
                string csFieldType = settings.GetCsTypeName(arrayType.ElementType, false);
                bool canUseFixed = false;
                if (arrayType.ElementType is CppPrimitiveType)
                {
                    canUseFixed = true;
                }
                else if (arrayType.ElementType is CppTypedef typedef && typedef.IsPrimitive(out var primitive))
                {
                    csFieldType = settings.GetCsTypeName(primitive, false);
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
    }
}