namespace ExampleFramework.ImGuiDemo
{
    using ExampleFramework;
    using Hexa.NET.ImGui;
    using Hexa.NET.Utilities;
    using Silk.NET.Core.Native;
    using Silk.NET.Maths;
    using Silk.NET.SDL;
    using System;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Window = Silk.NET.SDL.Window;

    [Obsolete("Use ImGuiImplSDL2 instead")]
    public static class ImGuiSDL2Platform
    {
        private static readonly Sdl sdl = Sdl.GetApi();

        public enum GamepadMode
        {
            AutoFirst,
            AutoAll,
            Manual
        }

        /// <summary>
        /// SDL Data
        /// </summary>
        private unsafe struct BackendData
        {
            public Window* Window;
            public Renderer* Renderer;
            public ulong Time;
            public byte* ClipboardTextData;
            public bool UseVulkan;
            public bool WantUpdateMonitors;

            public uint MouseWindowID;
            public int MouseButtonsDown;
            public Cursor** MouseCursors;
            public Cursor* LastMouseCursor;
            public int MouseLastLeaveFrame;
            public bool MouseCanUseGlobalState;
            public bool MouseCanReportHoveredViewport;  // This is hard to use/unreliable on SDL so we'll set ImGuiBackendFlags_HasMouseHoveredViewport dynamically based on state.

            public UnsafeList<Pointer<GameController>> Gamepads;
            public GamepadMode GamepadMode;
            public bool WantUpdateGamepadsList;
        }

        /// <summary>
        /// Backend data stored in io.BackendPlatformUserData to allow support for multiple Dear ImGui contexts
        /// It is STRONGLY preferred that you use docking branch with multi-viewports (== single Dear ImGui context + multiple windows) instead of multiple Dear ImGui contexts.
        /// FIXME: multi-context support is not well tested and probably dysfunctional in this backend.
        /// FIXME: some shared resources (mouse cursor shape, gamepad) are mishandled when using multi-context.
        /// </summary>
        /// <returns></returns>
        private static unsafe BackendData* GetBackendData()
        {
            return !ImGui.GetCurrentContext().IsNull ? (BackendData*)ImGui.GetIO().BackendPlatformUserData : null;
        }

        private static unsafe byte* GetClipboardText(ImGuiContext* data)
        {
            BackendData* bd = GetBackendData();
            if (bd->ClipboardTextData != null)
                sdl.Free(bd->ClipboardTextData);
            bd->ClipboardTextData = sdl.GetClipboardText();
            return bd->ClipboardTextData;
        }

        private static unsafe void SetClipboardText(ImGuiContext* data, byte* text)
        {
            sdl.SetClipboardText(text);
        }

        /// <summary>
        /// Note: native IME will only display if user calls SDL_SetHint(SDL_HINT_IME_SHOW_UI, "1") _before_ SDL_CreateWindow().
        /// </summary>
        /// <param name="vp"></param>
        /// <param name="data"></param>
        private static unsafe void SetPlatformImeData(ImGuiContext* context, ImGuiViewport* vp, ImGuiPlatformImeData* data)
        {
            if (data->WantVisible == 1)
            {
                Rectangle<int> r;
                r.Origin.X = (int)(data->InputPos.X - vp->Pos.X);
                r.Origin.Y = (int)(data->InputPos.Y - vp->Pos.Y + data->InputLineHeight);
                r.Size.X = 1;
                r.Size.Y = (int)data->InputLineHeight;
                sdl.SetTextInputRect(&r);
            }
        }

        private static unsafe byte OpenPlatformInShell(ImGuiContext* ctx, byte* path)
        {
            string url = ToStringFromUTF8(path)!;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else
            {
                return 0;
            }

            return 1;
        }

