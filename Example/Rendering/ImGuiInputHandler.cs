namespace HexaEngine.Rendering
{
    using HexaEngine.Core;
    using HexaEngine.Core.Input;
    using HexaEngine.Core.Windows;
    using HexaEngine.ImGui;
    using Silk.NET.SDL;

    internal class ImGuiInputHandler : IDisposable
    {
        private readonly SdlWindow window;

        private ImGuiMouseCursor lastCursor;

        private static unsafe char* g_ClipboardTextData = null;

        public unsafe ImGuiInputHandler(SdlWindow window)
        {
            this.window = window;
            window.MouseButtonInput += MouseButtonInput;
            window.KeyboardInput += KeyboardInput;
            window.KeyboardCharInput += KeyboardCharInput;

            var io = ImGui.GetIO();

            //io->SetClipboardTextFn = (delegate*<void*, byte*, void>*)(delegate*<void*, byte*, void>)&SetClipboardText;
            //io->GetClipboardTextFn = (delegate*<void*, byte*>*)(delegate*<void*, byte*>)&GetClipboardText;
            io->ClipboardUserData = null;

            io->KeyMap[(int)ImGuiKey.Tab] = (int)Scancode.ScancodeTab;
            io->KeyMap[(int)ImGuiKey.LeftArrow] = (int)Scancode.ScancodeLeft;
            io->KeyMap[(int)ImGuiKey.RightArrow] = (int)Scancode.ScancodeRight;
            io->KeyMap[(int)ImGuiKey.UpArrow] = (int)Scancode.ScancodeUp;
            io->KeyMap[(int)ImGuiKey.DownArrow] = (int)Scancode.ScancodeDown;
            io->KeyMap[(int)ImGuiKey.PageUp] = (int)Scancode.ScancodePageup;
            io->KeyMap[(int)ImGuiKey.PageDown] = (int)Scancode.ScancodePagedown;
            io->KeyMap[(int)ImGuiKey.Home] = (int)Scancode.ScancodeHome;
            io->KeyMap[(int)ImGuiKey.End] = (int)Scancode.ScancodeEnd;
            io->KeyMap[(int)ImGuiKey.Insert] = (int)Scancode.ScancodeInsert;
            io->KeyMap[(int)ImGuiKey.Delete] = (int)Scancode.ScancodeDelete;
            io->KeyMap[(int)ImGuiKey.Backspace] = (int)Scancode.ScancodeBackspace;
            io->KeyMap[(int)ImGuiKey.Space] = (int)Scancode.ScancodeSpace;
            io->KeyMap[(int)ImGuiKey.Enter] = (int)Scancode.ScancodeReturn;
            io->KeyMap[(int)ImGuiKey.Escape] = (int)Scancode.ScancodeEscape;
            io->KeyMap[(int)ImGuiKey.KeypadEnter] = (int)Scancode.ScancodeKPEnter;
            io->KeyMap[(int)ImGuiKey.A] = (int)Scancode.ScancodeA;
            io->KeyMap[(int)ImGuiKey.C] = (int)Scancode.ScancodeC;
            io->KeyMap[(int)ImGuiKey.V] = (int)Scancode.ScancodeV;
            io->KeyMap[(int)ImGuiKey.X] = (int)Scancode.ScancodeX;
            io->KeyMap[(int)ImGuiKey.Y] = (int)Scancode.ScancodeY;
            io->KeyMap[(int)ImGuiKey.Z] = (int)Scancode.ScancodeZ;
        }

        private bool disposedValue;

        /*
        private static unsafe byte* GetClipboardText(void* user_data)
        {
            if (g_ClipboardTextData != null)
            {
                Clipboard.Free(g_ClipboardTextData);
            }

            g_ClipboardTextData = Clipboard.GetClipboardText();
            return g_ClipboardTextData;
        }

        private static unsafe void SetClipboardText(void* user_data, byte* text)
        {
            Clipboard.SetClipboardText(text);
        }
        */

        private unsafe void KeyboardCharInput(object? sender, Core.Input.Events.KeyboardCharEventArgs e)
        {
            var io = ImGui.GetIO();
            ImGui.AddInputCharacterUTF16(io, e.Char);
        }

        private unsafe void KeyboardInput(object? sender, Core.Input.Events.KeyboardEventArgs e)
        {
            var io = ImGui.GetIO();
            int key = (int)e.Scancode;

            io->KeysDown[key] = e.KeyState == KeyState.Down;

            io->KeyShift = (Keyboard.GetModState() & Keymod.Shift) != 0;

            io->KeyCtrl = (Keyboard.GetModState() & Keymod.Ctrl) != 0;
            io->KeyAlt = (Keyboard.GetModState() & Keymod.Alt) != 0;
#if WINDOWS
            io->KeySuper = false;
#else
            io->KeySuper = (Keyboard.GetModState() & Keymod.Gui) != 0;
#endif
        }

        private unsafe void MouseButtonInput(object? sender, Core.Input.Events.MouseButtonEventArgs e)
        {
            var io = ImGui.GetIO();

            var index = e.Button switch
            {
                MouseButton.Left => 0,
                MouseButton.Right => 1,
                MouseButton.Middle => 2,
                MouseButton.X1 => 3,
                MouseButton.X2 => 4,
                _ => -1,
            };
            io->MouseDown[index] = e.State == MouseButtonState.Down;

            if (e.State == MouseButtonState.Down)
            {
                window.Capture();
            }
            else
            {
                window.ReleaseCapture();
            }
        }

        public unsafe void Update()
        {
            var io = ImGui.GetIO();
            io->DisplaySize = new(window.Width, window.Height);
            io->DeltaTime = Time.Delta;

            UpdateMousePosition();

            var mouseCursor = ImGui.GetIO()->MouseDrawCursor ? ImGuiMouseCursor.None : ImGui.GetMouseCursor();
            if (mouseCursor != lastCursor)
            {
                lastCursor = mouseCursor;
                UpdateMouseCursor();
            }

            io->MouseWheel = Mouse.DeltaWheel.Y;
            io->MouseWheelH = Mouse.DeltaWheel.X;

            UpdateGamepads();
        }

        public static unsafe bool UpdateMouseCursor()
        {
            var io = ImGui.GetIO();
            if ((io->ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0)
            {
                return false;
            }

            var requestedcursor = ImGui.GetMouseCursor();
            if (requestedcursor == ImGuiMouseCursor.None || io->MouseDrawCursor)
            {
                CursorHelper.SetCursor(IntPtr.Zero);
            }
            else
            {
                var cursor = SystemCursor.SystemCursorArrow;
                switch (requestedcursor)
                {
                    case ImGuiMouseCursor.Arrow: cursor = SystemCursor.SystemCursorArrow; break;
                    case ImGuiMouseCursor.TextInput: cursor = SystemCursor.SystemCursorIbeam; break;
                    case ImGuiMouseCursor.ResizeAll: cursor = SystemCursor.SystemCursorSizeall; break;
                    case ImGuiMouseCursor.ResizeEw: cursor = SystemCursor.SystemCursorSizewe; break;
                    case ImGuiMouseCursor.ResizeNs: cursor = SystemCursor.SystemCursorSizens; break;
                    case ImGuiMouseCursor.ResizeNesw: cursor = SystemCursor.SystemCursorSizenesw; break;
                    case ImGuiMouseCursor.ResizeNwse: cursor = SystemCursor.SystemCursorSizenwse; break;
                    case ImGuiMouseCursor.Hand: cursor = SystemCursor.SystemCursorHand; break;
                    case ImGuiMouseCursor.NotAllowed: cursor = SystemCursor.SystemCursorNo; break;
                }
                CursorHelper.SetCursor(cursor);
            }

            return true;
        }

        private static Sdl Sdl = Sdl.GetApi();

        public static unsafe void UpdateGamepads()
        {
            var io = ImGui.GetIO();
            Memset(io->NavInputs, 0, 16 * sizeof(float));
            if ((io->ConfigFlags & ImGuiConfigFlags.NavEnableGamepad) == 0)
            {
                return;
            }

            // Get gamepad
            GameController* game_controller = Sdl.GameControllerOpen(0);
            if (game_controller == null)
            {
                io->BackendFlags &= ~ImGuiBackendFlags.HasGamepad;
                return;
            }

            // Update gamepad inputs
            void MAP_BUTTON(ImGuiNavInput NAV_NO, GameControllerButton BUTTON_NO)
            {
                io->NavInputs[(int)NAV_NO] = (Sdl.GameControllerGetButton(game_controller, BUTTON_NO) != 0) ? 1.0f : 0.0f;
            }
            void MAP_ANALOG(ImGuiNavInput NAV_NO, GameControllerAxis AXIS_NO, int V0, int V1)
            {
                float vn = (float)(Sdl.GameControllerGetAxis(game_controller, AXIS_NO) - V0) / (V1 - V0);
                if (vn > 1.0f)
                {
                    vn = 1.0f;
                }

                if (vn > 0.0f && io->NavInputs[(int)NAV_NO] < vn)
                {
                    io->NavInputs[(int)NAV_NO] = vn;
                }
                else
                {
                    io->NavInputs[(int)NAV_NO] = 0;
                }
            }

            const int thumb_dead_zone = 8000;           // SDL_gamecontroller.h suggests using this value.
            MAP_BUTTON(ImGuiNavInput.Activate, GameControllerButton.A);               // Cross / A
            MAP_BUTTON(ImGuiNavInput.Cancel, GameControllerButton.B);               // Circle / B
            MAP_BUTTON(ImGuiNavInput.Menu, GameControllerButton.X);               // Square / X
            MAP_BUTTON(ImGuiNavInput.ImGuiNavInput_Input, GameControllerButton.Y);               // Triangle / Y
            MAP_BUTTON(ImGuiNavInput.DpadLeft, GameControllerButton.DpadLeft);       // D-Pad Left
            MAP_BUTTON(ImGuiNavInput.DpadRight, GameControllerButton.DpadRight);      // D-Pad Right
            MAP_BUTTON(ImGuiNavInput.DpadUp, GameControllerButton.DpadUp);         // D-Pad Up
            MAP_BUTTON(ImGuiNavInput.DpadDown, GameControllerButton.DpadDown);       // D-Pad Down
            MAP_BUTTON(ImGuiNavInput.FocusPrev, GameControllerButton.Leftshoulder);    // L1 / LB
            MAP_BUTTON(ImGuiNavInput.FocusNext, GameControllerButton.Rightshoulder);   // R1 / RB
            MAP_BUTTON(ImGuiNavInput.TweakSlow, GameControllerButton.Leftshoulder);    // L1 / LB
            MAP_BUTTON(ImGuiNavInput.TweakFast, GameControllerButton.Rightshoulder);   // R1 / RB
            MAP_ANALOG(ImGuiNavInput.LStickLeft, GameControllerAxis.Leftx, -thumb_dead_zone, -32768);
            MAP_ANALOG(ImGuiNavInput.LStickRight, GameControllerAxis.Leftx, +thumb_dead_zone, +32767);
            MAP_ANALOG(ImGuiNavInput.LStickUp, GameControllerAxis.Lefty, -thumb_dead_zone, -32767);
            MAP_ANALOG(ImGuiNavInput.LStickDown, GameControllerAxis.Lefty, +thumb_dead_zone, +32767);

            io->BackendFlags |= ImGuiBackendFlags.HasGamepad;
        }

        private static unsafe void UpdateMousePosition()
        {
            var io = ImGui.GetIO();
            io->MousePos = Mouse.Position;
        }

        protected virtual unsafe void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                window.MouseButtonInput -= MouseButtonInput;
                window.KeyboardInput -= KeyboardInput;
                window.KeyboardCharInput -= KeyboardCharInput;
                if (g_ClipboardTextData != null)
                {
                    Clipboard.Free(g_ClipboardTextData);
                }

                disposedValue = true;
            }
        }

        ~ImGuiInputHandler()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}