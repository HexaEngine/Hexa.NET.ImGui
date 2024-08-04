namespace Generator
{
    using CppAst;
    using System.Text.RegularExpressions;
    using System.Linq;
    using System.Text;

    internal unsafe class Program
    {

        private static void Main(string[] args)
        {
            PreProcess();

            GenerateImGui();

            var constants = CsCodeGenerator.DefinedConstants.ToList();
            var enums = CsCodeGenerator.DefinedEnums.ToList();
            var extensions = CsCodeGenerator.DefinedExtensions.ToList();
            var functions = CsCodeGenerator.DefinedFunctions.ToList();
            var typedefs = CsCodeGenerator.DefinedTypedefs.ToList();
            var types = CsCodeGenerator.DefinedTypes.ToList();
            var delegates = CsCodeGenerator.DefinedDelegates.ToList();


            GenerateImGuizmo();
            CsCodeGenerator.Reset();
            CsCodeGenerator.CopyFrom(constants, enums, extensions, functions, typedefs, types, delegates);
            GenerateImNodes();
            CsCodeGenerator.Reset();
            CsCodeGenerator.CopyFrom(constants, enums, extensions, functions, typedefs, types, delegates);
            GenerateImPlot();


            GenerateImGuiManual();
        }

        private static void PreProcess()
        {
            // Patch ImVector types
            PatchConfig("cimgui/cimgui.h", "cimgui/generator.json");
            PatchConfig("cimguizmo/cimguizmo.h", "cimguizmo/generator.json");
            PatchConfig("cimnodes/cimnodes.h", "cimnodes/generator.json");
            PatchConfig("cimplot/cimplot.h", "cimplot/generator.json");

            // Merge settings
            MergeConfig("cimgui/generator.json", "cimguizmo/generator.json");
            MergeConfig("cimgui/generator.json", "cimnodes/generator.json");
            MergeConfig("cimgui/generator.json", "cimplot/generator.json");
        }

        private static void MergeConfig(string from, string into)
        {
            var baseSettings = CsCodeGeneratorSettings.LoadInstance(from);
            var patchSettings = CsCodeGeneratorSettings.LoadInstance(into);

            foreach (var mapping in baseSettings.FunctionMappings)
            {
                patchSettings.FunctionMappings.Add(mapping);
            }

            foreach (var mapping in baseSettings.DelegateMappings)
            {
                patchSettings.DelegateMappings.Add(mapping);
            }

            foreach (var mapping in baseSettings.IgnoredEnums)
            {
                patchSettings.IgnoredEnums.Add(mapping);
            }

            foreach (var mapping in baseSettings.IgnoredFunctions)
            {
                patchSettings.IgnoredFunctions.Add(mapping);
            }

            foreach (var mapping in baseSettings.IgnoredTypes)
            {
                patchSettings.IgnoredTypes.Add(mapping);
            }

            foreach (var mapping in baseSettings.IgnoredTypedefs)
            {
                patchSettings.IgnoredTypedefs.Add(mapping);
            }

            foreach (var mapping in baseSettings.NameMappings)
            {
                patchSettings.NameMappings.TryAdd(mapping.Key, mapping.Value);
            }

            patchSettings.Save(into);
        }

        private static void PatchConfig(string file, string settingFile)
        {
            var settings = CsCodeGeneratorSettings.LoadInstance(settingFile);
            Regex regex = new("typedef struct (\\bImVector_(.*)\\b) {");
            string content = File.ReadAllText(file);
            var matches = regex.Matches(content);
            TextWriter writer = File.CreateText(Path.Combine(Path.GetDirectoryName(file) ?? string.Empty, "ImVectorPatch.json"));
            writer.WriteLine("{");
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                var name = match.Groups[1].Value;
                var nameValue = match.Groups[2].Value;
                switch (nameValue)
                {
                    case "const_charPtr":
                        nameValue = "ConstPointer<byte>";
                        break;
                    case "unsigned_char":
                        nameValue = "byte";
                        break;
                }
                if (settings.NameMappings.TryGetValue(nameValue, out var mappedName))
                {
                    nameValue = mappedName;
                }

                settings.IgnoredTypes.Add(name);
                settings.IgnoredTypedefs.Add(name);
                settings.NameMappings.TryAdd(name, $"ImVector<{nameValue}>");

                if (i < matches.Count - 1)
                    writer.WriteLine($"\t\"{name}\": \"ImVector<{nameValue}>\",");
                else
                    writer.WriteLine($"\t\"{name}\": \"ImVector<{nameValue}>\"");
            }
            writer.WriteLine("}");
            writer.Close();
            settings.Save(settingFile);
        }

        private static void GenerateImGuiManual()
        {
            CsCodeGeneratorSettings.Load("cimgui/generator.manual.json");
            CsCodeGeneratorSettings.Default.Save();
            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom("cimgui");

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    var overload = functionDefinition.Overloads[j];
                    CsCodeGeneratorSettings.Default.FunctionMappings.Add(new(overload.ExportedName, overload.FriendlyName, overload.DefaultValues, new()));

                    if (overload.IsMemberFunction && !overload.IsConstructor)
                    {
                        if (CsCodeGeneratorSettings.Default.KnownMemberFunctions.TryGetValue(overload.StructName, out var knownFunctions))
                        {
                            if (!knownFunctions.Contains(overload.ExportedName))
                                knownFunctions.Add(overload.ExportedName);
                        }
                        else
                        {
                            CsCodeGeneratorSettings.Default.KnownMemberFunctions.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                }
            }

            string headerFile = "cimgui/cimgui.h";

            var options = new CppParserOptions
            {
                ParseMacros = true,
            };

            options.Defines.Add("CIMGUI_DEFINE_ENUMS_AND_STRUCTS");

            var compilation = CppParser.ParseFile(headerFile, options);

            // Print diagnostic messages
            if (compilation.HasErrors)
            {
                for (int i = 0; i < compilation.Diagnostics.Messages.Count; i++)
                {
                    CppDiagnosticMessage? message = compilation.Diagnostics.Messages[i];
                    if (message.Type == CppLogMessageType.Error)
                    {
                        var currentColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message);
                        Console.ForegroundColor = currentColor;
                    }
                }


            }

            CsCodeGeneratorSettings.Default.DelegateMappings.Add(new("PlatformGetWindowPos", "Vector2*", "Vector2* pos, ImGuiViewport* viewport"));
            CsCodeGeneratorSettings.Default.DelegateMappings.Add(new("PlatformGetWindowSize", "Vector2*", "Vector2* size, ImGuiViewport* viewport"));

            const string manual = "../../../../Hexa.NET.ImGui/Manual/";
            const string generated = "../../../../Hexa.NET.ImGui/Generated/";

            // Patch VTable
            {
                const string vtTargetFile = generated + "Functions.VT.cs";
                const string vtPatchFile = manual + "Functions.VT.cs";

                var vtContent = File.ReadAllText(vtTargetFile);
                var rootMatch = Regex.Match(vtContent, "vt = new VTable\\(GetLibraryName\\(\\), (.*)\\);");
                int vtBaseIndex = int.Parse(rootMatch.Groups[1].Value);

                Regex loadPattern = new("vt.Load\\((.*), \"(.*)\"\\);", RegexOptions.Singleline);
                var matches = loadPattern.Matches(vtContent);
                var lastMatch = matches[^1];
                int start = lastMatch.Index + lastMatch.Length;

                CsCodeGeneratorSettings.Default.VTableStart = vtBaseIndex;

                CsCodeGenerator.Generate(compilation, manual);

                var content = File.ReadAllText(vtPatchFile);
                File.Delete(vtPatchFile);

                StringBuilder builder = new(vtContent[..(start + 1)]);


                foreach (Match match in loadPattern.Matches(content))
                {
                    builder.AppendLine($"\t\t\t" + match.Value);
                }

                builder.Remove(rootMatch.Index, rootMatch.Length);
                builder.Insert(rootMatch.Index, $"vt = new VTable(GetLibraryName(), {CsCodeGeneratorSettings.Default.VTableLength});");

                builder.Append(vtContent.AsSpan(start));

                var newContent = builder.ToString();

                File.WriteAllText(vtTargetFile, newContent);
            }

            // Patch Functions
            {
                Regex regex = new("\\b(.*) = Utils.GetByteCountUTF8\\b\\(buf\\);");
                const string manualFunctions = manual + "Functions/";

                foreach (var file in Directory.EnumerateFiles(manualFunctions, "*.cs"))
                {
                    int indexOffset = 0;
                    var content = File.ReadAllText(file);
                    var matches = regex.Matches(content);
                    if (matches.Count > 0)
                    {
                        var builder = new StringBuilder(content);
                        foreach (Match match in matches)
                        {
                            var name = match.Groups[1].Value.Trim();
                            var replacement = $"{name} = Math.Max(Utils.GetByteCountUTF8(buf), (int)bufSize);";
                            var delta = replacement.Length - match.Value.Length;

                            builder.Replace(match.Value, replacement, match.Index + indexOffset, match.Length);
                            indexOffset += delta;
                        }
                        File.WriteAllText(file, builder.ToString());
                        Console.WriteLine($"Patched file: {file}");
                    }
                }
            }
        }



        private static int GenerateImPlot()
        {
            CsCodeGeneratorSettings.Load("cimplot/generator.json");
            CsCodeGeneratorSettings.Default.Save();
            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom("cimgui");

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    CsCodeGeneratorSettings.Default.IgnoredFunctions.Add(functionDefinition.Overloads[j].ExportedName);
                }
            }
            for (int i = 0; i < imguiDefinitions.Enums.Length; i++)
            {
                var enumDefinition = imguiDefinitions.Enums[i];
                CsCodeGeneratorSettings.Default.IgnoredEnums.Add(enumDefinition.Names[0]);
            }
            for (int i = 0; i < imguiDefinitions.Types.Length; i++)
            {
                var typeDefinition = imguiDefinitions.Types[i];
                CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typeDefinition.Name);
            }
            for (int i = 0; i < imguiDefinitions.Typedefs.Length; i++)
            {
                var typedefDefinition = imguiDefinitions.Typedefs[i];
                CsCodeGeneratorSettings.Default.IgnoredTypedefs.Add(typedefDefinition.Name);
                if (typedefDefinition.IsStruct)
                {
                    CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typedefDefinition.Name);
                }
            }

            ImguiDefinitions implotDefinitions = new();
            implotDefinitions.LoadFrom("cimplot");

            for (int i = 0; i < implotDefinitions.Functions.Length; i++)
            {
                var functionDefinition = implotDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    var overload = functionDefinition.Overloads[j];
                    CsCodeGeneratorSettings.Default.FunctionMappings.Add(new(overload.ExportedName, overload.FriendlyName, overload.DefaultValues, new()));

                    if (overload.IsMemberFunction && !overload.IsConstructor)
                    {
                        if (CsCodeGeneratorSettings.Default.KnownMemberFunctions.TryGetValue(overload.StructName, out var knownFunctions))
                        {
                            if (!knownFunctions.Contains(overload.ExportedName))
                                knownFunctions.Add(overload.ExportedName);
                        }
                        else
                        {
                            CsCodeGeneratorSettings.Default.KnownMemberFunctions.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                }
            }

            string headerFile = "cimplot/cimplot.h";

            var options = new CppParserOptions
            {
                ParseMacros = true,

            };

            options.Defines.Add("CIMGUI_DEFINE_ENUMS_AND_STRUCTS");

            var compilation = CppParser.ParseFile(headerFile, options);

            // Print diagnostic messages
            if (compilation.HasErrors)
            {
                for (int i = 0; i < compilation.Diagnostics.Messages.Count; i++)
                {
                    CppDiagnosticMessage? message = compilation.Diagnostics.Messages[i];
                    if (message.Type == CppLogMessageType.Error)
                    {
                        var currentColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message);
                        Console.ForegroundColor = currentColor;
                    }
                }

                return 0;
            }

            CsCodeGenerator.Generate(compilation, "../../../../Hexa.NET.ImPlot/Generated");

            return 0;
        }

        private static int GenerateImNodes()
        {
            CsCodeGeneratorSettings.Load("cimnodes/generator.json");
            CsCodeGeneratorSettings.Default.Save();
            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom("cimgui");

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    CsCodeGeneratorSettings.Default.IgnoredFunctions.Add(functionDefinition.Overloads[j].ExportedName);
                }
            }
            for (int i = 0; i < imguiDefinitions.Enums.Length; i++)
            {
                var enumDefinition = imguiDefinitions.Enums[i];
                CsCodeGeneratorSettings.Default.IgnoredEnums.Add(enumDefinition.Names[0]);
            }
            for (int i = 0; i < imguiDefinitions.Types.Length; i++)
            {
                var typeDefinition = imguiDefinitions.Types[i];
                CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typeDefinition.Name);
            }
            for (int i = 0; i < imguiDefinitions.Typedefs.Length; i++)
            {
                var typedefDefinition = imguiDefinitions.Typedefs[i];
                CsCodeGeneratorSettings.Default.IgnoredTypedefs.Add(typedefDefinition.Name);
                if (typedefDefinition.IsStruct)
                {
                    CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typedefDefinition.Name);
                }
            }

            ImguiDefinitions imnodesDefinitions = new();
            imnodesDefinitions.LoadFrom("cimnodes");

            for (int i = 0; i < imnodesDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imnodesDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    var overload = functionDefinition.Overloads[j];
                    CsCodeGeneratorSettings.Default.FunctionMappings.Add(new(overload.ExportedName, overload.FriendlyName, overload.DefaultValues, new()));

                    if (overload.IsMemberFunction && !overload.IsConstructor)
                    {
                        if (CsCodeGeneratorSettings.Default.KnownMemberFunctions.TryGetValue(overload.StructName, out var knownFunctions))
                        {
                            if (!knownFunctions.Contains(overload.ExportedName))
                                knownFunctions.Add(overload.ExportedName);
                        }
                        else
                        {
                            CsCodeGeneratorSettings.Default.KnownMemberFunctions.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                }
            }

            string headerFile = "cimnodes/cimnodes.h";

            var options = new CppParserOptions
            {
                ParseMacros = true,
            };

            options.Defines.Add("CIMGUI_DEFINE_ENUMS_AND_STRUCTS");

            var compilation = CppParser.ParseFile(headerFile, options);

            // Print diagnostic messages
            if (compilation.HasErrors)
            {
                for (int i = 0; i < compilation.Diagnostics.Messages.Count; i++)
                {
                    CppDiagnosticMessage? message = compilation.Diagnostics.Messages[i];
                    if (message.Type == CppLogMessageType.Error)
                    {
                        var currentColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message);
                        Console.ForegroundColor = currentColor;
                    }
                }

                return 0;
            }

            CsCodeGenerator.Generate(compilation, "../../../../Hexa.NET.ImNodes/Generated");

            return 0;
        }

        private static int GenerateImGuizmo()
        {
            CsCodeGeneratorSettings.Load("cimguizmo/generator.json");
            CsCodeGeneratorSettings.Default.Save();
            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom("cimgui");

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    CsCodeGeneratorSettings.Default.IgnoredFunctions.Add(functionDefinition.Overloads[j].ExportedName);
                }
            }
            for (int i = 0; i < imguiDefinitions.Enums.Length; i++)
            {
                var enumDefinition = imguiDefinitions.Enums[i];
                CsCodeGeneratorSettings.Default.IgnoredEnums.Add(enumDefinition.Names[0]);
            }
            for (int i = 0; i < imguiDefinitions.Types.Length; i++)
            {
                var typeDefinition = imguiDefinitions.Types[i];
                CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typeDefinition.Name);
            }
            for (int i = 0; i < imguiDefinitions.Typedefs.Length; i++)
            {
                var typedefDefinition = imguiDefinitions.Typedefs[i];
                CsCodeGeneratorSettings.Default.IgnoredTypedefs.Add(typedefDefinition.Name);
                if (typedefDefinition.IsStruct)
                {
                    CsCodeGeneratorSettings.Default.IgnoredTypes.Add(typedefDefinition.Name);
                }
            }

            ImguiDefinitions imguizmoDefinitions = new();
            imguizmoDefinitions.LoadFrom("cimguizmo");

            for (int i = 0; i < imguizmoDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguizmoDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    var overload = functionDefinition.Overloads[j];
                    CsCodeGeneratorSettings.Default.FunctionMappings.Add(new(overload.ExportedName, overload.FriendlyName, overload.DefaultValues, new()));

                    if (overload.IsMemberFunction && !overload.IsConstructor)
                    {
                        if (CsCodeGeneratorSettings.Default.KnownMemberFunctions.TryGetValue(overload.StructName, out var knownFunctions))
                        {
                            if (!knownFunctions.Contains(overload.ExportedName))
                                knownFunctions.Add(overload.ExportedName);
                        }
                        else
                        {
                            CsCodeGeneratorSettings.Default.KnownMemberFunctions.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                }
            }

            CsCodeGeneratorSettings.Default.TryGetFunctionMapping("ImGuizmo_Manipulate", out FunctionMapping manipulateMapping);
            manipulateMapping.CustomVariations.Add(new()
            {
                { "view", "ref Matrix4x4" },
                { "projection", "ref Matrix4x4" },
                { "matrix", "ref Matrix4x4" },
                { "deltaMatrix", "ref Matrix4x4" }
            });

            CsCodeGeneratorSettings.Default.TryGetFunctionMapping("ImGuizmo_DecomposeMatrixToComponents", out FunctionMapping decomposeMatrixToComponentsMapping);
            decomposeMatrixToComponentsMapping.CustomVariations.Add(new()
            {
                { "matrix", "ref Matrix4x4" },
                { "translation", "ref Matrix4x4" },
                { "rotation", "ref Matrix4x4" },
                { "scale", "ref Matrix4x4" },
            });

            CsCodeGeneratorSettings.Default.TryGetFunctionMapping("ImGuizmo_RecomposeMatrixFromComponents", out FunctionMapping recomposeMatrixToComponentsMapping);
            recomposeMatrixToComponentsMapping.CustomVariations.Add(new()
            {
                { "translation", "ref Matrix4x4" },
                { "rotation", "ref Matrix4x4" },
                { "scale", "ref Matrix4x4" },
                { "matrix", "ref Matrix4x4" },
            });

            CsCodeGeneratorSettings.Default.TryGetFunctionMapping("ImGuizmo_DrawCubes", out FunctionMapping drawCubesMapping);
            drawCubesMapping.CustomVariations.Add(new()
            {
                { "view", "ref Matrix4x4" },
                { "projection", "ref Matrix4x4" },
                { "matrices", "Matrix4x4[]" },
            });
            drawCubesMapping.CustomVariations.Add(new()
            {
                { "view", "ref Matrix4x4" },
                { "projection", "ref Matrix4x4" },
                { "matrices", "ref Matrix4x4" },
            });

            CsCodeGeneratorSettings.Default.TryGetFunctionMapping("ImGuizmo_DrawGrid", out FunctionMapping drawGridMapping);
            drawGridMapping.CustomVariations.Add(new()
            {
                { "view", "ref Matrix4x4" },
                { "projection", "ref Matrix4x4" },
                { "matrix", "ref Matrix4x4" },
            });

            CsCodeGeneratorSettings.Default.TryGetFunctionMapping("ImGuizmo_ViewManipulate_Float", out FunctionMapping viewManipulateFloatMapping);
            viewManipulateFloatMapping.CustomVariations.Add(new()
            {
                { "view", "ref Matrix4x4" },
            });

            CsCodeGeneratorSettings.Default.TryGetFunctionMapping("ImGuizmo_ViewManipulate_FloatPtr", out FunctionMapping viewManipulateFloatPtrMapping);
            viewManipulateFloatPtrMapping.CustomVariations.Add(new()
            {
                { "view", "ref Matrix4x4" },
                { "projection", "ref Matrix4x4" },
                { "matrix", "ref Matrix4x4" },
            });

            string headerFile = "cimguizmo/cimguizmo.h";

            var options = new CppParserOptions
            {
                ParseMacros = true,
            };

            options.Defines.Add("CIMGUI_DEFINE_ENUMS_AND_STRUCTS");

            var compilation = CppParser.ParseFile(headerFile, options);

            // Print diagnostic messages
            if (compilation.HasErrors)
            {
                for (int i = 0; i < compilation.Diagnostics.Messages.Count; i++)
                {
                    CppDiagnosticMessage? message = compilation.Diagnostics.Messages[i];
                    if (message.Type == CppLogMessageType.Error)
                    {
                        var currentColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message);
                        Console.ForegroundColor = currentColor;
                    }
                }

                return 0;
            }

            CsCodeGenerator.Generate(compilation, "../../../../Hexa.NET.ImGuizmo/Generated");

            return 0;
        }

        private static int GenerateImGui()
        {
            CsCodeGeneratorSettings.Load("cimgui/generator.json");
            CsCodeGeneratorSettings.Default.Save();
            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom("cimgui");

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    var overload = functionDefinition.Overloads[j];
                    CsCodeGeneratorSettings.Default.FunctionMappings.Add(new(overload.ExportedName, overload.FriendlyName, overload.DefaultValues, new()));

                    if (overload.IsMemberFunction && !overload.IsConstructor)
                    {
                        if (CsCodeGeneratorSettings.Default.KnownMemberFunctions.TryGetValue(overload.StructName, out var knownFunctions))
                        {
                            if (!knownFunctions.Contains(overload.ExportedName))
                                knownFunctions.Add(overload.ExportedName);
                        }
                        else
                        {
                            CsCodeGeneratorSettings.Default.KnownMemberFunctions.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                }
            }

            string headerFile = "cimgui/cimgui.h";

            var options = new CppParserOptions
            {
                ParseMacros = true,
            };

            options.Defines.Add("CIMGUI_DEFINE_ENUMS_AND_STRUCTS");

            var compilation = CppParser.ParseFile(headerFile, options);

            // Print diagnostic messages
            if (compilation.HasErrors)
            {
                for (int i = 0; i < compilation.Diagnostics.Messages.Count; i++)
                {
                    CppDiagnosticMessage? message = compilation.Diagnostics.Messages[i];
                    if (message.Type == CppLogMessageType.Error)
                    {
                        var currentColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message);
                        Console.ForegroundColor = currentColor;
                    }
                }

                return 0;
            }

            CsCodeGeneratorSettings.Default.DelegateMappings.Add(new("PlatformGetWindowPos", "Vector2*", "Vector2* pos, ImGuiViewport* viewport"));
            CsCodeGeneratorSettings.Default.DelegateMappings.Add(new("PlatformGetWindowSize", "Vector2*", "Vector2* size, ImGuiViewport* viewport"));

            CsCodeGenerator.Generate(compilation, "../../../../Hexa.NET.ImGui/Generated");

            return 0;
        }
    }
}