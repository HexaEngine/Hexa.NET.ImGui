namespace HexaEngine.Core
{
    using HexaEngine.Core.Graphics;
    using HexaEngine.Core.Input;
    using HexaEngine.Core.Windows;
    using HexaEngine.Core.Windows.Events;
    using Silk.NET.SDL;
    using System.Collections.Generic;

    public static unsafe class Application
    {
        internal static readonly Sdl sdl = Sdl.GetApi();

        private static bool initialized = false;
        private static bool exiting = false;
        private static readonly Dictionary<uint, IRenderWindow> windowIdToWindow = new();
        private static readonly List<IRenderWindow> windows = new();
        private static readonly List<Action<Event>> hooks = new();
        private static IRenderWindow? mainWindow;
        private static bool inDesignMode;
        private static bool inEditorMode;

        private static IGraphicsDevice graphicsDevice;
        private static IGraphicsContext graphicsContext;

#nullable disable
        public static IRenderWindow MainWindow => mainWindow;
#nullable enable

        public enum SpecialFolder
        {
            Assets,
            Shaders,
            Sounds,
            Models,
            Textures,
            Scenes,
        }

        public static bool InDesignMode
        {
            get => inDesignMode; set
            {
                inDesignMode = value;
                OnDesignModeChanged?.Invoke(value);
            }
        }

        public static bool InEditorMode
        {
            get => inEditorMode; set
            {
                inEditorMode = value;
                OnEditorModeChanged?.Invoke(value);
            }
        }

        public static bool GraphicsDebugging { get; set; }

        public static event Action<bool>? OnDesignModeChanged;

        public static event Action<bool>? OnEditorModeChanged;

        public static string GetFolder(SpecialFolder folder)
        {
            return folder switch
            {
                SpecialFolder.Assets => Path.GetFullPath("assets/"),
                SpecialFolder.Shaders => Path.GetFullPath("assets/shaders/"),
                SpecialFolder.Sounds => Path.GetFullPath("assets/sounds/"),
                SpecialFolder.Models => Path.GetFullPath("assets/models/"),
                SpecialFolder.Textures => Path.GetFullPath("assets/textures/"),
                SpecialFolder.Scenes => Path.GetFullPath("assets/scenes/"),
                _ => throw new ArgumentOutOfRangeException(nameof(folder)),
            };
        }

        public static void Run(IRenderWindow mainWindow)
        {
            Init();
            Application.mainWindow = mainWindow;
            mainWindow.Closing += MainWindow_Closing;

            mainWindow.Show();
            PlatformRun();
        }

        private static void Init()
        {
            sdl.SetHint(Sdl.HintMouseFocusClickthrough, "1");
            sdl.SetHint(Sdl.HintAutoUpdateJoysticks, "1");
            sdl.SetHint(Sdl.HintJoystickHidapiPS4, "1");
            sdl.SetHint(Sdl.HintJoystickHidapiPS4Rumble, "1");
            sdl.SetHint(Sdl.HintJoystickRawinput, "0");
            sdl.Init(Sdl.InitEvents + Sdl.InitGamecontroller + Sdl.InitHaptic + Sdl.InitJoystick + Sdl.InitSensor);

            Keyboard.Init();
            Mouse.Init();
            Gamepads.Init();
            TouchDevices.Init();

            graphicsDevice = GraphicsAdapter.CreateGraphicsDevice(GraphicsBackend.Auto, GraphicsDebugging);
            graphicsContext = graphicsDevice.Context;

            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].Initialize(graphicsDevice);
            }

            initialized = true;
        }

        internal static void RegisterWindow(IRenderWindow window)
        {
            windows.Add(window);
            windowIdToWindow.Add(window.WindowID, window);
            if (initialized)
            {
                window.Initialize(graphicsDevice);
            }
        }

        private static void MainWindow_Closing(object? sender, CloseEventArgs e)
        {
            if (!e.Handled)
            {
                exiting = true;
            }
        }

        public static void RegisterHook(Action<Event> hook)
        {
            hooks.Add(hook);
        }

        private static void PlatformRun()
        {
            Event evnt;
            Time.Initialize();

            while (!exiting)
            {
                sdl.PumpEvents();
                while (sdl.PollEvent(&evnt) == (int)SdlBool.True)
                {
                    EventType type = (EventType)evnt.Type;
                    switch (type)
                    {
                        case EventType.Firstevent:
                            break;

                        case EventType.Quit:
                            exiting = true;
                            break;

                        case EventType.AppTerminating:
                            exiting = true;
                            break;

                        case EventType.AppLowmemory:
                            break;

                        case EventType.AppWillenterbackground:
                            break;

                        case EventType.AppDidenterbackground:
                            break;

                        case EventType.AppWillenterforeground:
                            break;

                        case EventType.AppDidenterforeground:
                            break;

                        case EventType.Localechanged:
                            break;

                        case EventType.Displayevent:
                            break;

                        case EventType.Windowevent:
                            {
                                SdlWindow window = (SdlWindow)windowIdToWindow[evnt.Window.WindowID];
                                window.ProcessEvent(evnt.Window);
                                if ((WindowEventID)evnt.Window.Event == WindowEventID.Close && window == mainWindow)
                                {
                                    exiting = true;
                                }
                            }
                            break;

                        case EventType.Syswmevent:
                            break;

                        case EventType.Keydown:
                            {
                                var even = evnt.Key;
                                Keyboard.OnKeyDown(even);
                                if (windowIdToWindow.TryGetValue(even.WindowID, out var window))
                                {
                                    ((SdlWindow)window).ProcessInputKeyboard(even);
                                }
                            }
                            break;

                        case EventType.Keyup:
                            {
                                var even = evnt.Key;
                                Keyboard.OnKeyUp(even);
                                if (windowIdToWindow.TryGetValue(even.WindowID, out var window))
                                {
                                    ((SdlWindow)window).ProcessInputKeyboard(even);
                                }
                            }
                            break;

                        case EventType.Textediting:
                            break;

                        case EventType.Textinput:
                            {
                                var even = evnt.Text;
                                Keyboard.OnTextInput(even);
                                if (windowIdToWindow.TryGetValue(even.WindowID, out var window))
                                {
                                    ((SdlWindow)window).ProcessInputText(even);
                                }
                            }
                            break;

                        case EventType.Keymapchanged:
                            break;

                        case EventType.Mousemotion:
                            {
                                var even = evnt.Motion;
                                Mouse.OnMotion(even);
                                if (windowIdToWindow.TryGetValue(even.WindowID, out var window))
                                {
                                    ((SdlWindow)window).ProcessInputMouse(even);
                                }
                            }
                            break;

                        case EventType.Mousebuttondown:
                            {
                                var even = evnt.Button;
                                Mouse.OnButtonDown(even);
                                if (windowIdToWindow.TryGetValue(even.WindowID, out var window))
                                {
                                    ((SdlWindow)window).ProcessInputMouse(even);
                                }
                            }
                            break;

                        case EventType.Mousebuttonup:
                            {
                                var even = evnt.Button;
                                Mouse.OnButtonUp(even);
                                if (windowIdToWindow.TryGetValue(even.WindowID, out var window))
                                {
                                    ((SdlWindow)window).ProcessInputMouse(even);
                                }
                            }
                            break;

                        case EventType.Mousewheel:
                            {
                                var even = evnt.Wheel;
                                Mouse.OnWheel(even);
                                if (windowIdToWindow.TryGetValue(even.WindowID, out var window))
                                {
                                    ((SdlWindow)window).ProcessInputMouse(even);
                                }
                            }
                            break;

                        case EventType.Joyaxismotion:
                            {
                                var even = evnt.Jaxis;
                                Joysticks.OnAxisMotion(even);
                            }
                            break;

                        case EventType.Joyballmotion:
                            {
                                var even = evnt.Jball;
                                Joysticks.OnBallMotion(even);
                            }
                            break;

                        case EventType.Joyhatmotion:
                            {
                                var even = evnt.Jhat;
                                Joysticks.OnHatMotion(even);
                            }
                            break;

                        case EventType.Joybuttondown:
                            {
                                var even = evnt.Jbutton;
                                Joysticks.OnButtonDown(even);
                            }
                            break;

                        case EventType.Joybuttonup:
                            {
                                var even = evnt.Jbutton;
                                Joysticks.OnButtonUp(even);
                            }
                            break;

                        case EventType.Joydeviceadded:
                            {
                                var even = evnt.Jdevice;
                                Joysticks.AddJoystick(even);
                            }
                            break;

                        case EventType.Joydeviceremoved:
                            {
                                var even = evnt.Jdevice;
                                Joysticks.RemoveJoystick(even);
                            }
                            break;

                        case EventType.Controlleraxismotion:
                            {
                                var even = evnt.Caxis;
                                Gamepads.AxisMotion(even);
                            }
                            break;

                        case EventType.Controllerbuttondown:
                            {
                                var even = evnt.Cbutton;
                                Gamepads.ButtonDown(even);
                            }
                            break;

                        case EventType.Controllerbuttonup:
                            {
                                var even = evnt.Cbutton;
                                Gamepads.ButtonUp(even);
                            }
                            break;

                        case EventType.Controllerdeviceadded:
                            {
                                var even = evnt.Cdevice;
                                Gamepads.AddController(even);
                            }
                            break;

                        case EventType.Controllerdeviceremoved:
                            {
                                var even = evnt.Cdevice;
                                Gamepads.RemoveController(even);
                            }
                            break;

                        case EventType.Controllerdeviceremapped:
                            {
                                var even = evnt.Cdevice;
                                Gamepads.Remapped(even);
                            }
                            break;

                        case EventType.Controllertouchpaddown:
                            {
                                var even = evnt.Ctouchpad;
                                Gamepads.TouchPadDown(even);
                            }
                            break;

                        case EventType.Controllertouchpadmotion:
                            {
                                var even = evnt.Ctouchpad;
                                Gamepads.TouchPadMotion(even);
                            }
                            break;

                        case EventType.Controllertouchpadup:
                            {
                                var even = evnt.Ctouchpad;
                                Gamepads.TouchPadUp(even);
                            }
                            break;

                        case EventType.Controllersensorupdate:
                            {
                                var even = evnt.Csensor;
                                Gamepads.SensorUpdate(even);
                            }
                            break;

                        case EventType.Fingerdown:
                            break;

                        case EventType.Fingerup:
                            break;

                        case EventType.Fingermotion:
                            break;

                        case EventType.Dollargesture:
                            break;

                        case EventType.Dollarrecord:
                            break;

                        case EventType.Multigesture:
                            break;

                        case EventType.Clipboardupdate:
                            break;

                        case EventType.Dropfile:
                            break;

                        case EventType.Droptext:
                            break;

                        case EventType.Dropbegin:
                            break;

                        case EventType.Dropcomplete:
                            break;

                        case EventType.Audiodeviceadded:
                            break;

                        case EventType.Audiodeviceremoved:
                            break;

                        case EventType.Sensorupdate:
                            break;

                        case EventType.RenderTargetsReset:
                            break;

                        case EventType.RenderDeviceReset:
                            break;

                        case EventType.Userevent:
                            break;

                        case EventType.Lastevent:
                            break;
                    }

                    for (int i = 0; i < hooks.Count; i++)
                    {
                        hooks[i](evnt);
                    }
                }

                for (int i = 0; i < windows.Count; i++)
                {
                    windows[i].Render(graphicsContext);
                }
                for (int i = 0; i < windows.Count; i++)
                {
                    windows[i].ClearState();
                }
                Time.FrameUpdate();
            }

            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].Uninitialize();
            }

            sdl.Quit();
        }
    }
}