        private static ImGuiKey KeycodeToImGuiKey(int keycode)
        {
            return (KeyCode)keycode switch
            {
                KeyCode.KTab => ImGuiKey.Tab,
                KeyCode.KLeft => ImGuiKey.LeftArrow,
                KeyCode.KRight => ImGuiKey.RightArrow,
                KeyCode.KUp => ImGuiKey.UpArrow,
                KeyCode.KDown => ImGuiKey.DownArrow,
                KeyCode.KPageup => ImGuiKey.PageUp,
                KeyCode.KPagedown => ImGuiKey.PageDown,
                KeyCode.KHome => ImGuiKey.Home,
                KeyCode.KEnd => ImGuiKey.End,
                KeyCode.KInsert => ImGuiKey.Insert,
                KeyCode.KDelete => ImGuiKey.Delete,
                KeyCode.KBackspace => ImGuiKey.Backspace,
                KeyCode.KSpace => ImGuiKey.Space,
                KeyCode.KReturn => ImGuiKey.Enter,
                KeyCode.KEscape => ImGuiKey.Escape,
                KeyCode.KQuote => ImGuiKey.Apostrophe,
                KeyCode.KComma => ImGuiKey.Comma,
                KeyCode.KMinus => ImGuiKey.Minus,
                KeyCode.KPeriod => ImGuiKey.Period,
                KeyCode.KSlash => ImGuiKey.Slash,
                KeyCode.KSemicolon => ImGuiKey.Semicolon,
                KeyCode.KEquals => ImGuiKey.Equal,
                KeyCode.KLeftbracket => ImGuiKey.LeftBracket,
                KeyCode.KBackslash => ImGuiKey.Backslash,
                KeyCode.KRightbracket => ImGuiKey.RightBracket,
                KeyCode.KBackquote => ImGuiKey.GraveAccent,
                KeyCode.KCapslock => ImGuiKey.CapsLock,
                KeyCode.KScrolllock => ImGuiKey.ScrollLock,
                KeyCode.KNumlockclear => ImGuiKey.NumLock,
                KeyCode.KPrintscreen => ImGuiKey.PrintScreen,
                KeyCode.KPause => ImGuiKey.Pause,
                KeyCode.KKP0 => ImGuiKey.Keypad0,
                KeyCode.KKP1 => ImGuiKey.Keypad1,
                KeyCode.KKP2 => ImGuiKey.Keypad2,
                KeyCode.KKP3 => ImGuiKey.Keypad3,
                KeyCode.KKP4 => ImGuiKey.Keypad4,
                KeyCode.KKP5 => ImGuiKey.Keypad5,
                KeyCode.KKP6 => ImGuiKey.Keypad6,
                KeyCode.KKP7 => ImGuiKey.Keypad7,
                KeyCode.KKP8 => ImGuiKey.Keypad8,
                KeyCode.KKP9 => ImGuiKey.Keypad9,
                KeyCode.KKPPeriod => ImGuiKey.KeypadDecimal,
                KeyCode.KKPDivide => ImGuiKey.KeypadDivide,
                KeyCode.KKPMultiply => ImGuiKey.KeypadMultiply,
                KeyCode.KKPMinus => ImGuiKey.KeypadSubtract,
                KeyCode.KKPPlus => ImGuiKey.KeypadAdd,
                KeyCode.KKPEnter => ImGuiKey.KeypadEnter,
                KeyCode.KKPEquals => ImGuiKey.KeypadEqual,
                KeyCode.KLctrl => ImGuiKey.LeftCtrl,
                KeyCode.KLshift => ImGuiKey.LeftShift,
                KeyCode.KLalt => ImGuiKey.LeftAlt,
                KeyCode.KLgui => ImGuiKey.LeftSuper,
                KeyCode.KRctrl => ImGuiKey.RightCtrl,
                KeyCode.KRshift => ImGuiKey.RightShift,
                KeyCode.KRalt => ImGuiKey.RightAlt,
                KeyCode.KRgui => ImGuiKey.RightSuper,
                KeyCode.KApplication => ImGuiKey.Menu,
                KeyCode.K0 => ImGuiKey.Key0,
                KeyCode.K1 => ImGuiKey.Key1,
                KeyCode.K2 => ImGuiKey.Key2,
                KeyCode.K3 => ImGuiKey.Key3,
                KeyCode.K4 => ImGuiKey.Key4,
                KeyCode.K5 => ImGuiKey.Key5,
                KeyCode.K6 => ImGuiKey.Key6,
                KeyCode.K7 => ImGuiKey.Key7,
                KeyCode.K8 => ImGuiKey.Key8,
                KeyCode.K9 => ImGuiKey.Key9,
                KeyCode.KA => ImGuiKey.A,
                KeyCode.KB => ImGuiKey.B,
                KeyCode.KC => ImGuiKey.C,
                KeyCode.KD => ImGuiKey.D,
                KeyCode.KE => ImGuiKey.E,
                KeyCode.KF => ImGuiKey.F,
                KeyCode.KG => ImGuiKey.G,
                KeyCode.KH => ImGuiKey.H,
                KeyCode.KI => ImGuiKey.I,
                KeyCode.KJ => ImGuiKey.J,
                KeyCode.KK => ImGuiKey.K,
                KeyCode.KL => ImGuiKey.L,
                KeyCode.KM => ImGuiKey.M,
                KeyCode.KN => ImGuiKey.N,
                KeyCode.KO => ImGuiKey.O,
                KeyCode.KP => ImGuiKey.P,
                KeyCode.KQ => ImGuiKey.Q,
                KeyCode.KR => ImGuiKey.R,
                KeyCode.KS => ImGuiKey.S,
                KeyCode.KT => ImGuiKey.T,
                KeyCode.KU => ImGuiKey.U,
                KeyCode.KV => ImGuiKey.V,
                KeyCode.KW => ImGuiKey.W,
                KeyCode.KX => ImGuiKey.X,
                KeyCode.KY => ImGuiKey.Y,
                KeyCode.KZ => ImGuiKey.Z,
                KeyCode.KF1 => ImGuiKey.F1,
                KeyCode.KF2 => ImGuiKey.F2,
                KeyCode.KF3 => ImGuiKey.F3,
                KeyCode.KF4 => ImGuiKey.F4,
                KeyCode.KF5 => ImGuiKey.F5,
                KeyCode.KF6 => ImGuiKey.F6,
                KeyCode.KF7 => ImGuiKey.F7,
                KeyCode.KF8 => ImGuiKey.F8,
                KeyCode.KF9 => ImGuiKey.F9,
                KeyCode.KF10 => ImGuiKey.F10,
                KeyCode.KF11 => ImGuiKey.F11,
                KeyCode.KF12 => ImGuiKey.F12,
                KeyCode.KACBack => ImGuiKey.AppBack,
                KeyCode.KACForward => ImGuiKey.AppForward,
                _ => ImGuiKey.None,
            };
        }

        private static void UpdateKeyModifiers(Keymod keymods)
        {
            ImGuiIOPtr io = ImGui.GetIO();

            io.AddKeyEvent(ImGuiKey.ModCtrl, (keymods & Keymod.Ctrl) != 0);
            io.AddKeyEvent(ImGuiKey.ModShift, (keymods & Keymod.Shift) != 0);
            io.AddKeyEvent(ImGuiKey.ModAlt, (keymods & Keymod.Alt) != 0);
            io.AddKeyEvent(ImGuiKey.ModSuper, (keymods & Keymod.Gui) != 0);
        }

