namespace Generator
{
    using CppAst;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;

    public class CsCodeGeneratorSettings
    {
        public static void Load(string file)
        {
            if (File.Exists(file))
            {
                Default = JsonSerializer.Deserialize<CsCodeGeneratorSettings>(File.ReadAllText(file)) ?? new();
            }
            else
            {
                Default = new();
            }
            Default.Save();
        }

        public static CsCodeGeneratorSettings Default { get; private set; }

        public string Namespace { get; set; } = string.Empty;

        public string ApiName { get; set; } = string.Empty;

        public string LibName { get; set; } = string.Empty;

        public bool GenerateSizeOfStructs { get; set; } = false;

        public Dictionary<string, string> KnownConstantNames { get; set; } = new();

        public Dictionary<string, string> KnownEnumValueNames { get; set; } = new();

        public Dictionary<string, string> KnownEnumPrefixes { get; set; } = new();

        public Dictionary<string, string> KnownExtensionPrefixes { get; set; } = new();

        public Dictionary<string, string> KnownExtensionNames { get; set; } = new();

        public Dictionary<string, string> KnownDefaultValueNames { get; set; } = new();

        public Dictionary<string, List<string>> KnownMemberFunctions { get; set; } = new();

        public HashSet<string> IgnoredParts { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public HashSet<string> PreserveCaps { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public HashSet<string> Keywords { get; set; } = new();

        public HashSet<string> IgnoredFunctions { get; set; } = new();

        public HashSet<string> IgnoredTypes { get; set; } = new();

        public HashSet<string> IgnoredEnums { get; set; } = new();

        public HashSet<string> IgnoredTypedefs { get; set; } = new();

        public List<FunctionMapping> FunctionMappings { get; set; } = new();

        public List<ArrayMapping> ArrayMappings { get; set; } = new();

        public Dictionary<string, string> NameMappings { get; set; } = new()
        {
            { "uint8_t", "byte" },
            { "uint16_t", "ushort" },
            { "uint32_t", "uint" },
            { "uint64_t", "ulong" },
            { "int8_t", "sbyte" },
            { "int32_t", "int" },
            { "int16_t", "short" },
            { "int64_t", "long" },
            { "int64_t*", "long*" },
            { "unsigned char", "byte" },
            { "signed char", "sbyte" },
            { "char", "byte" },
            { "size_t", "nuint" }
        };

        public List<string> AllowedFunctions { get; set; } = new();

        public List<string> AllowedTypes { get; set; } = new();

        public List<string> AllowedEnums { get; set; } = new();

        public List<string> AllowedTypedefs { get; set; } = new();

        public List<string> Usings { get; set; } = new();

        public void Save()
        {
            File.WriteAllText("generator.json", JsonSerializer.Serialize(Default));
        }

        public bool TryGetFunctionMapping(string functionName, [NotNullWhen(true)] out FunctionMapping? mapping)
        {
            for (int i = 0; i < FunctionMappings.Count; i++)
            {
                var functionMapping = FunctionMappings[i];
                if (functionMapping.ExportedName == functionName)
                {
                    mapping = functionMapping;
                    return true;
                }
            }

            mapping = null;
            return false;
        }
    }
}