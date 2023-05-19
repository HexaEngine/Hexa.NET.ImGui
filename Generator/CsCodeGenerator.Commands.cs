namespace Generator
{
    using ClangSharp;
    using CppAst;
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Numerics;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public static partial class CsCodeGenerator
    {
        private static readonly HashSet<string> s_instanceFunctions = new()
        {
        };

        private static readonly HashSet<string> s_outReturnFunctions = new()
        {
        };

        private static void GenerateCommands(CppCompilation compilation, string outputPath)
        {
            // Generate Functions
            using var writer = new CodeWriter(Path.Combine(outputPath, "Commands.cs"),
                "System",
                "System.Runtime.CompilerServices",
                "System.Runtime.InteropServices",
                "System.Numerics"
                );

            var commands = new Dictionary<string, CppFunction>();
            var instanceCommands = new Dictionary<string, CppFunction>();
            var deviceCommands = new Dictionary<string, CppFunction>();
            foreach (CppFunction? cppFunction in compilation.Functions)
            {
                string? returnType = GetCsTypeName(cppFunction.ReturnType, false);
                bool canUseOut = s_outReturnFunctions.Contains(cppFunction.Name);
                string? csName = GetPrettyCommandName(cppFunction.Name);

                commands.Add(csName, cppFunction);

                if (cppFunction.Parameters.Count > 0)
                {
                    var firstParameter = cppFunction.Parameters[0];
                    if (firstParameter.Type is CppTypedef typedef)
                    {
                        deviceCommands.Add(csName, cppFunction);
                    }
                }
            }

            using (writer.PushBlock($"public unsafe partial class {CsCodeGeneratorSettings.Default.ApiName}"))
            {
                writer.WriteLine($"internal const string LibName = \"{CsCodeGeneratorSettings.Default.LibName}\";\n");
                foreach (KeyValuePair<string, CppFunction> command in commands)
                {
                    CppFunction cppFunction = command.Value;

                    string returnCsName = GetCsTypeName(cppFunction.ReturnType, false);
                    bool boolReturn = returnCsName == "bool";
                    bool canUseOut = s_outReturnFunctions.Contains(cppFunction.Name);
                    var argumentsString = GetParameterSignature(cppFunction, canUseOut);

                    WriteCsSummary(cppFunction.Comment, writer);
                    if (boolReturn)
                    {
                        writer.WriteLine("[return: MarshalAs(UnmanagedType.Bool)]");
                    }

                    writer.WriteLine($"[DllImport(LibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = \"{cppFunction.Name}\")]");
                    writer.WriteLine($"public static extern {returnCsName} {command.Key}({argumentsString});");
                    writer.WriteLine();

                    var sign = GetParameterList(cppFunction.Parameters, canUseOut);
                    var sigs = GetVariantParameterLists(cppFunction.Parameters, argumentsString, canUseOut);
                    WriteMethods(writer, cppFunction, command.Key, sign, sigs);
                }
            }
        }

        public static void WriteMethods(CodeWriter writer, CppFunction cppFunction, string command, string[] nativeSignature, List<string[]> signatures)
        {
            string returnCsName = GetCsTypeName(cppFunction.ReturnType, false);
            bool voidReturn = IsVoid(cppFunction.ReturnType);
            bool stringReturn = IsString(cppFunction.ReturnType);

            if (stringReturn)
            {
                WriteMethod(writer, cppFunction, command, voidReturn, true, "string", nativeSignature);
            }

            for (int i = 0; i < signatures.Count; i++)
            {
                string[] signature = signatures[i];

                if (stringReturn)
                {
                    WriteMethod(writer, cppFunction, command, voidReturn, true, "string", signature);
                }

                WriteMethod(writer, cppFunction, command, voidReturn, false, returnCsName, signature);
            }
        }

        private static void WriteMethod(CodeWriter writer, CppFunction cppFunction, string command, bool voidReturn, bool stringReturn, string returnCsName, string[] paramList)
        {
            string signature = string.Join(string.Empty, paramList);

            WriteCsSummary(cppFunction.Comment, writer);
            string header;

            if (stringReturn)
            {
                header = $"public static string {command}S({signature})";
            }
            else
            {
                header = $"public static {returnCsName} {command}({signature})";
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

                sb.Append($"{command}(");
                Stack<(string, CppParameter, string)> stack = new();
                int strings = 0;
                Stack<string> arrays = new();
                int stacks = 0;
                for (int j = 0; j < cppFunction.Parameters.Count; j++)
                {
                    var isRef = paramList[j].StartsWith("ref");
                    var isStr = paramList[j].Contains("string");
                    var isArray = paramList[j].Contains("[]");
                    var cppParameter = cppFunction.Parameters[j];

                    var paramCsTypeName = GetCsTypeName(cppParameter.Type, false);
                    var paramCsName = GetParameterName(cppParameter.Type, cppParameter.Name);

                    if (isStr)
                    {
                        if (isArray)
                        {
                            WriteStringArrayConvertToUnmanaged(writer, cppParameter.Type, paramCsName, arrays.Count);
                            sb.Append($"pAStr{arrays.Count}");
                            arrays.Push(paramCsName);
                        }
                        else
                        {
                            if (isRef)
                            {
                                stack.Push((paramCsName, cppParameter, $"pStr{strings}"));
                            }

                            WriteStringConvertToUnmanaged(writer, cppParameter.Type, paramCsName, strings);
                            sb.Append($"pStr{strings}");
                            strings++;
                        }
                    }
                    else if (isRef)
                    {
                        writer.BeginBlock($"fixed ({GetCleanParamType(paramList[j])}* p{paramCsName} = &{paramCsName})");
                        sb.Append($"({paramCsTypeName})p{paramCsName}");
                        stacks++;
                    }
                    else
                    {
                        sb.Append(paramCsName);
                    }
                    if (j != cppFunction.Parameters.Count - 1)
                    {
                        sb.Append(", ");
                    }
                }

                if (stringReturn)
                {
                    sb.Append("));");
                }
                else
                {
                    sb.Append(");");
                }

                writer.WriteLine(sb.ToString());

                while (stack.TryPop(out var stackItem))
                {
                    WriteStringConvertToManaged(writer, stackItem.Item2.Type, stackItem.Item1, stackItem.Item3);
                }

                while (arrays.TryPop(out var arrayName))
                {
                    WriteFreeUnmanagedStringArray(writer, arrayName, arrays.Count);
                }

                while (strings > 0)
                {
                    strings--;
                    writer.WriteLine($"Marshal.FreeHGlobal((nint)pStr{strings});");
                }

                if (!voidReturn)
                {
                    writer.WriteLine("return ret;");
                }

                while (stacks > 0)
                {
                    stacks--;
                    writer.EndBlock();
                }
            }

            writer.WriteLine();
        }

        public static string GetParameterSignature(CppFunction cppFunction, bool canUseOut)
        {
            return GetParameterSignature(cppFunction.Parameters, canUseOut);
        }

        private static string GetCleanParamType(string param)
        {
            param = param.Replace("ref ", string.Empty);
            return param.Split(" ")[0];
        }

        private static bool IsVoid(CppType cppType)
        {
            if (cppType is CppPrimitiveType type)
            {
                return type.Kind == CppPrimitiveKind.Void;
            }
            return false;
        }

        private static bool IsString(CppType cppType, bool isPointer = false)
        {
            if (cppType is CppPointerType pointer && !isPointer)
            {
                return IsString(pointer.ElementType, true);
            }

            if (cppType is CppQualifiedType qualified)
            {
                return IsString(qualified.ElementType, isPointer);
            }

            if (isPointer && cppType is CppPrimitiveType primitive)
            {
                return primitive.Kind == CppPrimitiveKind.WChar || primitive.Kind == CppPrimitiveKind.Char;
            }

            return false;
        }

        private static CppPrimitiveKind GetPrimitiveKind(CppType cppType, bool isPointer)
        {
            if (cppType is CppArrayType arrayType)
            {
                return GetPrimitiveKind(arrayType.ElementType, true);
            }

            if (cppType is CppPointerType pointer)
            {
                return GetPrimitiveKind(pointer.ElementType, true);
            }

            if (cppType is CppQualifiedType qualified)
            {
                return GetPrimitiveKind(qualified.ElementType, isPointer);
            }

            if (isPointer && cppType is CppPrimitiveType primitive)
            {
                return primitive.Kind;
            }

            return CppPrimitiveKind.Void;
        }

        private static void WriteStringConvertToManaged(StringBuilder sb, CppType type)
        {
            CppPrimitiveKind primitiveKind = GetPrimitiveKind(type, false);
            if (primitiveKind == CppPrimitiveKind.Char)
            {
                sb.Append("Marshal.PtrToStringAnsi((nint)");
            }
            if (primitiveKind == CppPrimitiveKind.WChar)
            {
                sb.Append("Marshal.PtrToStringUni((nint)");
            }
        }

        private static void WriteStringConvertToManaged(CodeWriter writer, CppType type, string variable, string pointer)
        {
            CppPrimitiveKind primitiveKind = GetPrimitiveKind(type, false);
            if (primitiveKind == CppPrimitiveKind.Char)
            {
                writer.WriteLine($"{variable} = Marshal.PtrToStringAnsi((nint){pointer});");
            }
            if (primitiveKind == CppPrimitiveKind.WChar)
            {
                writer.WriteLine($"{variable} = Marshal.PtrToStringUni((nint){pointer});");
            }
        }

        private static void WriteStringConvertToUnmanaged(CodeWriter writer, CppType type, string name, int i)
        {
            CppPrimitiveKind primitiveKind = GetPrimitiveKind(type, false);
            if (primitiveKind == CppPrimitiveKind.Char)
            {
                writer.WriteLine($"byte* pStr{i} = (byte*)Marshal.StringToHGlobalAnsi({name});");
            }
            if (primitiveKind == CppPrimitiveKind.WChar)
            {
                writer.WriteLine($"char* pStr{i} = (char*)Marshal.StringToHGlobalUni({name});");
            }
        }

        private static void WriteStringArrayConvertToUnmanaged(CodeWriter writer, CppType type, string name, int i)
        {
            CppPrimitiveKind primitiveKind = GetPrimitiveKind(type, false);
            if (primitiveKind == CppPrimitiveKind.Char)
            {
                writer.WriteLine($"byte** pAStr{i} = (byte**)Marshal.AllocHGlobal(sizeof(nuint) * {name}.Length);");
                using (writer.PushBlock($"for (int i = 0; i < {name}.Length; i++)"))
                {
                    writer.WriteLine($"pAStr{i}[i] = (byte*)Marshal.StringToHGlobalAnsi({name}[i]);");
                }
            }
            if (primitiveKind == CppPrimitiveKind.WChar)
            {
                writer.WriteLine($"char** pAStr{i} = (char**)Marshal.AllocHGlobal(sizeof(nuint) * {name}.Length);");
                using (writer.PushBlock($"for (int i = 0; i < {name}.Length; i++)"))
                {
                    writer.WriteLine($"pAStr{i}[i] = (char*)Marshal.StringToHGlobalUni({name}[i]);");
                }
            }
        }

        private static void WriteFreeUnmanagedStringArray(CodeWriter writer, string name, int i)
        {
            using (writer.PushBlock($"for (int i = 0; i < {name}.Length; i++)"))
            {
                writer.WriteLine($"Marshal.FreeHGlobal((nint)pAStr{i}[i]);");
            }
            writer.WriteLine($"Marshal.FreeHGlobal((nint)pAStr{i});");
        }

        private static string GetParameterSignature(IList<CppParameter> parameters, bool canUseOut)
        {
            StringBuilder argumentBuilder = new();
            int index = 0;

            for (int i = 0; i < parameters.Count; i++)
            {
                CppParameter cppParameter = parameters[i];
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

        private static string[] GetParameterList(IList<CppParameter> parameters, bool canUseOut)
        {
            string[] parameterList = new string[parameters.Count];
            var argumentBuilder = new StringBuilder();
            int index = 0;

            for (int i = 0; i < parameters.Count; i++)
            {
                CppParameter cppParameter = parameters[i];
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

                parameterList[index] = argumentBuilder.ToString();
                argumentBuilder.Clear();
                index++;
            }

            return parameterList;
        }

        private static string GetNamelessParameterSignature(IList<CppParameter> parameters, bool canUseOut)
        {
            var argumentBuilder = new StringBuilder();
            int index = 0;

            foreach (CppParameter cppParameter in parameters)
            {
                string direction = string.Empty;
                var paramCsTypeName = GetCsTypeName(cppParameter.Type, false);

                if (canUseOut && CanBeUsedAsOutput(cppParameter.Type, out CppTypeDeclaration? cppTypeDeclaration))
                {
                    argumentBuilder.Append("out ");
                    paramCsTypeName = GetCsTypeName(cppTypeDeclaration, false);
                }

                argumentBuilder.Append(paramCsTypeName);
                if (index < parameters.Count - 1)
                {
                    argumentBuilder.Append(", ");
                }

                index++;
            }

            return argumentBuilder.ToString();
        }

        private static List<string> GetVariantParameterSignatures(IList<CppParameter> parameters, string originalSig, bool canUseOut)
        {
            List<string> result = new();
            StringBuilder argumentBuilder = new();

            for (long ix = 0; ix < Math.Pow(2, parameters.Count); ix++)
            {
                int index = 0;
                for (int j = 0; j < parameters.Count; j++)
                {
                    var bit = (ix & (1 << j - 64)) != 0;
                    CppParameter cppParameter = parameters[j];
                    string paramCsTypeName;
                    if (bit)
                    {
                        paramCsTypeName = GetCsWrapperTypeName(cppParameter.Type, false);
                    }
                    else
                    {
                        paramCsTypeName = GetCsTypeName(cppParameter.Type, false);
                    }

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
                    var bit = (ix & (1 << j - 64)) != 0;
                    CppParameter cppParameter = parameters[j];
                    Direction direction = GetDirection(cppParameter.Type);

                    string paramCsTypeName;
                    if (bit)
                    {
                        paramCsTypeName = IsString(cppParameter.Type) ? direction == Direction.InOut ? "ref string" : "string" : GetCsWrapperTypeName(cppParameter.Type, false);
                    }
                    else
                    {
                        paramCsTypeName = GetCsTypeName(cppParameter.Type, false);
                    }

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

        private static List<string[]> GetVariantParameterLists(IList<CppParameter> parameters, string originalSig, bool canUseOut)
        {
            List<string[]> signatures = new();
            StringBuilder signatureBuilder = new();
            HashSet<string> definedSignatures = new()
            {
                originalSig
            };
            for (long ix = 0; ix < Math.Pow(2, parameters.Count); ix++)
            {
                {
                    int index = 0;
                    string[] parameterList = new string[parameters.Count];
                    StringBuilder paramBuilder = new();
                    for (int j = 0; j < parameters.Count; j++)
                    {
                        var bit = (ix & (1 << j - 64)) != 0;
                        CppParameter cppParameter = parameters[j];
                        string paramCsTypeName;
                        if (bit)
                        {
                            if (cppParameter.Type is CppArrayType arrayType)
                            {
                                paramCsTypeName = GetCsTypeName(arrayType.ElementType, false);
                            }
                            else
                            {
                                paramCsTypeName = GetCsWrapperTypeName(cppParameter.Type, false);
                            }
                        }
                        else
                        {
                            paramCsTypeName = GetCsTypeName(cppParameter.Type, false);
                        }

                        var paramCsName = GetParameterName(cppParameter.Type, cppParameter.Name);

                        if (canUseOut && CanBeUsedAsOutput(cppParameter.Type, out CppTypeDeclaration? cppTypeDeclaration))
                        {
                            paramBuilder.Append("out ");
                            paramCsTypeName = GetCsWrapperTypeName(cppTypeDeclaration, false);
                        }

                        paramBuilder.Append(paramCsTypeName).Append(' ').Append(paramCsName);
                        if (index < parameters.Count - 1)
                        {
                            paramBuilder.Append(", ");
                        }

                        var param = paramBuilder.ToString();
                        signatureBuilder.Append(param);
                        parameterList[index] = param;
                        paramBuilder.Clear();
                        index++;
                    }

                    string sig = signatureBuilder.ToString();
                    signatureBuilder.Clear();
                    if (!definedSignatures.Contains(sig))
                    {
                        signatures.Add(parameterList);
                        definedSignatures.Add(sig);
                    }
                }

                {
                    int index = 0;
                    string[] parameterList = new string[parameters.Count];
                    StringBuilder paramBuilder = new();
                    for (int j = 0; j < parameters.Count; j++)
                    {
                        var bit = (ix & (1 << j - 64)) != 0;
                        CppParameter cppParameter = parameters[j];
                        Direction direction = GetDirection(cppParameter.Type);

                        string paramCsTypeName;
                        if (bit)
                        {
                            if (cppParameter.Type is CppArrayType arrayType)
                            {
                                if (IsString(arrayType.ElementType))
                                {
                                    paramCsTypeName = "string[]";
                                }
                                else
                                {
                                    paramCsTypeName = GetCsWrapperTypeName(cppParameter.Type, false);
                                }
                            }
                            else
                            {
                                paramCsTypeName = IsString(cppParameter.Type) ? direction == Direction.InOut ? "ref string" : "string" : GetCsWrapperTypeName(cppParameter.Type, false);
                            }
                        }
                        else
                        {
                            paramCsTypeName = GetCsTypeName(cppParameter.Type, false);
                        }

                        var paramCsName = GetParameterName(cppParameter.Type, cppParameter.Name);

                        if (canUseOut && CanBeUsedAsOutput(cppParameter.Type, out CppTypeDeclaration? cppTypeDeclaration))
                        {
                            paramBuilder.Append("out ");
                            paramCsTypeName = GetCsWrapperTypeName(cppTypeDeclaration, false);
                        }

                        paramBuilder.Append(paramCsTypeName).Append(' ').Append(paramCsName);
                        if (index < parameters.Count - 1)
                        {
                            paramBuilder.Append(", ");
                        }

                        var param = paramBuilder.ToString();
                        signatureBuilder.Append(param);
                        parameterList[index] = param;
                        paramBuilder.Clear();
                        index++;
                    }

                    string sig = signatureBuilder.ToString();
                    signatureBuilder.Clear();
                    if (!definedSignatures.Contains(sig) && sig != originalSig)
                    {
                        signatures.Add(parameterList);
                        definedSignatures.Add(sig);
                    }
                }
            }

            return signatures;
        }

        private static string GetParameterName(CppType type, string name)
        {
            if (name == "out")
            {
                return "output";
            }
            if (name == "ref")
            {
                return "reference";
            }
            if (name == "in")
            {
                return "input";
            }
            if (name == "base")
            {
                return "baseValue";
            }
            if (CsCodeGeneratorSettings.Default.Keywords.Contains(name))
            {
                return "@" + name;
            }

            if (name.StartsWith('p') && name.Length > 1 && char.IsUpper(name[1]))
            {
                name = char.ToLower(name[1]) + name[2..];
                return GetParameterName(type, name);
            }

            if (name == string.Empty)
            {
                switch (type.TypeKind)
                {
                    case CppTypeKind.Primitive:
                        return (type as CppPrimitiveType).GetDisplayName();

                    case CppTypeKind.Pointer:
                        return (type as CppPointerType).ElementType.GetDisplayName();

                    case CppTypeKind.Reference:
                        break;

                    case CppTypeKind.Array:
                        break;

                    case CppTypeKind.Qualified:
                        return (type as CppQualifiedType).ElementType.GetDisplayName();

                    case CppTypeKind.Function:
                        break;

                    case CppTypeKind.Typedef:
                        return GetParameterName((type as CppTypedef).ElementType, name);

                    case CppTypeKind.StructOrClass:
                        break;

                    case CppTypeKind.Enum:
                        return (type as CppEnum).GetDisplayName();

                    case CppTypeKind.TemplateParameterType:
                        break;

                    case CppTypeKind.TemplateParameterNonType:
                        break;

                    case CppTypeKind.Unexposed:
                        break;
                }
            }

            return NormalizeParameterName(name);
        }

        private static string NormalizeParameterName(string name)
        {
            var parts = name.Split('_', StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new();
            for (int i = 0; i < parts.Length; i++)
            {
                if (i == 0)
                {
                    sb.Append(char.ToLower(parts[i][0]));
                    sb.Append(parts[i][1..]);
                }
                else
                {
                    sb.Append(char.ToUpper(parts[i][0]));
                    sb.Append(parts[i][1..]);
                }
            }
            name = sb.ToString();
            if (CsCodeGeneratorSettings.Default.Keywords.Contains(name))
            {
                return "@" + name;
            }

            return name;
        }

        private static bool CanBeUsedAsOutput(CppType type, out CppTypeDeclaration? elementTypeDeclaration)
        {
            if (type is CppPointerType pointerType)
            {
                if (pointerType.ElementType is CppTypedef typedef)
                {
                    elementTypeDeclaration = typedef;
                    return true;
                }
                else if (pointerType.ElementType is CppClass @class
                    && @class.ClassKind != CppClassKind.Class
                    && @class.SizeOf > 0)
                {
                    elementTypeDeclaration = @class;
                    return true;
                }
                else if (pointerType.ElementType is CppEnum @enum
                    && @enum.SizeOf > 0)
                {
                    elementTypeDeclaration = @enum;
                    return true;
                }
            }

            elementTypeDeclaration = null;
            return false;
        }

        public static string GetPrettyCommandName(string function)
        {
            string[] parts = GetCsCleanName(function).SplitByCase();

            StringBuilder sb = new();
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];

                if (CsCodeGeneratorSettings.Default.IgnoredParts.Contains(part))
                {
                    continue;
                }

                sb.Append(part);
            }

            return sb.ToString();
        }
    }
}