        /// <summary>
        /// You can read the io.WantCaptureMouse, io.WantCaptureKeyboard flags to tell if dear imgui wants to use your inputs.
        /// - When io.WantCaptureMouse is true, do not dispatch mouse input data to your main application, or clear/overwrite your copy of the mouse data.
        /// - When io.WantCaptureKeyboard is true, do not dispatch keyboard input data to your main application, or clear/overwrite your copy of the keyboard data.
        /// Generally you may always pass all inputs to dear imgui, and hide them from your application based on those two flags.
        /// If you have multiple SDL events and some of them are not meant to be used by dear imgui, you may need to filter events based on their windowID field.
        /// </summary>
        private static unsafe bool ProcessEvent(Event env)
        {
            var io = ImGui.GetIO();
            var bd = GetBackendData();

            switch ((EventType)env.Type)
            {
                case EventType.Mousemotion:
                    {
                        Vector2 mouse_pos = new(env.Motion.X, env.Motion.Y);
                        if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
                        {
                            int window_x, window_y;
                            sdl.GetWindowPosition(sdl.GetWindowFromID(env.Motion.WindowID), &window_x, &window_y);
                            mouse_pos.X += window_x;
                            mouse_pos.Y += window_y;
                        }

                        io.AddMouseSourceEvent(env.Motion.Which == unchecked((uint)-1) ? ImGuiMouseSource.TouchScreen : ImGuiMouseSource.Mouse);
                        io.AddMousePosEvent(mouse_pos.X, mouse_pos.Y);
                        return true;
                    }
                case EventType.Mousewheel:
                    {
                        //IMGUI_DEBUG_LOG("wheel %.2f %.2f, precise %.2f %.2f\n", (float)event->wheel.x, (float)event->wheel.y, event->wheel.preciseX, event->wheel.preciseY);

                        float wheel_x = -(float)env.Wheel.X;
                        float wheel_y = env.Wheel.Y;

                        //wheel_x /= 100.0f;

                        io.AddMouseSourceEvent(env.Wheel.Which == unchecked((uint)-1) ? ImGuiMouseSource.TouchScreen : ImGuiMouseSource.Mouse);
                        io.AddMouseWheelEvent(wheel_x, wheel_y);
                        return true;
                    }
                case EventType.Mousebuttondown:
                case EventType.Mousebuttonup:
                    {
                        int mouse_button = -1;
                        if (env.Button.Button == Sdl.ButtonLeft) { mouse_button = 0; }
                        if (env.Button.Button == Sdl.ButtonRight) { mouse_button = 1; }
                        if (env.Button.Button == Sdl.ButtonMiddle) { mouse_button = 2; }
                        if (env.Button.Button == Sdl.ButtonX1) { mouse_button = 3; }
                        if (env.Button.Button == Sdl.ButtonX2) { mouse_button = 4; }
                        if (mouse_button == -1)
                            break;

                        io.AddMouseSourceEvent(env.Button.Which == unchecked((uint)-1) ? ImGuiMouseSource.TouchScreen : ImGuiMouseSource.Mouse);
                        io.AddMouseButtonEvent(mouse_button, env.Type == (int)EventType.Mousebuttondown);
                        bd->MouseButtonsDown = env.Type == (int)EventType.Mousebuttondown ? bd->MouseButtonsDown | 1 << mouse_button : bd->MouseButtonsDown & ~(1 << mouse_button);
                        return true;
                    }
                case EventType.Textinput:
                    {
                        io.AddInputCharactersUTF8(env.Text.Text);
                        return true;
                    }
                case EventType.Keydown:
                case EventType.Keyup:
                    {
                        UpdateKeyModifiers((Keymod)env.Key.Keysym.Mod);
                        ImGuiKey key = KeycodeToImGuiKey(env.Key.Keysym.Sym);
                        io.AddKeyEvent(key, env.Type == (int)EventType.Keydown);
                        io.SetKeyEventNativeData(key, env.Key.Keysym.Sym, (int)env.Key.Keysym.Scancode, (int)env.Key.Keysym.Scancode); // To support legacy indexing (<1.87 user code). Legacy backend uses SDLK_*** as indices to IsKeyXXX() functions.
                        return true;
                    }

                case EventType.Displayevent:
                    {
                        // 2.0.26 has SDL_DISPLAYEVENT_CONNECTED/SDL_DISPLAYEVENT_DISCONNECTED/SDL_DISPLAYEVENT_ORIENTATION,
                        // so change of DPI/Scaling are not reflected in this event. (SDL3 has it)
                        bd->WantUpdateMonitors = true;
                        return true;
                    }

                case EventType.Windowevent:
                    {
                        // - When capturing mouse, SDL will send a bunch of conflicting LEAVE/ENTER event on every mouse move, but the final ENTER tends to be right.
                        // - However we won't get a correct LEAVE event for a captured window.
                        // - In some cases, when detaching a window from main viewport SDL may send SDL_WINDOWEVENT_ENTER one frame too late,
                        //   causing SDL_WINDOWEVENT_LEAVE on previous frame to interrupt drag operation by clear mouse position. This is why
                        //   we delay process the SDL_WINDOWEVENT_LEAVE events by one frame. See issue #5012 for details.
                        WindowEventID window_event = (WindowEventID)env.Window.Event;
                        if (window_event == WindowEventID.Enter)
                        {
                            bd->MouseWindowID = env.Window.WindowID;
                            bd->MouseLastLeaveFrame = 0;
                        }
                        if (window_event == WindowEventID.Leave)
                            bd->MouseLastLeaveFrame = ImGui.GetFrameCount() + 1;
                        if (window_event == WindowEventID.FocusGained)
                        {
                            io.AddFocusEvent(true);
                        }
                        else if (window_event == WindowEventID.FocusLost)
                        {
                            io.AddFocusEvent(false);
                        }

                        Trace.WriteLine(window_event);

                        if (window_event == WindowEventID.Close || window_event == WindowEventID.Moved || window_event == WindowEventID.SizeChanged)
                        {
                            ImGuiViewport* viewport = ImGui.FindViewportByPlatformHandle(sdl.GetWindowFromID(env.Window.WindowID));

                            if (viewport != null)
                            {
                                if (window_event == WindowEventID.Close)
                                    viewport->PlatformRequestClose = 1;
                                if (window_event == WindowEventID.Moved)
                                    viewport->PlatformRequestMove = 1;
                                if (window_event == WindowEventID.SizeChanged)
                                    viewport->PlatformRequestResize = 1;
                                return true;
                            }
                        }

                        return true;
                    }

                case EventType.Controllerdeviceadded:
                case EventType.Controllerdeviceremoved:
                    {
                        bd->WantUpdateGamepadsList = true;
                        return true;
                    }
            }

            return false;
        }

        public static unsafe bool Init(Window* window, Renderer* renderer, void* sdlGLContext)
        {
            var io = ImGui.GetIO();
            Trace.Assert(io.BackendPlatformUserData == null, "Already initialized a platform backend!");

            App.RegisterHook(ProcessEvent);

            bool mouse_can_use_global_state = false;
            string sdl_backend = sdl.GetCurrentVideoDriverS();
            string[] global_mouse_whitelist = { "windows", "cocoa", "x11", "DIVE", "VMAN" };
            for (int n = 0; n < global_mouse_whitelist.Length; n++)
                if (sdl_backend == global_mouse_whitelist[n])
                    mouse_can_use_global_state = true;

            BackendData* bd = AllocT<BackendData>();
            ZeroMemoryT(bd);
            io.BackendPlatformUserData = bd;
            io.BackendPlatformName = "ImGui_SDL2_Platform".ToUTF8Ptr();
            io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors | ImGuiBackendFlags.HasSetMousePos;

            if (mouse_can_use_global_state)
                io.BackendFlags |= ImGuiBackendFlags.PlatformHasViewports;

            bd->Window = window;
            bd->Renderer = renderer;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                bd->MouseCanReportHoveredViewport = false;
            }
            else
            {
                bd->MouseCanReportHoveredViewport = bd->MouseCanUseGlobalState;
            }
            bd->WantUpdateMonitors = true;

