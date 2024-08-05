namespace Generator
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class ImguiDefinitions
    {
        public EnumDefinition[] Enums;
        public TypeDefinition[] Types;
        public FunctionDefinition[] Functions;
        public TypedefDefinition[] Typedefs;

        public static readonly List<string> WellKnownEnums = new List<string>()
        {
            "ImGuiMouseButton"
        };

        public static readonly Dictionary<string, string> AlternateEnumPrefixes = new Dictionary<string, string>()
        {
            { "ImGuiKey", "ImGuiMod" },
        };

        public static readonly Dictionary<string, string> AlternateEnumPrefixSubstitutions = new Dictionary<string, string>()
        {
            { "ImGuiMod_", "Mod" },
        };

        private static string? GetComment(JToken? token)
        {
            if (token == null)
                return null;
            var above = token["above"]?.ToString();
            var sameline = token["sameline"]?.ToString();
            if (above == null && sameline == null)
                return null;
            if (above == null)
                return sameline;
            if (sameline == null)
                return null;
            return above + sameline;
        }

        private static int GetInt(JToken token, string key)
        {
            var v = token[key];
            if (v == null) return 0;
            return v.ToObject<int>();
        }

        public ImguiDefinitions()
        {
        }

        public ImguiDefinitions(string dir)
        {
            LoadFrom(dir);
        }

        public void LoadFrom(string directory)
        {
            JObject typesJson;
            using (StreamReader fs = File.OpenText(Path.Combine(directory, "structs_and_enums.json")))
            using (JsonTextReader jr = new JsonTextReader(fs))
            {
                typesJson = JObject.Load(jr);
            }

            JObject functionsJson;
            using (StreamReader fs = File.OpenText(Path.Combine(directory, "definitions.json")))
            using (JsonTextReader jr = new JsonTextReader(fs))
            {
                functionsJson = JObject.Load(jr);
            }

            JObject typedefsJson;
            using (StreamReader fs = File.OpenText(Path.Combine(directory, "typedefs_dict.json")))
            using (JsonTextReader jr = new JsonTextReader(fs))
            {
                typedefsJson = JObject.Load(jr);
            }

            var typeLocations = typesJson["locations"];

            Enums = typesJson["enums"].Select(jt =>
            {
                JProperty jp = (JProperty)jt;
                string name = jp.Name;
                string? comment = typesJson["enum_comments"]?[name]?["above"]?.ToString();
                if (typeLocations?[jp.Name]?.Value<string>().Contains("internal") ?? false)
                {
                    return null;
                }
                EnumMember[] elements = jp.Values().Select(v =>
                {
                    return new EnumMember(v["name"].ToString(), v["calc_value"].ToString(), v["comment"]?.ToString());
                }).ToArray();
                return new EnumDefinition(name, elements, comment);
            }).Where(x => x != null).ToArray();

            Types = typesJson["structs"].Select(jt =>
            {
                JProperty jp = (JProperty)jt;
                string name = jp.Name;
                string? comment = typesJson["struct_comments"]?[name]?["above"]?.ToString();
                if (typeLocations?[jp.Name]?.Value<string>().Contains("internal") ?? false)
                {
                    return null;
                }
                TypeReference[] fields = jp.Values().Select(v =>
                {
                    if (v["type"].ToString().Contains("static")) { return null; }

                    return new TypeReference(
                        v["name"].ToString(),
                        GetComment(v["comment"]),
                        v["type"].ToString(),
                            GetInt(v, "size"),
                        v["template_type"]?.ToString(),
                        Enums);
                }).Where(tr => tr != null).ToArray();
                return new TypeDefinition(name, fields, comment);
            }).Where(x => x != null).ToArray();

            Functions = functionsJson.Children().Select(jt =>
            {
                JProperty jp = (JProperty)jt;
                string name = jp.Name;
                bool hasNonUdtVariants = jp.Values().Any(val => val["ov_cimguiname"]?.ToString().EndsWith("nonUDT") ?? false);
                OverloadDefinition[] overloads = jp.Values().Select(val =>
                {
                    string ov_cimguiname = val["ov_cimguiname"]?.ToString();
                    string cimguiname = val["cimguiname"].ToString();
                    string friendlyName = val["funcname"]?.ToString();
                    if (cimguiname.EndsWith("_destroy"))
                    {
                        friendlyName = "Destroy";
                    }
                    //skip internal functions
                    var typename = val["stname"]?.ToString();
                    if (!string.IsNullOrEmpty(typename))
                    {
                        if (!Types.Any(x => x.Name == val["stname"]?.ToString()))
                        {
                            return null;
                        }
                    }
                    if (friendlyName == null) { return null; }
                    if (val["location"]?.ToString().Contains("internal") ?? false) return null;

                    string exportedName = ov_cimguiname;
                    if (exportedName == null)
                    {
                        exportedName = cimguiname;
                    }

                    if (hasNonUdtVariants && !exportedName.EndsWith("nonUDT2"))
                    {
                        return null;
                    }

                    string selfTypeName = null;
                    int underscoreIndex = exportedName.IndexOf('_');
                    if (underscoreIndex > 0 && !exportedName.StartsWith("ig")) // Hack to exclude some weirdly-named non-instance functions.
                    {
                        selfTypeName = exportedName.Substring(0, underscoreIndex);
                    }

                    List<TypeReference> parameters = new List<TypeReference>();

                    // find any variants that can be applied to the parameters of this method based on the method name

                    Dictionary<string, string> defaultValues = new Dictionary<string, string>();
                    foreach (JToken dv in val["defaults"])
                    {
                        JProperty dvProp = (JProperty)dv;
                        defaultValues.Add(dvProp.Name, dvProp.Value.ToString());
                    }
                    string returnType = val["ret"]?.ToString() ?? "void";
                    string? comment = val["comment"]?.ToString();

                    string structName = val["stname"].ToString();
                    bool isConstructor = val.Value<bool>("constructor");
                    bool isDestructor = val.Value<bool>("destructor");
                    if (isConstructor)
                    {
                        returnType = structName + "*";
                    }

                    return new OverloadDefinition(
                        exportedName,
                        friendlyName,
                        parameters.ToArray(),
                        defaultValues,
                        returnType,
                        structName,
                        comment,
                        isConstructor,
                        isDestructor);
                }).Where(od => od != null).ToArray();
                if (overloads.Length == 0) return null;
                return new FunctionDefinition(name, overloads, Enums);
            }).Where(x => x != null).OrderBy(fd => fd.Name).ToArray();

            Typedefs = typedefsJson.Children().Select(jt =>
            {
                JProperty jp = (JProperty)jt;
                string name = jp.Name;
                string value = jp.Value.ToString();

                return new TypedefDefinition(name, value);
            }).ToArray();
        }
    }

    internal class EnumDefinition : IEquatable<EnumDefinition?>
    {
        private readonly Dictionary<string, string> _sanitizedNames;

        public string Name { get; }

        public string[] Names { get; }

        public string[] FriendlyNames { get; }

        public EnumMember[] Members { get; }

        public string? Comment { get; }

        public EnumDefinition(string name, EnumMember[] elements, string? comment)
        {
            Name = name;

            if (ImguiDefinitions.AlternateEnumPrefixes.TryGetValue(name, out string altName))
            {
                Names = new[] { name, altName };
            }
            else
            {
                Names = new[] { name };
            }
            FriendlyNames = new string[Names.Length];
            for (int i = 0; i < Names.Length; i++)
            {
                string n = Names[i];
                if (n.EndsWith('_'))
                {
                    FriendlyNames[i] = n.Substring(0, n.Length - 1);
                }
                else
                {
                    FriendlyNames[i] = n;
                }
            }

            Members = elements;

            _sanitizedNames = new Dictionary<string, string>();
            foreach (EnumMember el in elements)
            {
                _sanitizedNames.Add(el.Name, SanitizeMemberName(el.Name));
            }

            Comment = comment;
        }

        public string SanitizeNames(string text)
        {
            foreach (KeyValuePair<string, string> kvp in _sanitizedNames)
            {
                text = text.Replace(kvp.Key, kvp.Value);
            }

            return text;
        }

        private string SanitizeMemberName(string memberName)
        {
            string ret = memberName;
            bool altSubstitution = false;

            // Try alternate substitution first
            foreach (KeyValuePair<string, string> substitutionPair in ImguiDefinitions.AlternateEnumPrefixSubstitutions)
            {
                if (memberName.StartsWith(substitutionPair.Key))
                {
                    ret = ret.Replace(substitutionPair.Key, substitutionPair.Value);
                    altSubstitution = true;
                    break;
                }
            }

            if (!altSubstitution)
            {
                foreach (string name in Names)
                {
                    if (memberName.StartsWith(name))
                    {
                        ret = memberName.Substring(name.Length);
                        if (ret.StartsWith("_"))
                        {
                            ret = ret.Substring(1);
                        }
                    }
                }
            }

            if (ret.EndsWith('_'))
            {
                ret = ret.Substring(0, ret.Length - 1);
            }

            if (char.IsDigit(ret.First()))
                ret = "_" + ret;

            return ret;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EnumDefinition);
        }

        public bool Equals(EnumDefinition? other)
        {
            return other is not null &&
                   EqualityComparer<Dictionary<string, string>>.Default.Equals(_sanitizedNames, other._sanitizedNames) &&
                   EqualityComparer<string[]>.Default.Equals(Names, other.Names) &&
                   EqualityComparer<string[]>.Default.Equals(FriendlyNames, other.FriendlyNames) &&
                   EqualityComparer<EnumMember[]>.Default.Equals(Members, other.Members);
        }

        public static bool operator ==(EnumDefinition? left, EnumDefinition? right)
        {
            return EqualityComparer<EnumDefinition>.Default.Equals(left, right);
        }

        public static bool operator !=(EnumDefinition? left, EnumDefinition? right)
        {
            return !(left == right);
        }
    }

    internal class EnumMember
    {
        public EnumMember(string name, string value, string? comment)
        {
            Name = name;
            Value = value;
            Comment = comment;
        }

        public string Name { get; }

        public string Value { get; }

        public string? Comment { get; }
    }

    internal class TypeDefinition
    {
        public string Name { get; }

        public TypeReference[] Fields { get; }

        public string? Comment { get; }

        public TypeDefinition(string name, TypeReference[] fields, string? comment)
        {
            Name = name;
            Fields = fields;
            Comment = comment;
        }
    }

    internal class TypeReference
    {
        public string Name { get; }

        public string Type { get; }

        public string TemplateType { get; }

        public int ArraySize { get; }

        public bool IsFunctionPointer { get; }

        public string[] TypeVariants { get; }

        public bool IsEnum { get; }

        public string? Comment { get; }

        public TypeReference(string name, string? comment, string type, int asize, EnumDefinition[] enums)
            : this(name, comment, type, asize, null, enums, null) { }

        public TypeReference(string name, string? comment, string type, int asize, EnumDefinition[] enums, string[] typeVariants)
            : this(name, comment, type, asize, null, enums, typeVariants) { }

        public TypeReference(string name, string? comment, string type, int asize, string templateType, EnumDefinition[] enums)
            : this(name, comment, type, asize, templateType, enums, null) { }

        public TypeReference(string name, string? comment, string type, int asize, string templateType, EnumDefinition[] enums, string[] typeVariants)
        {
            Name = name;
            Type = type.Replace("const", string.Empty).Trim();

            if (Type.StartsWith("ImVector_"))
            {
                if (Type.EndsWith("*"))
                {
                    Type = "ImVector*";
                }
                else
                {
                    Type = "ImVector";
                }
            }

            if (Type.StartsWith("ImChunkStream_"))
            {
                if (Type.EndsWith("*"))
                {
                    Type = "ImChunkStream*";
                }
                else
                {
                    Type = "ImChunkStream";
                }
            }

            TemplateType = templateType;
            ArraySize = asize;
            int startBracket = name.IndexOf('[');
            if (startBracket != -1)
            {
                //This is only for older cimgui binding jsons
                int endBracket = name.IndexOf(']');
                string sizePart = name.Substring(startBracket + 1, endBracket - startBracket - 1);
                if (ArraySize == 0)
                    ArraySize = ParseSizeString(sizePart, enums);
                Name = Name.Substring(0, startBracket);
            }
            IsFunctionPointer = Type.IndexOf('(') != -1;

            TypeVariants = typeVariants;

            IsEnum = enums.Any(t => t.Names.Contains(type) || t.FriendlyNames.Contains(type) || ImguiDefinitions.WellKnownEnums.Contains(type));

            Comment = comment;
        }

        private int ParseSizeString(string sizePart, EnumDefinition[] enums)
        {
            int plusStart = sizePart.IndexOf('+');
            if (plusStart != -1)
            {
                string first = sizePart.Substring(0, plusStart);
                string second = sizePart.Substring(plusStart, sizePart.Length - plusStart);
                int firstVal = int.Parse(first);
                int secondVal = int.Parse(second);
                return firstVal + secondVal;
            }

            if (!int.TryParse(sizePart, out int ret))
            {
                foreach (EnumDefinition ed in enums)
                {
                    if (ed.Names.Any(n => sizePart.StartsWith(n)))
                    {
                        foreach (EnumMember member in ed.Members)
                        {
                            if (member.Name == sizePart)
                            {
                                return int.Parse(member.Value);
                            }
                        }
                    }
                }

                ret = -1;
            }

            return ret;
        }

        public TypeReference WithVariant(int variantIndex, EnumDefinition[] enums)
        {
            if (variantIndex == 0) return this;
            else return new TypeReference(Name, Comment, TypeVariants[variantIndex - 1], ArraySize, TemplateType, enums);
        }
    }

    internal class FunctionDefinition
    {
        public string Name { get; }
        public OverloadDefinition[] Overloads { get; }

        public FunctionDefinition(string name, OverloadDefinition[] overloads, EnumDefinition[] enums)
        {
            Name = name;
            Overloads = ExpandOverloadVariants(overloads, enums);
        }

        private OverloadDefinition[] ExpandOverloadVariants(OverloadDefinition[] overloads, EnumDefinition[] enums)
        {
            List<OverloadDefinition> newDefinitions = new List<OverloadDefinition>();

            foreach (OverloadDefinition overload in overloads)
            {
                bool hasVariants = false;
                int[] variantCounts = new int[overload.Parameters.Length];

                for (int i = 0; i < overload.Parameters.Length; i++)
                {
                    if (overload.Parameters[i].TypeVariants != null)
                    {
                        hasVariants = true;
                        variantCounts[i] = overload.Parameters[i].TypeVariants.Length + 1;
                    }
                    else
                    {
                        variantCounts[i] = 1;
                    }
                }

                if (hasVariants)
                {
                    int totalVariants = variantCounts[0];
                    for (int i = 1; i < variantCounts.Length; i++) totalVariants *= variantCounts[i];

                    for (int i = 0; i < totalVariants; i++)
                    {
                        TypeReference[] parameters = new TypeReference[overload.Parameters.Length];
                        int div = 1;

                        for (int j = 0; j < parameters.Length; j++)
                        {
                            int k = i / div % variantCounts[j];

                            parameters[j] = overload.Parameters[j].WithVariant(k, enums);

                            if (j > 0) div *= variantCounts[j];
                        }

                        newDefinitions.Add(overload.WithParameters(parameters));
                    }
                }
                else
                {
                    newDefinitions.Add(overload);
                }
            }

            return newDefinitions.ToArray();
        }
    }

    internal class OverloadDefinition
    {
        public string ExportedName { get; }
        public string FriendlyName { get; }
        public TypeReference[] Parameters { get; }
        public Dictionary<string, string> DefaultValues { get; }
        public string ReturnType { get; }
        public string StructName { get; }
        public bool IsMemberFunction { get; }
        public string Comment { get; }
        public bool IsConstructor { get; }
        public bool IsDestructor { get; }

        public OverloadDefinition(
            string exportedName,
            string friendlyName,
            TypeReference[] parameters,
            Dictionary<string, string> defaultValues,
            string returnType,
            string structName,
            string comment,
            bool isConstructor,
            bool isDestructor)
        {
            ExportedName = exportedName;
            FriendlyName = friendlyName;
            Parameters = parameters;
            DefaultValues = defaultValues;
            ReturnType = returnType.Replace("const", string.Empty).Replace("inline", string.Empty).Trim();
            StructName = structName;
            IsMemberFunction = !string.IsNullOrEmpty(structName);
            Comment = comment;
            IsConstructor = isConstructor;
            IsDestructor = isDestructor;
        }

        public OverloadDefinition WithParameters(TypeReference[] parameters)
        {
            return new OverloadDefinition(ExportedName, FriendlyName, parameters, DefaultValues, ReturnType, StructName, Comment, IsConstructor, IsDestructor);
        }
    }

    internal class TypedefDefinition
    {
        public TypedefDefinition(string name, string definition)
        {
            Name = name;
            Definition = definition;
        }

        public string Name { get; set; }

        public string Definition { get; set; }

        public bool IsStruct => Definition.StartsWith("struct");
    }
}