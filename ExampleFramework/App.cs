namespace ExampleFramework
{
    using ExampleFramework.Input;
    using Hexa.NET.ImGui.Backends.SDL2;
    using Silk.NET.SDL;
    using System;
    using System.Collections.Generic;

    public enum Backend
    {
        OpenGL,
        DirectX,
        Vulkan,
        Metal,
    }

    public static unsafe class App
    {
        public static readonly Sdl sdl = Sdl.GetApi();
        private static bool exiting = false;
        private static readonly List<Func<Event, bool>> hooks = new();

        private static uint mainWindowId;
        private static CoreWindow mainWindow;

        public static Backend Backend { get; private set; }

        public static CoreWindow MainWindow { get => mainWindow; private set => mainWindow = value; }

        public static void RegisterHook(Func<Event, bool> hook)
        {
            hooks.Add(hook);
        }

        public static void RemoveHook(Func<Event, bool> hook)
        {
            hooks.Remove(hook);
        }

        public static void Init(Backend backend)
        {
            Backend = backend;
            sdl.SetHint(Sdl.HintMouseFocusClickthrough, "1");
            sdl.SetHint(Sdl.HintMouseAutoCapture, "0");
            sdl.SetHint(Sdl.HintAutoUpdateJoysticks, "1");
            sdl.SetHint(Sdl.HintJoystickHidapiPS4, "1");
            sdl.SetHint(Sdl.HintJoystickHidapiPS4Rumble, "1");
            sdl.SetHint(Sdl.HintJoystickRawinput, "0");
            sdl.Init(Sdl.InitEvents + Sdl.InitVideo + Sdl.InitGamecontroller + Sdl.InitHaptic + Sdl.InitJoystick + Sdl.InitSensor);

            if (backend == Backend.OpenGL)
            {
                sdl.GLSetAttribute(GLattr.ContextMajorVersion, 3);
                sdl.GLSetAttribute(GLattr.ContextMinorVersion, 3);
                sdl.GLSetAttribute(GLattr.ContextProfileMask, (int)GLprofile.Core);
            }

            Keyboard.Init();
            Mouse.Init();
        }

        public static void Run(CoreWindow window)
        {
            mainWindow = window;
            mainWindowId = window.Id;
            window.InitGraphics();

            window.Show();

            PlatformRun();

            window.Dispose();

            sdl.Quit();
        }

        private static void PlatformRun()
        {
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

                mainWindow.Render();

                Keyboard.Flush();
                Mouse.Flush();
                Time.FrameUpdate();
            }
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
                            }

                            mainWindow.ProcessWindowEvent(even);
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
    }
}