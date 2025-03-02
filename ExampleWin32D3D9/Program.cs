// See https://aka.ms/new-console-template for more information
using ExampleWin32D3D9;
using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.D3D9;
using Hexa.NET.ImGui.Backends.Win32;
using HexaGen.Runtime;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D9;
using Silk.NET.Maths;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using IDirect3DDevice9 = Silk.NET.Direct3D9.IDirect3DDevice9;

nint hInstance = Win32.GetModuleHandle(null);
nint hWnd;
unsafe
{
    WNDCLASS wndClass = new()
    {
        style = 0x0040,
        lpfnWndProc = Marshal.GetFunctionPointerForDelegate<WindowProcCallback>(WindowProc),
        cbClsExtra = 0,
        cbWndExtra = 0,
        hInstance = hInstance,
        hIcon = nint.Zero,
        hCursor = nint.Zero,
        hbrBackground = 1,
        lpszMenuName = null,
        lpszClassName = Utils.StringToUTF16Ptr("ImGui Example")
    };

    if (Win32.RegisterClassA(ref wndClass) == 0)
    {
        Console.WriteLine("Failed to register window class. Error: " + Marshal.GetLastWin32Error());
        return;
    }

    byte* title = Utils.StringToUTF8Ptr("Dear ImGui DirectX9 Example");

    hWnd = Win32.CreateWindowEx(
       0,
       wndClass.lpszClassName,
       title,
       Win32.WS_OVERLAPPEDWINDOW,
       100, 100,
       1200, 800,
       nint.Zero,
       nint.Zero,
       hInstance,
       nint.Zero);

    Utils.Free(title);
    Utils.Free(wndClass.lpszClassName);

    if (hWnd == nint.Zero)
    {
        int error = Marshal.GetLastWin32Error();
        Console.WriteLine("Failed to create window. Error code: " + error);
        return;
    }
}

Win32.ShowWindow(hWnd, Win32.SW_SHOW);
Win32.UpdateWindow(hWnd);

var D3D9 = Silk.NET.Direct3D9.D3D9.GetApi();

var context = ImGui.CreateContext();

var io = ImGui.GetIO();

ImGuiImplWin32.SetCurrentContext(context);
ImGuiImplWin32.Init(hWnd);
PresentParameters d3dpp = default;
ComPtr<IDirect3D9> pD3D = null;
ComPtr<IDirect3DDevice9> pD3DDevice = null;
unsafe
{
    pD3D = D3D9.Direct3DCreate9(Silk.NET.Direct3D9.D3D9.SdkVersion);

    d3dpp.Windowed = true;                      // Run in windowed mode
    d3dpp.SwapEffect = Swapeffect.Discard;   // Discard old frames
    d3dpp.BackBufferFormat = Format.Unknown;    // Use current display format
    d3dpp.EnableAutoDepthStencil = true;
    d3dpp.AutoDepthStencilFormat = Format.D16;
    d3dpp.PresentationInterval = Silk.NET.Direct3D9.D3D9.PresentIntervalOne;

    // Create the Direct3D device

    HResult hr = pD3D.CreateDevice(
        Silk.NET.Direct3D9.D3D9.AdapterDefault,      // Use the primary display adapter
        Devtype.Hal,          // Use hardware rasterization
        hWnd,      // Handle to the window
        Silk.NET.Direct3D9.D3D9.CreateHardwareVertexprocessing,  // Use software vertex processing
        ref d3dpp,                  // Presentation parameters
        ref pD3DDevice              // Pointer to the created device
    );

    ImGuiImplD3D9.SetCurrentContext(context);
    ImGuiImplD3D9.Init(new((Hexa.NET.ImGui.Backends.D3D9.IDirect3DDevice9*)pD3DDevice.Handle));
}

bool running = true;

bool g_DeviceLost = false;
uint g_ResizeWidth = 0, g_ResizeHeight = 0;
MSG msg = default;

