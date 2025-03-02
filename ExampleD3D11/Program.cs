namespace ExampleD3D11
{
    using ExampleFramework;
    using Hexa.NET.ImGui;
    using System.Runtime.InteropServices;

    internal unsafe class Program
    {
        private static void Main(string[] args)
        {
            App.Init(Backend.DirectX);
            App.Run(new DX11Window());
        }
    }
}