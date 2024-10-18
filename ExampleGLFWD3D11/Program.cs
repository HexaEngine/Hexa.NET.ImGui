// See https://aka.ms/new-console-template for more information
using ExampleGLFWD3D11;
using Hexa.NET.GLFW;
using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.D3D11;
using Hexa.NET.ImGui.Backends.GLFW;
using Hexa.NET.ImGui.Utilities;
using Hexa.NET.ImGui.Widgets;
using Hexa.NET.ImGui.Widgets.Dialogs;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using System.Runtime.CompilerServices;
using GLFWwindowPtr = Hexa.NET.GLFW.GLFWwindowPtr;

GLFW.Init();

GLFW.WindowHint(GLFW.GLFW_CLIENT_API, GLFW.GLFW_NO_API);

GLFW.WindowHint(GLFW.GLFW_FOCUSED, 1);    // Make window focused on start
GLFW.WindowHint(GLFW.GLFW_RESIZABLE, 1);  // Make window resizable

GLFWwindowPtr window = GLFW.CreateWindow(800, 600, "GLFW Example", null, null);
if (window.IsNull)
{
    Console.WriteLine("Failed to create GLFW window.");
    GLFW.Terminate();
    return;
}

D3D11Manager manager = new(window, false);

var guiContext = ImGui.CreateContext();
ImGui.SetCurrentContext(guiContext);

// Setup ImGui config.
var io = ImGui.GetIO();
io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;     // Enable Keyboard Controls
io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;      // Enable Gamepad Controls
io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;         // Enable Docking
io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;       // Enable Multi-Viewport / Platform Windows
io.ConfigViewportsNoAutoMerge = false;
io.ConfigViewportsNoTaskBarIcon = false;

// OPTIONAL: For custom fonts and icon fonts.
char[] range = [(char)0xe003, (char)0xF8FF];
ImGuiFontBuilder builder = new(io.Fonts);
builder
    .AddDefaultFont()
    .SetOption(c => { c.GlyphMinAdvanceX = 18; c.GlyphOffset = new(0, 4); })
    .AddFontFromFileTTF("MaterialSymbolsRounded.ttf", 14, range);

ImGuiImplGLFW.SetCurrentContext(guiContext);
if (!ImGuiImplGLFW.InitForOther(Unsafe.BitCast<GLFWwindowPtr, Hexa.NET.ImGui.Backends.GLFW.GLFWwindowPtr>(window), true))
{
    Console.WriteLine("Failed to init ImGui Impl GLFW");
    GLFW.Terminate();
    return;
}

ImGuiImplD3D11.SetCurrentContext(guiContext);
if (!ImGuiImplD3D11.Init(Unsafe.BitCast<ComPtr<ID3D11Device1>, ID3D11DevicePtr>(manager.Device), Unsafe.BitCast<ComPtr<ID3D11DeviceContext1>, ID3D11DeviceContextPtr>(manager.DeviceContext)))
{
    Console.WriteLine("Failed to init ImGui Impl D3D11");
    GLFW.Terminate();
    return;
}

// OPTIONAL: For the widgets framework which makes using ImGui easier in C# with classes.
WidgetManager.Init();
OpenFileDialog dialog = new();
dialog.Show();

// Setup resizing.
unsafe
{
    GLFW.SetFramebufferSizeCallback(window, Resized);

    unsafe void Resized(Hexa.NET.GLFW.GLFWwindow* window, int width, int height)
    {
        manager.Resize(width, height);
    }
}

// Main loop
while (GLFW.WindowShouldClose(window) == 0)
{
    // Poll for and process events
    GLFW.PollEvents();

    if (GLFW.GetKey(window, (int)GlfwKey.Escape) == GLFW.GLFW_PRESS)
    {
        GLFW.SetWindowShouldClose(window, 1); // Request to close the window
    }

    manager.Clear(new(1, 0.8f, 0.75f, 1));

    ImGuiImplD3D11.NewFrame();
    ImGuiImplGLFW.NewFrame();
    ImGui.NewFrame();

    ImGui.ShowDemoWindow();

    // OPTIONAL: For the widgets framework which makes using ImGui easier in C# with classes.
    WidgetManager.Draw();

    ImGui.Render();
    ImGui.EndFrame();

    manager.SetTarget();
    ImGuiImplD3D11.RenderDrawData(ImGui.GetDrawData());

    if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
    {
        ImGui.UpdatePlatformWindows();
        ImGui.RenderPlatformWindowsDefault();
    }

    manager.Present(1, 0);
}

ImGuiImplD3D11.Shutdown();
ImGuiImplGLFW.Shutdown();
ImGui.DestroyContext();

manager.Dispose();

// Clean up and terminate GLFW
GLFW.DestroyWindow(window);
GLFW.Terminate();