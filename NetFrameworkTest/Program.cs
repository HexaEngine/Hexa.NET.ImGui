namespace ExampleD3D11
{
    using ExampleD3D11.ImGuiDemo;
    using ExampleD3D11.Input;
    using Hexa.NET.ImGui;
    using Hexa.NET.SDL2;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;
    using System;

    // This example project and the other C# backends are deprecated and will not receive updates
    // Look at ExampleGFWLD3D11 for an example derived directly from the ImGui backends
    [Obsolete("The C# backends are no longer maintained")]
    internal unsafe class Program
    {
        private static bool exiting = false;
        private static readonly List<Func<SDLEvent, bool>> hooks = new();
        private static SDLWindow* mainWindow;
        private static uint mainWindowId;

        private static int width;
        private static int height;

        private static D3D11Manager d3d11Manager;

        private static ImGuiManager imGuiManager;
        private static ImGuiDemo.ImGuiDemo imGuiDemo;
        //private static ImGuizmoDemo.ImGuizmoDemo imGuizmoDemo;
        //private static ImNodesDemo.ImNodesDemo imNodesDemo;
        //private static ImPlotDemo.ImPlotDemo imPlotDemo;

        public static int Width => width;

        public static int Height => height;

        public static event EventHandler<ResizedEventArgs>? Resized;

        private static void Main(string[] args)
        {
            SDL.SetHint(SDL.SDL_HINT_MOUSE_FOCUS_CLICKTHROUGH, "1");
            SDL.SetHint(SDL.SDL_HINT_AUTO_UPDATE_JOYSTICKS, "1");
            SDL.SetHint(SDL.SDL_HINT_JOYSTICK_HIDAPI_PS4, "1");//HintJoystickHidapiPS4
            SDL.SetHint(SDL.SDL_HINT_JOYSTICK_HIDAPI_PS4_RUMBLE, "1"); //HintJoystickHidapiPS4Rumble
            SDL.SetHint(SDL.SDL_HINT_JOYSTICK_RAWINPUT, "0");
            SDL.SetHint(SDL.SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1"); //HintWindowsDisableThreadNaming
            SDL.SetHint(SDL.SDL_HINT_MOUSE_NORMAL_SPEED_SCALE, "1");

            SDL.Init(SDL.SDL_INIT_EVENTS + SDL.SDL_INIT_GAMECONTROLLER + SDL.SDL_INIT_HAPTIC + SDL.SDL_INIT_JOYSTICK + SDL.SDL_INIT_SENSOR);

            Keyboard.Init();
            Mouse.Init();

            int width = 1280;
            int height = 720;
            int y = 100;
            int x = 100;

            SDLWindowFlags flags = SDLWindowFlags.Resizable | SDLWindowFlags.Hidden | SDLWindowFlags.AllowHighdpi;
            mainWindow = SDL.CreateWindow("", x, y, width, height, (uint)flags);
            mainWindowId = SDL.GetWindowID(mainWindow);

            InitGraphics(mainWindow);
            InitImGui(mainWindow);

            SDL.ShowWindow(mainWindow);

            Time.Initialize();

            SDLEvent evnt;
            while (!exiting)
            {
                SDL.PumpEvents();
                while (SDL.PollEvent(&evnt) == (int)SDLBool.True)
                {
                    for (int i = 0; i < hooks.Count; i++)
                    {
                        hooks[i](evnt);
                    }

                    HandleEvent(evnt);
                }

                Render(d3d11Manager.DeviceContext);

                Keyboard.Flush();
                Mouse.Flush();
                Time.FrameUpdate();
            }

            imGuiManager.Dispose();

            d3d11Manager.Dispose();

            SDL.DestroyWindow(mainWindow);
            //sdl.Quit();
        }

        private static void Render(ComPtr<ID3D11DeviceContext1> deviceContext)
        {
            imGuiManager.NewFrame();

            ImGui.ShowDemoWindow();

            d3d11Manager.Clear(default);
            d3d11Manager.SetTarget();

            imGuiManager.EndFrame();

            deviceContext.ClearState();

            d3d11Manager.Present(1, 0);
        }

        private static void HandleEvent(SDLEvent evnt)
        {
            SDLEventType type = (SDLEventType)evnt.Type;
            switch (type)
            {
                case SDLEventType.Windowevent:
                    {
                        var even = evnt.Window;
                        if (even.WindowID == mainWindowId)
                        {
                            switch ((SDLWindowEventID)evnt.Window.Event)
                            {
                                case SDLWindowEventID.Close:
                                    exiting = true;
                                    break;

                                case SDLWindowEventID.Resized:
                                    int oldWidth = Program.width;
                                    int oldHeight = Program.height;
                                    int width = even.Data1;
                                    int height = even.Data2;
                                    Resize(width, height, oldWidth, oldHeight);
                                    Program.width = width;
                                    Program.height = height;
                                    break;
                            }
                        }
                    }
                    break;

                case SDLEventType.Mousemotion:
                    {
                        var even = evnt.Motion;
                        Mouse.OnMotion(even);
                    }
                    break;

                case SDLEventType.Mousebuttondown:
                    {
                        var even = evnt.Button;
                        Mouse.OnButtonDown(even);
                    }
                    break;

                case SDLEventType.Mousebuttonup:
                    {
                        var even = evnt.Button;
                        Mouse.OnButtonUp(even);
                    }
                    break;

                case SDLEventType.Mousewheel:
                    {
                        var even = evnt.Wheel;
                        Mouse.OnWheel(even);
                    }
                    break;

                case SDLEventType.Keydown:
                    {
                        var even = evnt.Key;
                        Keyboard.OnKeyDown(even);
                    }
                    break;

                case SDLEventType.Keyup:
                    {
                        var even = evnt.Key;
                        Keyboard.OnKeyUp(even);
                    }
                    break;

                case SDLEventType.Textediting:
                    break;

                case SDLEventType.Textinput:
                    {
                        var even = evnt.Text;
                        Keyboard.OnTextInput(even);
                    }
                    break;
            }
        }

        private static void InitGraphics(SDLWindow* mainWindow)
        {
            d3d11Manager = new(mainWindow, true);
        }

        private static void InitImGui(SDLWindow* mainWindow)
        {
            imGuiManager = new(mainWindow, d3d11Manager.Device, d3d11Manager.DeviceContext);

            imGuiDemo = new();
            //imGuizmoDemo = new();
            //imNodesDemo = new();
            //imPlotDemo = new();
        }

        private static void Resize(int width, int height, int oldWidth, int oldHeight)
        {
            d3d11Manager.Resize(width, height);
            Resized?.Invoke(null, new(width, height, oldWidth, oldHeight));
        }

        public static void RegisterHook(Func<SDLEvent, bool> hook)
        {
            hooks.Add(hook);
        }

        public static void UnregisterHook(Func<SDLEvent, bool> hook)
        {
            hooks.Remove(hook);
        }
    }
}