namespace ExampleD3D11
{
    using ExampleD3D11.ImGuiDemo;
    using ExampleD3D11.ImGuizmoDemo;
    using ExampleD3D11.ImNodesDemo;
    using ExampleD3D11.ImPlotDemo;
    using ExampleD3D11.Input;
    using Silk.NET.Core.Native;
    using Silk.NET.Direct3D11;
    using Silk.NET.SDL;
    using System;
    using System.Diagnostics;

    public class ResizedEventArgs : EventArgs
    {
        public ResizedEventArgs(int width, int height, int oldWidth, int oldHeight)
        {
            Width = width;
            Height = height;
            OldWidth = oldWidth;
            OldHeight = oldHeight;
        }

        public int Width { get; }

        public int Height { get; }

        public int OldWidth { get; }

        public int OldHeight { get; }
    }

    internal unsafe class Program
    {
        internal static readonly Sdl sdl = Sdl.GetApi();
        private static bool exiting = false;
        private static readonly List<Func<Event, bool>> hooks = new();
        private static Window* mainWindow;
        private static uint mainWindowId;

        private static int width;
        private static int height;

        private static D3D11Manager d3d11Manager;

        private static ImGuiManager imGuiManager;
        private static ImGuiDemo.ImGuiDemo imGuiDemo;
        private static ImGuizmoDemo.ImGuizmoDemo imGuizmoDemo;
        private static ImNodesDemo.ImNodesDemo imNodesDemo;
        private static ImPlotDemo.ImPlotDemo imPlotDemo;

        public static int Width => width;

        public static int Height => height;

        public static event EventHandler<ResizedEventArgs>? Resized;

        private static void Main(string[] args)
        {
            sdl.SetHint(Sdl.HintMouseFocusClickthrough, "1");
            sdl.SetHint(Sdl.HintMouseAutoCapture, "0");
            sdl.SetHint(Sdl.HintAutoUpdateJoysticks, "1");
            sdl.SetHint(Sdl.HintJoystickHidapiPS4, "1");
            sdl.SetHint(Sdl.HintJoystickHidapiPS4Rumble, "1");
            sdl.SetHint(Sdl.HintJoystickRawinput, "0");
            sdl.Init(Sdl.InitEvents + Sdl.InitGamecontroller + Sdl.InitHaptic + Sdl.InitJoystick + Sdl.InitSensor);

            Keyboard.Init();
            Mouse.Init();

            int width = 1280;
            int height = 720;
            int y = 100;
            int x = 100;

            WindowFlags flags = WindowFlags.Resizable | WindowFlags.Hidden | WindowFlags.AllowHighdpi;
            mainWindow = sdl.CreateWindow("", x, y, width, height, (uint)flags);
            mainWindowId = sdl.GetWindowID(mainWindow);

            InitGraphics(mainWindow);
            InitImGui(mainWindow);

            sdl.ShowWindow(mainWindow);

            Time.Initialize();

            Event evnt;
            while (!exiting)
            {
                sdl.PumpEvents();
                while (sdl.PollEvent(&evnt) == (int)SdlBool.True)
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

            sdl.DestroyWindow(mainWindow);
            //sdl.Quit();
        }

        private static void Render(ComPtr<ID3D11DeviceContext1> deviceContext)
        {
            imGuiManager.NewFrame();

            imGuiDemo.Draw();
            imGuizmoDemo.Draw();
            imNodesDemo.Draw();
            imPlotDemo.Draw();

            d3d11Manager.Clear(default);
            d3d11Manager.SetTarget();

            imGuiManager.EndFrame();

            deviceContext.ClearState();

            d3d11Manager.Present(1, 0);
        }

        private static void HandleEvent(Event evnt)
        {
            EventType type = (EventType)evnt.Type;
            switch (type)
            {
                case EventType.Windowevent:
                    {
                        var even = evnt.Window;
                        if (even.WindowID == mainWindowId)
                        {
                            switch ((WindowEventID)evnt.Window.Event)
                            {
                                case WindowEventID.Close:
                                    exiting = true;
                                    break;

                                case WindowEventID.Resized:
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

                case EventType.Mousemotion:
                    {
                        var even = evnt.Motion;
                        Mouse.OnMotion(even);
                    }
                    break;

                case EventType.Mousebuttondown:
                    {
                        var even = evnt.Button;
                        Mouse.OnButtonDown(even);
                    }
                    break;

                case EventType.Mousebuttonup:
                    {
                        var even = evnt.Button;
                        Mouse.OnButtonUp(even);
                    }
                    break;

                case EventType.Mousewheel:
                    {
                        var even = evnt.Wheel;
                        Mouse.OnWheel(even);
                    }
                    break;

                case EventType.Keydown:
                    {
                        var even = evnt.Key;
                        Keyboard.OnKeyDown(even);
                    }
                    break;

                case EventType.Keyup:
                    {
                        var even = evnt.Key;
                        Keyboard.OnKeyUp(even);
                    }
                    break;

                case EventType.Textediting:
                    break;

                case EventType.Textinput:
                    {
                        var even = evnt.Text;
                        Keyboard.OnTextInput(even);
                    }
                    break;
            }
        }

        private static void InitGraphics(Window* mainWindow)
        {
            d3d11Manager = new(mainWindow, true);
        }

        private static void InitImGui(Window* mainWindow)
        {
            imGuiManager = new(mainWindow, d3d11Manager.Device, d3d11Manager.DeviceContext);

            imGuiDemo = new();
            imGuizmoDemo = new();
            imNodesDemo = new();
            imPlotDemo = new();
        }

        private static void Resize(int width, int height, int oldWidth, int oldHeight)
        {
            d3d11Manager.Resize(width, height);
            Resized?.Invoke(null, new(width, height, oldWidth, oldHeight));
        }

        public static void RegisterHook(Func<Event, bool> hook)
        {
            hooks.Add(hook);
        }
    }
}