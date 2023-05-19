namespace HexaEngine.Core.Windows
{
    using HexaEngine.Core.Graphics;
    using HexaEngine.Core.Input.Events;
    using HexaEngine.Core.Windows.Events;
    using HexaEngine.Mathematics;
    using Silk.NET.Core.Contexts;
    using Silk.NET.Core.Native;
    using Silk.NET.SDL;

    public interface IRenderWindow : IWindow
    {
        IGraphicsDevice Device { get; }

        IGraphicsContext Context { get; }

        ISwapChain SwapChain { get; }

        Viewport RenderViewport { get; }

        void Initialize(IGraphicsDevice graphicsDevice);

        void Render(IGraphicsContext context);

        void Uninitialize();
    }

    public interface IWindow
    {
        bool Bordered { get; set; }
        bool Focused { get; }
        int Height { get; set; }
        bool Hovering { get; }
        bool LockCursor { get; set; }
        bool Resizeable { get; set; }
        WindowState State { get; set; }
        string Title { get; set; }
        Viewport Viewport { get; }
        int Width { get; set; }
        uint WindowID { get; }
        int X { get; set; }
        int Y { get; set; }

        event EventHandler<CloseEventArgs>? Closing;

        event EventHandler<EnterEventArgs>? Enter;

        event EventHandler<ExposedEventArgs>? Exposed;

        event EventHandler<FocusGainedEventArgs>? FocusGained;

        event EventHandler<FocusLostEventArgs>? FocusLost;

        event EventHandler<HiddenEventArgs>? Hidden;

        event EventHandler<HitTestEventArgs>? HitTest;

        event EventHandler<KeyboardCharEventArgs>? KeyboardCharInput;

        event EventHandler<KeyboardEventArgs>? KeyboardInput;

        event EventHandler<LeaveEventArgs>? Leave;

        event EventHandler<MaximizedEventArgs>? Maximized;

        event EventHandler<MinimizedEventArgs>? Minimized;

        event EventHandler<MouseButtonEventArgs>? MouseButtonInput;

        event EventHandler<MouseMotionEventArgs>? MouseMotionInput;

        event EventHandler<MouseWheelEventArgs>? MouseWheelInput;

        event EventHandler<MovedEventArgs>? Moved;

        event EventHandler<ResizedEventArgs>? Resized;

        event EventHandler<RestoredEventArgs>? Restored;

        event EventHandler<ShownEventArgs>? Shown;

        event EventHandler<SizeChangedEventArgs>? SizeChanged;

        event EventHandler<TakeFocusEventArgs>? TakeFocus;

        void Capture();

        void Close();

        nint GetWin32HWND();

        unsafe bool VulkanCreateSurface(VkHandle vkHandle, VkNonDispatchableHandle* vkNonDispatchableHandle);

        IGLContext OpenGLCreateContext();

        unsafe Window* GetWindow();

        void ReleaseCapture();

        void Show();

        void ShowHidden();

        internal void ClearState();
    }
}