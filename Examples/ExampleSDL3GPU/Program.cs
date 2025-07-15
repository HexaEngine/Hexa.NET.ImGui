using Hexa.NET.ImGui;
using Hexa.NET.ImGui.Backends.SDL3;
using Hexa.NET.SDL3;
using System.Numerics;
using ImSDLEvent = Hexa.NET.ImGui.Backends.SDL3.SDLEvent;
using ImSDLWindow = Hexa.NET.ImGui.Backends.SDL3.SDLWindow;
using SDLWindow = Hexa.NET.SDL3.SDLWindow;
using SDLEvent = Hexa.NET.SDL3.SDLEvent;
using SDLGPUDevice = Hexa.NET.SDL3.SDLGPUDevice;
using ImSDLGPUDevice = Hexa.NET.ImGui.Backends.SDL3.SDLGPUDevice;
using SDLGPUCommandBuffer = Hexa.NET.SDL3.SDLGPUCommandBuffer;
using ImSDLGPUCommandBuffer = Hexa.NET.ImGui.Backends.SDL3.SDLGPUCommandBuffer;
using SDLGPURenderPass = Hexa.NET.SDL3.SDLGPURenderPass;
using ImSDLGPURenderPass = Hexa.NET.ImGui.Backends.SDL3.SDLGPURenderPass;

