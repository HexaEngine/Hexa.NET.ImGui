namespace Generator
{
    using CppAst;

    internal unsafe class Program
    {
        private static void Main(string[] args)
        {
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