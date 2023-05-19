namespace HexaEngine.Core.Graphics
{
    using System.Collections.Generic;

    public static class ShaderCompilers
    {
        private static Dictionary<GraphicsBackend, IShaderCompiler> compilers = new();

        public static void Register(GraphicsBackend backend, IShaderCompiler compiler)
        {
            compilers.Add(backend, compiler);
        }

        public static IShaderCompiler GetShaderCompiler(GraphicsBackend backend)
        {
            return compilers[backend];
        }
    }
}