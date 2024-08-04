namespace Generator
{
    using System.Text;
    using System.Text.RegularExpressions;

    internal unsafe class Program
    {
        private const string CImGuiConfig = "cimgui/generator.json";
        private const string CImGuizmoConfig = "cimguizmo/generator.json";
        private const string CImNodesConfig = "cimnodes/generator.json";
        private const string CImPlotConfig = "cimplot/generator.json";

        private const string CImGuiManualConfig = "cimgui/generator.manual.json";

        private const string CImGuiHeader = "cimgui/cimgui.h";
        private const string CImGuizmoHeader = "cimguizmo/cimguizmo.h";
        private const string CImNodesHeader = "cimnodes/cimnodes.h";
        private const string CImPlotHeader = "cimplot/cimplot.h";

        private const string ImGuiOutputPath = "../../../../Hexa.NET.ImGui/Generated";
        private const string ImGuizmoOutputPath = "../../../../Hexa.NET.ImGuizmo/Generated";
        private const string ImNodesOutputPath = "../../../../Hexa.NET.ImNodes/Generated";
        private const string ImPlotOutputPath = "../../../../Hexa.NET.ImPlot/Generated";

        private static void Main(string[] args)
        {
            PreProcess();

            Generate(CImGuiHeader, CImGuiConfig, ImGuiOutputPath, null, out var metadata);
            Generate(CImGuizmoHeader, CImGuizmoConfig, ImGuizmoOutputPath, metadata, out _);
            Generate(CImPlotHeader, CImPlotConfig, ImPlotOutputPath, metadata, out _);
            Generate(CImNodesHeader, CImNodesConfig, ImNodesOutputPath, metadata, out _);

            GenerateImGuiPatch(metadata);
        }

        private static void PreProcess()
        {
            // Patch ImVector types
            PatchConfigImVector(CImGuiHeader, CImGuiConfig);
            PatchConfigImVector(CImGuizmoHeader, CImGuizmoConfig);
            PatchConfigImVector(CImNodesHeader, CImNodesConfig);
            PatchConfigImVector(CImPlotHeader, CImPlotConfig);
            PatchConfigImVector(CImGuiHeader, CImGuiManualConfig);

            // Merge settings
            MergeConfig(CImGuiConfig, CImGuizmoConfig);
            MergeConfig(CImGuiConfig, CImNodesConfig);
            MergeConfig(CImGuiConfig, CImPlotConfig);

            PatchConfigIgnoreDefinitions("cimgui", CImGuizmoConfig);
            PatchConfigIgnoreDefinitions("cimgui", CImPlotConfig);
            PatchConfigIgnoreDefinitions("cimgui", CImNodesConfig);

            // Patch Definitions
            PatchConfigDefinitions("cimgui", CImGuiConfig);
            PatchConfigDefinitions("cimguizmo", CImGuizmoConfig);
            PatchConfigDefinitions("cimnodes", CImNodesConfig);
            PatchConfigDefinitions("cimplot", CImPlotConfig);
            PatchConfigDefinitions("cimgui", CImGuiManualConfig);

            PatchImGui(CImGuiConfig);
            PatchImGuizmo(CImGuizmoConfig);
        }

        private static void MergeConfig(string from, string into)
        {
            var baseSettings = CsCodeGeneratorSettings.Load(from);
            var patchSettings = CsCodeGeneratorSettings.Load(into);

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

        private static void PatchConfigImVector(string file, string settingFile)
        {
            var settings = CsCodeGeneratorSettings.Load(settingFile);
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

        private static void GenerateImGuiPatch(GeneratorMetadata metadata)
        {
            var settings = CsCodeGeneratorSettings.Load(CImGuiManualConfig);

            settings.DelegateMappings.Add(new("PlatformGetWindowPos", "Vector2*", "Vector2* pos, ImGuiViewport* viewport"));
            settings.DelegateMappings.Add(new("PlatformGetWindowSize", "Vector2*", "Vector2* size, ImGuiViewport* viewport"));

            const string manual = "../../../../Hexa.NET.ImGui/Manual/";
            const string generated = "../../../../Hexa.NET.ImGui/Generated/";

            // Patch VTable
            {
                const string vtTargetFile = generated + "Functions.VT.cs";
                const string vtPatchFile = manual + "Functions.VT.cs";

                var vtContent = File.ReadAllText(vtTargetFile);
                var rootMatch = Regex.Match(vtContent, "vt = new VTable\\(GetLibraryName\\(\\), (.*)\\);");
                int vtBaseIndex = metadata.VTableLength;

                Regex loadPattern = new("vt.Load\\((.*), \"(.*)\"\\);", RegexOptions.Singleline);
                var matches = loadPattern.Matches(vtContent);
                var lastMatch = matches[^1];
                int start = lastMatch.Index + lastMatch.Length;

                settings.VTableStart = vtBaseIndex;

                CsCodeGenerator generator = new(settings);
                generator.Generate(CImGuiHeader, manual);
                var patchMetadata = generator.GetMetadata();

                var content = File.ReadAllText(vtPatchFile);
                File.Delete(vtPatchFile);

                StringBuilder builder = new(vtContent[..(start + 1)]);

                foreach (Match match in loadPattern.Matches(content))
                {
                    builder.AppendLine($"\t\t\t" + match.Value);
                }

                builder.Remove(rootMatch.Index, rootMatch.Length);
                builder.Insert(rootMatch.Index, $"vt = new VTable(GetLibraryName(), {patchMetadata.VTableLength});");

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

        private static bool Generate(string header, string settingsPath, string output, GeneratorMetadata? lib, out GeneratorMetadata metadata)
        {
            CsCodeGenerator generator = new(settingsPath);

            if (lib != null)
            {
                generator.DefineLib(lib);
            }

            bool result = generator.Generate(header, output);
            metadata = generator.GetMetadata();

            return result;
        }

        private static void PatchImGui(string settingsPath)
        {
            var settings = CsCodeGeneratorSettings.Load(settingsPath);
            settings.DelegateMappings.Add(new("PlatformGetWindowPos", "Vector2*", "Vector2* pos, ImGuiViewport* viewport"));
            settings.DelegateMappings.Add(new("PlatformGetWindowSize", "Vector2*", "Vector2* size, ImGuiViewport* viewport"));
            settings.Save(settingsPath);
        }

        private static void PatchImGuizmo(string settingsPath)
        {
            var settings = CsCodeGeneratorSettings.Load(settingsPath);
            settings.TryGetFunctionMapping("ImGuizmo_Manipulate", out FunctionMapping manipulateMapping);
            manipulateMapping.CustomVariations.Add(new()
            {
                { "view", "ref Matrix4x4" },
                { "projection", "ref Matrix4x4" },
                { "matrix", "ref Matrix4x4" },
                { "deltaMatrix", "ref Matrix4x4" }
            });

            settings.TryGetFunctionMapping("ImGuizmo_DecomposeMatrixToComponents", out FunctionMapping decomposeMatrixToComponentsMapping);
            decomposeMatrixToComponentsMapping.CustomVariations.Add(new()
            {
                { "matrix", "ref Matrix4x4" },
                { "translation", "ref Matrix4x4" },
                { "rotation", "ref Matrix4x4" },
                { "scale", "ref Matrix4x4" },
            });

            settings.TryGetFunctionMapping("ImGuizmo_RecomposeMatrixFromComponents", out FunctionMapping recomposeMatrixToComponentsMapping);
            recomposeMatrixToComponentsMapping.CustomVariations.Add(new()
            {
                { "translation", "ref Matrix4x4" },
                { "rotation", "ref Matrix4x4" },
                { "scale", "ref Matrix4x4" },
                { "matrix", "ref Matrix4x4" },
            });

            settings.TryGetFunctionMapping("ImGuizmo_DrawCubes", out FunctionMapping drawCubesMapping);
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

            settings.TryGetFunctionMapping("ImGuizmo_DrawGrid", out FunctionMapping drawGridMapping);
            drawGridMapping.CustomVariations.Add(new()
            {
                { "view", "ref Matrix4x4" },
                { "projection", "ref Matrix4x4" },
                { "matrix", "ref Matrix4x4" },
            });

            settings.TryGetFunctionMapping("ImGuizmo_ViewManipulate_Float", out FunctionMapping viewManipulateFloatMapping);
            viewManipulateFloatMapping.CustomVariations.Add(new()
            {
                { "view", "ref Matrix4x4" },
            });

            settings.TryGetFunctionMapping("ImGuizmo_ViewManipulate_FloatPtr", out FunctionMapping viewManipulateFloatPtrMapping);
            viewManipulateFloatPtrMapping.CustomVariations.Add(new()
            {
                { "view", "ref Matrix4x4" },
                { "projection", "ref Matrix4x4" },
                { "matrix", "ref Matrix4x4" },
            });

            settings.Save(settingsPath);
        }

        private static void PatchConfigDefinitions(string name, string settingsPath)
        {
            var settings = CsCodeGeneratorSettings.Load(settingsPath);

            ImguiDefinitions implotDefinitions = new();
            implotDefinitions.LoadFrom(name);

            for (int i = 0; i < implotDefinitions.Functions.Length; i++)
            {
                var functionDefinition = implotDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    var overload = functionDefinition.Overloads[j];
                    settings.FunctionMappings.Add(new(overload.ExportedName, overload.FriendlyName, overload.DefaultValues, new()));

                    if (overload.IsMemberFunction && !overload.IsConstructor)
                    {
                        if (settings.KnownMemberFunctions.TryGetValue(overload.StructName, out var knownFunctions))
                        {
                            if (!knownFunctions.Contains(overload.ExportedName))
                                knownFunctions.Add(overload.ExportedName);
                        }
                        else
                        {
                            settings.KnownMemberFunctions.Add(overload.StructName, new List<string>() { overload.ExportedName });
                        }
                    }
                }
            }

            settings.Save(settingsPath);
        }

        private static void PatchConfigIgnoreDefinitions(string name, string settingsPath)
        {
            var settings = CsCodeGeneratorSettings.Load(settingsPath);

            ImguiDefinitions imguiDefinitions = new();
            imguiDefinitions.LoadFrom(name);

            for (int i = 0; i < imguiDefinitions.Functions.Length; i++)
            {
                var functionDefinition = imguiDefinitions.Functions[i];
                for (int j = 0; j < functionDefinition.Overloads.Length; j++)
                {
                    settings.IgnoredFunctions.Add(functionDefinition.Overloads[j].ExportedName);
                }
            }
            for (int i = 0; i < imguiDefinitions.Enums.Length; i++)
            {
                var enumDefinition = imguiDefinitions.Enums[i];
                settings.IgnoredEnums.Add(enumDefinition.Names[0]);
            }
            for (int i = 0; i < imguiDefinitions.Types.Length; i++)
            {
                var typeDefinition = imguiDefinitions.Types[i];
                settings.IgnoredTypes.Add(typeDefinition.Name);
            }
            for (int i = 0; i < imguiDefinitions.Typedefs.Length; i++)
            {
                var typedefDefinition = imguiDefinitions.Typedefs[i];
                settings.IgnoredTypedefs.Add(typedefDefinition.Name);
                if (typedefDefinition.IsStruct)
                {
                    settings.IgnoredTypes.Add(typedefDefinition.Name);
                }
            }

            settings.Save(settingsPath);
        }
    }
}