namespace Generator
{
    using HexaGen.Metadata;
    using HexaGen.Patching;

    public class ImGuiImGuiDataTypePrivatePatch : PostPatch
    {
        private const string ImGuiOutputPath = "../../../../Hexa.NET.ImGui/Generated/";

        public override void Apply(PatchContext context, CsCodeGeneratorMetadata metadata, List<string> files)
        {
            if (metadata.Settings.LibName != "cimgui")
            {
                return;
            }

            // Fix for generator artifact in ImGuiDataTypePrivate.cs
            // Where:
            // Pointer = unchecked((int)Count),
            // but should be
            // Pointer = unchecked((int)ImGuiDataType.Count),

            var enumPath = Path.Combine(ImGuiOutputPath, "Enums/ImGuiDataTypePrivate.cs");
            if (File.Exists(enumPath))
            {
                File.WriteAllText(enumPath, File.ReadAllText(enumPath).Replace("unchecked((int)Count)", "unchecked((int)ImGuiDataType.Count)"));
            }
        }
    }
}