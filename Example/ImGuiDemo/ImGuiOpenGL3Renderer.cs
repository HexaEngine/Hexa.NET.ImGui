namespace Example.ImGuiDemo
{
    using Silk.NET.OpenGL;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public unsafe class ImGuiOpenGL3Renderer
    {
        private struct ImGui_ImplOpenGL3_Data
        {
            public uint GlVersion;               // Extracted at runtime using GL_MAJOR_VERSION, GL_MINOR_VERSION queries (e.g. 320 for GL 3.2)
            public byte* GlslVersionString;     // Specified by user or detected based on compile time GL settings.
            public bool GlProfileIsES2;
            public bool GlProfileIsES3;
            public bool GlProfileIsCompat;
            public int GlProfileMask;
            public uint FontTexture;
            public uint ShaderHandle;
            public int AttribLocationTex;       // Uniforms location
            public int AttribLocationProjMtx;
            public uint AttribLocationVtxPos;    // Vertex attributes location
            public uint AttribLocationVtxUV;
            public uint AttribLocationVtxColor;
            public uint VboHandle, ElementsHandle;
            public uint VertexBufferSize;
            public uint IndexBufferSize;
            public bool HasClipOrigin;
            public bool UseBufferSubData;
        };

        public ImGuiOpenGL3Renderer()
        {
        }
    }
}