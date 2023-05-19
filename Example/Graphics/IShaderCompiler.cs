namespace HexaEngine.Core.Graphics
{
    public interface IShaderCompiler
    {
        bool Compile(string source, ShaderMacro[] macros, string entryPoint, string sourceName, string profile, out Blob? shaderBlob, out Blob? errorBlob);

        unsafe void Compile(string code, ShaderMacro[] macros, string entry, string sourceName, string profile, Shader** shader);

        unsafe void Compile(string code, ShaderMacro[] macros, string entry, string sourceName, string profile, Shader** shader, out Blob? errorBlob);

        unsafe void Compile(string code, string entry, string sourceName, string profile, Shader** shader);

        unsafe void Compile(string code, string entry, string sourceName, string profile, Shader** shader, out Blob? errorBlob);

        unsafe void CompileFromFile(string path, ShaderMacro[] macros, string entry, string profile, Shader** shader);

        unsafe void CompileFromFile(string path, ShaderMacro[] macros, string entry, string profile, Shader** shader, out Blob? errorBlob);

        unsafe void CompileFromFile(string path, string entry, string profile, Shader** shader);

        unsafe void CompileFromFile(string path, string entry, string profile, Shader** shader, out Blob? errorBlob);
    }
}