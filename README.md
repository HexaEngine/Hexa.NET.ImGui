# Hexa.NET.ImGui 

Welcome to Hexa.NET.ImGui! This custom wrapper is designed to be a high-performance, API-compatible alternative to ImGuiNET, offering enhanced speed, additional functionality, and comprehensive access to ImGui's internal structures. With optimizations that bring near C performance and significantly reduced startup times, Hexa.NET.ImGui provides the best of both worlds: the power of C and the productivity of C#.

## Features

- **Comprehensive Wrapper**: Integrates the core Dear ImGui library along with essential addons such as ImGuizmo, ImNodes, and ImPlot.
- **Multi Viewport Support**: Enables seamless multi-viewport rendering for advanced UI scenarios.
- **Active Development**: Regular updates and improvements to ensure compatibility with the latest Dear ImGui features and .NET advancements.
- **Trustworthy Builds**: Native libraries are built using GitHub Actions for added trustworthiness and can be found [here](https://github.com/HexaEngine/Hexa.NET.ImGui/actions).
- **Open Source**: The source code for the native libraries is public and can be reviewed by anyone.
- **Access to Internals**: Allows users to access Dear ImGui internals for advanced customization and functionality.
- **Drop in replacement for ImGuiNET** Adapt to the library with minimal effort by simply changing the namespace.
- **High performance** Using a static VTable, all API calls are faster and startup time is reduced.
- **Optimized String Handling**: Overloads that bypass UTF-8 encoding and avoid allocations.

## Packages

Hexa.NET.ImGui is divided into four different packages to provide modularity and flexibility. You can choose to install only the packages you need:

- [Hexa.NET.ImGui](https://www.nuget.org/packages/Hexa.NET.ImGui/)
- [Hexa.NET.ImGuizmo](https://www.nuget.org/packages/Hexa.NET.ImGuizmo/)
- [Hexa.NET.ImNodes](https://www.nuget.org/packages/Hexa.NET.ImNodes/)
- [Hexa.NET.ImPlot](https://www.nuget.org/packages/Hexa.NET.ImPlot/)

## Getting Started

To get started with Hexa.NET.ImGui, follow these steps:

1. **Install the NuGet packages**:

    For the core library:
    ```bash
    dotnet add package Hexa.NET.ImGui
    ```

    For ImGuizmo addon:
    ```bash
    dotnet add package Hexa.NET.ImGuizmo
    ```

    For ImNodes addon:
    ```bash
    dotnet add package Hexa.NET.ImNodes
    ```

    For ImPlot addon:
    ```bash
    dotnet add package Hexa.NET.ImPlot
    ```

2. **Initialize the library** in your project:
    ```csharp
    using Hexa.NET.ImGui;
    // Your initialization code here
    ```

3. **Explore the demos** to see the library in action.

### Usage Example

For a comprehensive example of how to use the library, refer to the [ExampleD3D11 project](https://github.com/HexaEngine/Hexa.NET.ImGui/blob/master/ExampleD3D11/).

### Setup Guide

For details on how to set up the library, check the [ImGuiManager.cs](https://github.com/HexaEngine/Hexa.NET.ImGui/blob/master/ExampleD3D11/ImGuiDemo/ImGuiManager.cs) file in the ExampleD3D11 project.

### Using the Flexible and Optimized API

Hexa.NET.ImGui supports both safe and unsafe API calls, along with optimized string handling to bypass UTF-8 encoding and avoid allocations. Here are some examples:
    
 1. Safe API Call:
    
```cs
ImGui.Text("A normal C# string");
```

 2. Unsafe API Call:
```cs
unsafe
{
    byte* pText = (byte*)Marshal.StringToHGlobalAnsi("A string from an unsafe pointer").ToPointer();
    ImGui.Text(pText);
    Marshal.FreeHGlobal((IntPtr)pText);
}
```

 3. Optimized String Handling:
```cs
ImGui.Text("A C# string"u8);
```
**This overload bypasses UTF-8 encoding and avoids allocations, providing a highly optimized way to render text.**



## Projects Using Hexa.NET.ImGui

[HexaEngine](https://github.com/HexaEngine/HexaEngine)

<img src="https://github.com/user-attachments/assets/b54145fe-5bd5-4998-b36f-24efe8345aba" alt="HexaEngine Editor" width="400"/>

## Screenshots

### Main Interface
![Screenshot 2023-05-23 163105](https://github.com/JunaMeinhold/HexaEngine.ImGui/assets/46632782/e15288c5-e0f1-4feb-8589-abd2ca92fffb)

### Multi Viewport Support
![Screenshot 2023-07-07 153108](https://github.com/JunaMeinhold/HexaEngine.ImGui/assets/46632782/efb715f8-2dee-4bd2-8fa5-d1bc2195129a)

## Contributing

Contributions are welcome! If you have ideas for improvements or new features, feel free to submit a pull request or open an issue.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
