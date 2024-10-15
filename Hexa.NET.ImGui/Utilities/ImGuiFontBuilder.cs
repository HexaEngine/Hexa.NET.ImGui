namespace Hexa.NET.ImGui.Utilities
{
    using Hexa.NET.ImGui;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public unsafe struct GlyphRanges
    {
        private char* glyphs;

        public GlyphRanges(params char[] glyphs)
        {
            int length = glyphs.Length;
            if (glyphs[length - 1] != '\0')
            {
                length += 1;
            }
            this.glyphs = (char*)Marshal.AllocHGlobal(sizeof(char) * length);
            for (int i = 0; i < glyphs.Length; i++)
            {
                this.glyphs[i] = glyphs[i];
            }
            this.glyphs[length - 1] = '\0';
        }

        public GlyphRanges(ReadOnlySpan<char> glyphs)
        {
            int length = glyphs.Length;
            if (glyphs[length - 1] != '\0')
            {
                length += 1;
            }
            this.glyphs = (char*)Marshal.AllocHGlobal(sizeof(char) * length);
            for (int i = 0; i < glyphs.Length; i++)
            {
                this.glyphs[i] = glyphs[i];
            }
            this.glyphs[length - 1] = '\0';
        }

        public readonly char* GetRanges()
        {
            return glyphs;
        }

        public void Release()
        {
            if (glyphs != null)
            {
                Marshal.FreeHGlobal((nint)glyphs);
                glyphs = null;
            }
        }
    }

    public unsafe class ImGuiFontBuilder
    {
        private ImFontAtlasPtr fontAtlas;
        private ImFontConfigPtr config;
        private ImFontPtr font;
        private readonly List<GlyphRanges> ranges = [];

        public ImGuiFontBuilder(ImFontAtlasPtr fontAtlasPtr)
        {
            config = ImGui.ImFontConfig();
            config.FontDataOwnedByAtlas = false;
            fontAtlas = fontAtlasPtr;
        }

        public ImFontConfigPtr Config => config;

        public ImFontPtr Font => font;

        public ImGuiFontBuilder SetOption(Action<ImFontConfigPtr> action)
        {
            action(config);
            return this;
        }

        public ImGuiFontBuilder AddDefaultFont()
        {
            font = fontAtlas.AddFontDefault();
            config.MergeMode = true;
            return this;
        }

        public ImGuiFontBuilder AddFontFromFileTTF(string path, float size, GlyphRanges glyphRanges)
        {
            ranges.Add(glyphRanges);
            return AddFontFromFileTTF(path, size, glyphRanges.GetRanges());
        }

        public ImGuiFontBuilder AddFontFromFileTTF(string path, float size, ReadOnlySpan<char> glyphRanges)
        {
            return AddFontFromFileTTF(path, size, new GlyphRanges(glyphRanges));
        }

        public ImGuiFontBuilder AddFontFromFileTTF(string path, float size, char* glyphRanges)
        {
            font = fontAtlas.AddFontFromFileTTF(Path.GetFullPath(path), size, config, glyphRanges);
            config.MergeMode = true;
            return this;
        }

        public ImGuiFontBuilder AddFontFromFileTTF(string path, float size)
        {
            var fullpath = Path.GetFullPath(path);
            bool exists = File.Exists(fullpath);
            if (!exists)
            {
                throw new FileNotFoundException($"Font file not found: {fullpath}");
            }
            font = fontAtlas.AddFontFromFileTTF(fullpath, size, config);
            config.MergeMode = true;
            return this;
        }

        public ImGuiFontBuilder AddFontFromEmbeddedResource(string path, float size)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path) ?? throw new FileNotFoundException($"Embedded resource not found: {path}");
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            fixed (byte* pFontData = buffer)
            {
                font = fontAtlas.AddFontFromMemoryTTF(pFontData, buffer.Length, size, config);
                config.MergeMode = true;
            }
            return this;
        }

        public ImGuiFontBuilder AddFontFromEmbeddedResource(string path, float size, ReadOnlySpan<char> glyphRanges)
        {
            return AddFontFromEmbeddedResource(path, size, new GlyphRanges(glyphRanges));
        }

        public ImGuiFontBuilder AddFontFromEmbeddedResource(string path, float size, GlyphRanges glyphRanges)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path) ?? throw new FileNotFoundException($"Embedded resource not found: {path}");
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            ranges.Add(glyphRanges);
            fixed (byte* pFontData = buffer)
            {
                font = fontAtlas.AddFontFromMemoryTTF(pFontData, buffer.Length, size, config, glyphRanges.GetRanges());
                config.MergeMode = true;
            }
            return this;
        }

        public ImGuiFontBuilder AddFontFromMemoryTTF(byte* fontData, int fontDataSize, float size)
        {
            // IMPORTANT: AddFontFromMemoryTTF() by default transfer ownership of the data buffer to the font atlas, which will attempt to free it on destruction.
            // This was to avoid an unnecessary copy, and is perhaps not a good API (a future version will redesign it).
            font = fontAtlas.AddFontFromMemoryTTF(fontData, fontDataSize, size, config);
            config.MergeMode = true;
            return this;
        }

        public ImGuiFontBuilder AddFontFromMemoryTTF(ReadOnlySpan<byte> fontData, float size, ReadOnlySpan<char> glyphRanges)
        {
            fixed (byte* pFontData = fontData)
            {
                fixed (char* pGlyphRanges = glyphRanges)
                {
                    return AddFontFromMemoryTTF(pFontData, fontData.Length, size, pGlyphRanges);
                }
            }
        }

        public ImGuiFontBuilder AddFontFromMemoryTTF(byte* fontData, int fontDataSize, float size, ReadOnlySpan<char> glyphRanges)
        {
            fixed (char* pGlyphRanges = glyphRanges)
                return AddFontFromMemoryTTF(fontData, fontDataSize, size, pGlyphRanges);
        }

        public ImGuiFontBuilder AddFontFromMemoryTTF(byte* fontData, int fontDataSize, float size, char* pGlyphRanges)
        {
            // IMPORTANT: AddFontFromMemoryTTF() by default transfer ownership of the data buffer to the font atlas, which will attempt to free it on destruction.
            // This was to avoid an unnecessary copy, and is perhaps not a good API (a future version will redesign it).
            font = fontAtlas.AddFontFromMemoryTTF(fontData, fontDataSize, size, config, pGlyphRanges);

            return this;
        }

        public void Destroy()
        {
            for (int i = 0; i < ranges.Count; i++)
            {
                ranges[i].Release();
            }
            config.Destroy();
            config = default;
            fontAtlas = default;
        }
    }
}