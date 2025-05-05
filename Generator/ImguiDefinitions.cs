/*
The MIT License (MIT)

Copyright (c) 2017 Eric Mellino and ImGui.NET contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace Generator
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ImguiDefinitions
    {
        public EnumDefinition[] Enums = [];
        public TypeDefinition[] Types = [];
        public FunctionDefinition[] Functions = [];

        public static readonly List<string> WellKnownEnums =
        [
            "ImGuiMouseButton"
        ];

        public static readonly Dictionary<string, string> AlternateEnumPrefixes = new()
        {
            { "ImGuiKey", "ImGuiMod" },
        };

        public static readonly Dictionary<string, string> AlternateEnumPrefixSubstitutions = new()
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
            using (JsonTextReader jr = new(fs))
            {
                typesJson = JObject.Load(jr);
            }

            JObject functionsJson;
            using (StreamReader fs = File.OpenText(Path.Combine(directory, "definitions.json")))
            using (JsonTextReader jr = new(fs))
            {
                functionsJson = JObject.Load(jr);
            }

            JObject typedefsJson;
            using (StreamReader fs = File.OpenText(Path.Combine(directory, "typedefs_dict.json")))
            using (JsonTextReader jr = new(fs))
            {
                typedefsJson = JObject.Load(jr);
            }

            var typeLocations = typesJson["locations"];

            Enums = ParseEnums(typesJson);

            Types = [.. typesJson["structs"]!.Select(jt =>
            {
                JProperty jp = (JProperty)jt;
                string name = jp.Name;
                var structComments = typesJson["struct_comments"];
                string? comment = null;
                if (structComments is JArray array)
                {
                    if (array.Count > 0)
                    {
                        comment = structComments?[name]?["above"]?.ToString();
                    }
                }
                else
                {
                    comment = structComments?[name]?["above"]?.ToString();
                }

                Field[] fields = [.. jp.Values().Select(v =>
                {
                    if (v["type"]!.ToString().Contains("static")) { return null; }

                    return new Field(
                        v["name"]!.ToString(),
                        GetComment(v["comment"]), GetInt(v, "size"),
                        Enums);
                }).Where(tr => tr != null).Select(x => x!)];
                return new TypeDefinition(name, fields, comment);
            }).Where(x => x != null)];

            Functions = [.. functionsJson.Children().Select(jt =>
            {
                JProperty jp = (JProperty)jt;
                string name = jp.Name;
                bool hasNonUdtVariants = jp.Values().Any(val => val["ov_cimguiname"]?.ToString().EndsWith("nonUDT") ?? false);
                OverloadDefinition[] overloads = [.. jp.Values().Select(val =>
                {
                    string? ov_cimguiname = val["ov_cimguiname"]?.ToString();
                    string cimguiname = val["cimguiname"]!.ToString();
                    string? friendlyName = val["funcname"]?.ToString();
                    if (cimguiname.EndsWith("_destroy"))
                    {
                        friendlyName = "Destroy";
                    }

                    var typename = val["stname"]?.ToString();
                    if (!string.IsNullOrEmpty(typename))
                    {
                        if (!Types.Any(x => x.Name == val["stname"]?.ToString()))
                        {
                            return null;
                        }
                    }
                    if (friendlyName == null) { return null; }

                    string exportedName = ov_cimguiname ?? cimguiname;

                    if (hasNonUdtVariants && !exportedName.EndsWith("nonUDT2"))
                    {
                        return null;
                    }

                    string? selfTypeName = null;
                    int underscoreIndex = exportedName.IndexOf('_');
                    if (underscoreIndex > 0 && !exportedName.StartsWith("ig")) // Hack to exclude some weirdly-named non-instance functions.
                    {
                        selfTypeName = exportedName[..underscoreIndex];
                    }

                    // find any variants that can be applied to the parameters of this method based on the method name

                    Dictionary<string, string> defaultValues = [];
                    foreach (JToken dv in val["defaults"]!)
                    {
                        JProperty dvProp = (JProperty)dv;
                        defaultValues.Add(dvProp.Name, dvProp.Value.ToString());
                    }
                    string returnType = val["ret"]?.ToString() ?? "void";
                    string? comment = val["comment"]?.ToString();

                    string structName = val["stname"]!.ToString();
                    bool isConstructor = val.Value<bool>("constructor");
                    bool isDestructor = val.Value<bool>("destructor");
                    if (isConstructor)
                    {
                        returnType = structName + "*";
                    }

                    var args = val["args"]!.ToString();

                    return new OverloadDefinition(exportedName, friendlyName, defaultValues, returnType, structName, comment, isConstructor, isDestructor, args)
                    {
                        Internal = val["location"]?.ToString().Contains("internal") ?? false,
                    };
                }).Where(od => od != null).Select(x => x!)];

                if (overloads.Length == 0) return null;

                return new FunctionDefinition(name, overloads, Enums);
            }).Where(x => x != null).Select(x => x!).OrderBy(fd => fd.Name)];
        }

        private static EnumDefinition[] ParseEnums(JObject json)
        {
            var enumsToken = json["enums"] ?? throw new InvalidOperationException("'enumsToken' was null.");
            var commentsToken = json["enum_comments"] as JObject ?? [];

            return [.. enumsToken.Children<JProperty>().Select(prop => EnumDefinition.FromJson(prop, commentsToken))];
        }
    }

    public class EnumDefinition : IEquatable<EnumDefinition?>
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

            if (ImguiDefinitions.AlternateEnumPrefixes.TryGetValue(name, out string? altName))
            {
                Names = [name, altName];
            }
            else
            {
                Names = [name];
            }
            FriendlyNames = new string[Names.Length];
            for (int i = 0; i < Names.Length; i++)
            {
                string n = Names[i];
                if (n.EndsWith('_'))
                {
                    FriendlyNames[i] = n[..^1];
                }
                else
                {
                    FriendlyNames[i] = n;
                }
            }

            Members = elements;

            _sanitizedNames = [];
            foreach (EnumMember el in elements)
            {
                _sanitizedNames.Add(el.Name, SanitizeMemberName(el.Name));
            }

            Comment = comment;
        }

        public static EnumDefinition FromJson(JProperty prop, JObject enumComments)
        {
            string name = prop.Name;
            string? comment = enumComments?[name]?["above"]?.ToString();

            EnumMember[] members = [.. prop.Values().Select(EnumMember.FromJson)];

            return new EnumDefinition(name, members, comment);
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
                        ret = memberName[name.Length..];
                        if (ret.StartsWith('_'))
                        {
                            ret = ret[1..];
                        }
                    }
                }
            }

            if (ret.EndsWith('_'))
            {
                ret = ret[..^1];
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

        public override int GetHashCode()
        {
            return HashCode.Combine(_sanitizedNames, Names, FriendlyNames, Members);
        }
    }

    public class EnumMember
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

        public static EnumMember FromJson(JToken token)
        {
            return new EnumMember(token["name"]?.ToString() ?? string.Empty, token["calc_value"]?.ToString() ?? string.Empty, token["comment"]?.ToString());
        }
    }

    public class TypeDefinition
    {
        public string Name { get; }

        public Field[] Fields { get; }

        public string? Comment { get; }

        public TypeDefinition(string name, Field[] fields, string? comment)
        {
            Name = name;
            Fields = fields;
            Comment = comment;
        }
    }

    public class Field
    {
        public string Name { get; }

        public int ArraySize { get; }

        public string? Comment { get; }

        public Field(string name, string? comment, int asize, EnumDefinition[] enums)
        {
            Name = name;

            ArraySize = asize;
            int startBracket = name.IndexOf('[');
            if (startBracket != -1)
            {
                //This is only for older cimgui binding jsons
                int endBracket = name.IndexOf(']');
                string sizePart = name.Substring(startBracket + 1, endBracket - startBracket - 1);
                if (ArraySize == 0)
                    ArraySize = ParseSizeString(sizePart, enums);
                Name = Name[..startBracket];
            }

            Comment = comment;
        }

        private static int ParseSizeString(string sizePart, EnumDefinition[] enums)
        {
            int plusStart = sizePart.IndexOf('+');
            if (plusStart != -1)
            {
                string first = sizePart[..plusStart];
                string second = sizePart[plusStart..];
                int firstVal = int.Parse(first);
                int secondVal = int.Parse(second);
                return firstVal + secondVal;
            }

            if (!int.TryParse(sizePart, out int ret))
            {
                foreach (EnumDefinition ed in enums)
                {
                    if (ed.Names.Any(sizePart.StartsWith))
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
    }

    public class FunctionDefinition
    {
        public string Name { get; }

        public OverloadDefinition[] Overloads { get; }

        public FunctionDefinition(string name, OverloadDefinition[] overloads, EnumDefinition[] enums)
        {
            Name = name;
            Overloads = overloads;
        }
    }

    public class OverloadDefinition
    {
        public string ExportedName { get; }

        public string? FriendlyName { get; }

        public Dictionary<string, string> DefaultValues { get; }

        public string ReturnType { get; }

        public string StructName { get; }

        public bool IsMemberFunction { get; }

        public string? Comment { get; }

        public bool IsConstructor { get; }

        public bool IsDestructor { get; }

        public bool Internal { get; init; }

        public string Args { get; init; }

        public OverloadDefinition(
            string exportedName,
            string? friendlyName,
            Dictionary<string, string> defaultValues,
            string returnType,
            string structName,
            string? comment,
            bool isConstructor,
            bool isDestructor,
            string args)
        {
            ExportedName = exportedName;
            FriendlyName = friendlyName;
            DefaultValues = defaultValues;
            ReturnType = returnType.Replace("const", string.Empty).Replace("inline", string.Empty).Trim();
            StructName = structName;
            IsMemberFunction = !string.IsNullOrEmpty(structName);
            Comment = comment;
            IsConstructor = isConstructor;
            IsDestructor = isDestructor;
            Args = args;
        }
    }

    public class TypedefDefinition
    {
        public TypedefDefinition(string name, string definition)
        {
            Name = name;
            Definition = definition;
        }

        public string Name { get; set; }

        public string Definition { get; set; }
    }
}