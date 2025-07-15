namespace ExampleD3D11
{
    using ExampleFramework;
    using Hexa.NET.ImGui;
    using System.Runtime.InteropServices;

    // This example project and the other C# backends are deprecated and will not receive updates
    // Look at ExampleGFWLD3D11 for an example derived directly from the ImGui backends
    [Obsolete("The C# backends are no longer maintained")]
    internal unsafe class Program
    {
        private static void Main(string[] args)
        {
            App.Init(Backend.DirectX);
            App.Run(new DX11Window());
        }
    }
}