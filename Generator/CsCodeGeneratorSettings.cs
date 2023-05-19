namespace Generator
{
    using CppAst;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    public class CsCodeGeneratorSettings
    {
        static CsCodeGeneratorSettings()
        {
            if (File.Exists("generator.json"))
            {
                Default = JsonSerializer.Deserialize<CsCodeGeneratorSettings>(File.ReadAllText("generator.json")) ?? new();
            }
            else
            {
                Default = new();
            }
            Default.Save();
        }

        public static CsCodeGeneratorSettings Default { get; }

        public string Namespace { get; set; } = string.Empty;

        public string ApiName { get; set; } = string.Empty;

        public string LibName { get; set; } = string.Empty;

        public bool GenerateSizeOfStructs { get; set; } = false;

        public Dictionary<string, string> KnownConstantNames { get; set; } = new();

        public Dictionary<string, string> KnownEnumValueNames { get; set; } = new();

        public Dictionary<string, string> KnownEnumPrefixes { get; set; } = new();

        public Dictionary<string, string> KnownExtensionPrefixes { get; set; } = new();

        public Dictionary<string, string> KnownExtensionNames { get; set; } = new();

        public Dictionary<string, List<string>> KnownStructMethods { get; set; } = new();

        public HashSet<string> IgnoredParts { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public HashSet<string> PreserveCaps { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        public HashSet<string> Keywords { get; set; } = new();

        public List<string> IgnoredTypes { get; set; } = new();

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
            { "size_t", "nuint" },

            { "spvc_bool", "bool" },
            { "spvc_constant_id", "uint" },
            { "spvc_variable_id", "uint" },
            { "spvc_type_id", "uint" },
            { "spvc_hlsl_binding_flags", "uint" },
            { "spvc_msl_shader_input", "SpvcMslShaderInterfaceVar" },
            { "spvc_msl_vertex_format", "SpvcMslShaderVariableFormat" }
        };

        public void Save()
        {
            File.WriteAllText("generator.json", JsonSerializer.Serialize(Default));
        }
    }
}