            bd->MouseCursors = (Cursor**)AllocArray((uint)ImGuiMouseCursor.Count);
            bd->MouseCursors[(int)ImGuiMouseCursor.Arrow] = sdl.CreateSystemCursor(SystemCursor.SystemCursorArrow);
            bd->MouseCursors[(int)ImGuiMouseCursor.TextInput] = sdl.CreateSystemCursor(SystemCursor.SystemCursorIbeam);
            bd->MouseCursors[(int)ImGuiMouseCursor.ResizeAll] = sdl.CreateSystemCursor(SystemCursor.SystemCursorSizeall);
            bd->MouseCursors[(int)ImGuiMouseCursor.ResizeNs] = sdl.CreateSystemCursor(SystemCursor.SystemCursorSizens);
            bd->MouseCursors[(int)ImGuiMouseCursor.ResizeEw] = sdl.CreateSystemCursor(SystemCursor.SystemCursorSizewe);
            bd->MouseCursors[(int)ImGuiMouseCursor.ResizeNesw] = sdl.CreateSystemCursor(SystemCursor.SystemCursorSizenesw);
            bd->MouseCursors[(int)ImGuiMouseCursor.ResizeNwse] = sdl.CreateSystemCursor(SystemCursor.SystemCursorSizenwse);
            bd->MouseCursors[(int)ImGuiMouseCursor.Hand] = sdl.CreateSystemCursor(SystemCursor.SystemCursorHand);
            bd->MouseCursors[(int)ImGuiMouseCursor.NotAllowed] = sdl.CreateSystemCursor(SystemCursor.SystemCursorNo);

            // Set platform dependent data in viewport
            // Our mouse update function expect PlatformHandle to be filled for the main viewport
            ImGuiViewport* main_viewport = ImGui.GetMainViewport();
            main_viewport->PlatformHandle = window;
            main_viewport->PlatformHandleRaw = null;
            SysWMInfo info;
            sdl.GetVersion(&info.Version);
            if (sdl.GetWindowWMInfo(window, &info))
            {
                if (sdl_backend == "windows")
                {
                    main_viewport->PlatformHandleRaw = (void*)info.Info.Win.Hwnd;
                }
                else if (sdl_backend == "cocoa")
                {
                    main_viewport->PlatformHandleRaw = info.Info.Cocoa.Window;
                }
            }

            // From 2.0.5: Set SDL hint to receive mouse click events on window focus, otherwise SDL doesn't emit the event.
            // Without this, when clicking to gain focus, our widgets wouldn't activate even though they showed as hovered.
            // (This is unfortunately a global SDL setting, so enabling it might have a side-effect on your application.
            // It is unlikely to make a difference, but if your app absolutely needs to ignore the initial on-focus click:
            // you can ignore SDL_MOUSEBUTTONDOWN events coming right after a SDL_WINDOWEVENT_FOCUS_GAINED)
            sdl.SetHint(Sdl.HintMouseFocusClickthrough, "1");

            // From 2.0.18: Enable native IME.
            // IMPORTANT: This is used at the time of SDL_CreateWindow() so this will only affects secondary windows, if any.
            // For the main window to be affected, your application needs to call this manually before calling SDL_CreateWindow().
            sdl.SetHint(Sdl.HintImeShowUI, "1");

            // From 2.0.22: Disable auto-capture, this is preventing drag and drop across multiple windows (see #5710)
            sdl.SetHint(Sdl.HintMouseAutoCapture, "0");

            // We need SDL_CaptureMouse(), SDL_GetGlobalMouseState() from SDL 2.0.4+ to support multiple viewports.
            // We left the call to ImGui_ImplSDL2_InitPlatformInterface() outside of #ifdef to avoid unused-function warnings.
            if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0 && (io.BackendFlags & ImGuiBackendFlags.PlatformHasViewports) != 0)
                InitPlatformInterface(window, sdlGLContext);

