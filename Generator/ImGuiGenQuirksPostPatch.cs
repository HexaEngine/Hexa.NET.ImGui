namespace Generator
{
    using HexaGen.Metadata;
    using HexaGen.Patching;
    using System.Text;

    public class ImGuiGenQuirksPostPatch : PostPatch
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
            FilePatch.Create(enumPath)
                .Replace("unchecked((int)Count)", "unchecked((int)ImGuiDataType.Count)")
                .Done();

            // Fix for generator artifact in ImDrawData.cs
            // Where:
            // ImVector<ImTextureDataPtr>Ptr
            // but should be
            // ImVector<ImTextureDataPtr>
            //
            // Also:
            // ref Unsafe.AsRef<ImVector<ImTextureDataPtr>Ptr>(&Handle->Textures)
            // but should be
            // ref Unsafe.AsRef<ImVector<ImTextureDataPtr>>(Handle->Textures)

            var imDrawDataPath = Path.Combine(ImGuiOutputPath, "Structs/ImDrawData.cs");
            FilePatch.Create(imDrawDataPath)
                .Replace("ref Unsafe.AsRef<ImVector<ImTextureDataPtr>Ptr>(&Handle->Textures)", "ref Unsafe.AsRef<ImVector<ImTextureDataPtr>>(Handle->Textures)")
                .Replace("ImVector<ImTextureDataPtr>Ptr", "ImVector<ImTextureDataPtr>")
                .Done();
        }
    }

    public ref struct FilePatch
    {
        string? path;
        StringBuilder? content;

        public static FilePatch Create(string path)
        {
            return new FilePatch().WithTarget(path);
        }

        public FilePatch WithTarget(string path)
        {
            this.path = path;
            content = null!;
            if (File.Exists(path))
            {
                content = new(File.ReadAllText(path));
            }
            return this;
        }

        public readonly FilePatch Replace(string target, string replacement)
        {
            content?.Replace(target, replacement);
            return this;
        }

        public FilePatch Done()
        {
            if (path == null || content == null) return this;
            File.WriteAllText(path, content.ToString());
            path = null;
            content = null;
            return this;
        }
    }
}