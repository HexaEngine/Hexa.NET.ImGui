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
                    writer.WriteLine();

                    for (int j = 0; j < functions.Count; j++)
                    {
                        CppFunction cppFunction = FindFunction(compilation, functions[j]);
                        var csFunctionName = GetPrettyCommandName(cppFunction.Name);
                        bool canUseOut = s_outReturnFunctions.Contains(cppFunction.Name);
                        var argumentsString = GetStructParameterSignature(cppClass, cppFunction.Parameters, canUseOut);
                        var sigs = GetStructVariantParameterSignatures(cppClass, cppFunction.Parameters, argumentsString, canUseOut);
                        sigs.Add(argumentsString);

                        WriteStructMethods(writer, cppFunction, cppClass, csName, csFunctionName, sigs);
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

        private static void WriteStructMethods(CodeWriter writer, CppFunction cppFunction, CppClass cppClass, string structName, string command, List<string> signatures)
        {
            bool thisRef = false;
            if (cppFunction.Parameters.Count > 0 && IsPointerOf(cppClass, cppFunction.Parameters[0].Type))
            {
                thisRef = true;
            }
            bool thisUse = false;
            if (cppFunction.Parameters.Count > 0 && IsType(cppClass, cppFunction.Parameters[0].Type))
            {
                thisUse = true;
            }

            bool voidReturn = IsVoid(cppFunction.ReturnType);
            bool stringReturn = IsString(cppFunction.ReturnType);
            string returnCsName = GetCsTypeName(cppFunction.ReturnType, false);

            for (int i = 0; i < signatures.Count; i++)
            {
                string signature = signatures[i];

                if (stringReturn)
                    WriteStructMethod(writer, cppFunction, structName, command, thisRef, thisUse, voidReturn, true, "string", signature);

                WriteStructMethod(writer, cppFunction, structName, command, thisRef, thisUse, voidReturn, false, returnCsName, signature);
            }
        }

        private static void WriteStructMethod(CodeWriter writer, CppFunction cppFunction, string structName, string command, bool thisRef, bool thisUse, bool voidReturn, bool stringReturn, string returnCsName, string signature)
        {
            string[] paramList = signature.Split(',', StringSplitOptions.RemoveEmptyEntries);

            WriteCsSummary(cppFunction.Comment, writer);
            string header;

            if (stringReturn)
            {
                header = $"public unsafe string {command.Replace(structName, string.Empty)}S({signature})";
            }
            else
            {
                header = $"public unsafe {returnCsName} {command.Replace(structName, string.Empty)}({signature})";
            }

            using (writer.PushBlock(header))
            {
                StringBuilder sb = new();
                if (!voidReturn)
                {
                    sb.Append($"{returnCsName} ret = ");
                }

                if (stringReturn)
                {
                    WriteStringConvertToManaged(sb, cppFunction.ReturnType);
                }

                sb.Append($"{CsCodeGeneratorSettings.Default.ApiName}.{command}Native(");
                int strings = 0;
                int stacks = 0;
                int index = 0;
                for (int j = 0; j < cppFunction.Parameters.Count; j++)
                {
                    if (thisUse && j == 0)
                    {
                        sb.Append("this");
                    }
                    else if (thisRef && j == 0)
                    {
                        writer.BeginBlock($"fixed ({structName}* @this = &this)");
                        sb.Append("@this");
                        stacks++;
                    }
                    else
                    {
                        var isRef = paramList[index].Contains("ref");
                        var isStr = paramList[index].Contains("string");
                        var cppParameter = cppFunction.Parameters[j];
                        var paramCsTypeName = GetCsTypeName(cppParameter.Type, false);
                        var paramCsName = GetParameterName(cppParameter.Type, cppParameter.Name);
                        if (isRef)
                        {
                            writer.BeginBlock($"fixed ({paramCsTypeName} p{paramCsName} = &{paramCsName})");
                            sb.Append($"p{paramCsName}");
                            stacks++;
                        }
                        else if (isStr)
                        {
                            WriteStringConvertToUnmanaged(writer, cppParameter.Type, paramCsName, strings);
                            sb.Append($"pStr{strings}");
                            strings++;
                        }
                        else
                        {
                            sb.Append(paramCsName);
                        }
                        index++;
                    }

                    if (j != cppFunction.Parameters.Count - 1)
                        sb.Append(", ");
                }

                if (stringReturn)
                    sb.Append("));");
                else
                    sb.Append(");");

                writer.WriteLine(sb.ToString());

                while (strings > 0)
                {
                    strings--;
                    writer.WriteLine($"Marshal.FreeHGlobal((nint)pStr{strings});");
                }

                if (!voidReturn)
                    writer.WriteLine("return ret;");

                while (stacks > 0)
                {
                    stacks--;
                    writer.EndBlock();
                }
            }

            writer.WriteLine();
        }

        private static string GetStructParameterSignature(CppClass cppClass, IList<CppParameter> parameters, bool canUseOut)
        {
            var argumentBuilder = new StringBuilder();
            int index = 0;

            bool thisRef = false;
            if (parameters.Count > 0)
            {
                thisRef = IsPointerOf(cppClass, parameters[0].Type) || IsType(cppClass, parameters[0].Type);
            }

            for (int i = 0; i < parameters.Count; i++)
            {
                CppParameter cppParameter = parameters[i];
                if (thisRef && i == 0)
                {
                    index++;
                    continue;
                }

                string direction = string.Empty;
                var paramCsTypeName = GetCsTypeName(cppParameter.Type, false);
                var paramCsName = GetParameterName(cppParameter.Type, cppParameter.Name);

                if (canUseOut && CanBeUsedAsOutput(cppParameter.Type, out CppTypeDeclaration? cppTypeDeclaration))
                {
                    argumentBuilder.Append("out ");
                    paramCsTypeName = GetCsTypeName(cppTypeDeclaration, false);
                }

                argumentBuilder.Append(paramCsTypeName).Append(' ').Append(paramCsName);
                if (index < parameters.Count - 1)
                {
                    argumentBuilder.Append(", ");
                }

                index++;
            }

            return argumentBuilder.ToString();
        }

        private static List<string> GetStructVariantParameterSignatures(CppClass cppClass, IList<CppParameter> parameters, string originalSig, bool canUseOut)
        {
            List<string> result = new();
            StringBuilder argumentBuilder = new();

            bool thisRef = false;
            if (parameters.Count > 0)
            {
                thisRef = IsPointerOf(cppClass, parameters[0].Type) || IsType(cppClass, parameters[0].Type);
            }

            for (long ix = 0; ix < Math.Pow(2, parameters.Count); ix++)
            {
                int index = 0;
                for (int j = 0; j < parameters.Count; j++)
                {
                    if (thisRef && j == 0)
                    {
                        index++;
                        continue;
                    }

                    var bit = (ix & (1 << j - 64)) != 0;
                    CppParameter cppParameter = parameters[j];
                    string paramCsTypeName;
                    if (bit)
                        paramCsTypeName = GetCsWrapperTypeName(cppParameter.Type, false);
                    else
                        paramCsTypeName = GetCsTypeName(cppParameter.Type, false);

                    var paramCsName = GetParameterName(cppParameter.Type, cppParameter.Name);

                    if (canUseOut && CanBeUsedAsOutput(cppParameter.Type, out CppTypeDeclaration? cppTypeDeclaration))
                    {
                        argumentBuilder.Append("out ");
                        paramCsTypeName = GetCsWrapperTypeName(cppTypeDeclaration, false);
                    }

                    argumentBuilder.Append(paramCsTypeName).Append(' ').Append(paramCsName);
                    if (index < parameters.Count - 1)
                    {
                        argumentBuilder.Append(", ");
                    }

                    index++;
                }
                string sig = argumentBuilder.ToString();
                if (!result.Contains(sig) && sig != originalSig)
                {
                    result.Add(sig);
                    Console.WriteLine(sig);
                }

                argumentBuilder.Clear();

                index = 0;
                for (int j = 0; j < parameters.Count; j++)
                {
                    if (thisRef && j == 0)
                    {
                        index++;
                        continue;
                    }

                    var bit = (ix & (1 << j - 64)) != 0;
                    CppParameter cppParameter = parameters[j];

                    string paramCsTypeName;
                    if (bit)
                        paramCsTypeName = IsString(cppParameter.Type) ? "string" : GetCsWrapperTypeName(cppParameter.Type, false);
                    else
                        paramCsTypeName = GetCsTypeName(cppParameter.Type, false);

                    var paramCsName = GetParameterName(cppParameter.Type, cppParameter.Name);

                    if (canUseOut && CanBeUsedAsOutput(cppParameter.Type, out CppTypeDeclaration? cppTypeDeclaration))
                    {
                        argumentBuilder.Append("out ");
                        paramCsTypeName = GetCsWrapperTypeName(cppTypeDeclaration, false);
                    }

                    argumentBuilder.Append(paramCsTypeName).Append(' ').Append(paramCsName);
                    if (index < parameters.Count - 1)
                    {
                        argumentBuilder.Append(", ");
                    }

                    index++;
                }
                sig = argumentBuilder.ToString();
                if (!result.Contains(sig) && sig != originalSig)
                {
                    result.Add(sig);
                    Console.WriteLine(sig);
                }

                argumentBuilder.Clear();
            }

            return result;
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

    public enum Direction
    {
        In = 0,
        Out = 1,
        InOut = 2,
    }
}