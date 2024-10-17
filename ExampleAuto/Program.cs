using ExampleD3D11;
using ExampleFramework;
using ExampleOpenGL3;

if (OperatingSystem.IsWindows())
{
    App.Init(Backend.DirectX);
    App.Run(new DX11Window());
}
else
{
    App.Init(Backend.OpenGL);
    App.Run(new OpenGLWindow());
}