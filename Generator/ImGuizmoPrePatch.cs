namespace Generator
{
    using HexaGen;
    using HexaGen.Core.Mapping;
    using HexaGen.Patching;

    public class ImGuizmoPrePatch : PrePatch
    {
        protected override void PatchCompilation(CsCodeGeneratorConfig settings, ParseResult parseResult)
        {
            if (settings.LibName != "cimguizmo")
            {
                return;
            }

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
        }
    }
}