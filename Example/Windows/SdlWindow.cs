namespace HexaEngine.Core.Windows
{
    using HexaEngine.Core.Graphics;
    using HexaEngine.Core.Input;
    using HexaEngine.Core.Input.Events;
    using HexaEngine.Core.Windows.Events;
    using HexaEngine.Mathematics;
    using Silk.NET.Core.Contexts;
    using Silk.NET.Core.Native;
    using Silk.NET.SDL;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Text;
    using Key = Input.Key;

    public enum FullscreenMode
    {
        Windowed,
        Fullscreen,
        WindowedFullscreen,
    }

    public unsafe class SdlWindow : IWindow
    {
        protected readonly Sdl Sdl = Sdl.GetApi();
        private readonly ShownEventArgs shownEventArgs = new();
        private readonly HiddenEventArgs hiddenEventArgs = new();
        private readonly ExposedEventArgs exposedEventArgs = new();
        private readonly MovedEventArgs movedEventArgs = new();
        private readonly ResizedEventArgs resizedEventArgs = new();
        private readonly SizeChangedEventArgs sizeChangedEventArgs = new();
        private readonly MinimizedEventArgs minimizedEventArgs = new();
        private readonly MaximizedEventArgs maximizedEventArgs = new();
        private readonly RestoredEventArgs restoredEventArgs = new();
        private readonly EnterEventArgs enterEventArgs = new();
        private readonly LeaveEventArgs leaveEventArgs = new();
        private readonly FocusGainedEventArgs focusGainedEventArgs = new();
        private readonly FocusLostEventArgs focusLostEventArgs = new();
        private readonly CloseEventArgs closeEventArgs = new();
        private readonly TakeFocusEventArgs takeFocusEventArgs = new();
        private readonly HitTestEventArgs hitTestEventArgs = new();
        private readonly KeyboardEventArgs keyboardEventArgs = new();
        private readonly KeyboardCharEventArgs keyboardCharEventArgs = new();
        private readonly MouseButtonEventArgs mouseButtonEventArgs = new();
        private readonly MouseMotionEventArgs mouseMotionEventArgs = new();
        private readonly MouseWheelEventArgs mouseWheelEventArgs = new();

        private Window* window;
        private bool created;
        private int width = 1280;
        private int height = 720;
        private int y = 100;
        private int x = 100;
        private bool hovering;
        private bool focused;
        private WindowState state;
        private string title = "Window";
        private bool lockCursor;
        private bool resizable = true;
        private bool bordered = true;

        private void PlatformConstruct()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(title);
            byte* ptr = (byte*)Unsafe.AsPointer(ref bytes[0]);

            WindowFlags flags = WindowFlags.Resizable | WindowFlags.Hidden | WindowFlags.AllowHighdpi;

            window = Sdl.CreateWindow(ptr, x, y, width, height, (uint)flags);
            WindowID = Sdl.GetWindowID(window);

            int w;
            int h;
            Sdl.GetWindowSize(window, &w, &h);

            Width = w;
            Height = h;
            Viewport = new(0, 0, w, h, 0, 1);
            created = true;
        }

        public void Show()
        {
            if (!created)
            {
                PlatformConstruct();
            }

            Application.RegisterWindow((IRenderWindow)this);
            Sdl.ShowWindow(window);
        }

        public void ShowHidden()
        {
            if (!created)
            {
                PlatformConstruct();
            }
        }

        public void Close()
        {
            CloseEventArgs args = new();
            OnClose(args);
        }

        public void ReleaseCapture()
        {
            Sdl.CaptureMouse(SdlBool.False);
        }

        public void Capture()
        {
            Sdl.CaptureMouse(SdlBool.True);
        }

        public void Fullscreen(FullscreenMode mode)
        {
            Sdl.SetWindowFullscreen(window, (uint)mode);
        }

        public IntPtr GetWin32HWND()
        {
            SysWMInfo wmInfo;
            Sdl.GetVersion(&wmInfo.Version);
            Sdl.GetWindowWMInfo(window, &wmInfo);
            return wmInfo.Info.Win.Hwnd;
        }

        public bool VulkanCreateSurface(VkHandle vkHandle, VkNonDispatchableHandle* vkNonDispatchableHandle)
        {
            return Sdl.VulkanCreateSurface(window, vkHandle, vkNonDispatchableHandle) == SdlBool.True;
        }

        public IGLContext OpenGLCreateContext()
        {
            return new SdlContext(Sdl, window);
        }

        public Window* GetWindow() => window;

        public uint WindowID { get; private set; }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                Sdl.SetWindowTitle(window, value);
            }
        }

        public int X
        {
            get => x;
            set
            {
                x = value;
                Sdl.SetWindowPosition(window, value, y);
            }
        }

        public int Y
        {
            get => y;
            set
            {
                y = value;
                Sdl.SetWindowPosition(window, x, value);
            }
        }

        public int Width
        {
            get => width;
            set
            {
                resizedEventArgs.OldWidth = width;
                resizedEventArgs.NewWidth = value;
                width = value;
                Sdl.SetWindowSize(window, value, height);
                Viewport = new(width, height);
                OnResized(resizedEventArgs);
            }
        }

        public int Height
        {
            get => height;
            set
            {
                resizedEventArgs.OldHeight = height;
                resizedEventArgs.NewHeight = value;
                height = value;
                Sdl.SetWindowSize(window, width, value);
                Viewport = new(width, height);
                OnResized(resizedEventArgs);
            }
        }

        public bool Hovering => hovering;

        public bool Focused => focused;

        public WindowState State
        {
            get => state;
            set
            {
                state = value;
                switch (value)
                {
                    case WindowState.Hidden:
                        Sdl.HideWindow(window);
                        break;

                    case WindowState.Normal:
                        Sdl.ShowWindow(window);
                        break;

                    case WindowState.Minimized:
                        Sdl.MinimizeWindow(window);
                        break;

                    case WindowState.Maximized:
                        Sdl.MaximizeWindow(window);
                        break;
                }
            }
        }

        public bool LockCursor
        {
            get => lockCursor;
            set
            {
                lockCursor = value;
                Sdl.SetRelativeMouseMode(value ? SdlBool.True : SdlBool.False);
            }
        }

        public bool Resizeable
        {
            get => resizable;
            set
            {
                resizable = value;
                Sdl.SetWindowResizable(window, value ? SdlBool.True : SdlBool.False);
            }
        }

        public bool Bordered
        {
            get => bordered;
            set
            {
                bordered = value;
                Sdl.SetWindowBordered(window, value ? SdlBool.True : SdlBool.False);
            }
        }

        public Viewport Viewport { get; private set; }

        #region Events

        public event EventHandler<ShownEventArgs>? Shown;

        public event EventHandler<HiddenEventArgs>? Hidden;

        public event EventHandler<ExposedEventArgs>? Exposed;

        public event EventHandler<MovedEventArgs>? Moved;

        public event EventHandler<ResizedEventArgs>? Resized;

        public event EventHandler<SizeChangedEventArgs>? SizeChanged;

        public event EventHandler<MinimizedEventArgs>? Minimized;

        public event EventHandler<MaximizedEventArgs>? Maximized;

        public event EventHandler<RestoredEventArgs>? Restored;

        public event EventHandler<EnterEventArgs>? Enter;

        public event EventHandler<LeaveEventArgs>? Leave;

        public event EventHandler<FocusGainedEventArgs>? FocusGained;

        public event EventHandler<FocusLostEventArgs>? FocusLost;

        public event EventHandler<CloseEventArgs>? Closing;

        public event EventHandler<TakeFocusEventArgs>? TakeFocus;

        public event EventHandler<HitTestEventArgs>? HitTest;

        public event EventHandler<KeyboardEventArgs>? KeyboardInput;

        public event EventHandler<KeyboardCharEventArgs>? KeyboardCharInput;

        public event EventHandler<MouseButtonEventArgs>? MouseButtonInput;

        public event EventHandler<MouseMotionEventArgs>? MouseMotionInput;

        public event EventHandler<MouseWheelEventArgs>? MouseWheelInput;

        #endregion Events

        #region EventCallMethods

        protected virtual void OnShown(ShownEventArgs args)
        {
            Shown?.Invoke(this, args);
        }

        protected virtual void OnHidden(HiddenEventArgs args)
        {
            Hidden?.Invoke(this, args);
        }

        protected virtual void OnExposed(ExposedEventArgs args)
        {
            Exposed?.Invoke(this, args);
        }

        protected virtual void OnMoved(MovedEventArgs args)
        {
            Moved?.Invoke(this, args);
        }

        protected virtual void OnResized(ResizedEventArgs args)
        {
            Resized?.Invoke(this, args);
        }

        protected virtual void OnSizeChanged(SizeChangedEventArgs args)
        {
            SizeChanged?.Invoke(this, args);
        }

        protected virtual void OnMinimized(MinimizedEventArgs args)
        {
            Minimized?.Invoke(this, args);
        }

        protected virtual void OnMaximized(MaximizedEventArgs args)
        {
            Maximized?.Invoke(this, args);
        }

        protected virtual void OnRestored(RestoredEventArgs args)
        {
            Restored?.Invoke(this, args);
        }

        protected virtual void OnEnter(EnterEventArgs args)
        {
            Enter?.Invoke(this, args);
        }

        protected virtual void OnLeave(LeaveEventArgs args)
        {
            Leave?.Invoke(this, args);
        }

        protected virtual void OnFocusGained(FocusGainedEventArgs args)
        {
            FocusGained?.Invoke(this, args);
        }

        protected virtual void OnFocusLost(FocusLostEventArgs args)
        {
            FocusLost?.Invoke(this, args);
        }

        protected virtual void OnClose(CloseEventArgs args)
        {
            Closing?.Invoke(this, args);
        }

        protected virtual void OnTakeFocus(TakeFocusEventArgs args)
        {
            TakeFocus?.Invoke(this, args);
        }

        protected virtual void OnHitTest(HitTestEventArgs args)
        {
            HitTest?.Invoke(this, args);
        }

        protected virtual void OnKeyboardInput(KeyboardEventArgs args)
        {
            KeyboardInput?.Invoke(this, args);
        }

        protected virtual void OnKeyboardCharInput(KeyboardCharEventArgs args)
        {
            KeyboardCharInput?.Invoke(this, args);
        }

        protected virtual void OnMouseButtonInput(MouseButtonEventArgs args)
        {
            MouseButtonInput?.Invoke(this, args);
        }

        protected virtual void OnMouseMotionInput(MouseMotionEventArgs args)
        {
            MouseMotionInput?.Invoke(this, args);
        }

        protected virtual void OnMouseWheelInput(MouseWheelEventArgs args)
        {
            MouseWheelInput?.Invoke(this, args);
        }

        #endregion EventCallMethods

        internal void ProcessEvent(WindowEvent evnt)
        {
            WindowEventID type = (WindowEventID)evnt.Event;
            switch (type)
            {
                case WindowEventID.None:
                    return;

                case WindowEventID.Shown:
                    {
                        shownEventArgs.Handled = false;
                        OnShown(shownEventArgs);
                        if (shownEventArgs.Handled)
                        {
                            Sdl.HideWindow(window);
                        }
                    }
                    break;

                case WindowEventID.Hidden:
                    {
                        WindowState oldState = state;
                        state = WindowState.Hidden;
                        hiddenEventArgs.OldState = oldState;
                        hiddenEventArgs.NewState = WindowState.Hidden;
                        hiddenEventArgs.Handled = false;
                        OnHidden(hiddenEventArgs);
                        if (hiddenEventArgs.Handled)
                        {
                            Sdl.ShowWindow(window);
                        }
                    }
                    break;

                case WindowEventID.Exposed:
                    {
                        OnExposed(exposedEventArgs);
                    }
                    break;

                case WindowEventID.Moved:
                    {
                        int xold = x;
                        int yold = y;
                        x = evnt.Data1;
                        y = evnt.Data2;
                        movedEventArgs.OldX = xold;
                        movedEventArgs.OldY = yold;
                        movedEventArgs.NewX = x;
                        movedEventArgs.NewY = y;
                        movedEventArgs.Handled = false;
                        OnMoved(movedEventArgs);
                        if (movedEventArgs.Handled)
                        {
                            Sdl.SetWindowPosition(window, xold, yold);
                        }
                    }
                    break;

                case WindowEventID.Resized:
                    {
                        int widthOld = width;
                        int heightOld = height;
                        width = evnt.Data1;
                        height = evnt.Data2;
                        Viewport = new(width, height);
                        resizedEventArgs.OldWidth = widthOld;
                        resizedEventArgs.OldWidth = heightOld;
                        resizedEventArgs.NewWidth = width;
                        resizedEventArgs.NewHeight = height;
                        resizedEventArgs.Handled = false;
                        OnResized(resizedEventArgs);
                        if (resizedEventArgs.Handled)
                        {
                            Sdl.SetWindowSize(window, widthOld, heightOld);
                        }
                    }
                    break;

                case WindowEventID.SizeChanged:
                    {
                        OnSizeChanged(sizeChangedEventArgs);
                    }
                    break;

                case WindowEventID.Minimized:
                    {
                        WindowState oldState = state;
                        state = WindowState.Minimized;
                        minimizedEventArgs.OldState = oldState;
                        minimizedEventArgs.NewState = WindowState.Minimized;
                        minimizedEventArgs.Handled = false;
                        OnMinimized(minimizedEventArgs);
                        if (minimizedEventArgs.Handled)
                        {
                            State = oldState;
                        }
                    }
                    break;

                case WindowEventID.Maximized:
                    {
                        WindowState oldState = state;
                        state = WindowState.Maximized;
                        maximizedEventArgs.OldState = oldState;
                        maximizedEventArgs.NewState = WindowState.Maximized;
                        maximizedEventArgs.Handled = false;
                        OnMaximized(maximizedEventArgs);
                        if (maximizedEventArgs.Handled)
                        {
                            State = oldState;
                        }
                    }
                    break;

                case WindowEventID.Restored:
                    {
                        WindowState oldState = state;
                        state = WindowState.Normal;
                        restoredEventArgs.OldState = oldState;
                        restoredEventArgs.NewState = WindowState.Normal;
                        restoredEventArgs.Handled = false;
                        OnRestored(restoredEventArgs);
                        if (restoredEventArgs.Handled)
                        {
                            State = oldState;
                        }
                    }
                    break;

                case WindowEventID.Enter:
                    {
                        hovering = true;
                        OnEnter(enterEventArgs);
                    }
                    break;

                case WindowEventID.Leave:
                    {
                        hovering = false;
                        OnLeave(leaveEventArgs);
                    }
                    break;

                case WindowEventID.FocusGained:
                    {
                        focused = true;
                        OnFocusGained(focusGainedEventArgs);
                    }
                    break;

                case WindowEventID.FocusLost:
                    {
                        focused = false;
                        OnFocusLost(focusLostEventArgs);
                    }
                    break;

                case WindowEventID.Close:
                    {
                        closeEventArgs.Handled = false;
                        OnClose(closeEventArgs);
                        if (!closeEventArgs.Handled)
                        {
                            Close();
                        }
                    }
                    break;

                case WindowEventID.TakeFocus:
                    {
                        takeFocusEventArgs.Handled = false;
                        OnTakeFocus(takeFocusEventArgs);
                        if (!takeFocusEventArgs.Handled)
                        {
                            Sdl.SetWindowInputFocus(window);
                        }
                    }
                    break;

                case WindowEventID.HitTest:
                    {
                        OnHitTest(hitTestEventArgs);
                    }
                    break;
            }
        }

        internal void ProcessInputKeyboard(KeyboardEvent evnt)
        {
            KeyState state = (KeyState)evnt.State;
            Key keyCode = (Key)Sdl.GetKeyFromScancode(evnt.Keysym.Scancode);
            keyboardEventArgs.KeyState = state;
            keyboardEventArgs.KeyCode = keyCode;
            keyboardEventArgs.Scancode = evnt.Keysym.Scancode;
            OnKeyboardInput(keyboardEventArgs);
        }

        internal void ProcessInputText(TextInputEvent evnt)
        {
            keyboardCharEventArgs.Char = (char)evnt.Text[0];
            OnKeyboardCharInput(keyboardCharEventArgs);
        }

        internal void ProcessInputMouse(MouseButtonEvent evnt)
        {
            MouseButtonState state = (MouseButtonState)evnt.State;
            MouseButton button = (MouseButton)evnt.Button;
            mouseButtonEventArgs.Button = button;
            mouseButtonEventArgs.State = state;
            mouseButtonEventArgs.Clicks = evnt.Clicks;
            OnMouseButtonInput(mouseButtonEventArgs);
        }

        internal void ProcessInputMouse(MouseMotionEvent evnt)
        {
            if (lockCursor)
            {
                Sdl.WarpMouseInWindow(window, 0, 0);
            }

            mouseMotionEventArgs.X = evnt.X;
            mouseMotionEventArgs.Y = evnt.Y;
            mouseMotionEventArgs.RelX = evnt.Xrel;
            mouseMotionEventArgs.RelY = evnt.Yrel;
            OnMouseMotionInput(mouseMotionEventArgs);
        }

        internal void ProcessInputMouse(MouseWheelEvent evnt)
        {
            mouseWheelEventArgs.Wheel = new(evnt.X, evnt.Y);
            mouseWheelEventArgs.Direction = (Input.MouseWheelDirection)evnt.Direction;
            OnMouseWheelInput(mouseWheelEventArgs);
        }

        internal void ProcessInputController()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearState()
        {
            Keyboard.Flush();
            Mouse.Flush();
        }
    }
}