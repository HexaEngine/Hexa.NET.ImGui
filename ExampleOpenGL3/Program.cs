namespace ExampleOpenGL3
{
    using ExampleFramework;
    using Hexa.NET.ImGui;

    // This example project and the other C# backends are deprecated and will not receive updates
    // Look at ExampleSDL3OpenGL3 for an example derived directly from the ImGui backends
    [Obsolete("The C# backends are no longer maintained")]
    public unsafe class Program
    {
        private static void Main(string[] args)
        {
            App.Init(Backend.OpenGL);
            App.Run(new OpenGLWindow());
        }
    }
}