Vector4 clear_color = new(1, 0.8f, 0.75f, 1);
unsafe
{
    while (running)
    {
        while (Win32.PeekMessage(ref msg, 0, 0U, 0U, Win32.PM_REMOVE))
        {
            Win32.TranslateMessage(ref msg);
            Win32.DispatchMessage(ref msg);
            if (msg.message == Win32.WM_QUIT)
                running = false;
        }

        if (g_DeviceLost)
        {
            ResultCode hr = (ResultCode)pD3DDevice.TestCooperativeLevel();
            if (hr == ResultCode.D3DERR_DEVICELOST)
            {
                Thread.Sleep(10);
                continue;
            }
            if (hr == ResultCode.D3DERR_DEVICENOTRESET)
                ResetDevice();
            g_DeviceLost = false;
        }

        if (g_ResizeWidth != 0 && g_ResizeHeight != 0)
        {
            d3dpp.BackBufferWidth = g_ResizeWidth;
            d3dpp.BackBufferHeight = g_ResizeHeight;
            g_ResizeWidth = g_ResizeHeight = 0;
            ResetDevice();
        }

        ImGuiImplD3D9.NewFrame();
        ImGuiImplWin32.NewFrame();
        ImGui.NewFrame();

        ImGui.ShowDemoWindow();

        ImGui.EndFrame();

        pD3DDevice.SetRenderState(Renderstatetype.Zenable, 0);
        pD3DDevice.SetRenderState(Renderstatetype.Alphablendenable, 0);
        pD3DDevice.SetRenderState(Renderstatetype.Scissortestenable, 0);
        uint clear_col_dx = (uint)(
            ((int)(clear_color.W * 255.0f) << 24) |  // Alpha
            ((int)(clear_color.X * clear_color.W * 255.0f) << 16) |  // Red
            ((int)(clear_color.Y * clear_color.W * 255.0f) << 8) |   // Green
            ((int)(clear_color.Z * clear_color.W * 255.0f))          // Blue
        );
        pD3DDevice.Clear(0, (Rect*)null, Silk.NET.Direct3D9.D3D9.ClearTarget | Silk.NET.Direct3D9.D3D9.ClearZbuffer, clear_col_dx, 1.0f, 0);
        if (pD3DDevice.BeginScene() >= 0)
        {
            ImGui.Render();
            ImGuiImplD3D9.RenderDrawData(ImGui.GetDrawData());
            pD3DDevice.EndScene();
        }

        ResultCode result = (ResultCode)pD3DDevice.Present((Box2D<int>*)null, (Box2D<int>*)null, 0, (RGNData*)null);
        if (result == ResultCode.D3DERR_DEVICELOST)
            g_DeviceLost = true;
    }
}

ImGuiImplD3D9.Shutdown();
ImGuiImplWin32.Shutdown();
ImGui.DestroyContext();

pD3DDevice.Release();
pD3D.Release();

Win32.DestroyWindow(hWnd);

nint WindowProc(nint hWnd, uint msg, nuint wParam, nint lParam)
{
    if (ImGuiImplWin32.WndProcHandler(hWnd, msg, wParam, lParam) != 0)
    {
        return 1;
    }

    const uint WM_SIZE = 0x0005;
    const uint SIZE_MINIMIZED = 1;
    const uint WM_SYSCOMMAND = 0x0112;
    const uint SC_KEYMENU = 0xF100;
    const uint WM_DESTROY = 0x0002;

    switch (msg)
    {
        case WM_SIZE:
            if (wParam == SIZE_MINIMIZED)
                return 0;
            g_ResizeWidth = (uint)(lParam & 0xFFFF); // Queue resize
            g_ResizeHeight = (uint)((lParam >> 16) & 0xFFFF);
            return 0;

        case WM_SYSCOMMAND:
            if ((wParam & 0xfff0) == SC_KEYMENU) // Disable ALT application menu
                return 0;
            break;

        case WM_DESTROY:
            Environment.Exit(0);
            return 0;
    }

    return Win32.DefWindowProc(hWnd, msg, wParam, lParam);
}

unsafe void ResetDevice()
{
    ImGuiImplD3D9.InvalidateDeviceObjects();
    ResultCode hr = (ResultCode)pD3DDevice.Reset(ref d3dpp);
    if (hr == ResultCode.D3DERR_INVALIDCALL)
        Trace.Assert(false);
    ImGuiImplD3D9.CreateDeviceObjects();
}