unsafe
{
    if (!SDL.Init(SDLInitFlags.Video | SDLInitFlags.Gamepad))
    {
        Console.WriteLine($"Error: SDL_Init(): {SDL.GetErrorS()}");
        return;
    }

    float mainScale = SDL.GetDisplayContentScale(SDL.GetPrimaryDisplay());
    var windowFlags = SDLWindowFlags.Resizable | SDLWindowFlags.Hidden | SDLWindowFlags.HighPixelDensity;
    SDLWindow* window = SDL.CreateWindow("Dear ImGui SDL3+SDL_GPU example",
        (int)(1280 * mainScale), (int)(720 * mainScale), windowFlags);
    if (window == null)
    {
        Console.WriteLine($"Error: SDL_CreateWindow(): {SDL.GetErrorS()}");
        return;
    }

    SDL.SetWindowPosition(window, (int)SDL.SDL_WINDOWPOS_CENTERED_MASK, (int)SDL.SDL_WINDOWPOS_CENTERED_MASK);
    SDL.ShowWindow(window);

    SDLGPUDevice* gpuDevice = SDL.CreateGPUDevice(
        SDLGPUShaderFormat.Spirv | SDLGPUShaderFormat.Dxil | SDLGPUShaderFormat.Metallib,
        true, (byte*)null);
    if (gpuDevice == null)
    {
        Console.WriteLine($"Error: SDL_CreateGPUDevice(): {SDL.GetErrorS()}");
        return;
    }

    if (!SDL.ClaimWindowForGPUDevice(gpuDevice, window))
    {
        Console.WriteLine($"Error: SDL_ClaimWindowForGPUDevice(): {SDL.GetErrorS()}");
        return;
    }

    SDL.SetGPUSwapchainParameters(gpuDevice, window,
        SDLGPUSwapchainComposition.Sdr, SDLGPUPresentMode.Mailbox);

    var ctx = ImGui.CreateContext();
    ImGui.SetCurrentContext(ctx);
    ImGuiIOPtr io = ImGui.GetIO();
    io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard
                    | ImGuiConfigFlags.NavEnableGamepad
                    | ImGuiConfigFlags.DockingEnable
                    | ImGuiConfigFlags.ViewportsEnable;

    ImGui.StyleColorsDark();
    var style = ImGui.GetStyle();
    style.ScaleAllSizes(mainScale);
    style.FontScaleDpi = mainScale;
    io.ConfigDpiScaleFonts = true;
    io.ConfigDpiScaleViewports = true;

    if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
    {
        style.WindowRounding = 0.0f;
        style.Colors[(int)ImGuiCol.WindowBg].W = 1.0f;
    }

    ImGuiImplSDL3.SetCurrentContext(ctx);
    ImGuiImplSDL3.InitForSDLGPU((ImSDLWindow*)window);

    ImGuiImplSDLGPU3InitInfo initInfo = new()
    {
        Device = (ImSDLGPUDevice*)gpuDevice,
        ColorTargetFormat = (int)SDL.GetGPUSwapchainTextureFormat(gpuDevice, window),
        MSAASamples = (int)SDLGPUSampleCount.Samplecount1
    };
    ImGuiImplSDL3.SDLGPU3Init(&initInfo);

    bool showDemoWindow = true, showAnotherWindow = false;
    Vector4 clearColor = new(0.45f, 0.55f, 0.60f, 1.00f);

    float f = 0.0f;
    int counter = 0;
    bool done = false;
    while (!done)
    {
        SDLEvent e;
        while (SDL.PollEvent(&e))
        {
            ImGuiImplSDL3.ProcessEvent((ImSDLEvent*)&e);
            var type = (SDLEventType)e.Type;
            if (type == SDLEventType.Quit ||
                (type == SDLEventType.WindowCloseRequested &&
                 e.Window.WindowID == SDL.GetWindowID(window)))
            {
                done = true;
            }
        }

        if ((SDL.GetWindowFlags(window) & SDLWindowFlags.Minimized) != 0)
        {
            SDL.Delay(10);
            continue;
        }

        ImGuiImplSDL3.SDLGPU3NewFrame();
        ImGuiImplSDL3.NewFrame();
        ImGui.NewFrame();

        if (showDemoWindow) ImGui.ShowDemoWindow(ref showDemoWindow);

        ImGui.Begin("Hello, world!");
        ImGui.Text("This is some useful text.");
        ImGui.Checkbox("Demo Window", ref showDemoWindow);
        ImGui.Checkbox("Another Window", ref showAnotherWindow);

        ImGui.SliderFloat("float", ref f, 0.0f, 1.0f);
        ImGui.ColorEdit4("clear color", (float*)&clearColor);
        if (ImGui.Button("Button")) counter++;
        ImGui.SameLine();
        ImGui.Text($"counter = {counter}");
        ImGui.Text($"Application average {1000.0f / io.Framerate:F3} ms/frame ({io.Framerate:F1} FPS)");
        ImGui.End();

        if (showAnotherWindow)
        {
            ImGui.Begin("Another Window", ref showAnotherWindow);
            ImGui.Text("Hello from another window!");
            if (ImGui.Button("Close Me")) showAnotherWindow = false;
            ImGui.End();
        }

        ImGui.Render();
        ImDrawData* drawData = ImGui.GetDrawData();
        bool isMinimized = drawData->DisplaySize.X <= 0 || drawData->DisplaySize.Y <= 0;

        SDLGPUCommandBuffer* commandBuffer = SDL.AcquireGPUCommandBuffer(gpuDevice);
        SDLGPUTexture* swapTexture;
        SDL.AcquireGPUSwapchainTexture(commandBuffer, window, &swapTexture, null, null);

        if (swapTexture != null && !isMinimized)
        {
            ImGuiImplSDL3.SDLGPU3PrepareDrawData(drawData, (ImSDLGPUCommandBuffer*)commandBuffer);

            SDLGPUColorTargetInfo targetInfo = new()
            {
                Texture = swapTexture,
                ClearColor = new SDLFColor
                {
                    R = clearColor.X,
                    G = clearColor.Y,
                    B = clearColor.Z,
                    A = clearColor.W
                },
                LoadOp = SDLGPULoadOp.Clear,
                StoreOp = SDLGPUStoreOp.Store,
                MipLevel = 0,
                LayerOrDepthPlane = 0,
                Cycle = 0
            };

            SDLGPURenderPass* renderPass = SDL.BeginGPURenderPass(commandBuffer, &targetInfo, 1, null);
            ImGuiImplSDL3.SDLGPU3RenderDrawData(drawData, (ImSDLGPUCommandBuffer*)commandBuffer, (ImSDLGPURenderPass*)renderPass, null);
            SDL.EndGPURenderPass(renderPass);
        }

        if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
        {
            ImGui.UpdatePlatformWindows();
            ImGui.RenderPlatformWindowsDefault();
        }

        SDL.SubmitGPUCommandBuffer(commandBuffer);
    }

    SDL.WaitForGPUIdle(gpuDevice);
    ImGuiImplSDL3.Shutdown();
    ImGuiImplSDL3.SDLGPU3Shutdown();
    ImGui.DestroyContext();

    SDL.ReleaseWindowFromGPUDevice(gpuDevice, window);
    SDL.DestroyGPUDevice(gpuDevice);
    SDL.DestroyWindow(window);
    SDL.Quit();
}