namespace ExampleOpenGL3
{
    using ExampleFramework;
    using Hexa.NET.ImGui;

    public unsafe class Program
    {
        private static void Main(string[] args)
        {
            App.Init(Backend.OpenGL);
            App.Run(new OpenGLWindow());
        }
    }
}