            return true;
        }

        public static unsafe bool InitForOpenGL(Window* window, void* sdl_gl_context)
        {
            return Init(window, null, sdl_gl_context);
        }

        public static unsafe bool InitForVulkan(Window* window)
        {
            if (!Init(window, null, null))
                return false;
            BackendData* bd = GetBackendData();
            bd->UseVulkan = true;
            return true;
        }

        public static unsafe bool InitForD3D(Window* window)
        {
            return Init(window, null, null);
        }

        public static unsafe bool InitForMetal(Window* window)
        {
            return Init(window, null, null);
        }

        public static unsafe bool InitForSDLRenderer(Window* window, Renderer* renderer)
        {
            return Init(window, renderer, null);
        }

        public static unsafe void Shutdown()
        {
            BackendData* bd = GetBackendData();
            Trace.Assert(bd != null, "No platform backend to shutdown, or already shutdown?");
            var io = ImGui.GetIO();

            ShutdownPlatformInterface();

            if (bd->ClipboardTextData != null)
                sdl.Free(bd->ClipboardTextData);
            for (ImGuiMouseCursor cursor_n = 0; cursor_n < ImGuiMouseCursor.Count; cursor_n++)
                sdl.FreeCursor(bd->MouseCursors[(int)cursor_n]);
            Free(bd->MouseCursors);
            CloseGamepads();

            io.BackendPlatformName = null;
            io.BackendPlatformUserData = null;
            io.BackendFlags &= ~(ImGuiBackendFlags.HasMouseCursors | ImGuiBackendFlags.HasSetMousePos | ImGuiBackendFlags.HasGamepad | ImGuiBackendFlags.PlatformHasViewports | ImGuiBackendFlags.HasMouseHoveredViewport);
            Free(bd);
        }

        private static unsafe void UpdateMouseData()
        {
            var bd = GetBackendData();
            var io = ImGui.GetIO();

            sdl.CaptureMouse(bd->MouseButtonsDown != 0 ? SdlBool.True : SdlBool.False);
            Window* focused_window = sdl.GetKeyboardFocus();
            bool isAppFocused = focused_window != null && (bd->Window == focused_window || !ImGui.FindViewportByPlatformHandle(focused_window).IsNull);

            if (isAppFocused)
            {
                if (io.WantSetMousePos)
                {
                    // (Optional) Set OS mouse position from Dear ImGui if requested (rarely used, only when ImGuiConfigFlags_NavEnableSetMousePos is enabled by user)
                    if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
                    {
                        sdl.WarpMouseGlobal((int)io.MousePos.X, (int)io.MousePos.Y);
                    }
                    else
                    {
                        sdl.WarpMouseInWindow(focused_window, (int)io.MousePos.X, (int)io.MousePos.Y);
                    }
                }

                // (Optional) Fallback to provide mouse position when focused (SDL_MOUSEMOTION already provides this when hovered or captured)
                if (bd->MouseCanUseGlobalState && bd->MouseButtonsDown == 0)
                {
                    // Single-viewport mode: mouse position in client window coordinates (io.MousePos is (0,0) when the mouse is on the upper-left corner of the app window)
                    // Multi-viewport mode: mouse position in OS absolute coordinates (io.MousePos is (0,0) when the mouse is on the upper-left of the primary monitor)
                    int gx, gy;
                    sdl.GetGlobalMouseState(&gx, &gy);
                    var global = new Vector2(gx, gy);
                    if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) == 0)
                    {
                        int x, y;
                        sdl.GetWindowPosition(focused_window, &x, &y);
                        global.X -= x;
                        global.Y -= y;
                    }
                    io.AddMousePosEvent(global.X, global.Y);
                }
            }

            // (Optional) When using multiple viewports: call io.AddMouseViewportEvent() with the viewport the OS mouse cursor is hovering.
            // If ImGuiBackendFlags_HasMouseHoveredViewport is not set by the backend, Dear imGui will ignore this field and infer the information using its flawed heuristic.
            // - [!] SDL backend does NOT correctly ignore viewports with the _NoInputs flag.
            //       Some backend are not able to handle that correctly. If a backend report an hovered viewport that has the _NoInputs flag (e.g. when dragging a window
            //       for docking, the viewport has the _NoInputs flag in order to allow us to find the viewport under), then Dear ImGui is forced to ignore the value reported
            //       by the backend, and use its flawed heuristic to guess the viewport behind.
            // - [X] SDL backend correctly reports this regardless of another viewport behind focused and dragged from (we need this to find a useful drag and drop target).
            if ((io.BackendFlags & ImGuiBackendFlags.HasMouseHoveredViewport) != 0)
            {
                uint mouse_viewport_id = 0;
                Window* sdl_mouse_window = sdl.GetWindowFromID(bd->MouseWindowID);
                if (sdl_mouse_window != null)
                {
                    ImGuiViewport* mouse_viewport = ImGui.FindViewportByPlatformHandle(sdl_mouse_window);
                    if (mouse_viewport != null)
                    {
                        mouse_viewport_id = mouse_viewport->ID;
                    }
                }

                io.AddMouseViewportEvent(mouse_viewport_id);
            }
        }

        private static unsafe void UpdateMouseCursor()
        {
            var io = ImGui.GetIO();
            if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0)
            {
                return;
            }

            var bd = GetBackendData();

            var imgui_cursor = ImGui.GetMouseCursor();
            if (io.MouseDrawCursor || imgui_cursor == ImGuiMouseCursor.None)
            {
                // Hide OS mouse cursor if imgui is drawing it or if it wants no cursor
                sdl.ShowCursor((int)SdlBool.False);
            }
            else
            {
                // Show OS mouse cursor
                Cursor* expected_cursor = bd->MouseCursors[(int)imgui_cursor] != null ? bd->MouseCursors[(int)imgui_cursor] : bd->MouseCursors[(int)ImGuiMouseCursor.Arrow];
                if (bd->LastMouseCursor != expected_cursor)
                {
                    sdl.SetCursor(expected_cursor); // SDL function doesn't have an early out (see #6113)
                    bd->LastMouseCursor = expected_cursor;
                }
                sdl.ShowCursor((int)SdlBool.True);
            }
        }

        private static unsafe void CloseGamepads()
        {
            var bd = GetBackendData();
            if (bd->GamepadMode != GamepadMode.Manual)
            {
                for (int i = 0; i < bd->Gamepads.Size; i++)
                {
                    GameController* gamepad = bd->Gamepads[i];
                    sdl.GameControllerClose(gamepad);
                }
            }

            bd->Gamepads.Resize(0);
        }

        public static unsafe void SetGamepadMode(GamepadMode mode, GameController** manual_gamepads_array, int manual_gamepads_count)
        {
            BackendData* bd = GetBackendData();
            CloseGamepads();
            if (mode == GamepadMode.Manual)
            {
                Debug.Assert(manual_gamepads_array != null && manual_gamepads_count > 0);
                for (int n = 0; n < manual_gamepads_count; n++)
                    bd->Gamepads.PushBack(manual_gamepads_array[n]);
            }
            else
            {
                Debug.Assert(manual_gamepads_array == null && manual_gamepads_count <= 0);
                bd->WantUpdateGamepadsList = true;
            }
            bd->GamepadMode = mode;
        }

        private static unsafe void UpdateGamepadButton(BackendData* bd, ImGuiIO* io, ImGuiKey key, GameControllerButton button_no)
        {
            bool merged_value = false;
            for (int i = 0; i < bd->Gamepads.Size; i++)
            {
                GameController* gamepad = bd->Gamepads[i];
                merged_value |= sdl.GameControllerGetButton(gamepad, button_no) != 0;
            }

            io->AddKeyEvent(key, merged_value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Saturate(float v)
        {
            return v < 0.0f ? 0.0f : v > 1.0f ? 1.0f : v;
        }

        private static unsafe void UpdateGamepadAnalog(BackendData* bd, ImGuiIO* io, ImGuiKey key, GameControllerAxis axis_no, float v0, float v1)
        {
            float merged_value = 0.0f;
            for (int i = 0; i < bd->Gamepads.Size; i++)
            {
                GameController* gamepad = bd->Gamepads[i];
                float vn = Saturate((float)(sdl.GameControllerGetAxis(gamepad, axis_no) - v0) / (float)(v1 - v0));
                if (merged_value < vn)
                    merged_value = vn;
            }
            io->AddKeyAnalogEvent(key, merged_value > 0.1f, merged_value);
        }

        private static unsafe void UpdateGamepads()
        {
            BackendData* bd = GetBackendData();
            ImGuiIOPtr io = ImGui.GetIO();

            // Update list of controller(s) to use
            if (bd->WantUpdateGamepadsList && bd->GamepadMode != GamepadMode.Manual)
            {
                CloseGamepads();
                int joystick_count = sdl.NumJoysticks();
                for (int n = 0; n < joystick_count; n++)
                {
                    if (sdl.IsGameController(n) == SdlBool.True)
                    {
                        GameController* gamepad = sdl.GameControllerOpen(n);
                        if (gamepad != null)
                        {
                            bd->Gamepads.PushBack(gamepad);
                            if (bd->GamepadMode == GamepadMode.AutoFirst)
                            {
                                break;
                            }
                        }
                    }
                }

                bd->WantUpdateGamepadsList = false;
            }

            // FIXME: Technically feeding gamepad shouldn't depend on this now that they are regular inputs.
            if ((io.ConfigFlags & ImGuiConfigFlags.NavEnableGamepad) == 0)
                return;
            io.BackendFlags &= ~ImGuiBackendFlags.HasGamepad;
            if (bd->Gamepads.Size == 0)
                return;
            io.BackendFlags |= ImGuiBackendFlags.HasGamepad;

            // Update gamepad inputs
            const int thumb_dead_zone = 8000; // SDL_gamecontroller.h suggests using this value.
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadStart, GameControllerButton.Start);
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadBack, GameControllerButton.Back);
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadFaceLeft, GameControllerButton.X);              // Xbox X, PS Square
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadFaceRight, GameControllerButton.B);              // Xbox B, PS Circle
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadFaceUp, GameControllerButton.Y);              // Xbox Y, PS Triangle
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadFaceDown, GameControllerButton.A);              // Xbox A, PS Cross
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadDpadLeft, GameControllerButton.DpadLeft);
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadDpadRight, GameControllerButton.DpadRight);
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadDpadUp, GameControllerButton.DpadUp);
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadDpadDown, GameControllerButton.DpadDown);
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadL1, GameControllerButton.Leftshoulder);
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadR1, GameControllerButton.Rightshoulder);
            UpdateGamepadAnalog(bd, io, ImGuiKey.GamepadL2, GameControllerAxis.Triggerleft, 0.0f, 32767);
            UpdateGamepadAnalog(bd, io, ImGuiKey.GamepadR2, GameControllerAxis.Triggerright, 0.0f, 32767);
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadL3, GameControllerButton.Leftstick);
            UpdateGamepadButton(bd, io, ImGuiKey.GamepadR3, GameControllerButton.Rightstick);
            UpdateGamepadAnalog(bd, io, ImGuiKey.GamepadLStickLeft, GameControllerAxis.Leftx, -thumb_dead_zone, -32768);
            UpdateGamepadAnalog(bd, io, ImGuiKey.GamepadLStickRight, GameControllerAxis.Leftx, +thumb_dead_zone, +32767);
            UpdateGamepadAnalog(bd, io, ImGuiKey.GamepadLStickUp, GameControllerAxis.Lefty, -thumb_dead_zone, -32768);
            UpdateGamepadAnalog(bd, io, ImGuiKey.GamepadLStickDown, GameControllerAxis.Lefty, +thumb_dead_zone, +32767);
            UpdateGamepadAnalog(bd, io, ImGuiKey.GamepadRStickLeft, GameControllerAxis.Rightx, -thumb_dead_zone, -32768);
            UpdateGamepadAnalog(bd, io, ImGuiKey.GamepadRStickRight, GameControllerAxis.Rightx, +thumb_dead_zone, +32767);
            UpdateGamepadAnalog(bd, io, ImGuiKey.GamepadRStickUp, GameControllerAxis.Righty, -thumb_dead_zone, -32768);
            UpdateGamepadAnalog(bd, io, ImGuiKey.GamepadRStickDown, GameControllerAxis.Righty, +thumb_dead_zone, +32767);
        }

        /// <summary>
        /// FIXME: Note that doesn't update with DPI/Scaling change only as SDL2 doesn't have an event for it (SDL3 has).
        /// </summary>
        private static unsafe void UpdateMonitors()
        {
            BackendData* bd = GetBackendData();
            ImGuiPlatformIO* platform_io = ImGui.GetPlatformIO();
            ImVector<ImGuiPlatformMonitor>* monitors = &platform_io->Monitors;
            monitors->Resize(0);
            bd->WantUpdateMonitors = false;
            int display_count = sdl.GetNumVideoDisplays();
            for (int n = 0; n < display_count; n++)
            {
                // Warning: the validity of monitor DPI information on Windows depends on the application DPI awareness settings, which generally needs to be set in the manifest or at runtime.
                ImGuiPlatformMonitor monitor = default;
                Rectangle<int> r;
                sdl.GetDisplayBounds(n, &r);
                monitor.MainPos = monitor.WorkPos = new Vector2(r.Origin.X, r.Origin.Y);
                monitor.MainSize = monitor.WorkSize = new Vector2(r.Size.X, r.Size.Y);

                sdl.GetDisplayUsableBounds(n, &r);
                monitor.WorkPos = new(r.Origin.X, r.Origin.Y);
                monitor.WorkSize = new(r.Size.X, r.Size.Y);

                float dpi;
                sdl.GetDisplayDPI(n, &dpi, null, null);
                monitor.DpiScale = dpi / 96.0f;

                monitors->PushBack(monitor);
            }
        }

        public static unsafe void NewFrame()
        {
            var bd = GetBackendData();
            var io = ImGui.GetIO();
            int w, h, displayW, displayH;
            sdl.GetWindowSize(bd->Window, &w, &h);
            if (((WindowFlags)sdl.GetWindowFlags(bd->Window) & WindowFlags.Minimized) != 0)
            {
                w = h = 0;
            }
            if (bd->Renderer != null)
            {
                sdl.GetRendererOutputSize(bd->Renderer, &displayW, &displayH);
            }
            else
            {
                sdl.GLGetDrawableSize(bd->Window, &displayW, &displayH);
            }

            io.DisplaySize = new(w, h);
            if (w > 0 && h > 0)
                io.DisplayFramebufferScale = new((float)displayW / w, (float)displayH / h);

            if (bd->WantUpdateMonitors)
                UpdateMonitors();

            io.DeltaTime = Time.Delta;

            if (bd->MouseLastLeaveFrame != 0 && bd->MouseLastLeaveFrame >= ImGui.GetFrameCount() && bd->MouseButtonsDown == 0)
            {
                bd->MouseWindowID = 0;
                bd->MouseLastLeaveFrame = 0;
                io.AddMousePosEvent(-float.MaxValue, -float.MaxValue);
            }

            var mouseCursor = ImGui.GetIO().MouseDrawCursor ? ImGuiMouseCursor.None : ImGui.GetMouseCursor();

            if (bd->MouseCanReportHoveredViewport && ImGui.GetDragDropPayload().IsNull)
                io.BackendFlags |= ImGuiBackendFlags.HasMouseHoveredViewport;
            else
                io.BackendFlags &= ~ImGuiBackendFlags.HasMouseHoveredViewport;

            UpdateMouseData();
            UpdateMouseCursor();

            // Update game controllers (if enabled and available)
            UpdateGamepads();
        }

        //--------------------------------------------------------------------------------------------------------
        // MULTI-VIEWPORT / PLATFORM INTERFACE SUPPORT
        // This is an _advanced_ and _optional_ feature, allowing the backend to create and handle multiple viewports simultaneously.
        // If you are new to dear imgui or creating a new binding for dear imgui, it is recommended that you completely ignore this section first..
        //--------------------------------------------------------------------------------------------------------

        // Helper structure we store in the void* RendererUserData field of each ImGuiViewport to easily retrieve our backend data.
        private unsafe struct ViewportData
        {
            public Window* Window;
            public uint WindowID;
            public bool WindowOwned;
            public void* GLContext;
        }

        private static unsafe void CreateWindow(ImGuiViewport* viewport)
        {
            BackendData* bd = GetBackendData();
            ViewportData* vd = AllocT<ViewportData>();
            ZeroMemoryT(vd);
            viewport->PlatformUserData = vd;

            ImGuiViewport* main_viewport = ImGui.GetMainViewport();
            ViewportData* main_viewport_data = (ViewportData*)main_viewport->PlatformUserData;

            // Share GL resources with main context
            bool use_opengl = main_viewport_data->GLContext != null;
            void* backup_context = null;
            if (use_opengl)
            {
                backup_context = sdl.GLGetCurrentContext();
                sdl.GLSetAttribute(GLattr.ShareWithCurrentContext, 1);
                sdl.GLMakeCurrent(main_viewport_data->Window, main_viewport_data->GLContext);
            }

            WindowFlags sdl_flags = 0;
            sdl_flags |= use_opengl ? WindowFlags.Opengl : bd->UseVulkan ? WindowFlags.Vulkan : 0;
            sdl_flags |= (WindowFlags)sdl.GetWindowFlags(bd->Window) & WindowFlags.AllowHighdpi;
            sdl_flags |= WindowFlags.Hidden;
            sdl_flags |= (viewport->Flags & ImGuiViewportFlags.NoDecoration) != 0 ? WindowFlags.Borderless : 0;
            sdl_flags |= (viewport->Flags & ImGuiViewportFlags.NoDecoration) != 0 ? 0 : WindowFlags.Resizable;
            sdl_flags |= (viewport->Flags & ImGuiViewportFlags.NoTaskBarIcon) != 0 ? WindowFlags.SkipTaskbar : 0;
            sdl_flags |= (viewport->Flags & ImGuiViewportFlags.TopMost) != 0 ? WindowFlags.AlwaysOnTop : 0;

            vd->Window = sdl.CreateWindow("No Title Yet", (int)viewport->Pos.X, (int)viewport->Pos.Y, (int)viewport->Size.X, (int)viewport->Size.Y, (uint)sdl_flags);
            vd->WindowOwned = true;
            if (use_opengl)
            {
                vd->GLContext = sdl.GLCreateContext(vd->Window);
                sdl.GLSetSwapInterval(0);
            }
            if (use_opengl && backup_context != null)
                sdl.GLMakeCurrent(vd->Window, backup_context);

            viewport->PlatformHandle = vd->Window;
            viewport->PlatformHandleRaw = null;
            var sdl_backend = sdl.GetCurrentVideoDriverS();
            SysWMInfo info;
            sdl.GetVersion(&info.Version);
            if (sdl.GetWindowWMInfo(vd->Window, &info))
            {
                if (sdl_backend == "windows")
                {
                    viewport->PlatformHandleRaw = (void*)info.Info.Win.Hwnd;
                }
                else if (sdl_backend == "cocoa")
                {
                    viewport->PlatformHandleRaw = info.Info.Cocoa.Window;
                }
            }
        }

        private static unsafe void DestroyWindow(ImGuiViewport* viewport)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            if (vd != null)
            {
                if (vd->GLContext != null && vd->WindowOwned)
                    sdl.GLDeleteContext(vd->GLContext);
                if (vd->Window != null && vd->WindowOwned)
                    sdl.DestroyWindow(vd->Window);
                vd->GLContext = null;
                vd->Window = null;
                Free(vd);
            }
            viewport->PlatformUserData = viewport->PlatformHandle = null;
        }

        private static unsafe void ShowWindow(ImGuiViewport* viewport)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            sdl.ShowWindow(vd->Window);
        }

        private static unsafe Vector2* GetWindowPos(Vector2* size, ImGuiViewport* viewport)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            int x = 0, y = 0;
            sdl.GetWindowPosition(vd->Window, &x, &y);
            *size = new Vector2(x, y);
            return size;
        }

        private static unsafe void SetWindowPos(ImGuiViewport* viewport, Vector2 pos)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            sdl.SetWindowPosition(vd->Window, (int)pos.X, (int)pos.Y);
        }

        private static unsafe Vector2* GetWindowSize(Vector2* size, ImGuiViewport* viewport)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            int w = 0, h = 0;
            sdl.GetWindowSize(vd->Window, &w, &h);
            *size = new Vector2(w, h);
            return size;
        }

        private static unsafe void SetWindowSize(ImGuiViewport* viewport, Vector2 size)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            sdl.SetWindowSize(vd->Window, (int)size.X, (int)size.Y);
        }

        private static unsafe void SetWindowTitle(ImGuiViewport* viewport, byte* title)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            sdl.SetWindowTitle(vd->Window, title);
        }

        private static unsafe void SetWindowAlpha(ImGuiViewport* viewport, float alpha)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            sdl.SetWindowOpacity(vd->Window, alpha);
        }

        private static unsafe void SetWindowFocus(ImGuiViewport* viewport)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            sdl.RaiseWindow(vd->Window);
        }

        private static unsafe byte GetWindowFocus(ImGuiViewport* viewport)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            var focused = ((WindowFlags)sdl.GetWindowFlags(vd->Window) & WindowFlags.InputFocus) != 0;
            return (byte)(focused ? 1 : 0);
        }

        private static unsafe byte GetWindowMinimized(ImGuiViewport* viewport)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            var minimized = ((WindowFlags)sdl.GetWindowFlags(vd->Window) & WindowFlags.Minimized) != 0;
            return (byte)(minimized ? 1 : 0);
        }

        private static unsafe void RenderWindow(ImGuiViewport* viewport, void* unknown)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            if (vd->GLContext != null)
                sdl.GLMakeCurrent(vd->Window, vd->GLContext);
        }

        private static unsafe void SwapBuffers(ImGuiViewport* viewport, void* unknown)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            if (vd->GLContext != null)
            {
                sdl.GLMakeCurrent(vd->Window, vd->GLContext);
                sdl.GLSwapWindow(vd->Window);
            }
        }

        private static unsafe int CreateVkSurface(ImGuiViewport* viewport, ulong vk_instance, void* vk_allocator, ulong* out_vk_surface)
        {
            ViewportData* vd = (ViewportData*)viewport->PlatformUserData;
            SdlBool ret = sdl.VulkanCreateSurface(vd->Window, *(VkHandle*)&vk_instance, (VkNonDispatchableHandle*)out_vk_surface);
            return ret == SdlBool.True ? 0 : 1; // ret ? VK_SUCCESS : VK_NOT_READY
        }

        private static unsafe void InitPlatformInterface(Window* window, void* sdl_gl_context)
        {
            ImGuiPlatformIO* platform_io = ImGui.GetPlatformIO();

            platform_io->PlatformCreateWindow = (void*)Marshal.GetFunctionPointerForDelegate<PlatformCreateWindow>(CreateWindow);
            platform_io->PlatformDestroyWindow = (void*)Marshal.GetFunctionPointerForDelegate<PlatformDestroyWindow>(DestroyWindow);
            platform_io->PlatformShowWindow = (void*)Marshal.GetFunctionPointerForDelegate<PlatformShowWindow>(ShowWindow);
            platform_io->PlatformSetWindowPos = (void*)Marshal.GetFunctionPointerForDelegate<PlatformSetWindowPos>(SetWindowPos);
            platform_io->PlatformGetWindowPos = (void*)Marshal.GetFunctionPointerForDelegate<PlatformGetWindowPos>(GetWindowPos);
            platform_io->PlatformSetWindowSize = (void*)Marshal.GetFunctionPointerForDelegate<PlatformSetWindowSize>(SetWindowSize);
            platform_io->PlatformGetWindowSize = (void*)Marshal.GetFunctionPointerForDelegate<PlatformGetWindowSize>(GetWindowSize);
            platform_io->PlatformSetWindowFocus = (void*)Marshal.GetFunctionPointerForDelegate<PlatformSetWindowFocus>(DestroyWindow);
            platform_io->PlatformGetWindowFocus = (void*)Marshal.GetFunctionPointerForDelegate<PlatformGetWindowFocus>(GetWindowFocus);
            platform_io->PlatformGetWindowMinimized = (void*)Marshal.GetFunctionPointerForDelegate<PlatformGetWindowMinimized>(GetWindowMinimized);
            platform_io->PlatformSetWindowTitle = (void*)Marshal.GetFunctionPointerForDelegate<PlatformSetWindowTitle>(SetWindowTitle);
            platform_io->PlatformRenderWindow = (void*)Marshal.GetFunctionPointerForDelegate<PlatformRenderWindow>(RenderWindow);
            platform_io->PlatformSwapBuffers = (void*)Marshal.GetFunctionPointerForDelegate<PlatformSwapBuffers>(SwapBuffers);
            platform_io->PlatformSetWindowAlpha = (void*)Marshal.GetFunctionPointerForDelegate<PlatformSetWindowAlpha>(SetWindowAlpha);
            platform_io->PlatformCreateVkSurface = (void*)Marshal.GetFunctionPointerForDelegate<PlatformCreateVkSurface>(CreateVkSurface);
            platform_io->PlatformSetClipboardTextFn = (void*)Marshal.GetFunctionPointerForDelegate<PlatformSetClipboardTextFn>(SetClipboardText);
            platform_io->PlatformGetClipboardTextFn = (void*)Marshal.GetFunctionPointerForDelegate<PlatformGetClipboardTextFn>(GetClipboardText);
            platform_io->PlatformClipboardUserData = null;

            platform_io->PlatformSetImeDataFn = (void*)Marshal.GetFunctionPointerForDelegate<PlatformSetImeDataFn>(SetPlatformImeData);
            platform_io->PlatformOpenInShellFn = (void*)Marshal.GetFunctionPointerForDelegate<PlatformOpenInShellFn>(OpenPlatformInShell);

            ImGuiViewport* main_viewport = ImGui.GetMainViewport().Handle;
            ViewportData* vd = AllocT<ViewportData>();
            ZeroMemoryT(vd);
            vd->Window = window;
            vd->WindowID = sdl.GetWindowID(window);
            vd->WindowOwned = false;
            vd->GLContext = sdl_gl_context;
            main_viewport->PlatformUserData = vd;
            main_viewport->PlatformHandle = vd->Window;
        }

        private static void ShutdownPlatformInterface()
        {
            ImGui.DestroyPlatformWindows();
        }
    }
}