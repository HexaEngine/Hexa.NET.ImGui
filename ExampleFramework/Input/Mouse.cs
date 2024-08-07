namespace ExampleFramework.Input
{
    using ExampleFramework;
    using ExampleFramework.Input.Events;
    using Hexa.NET.Mathematics;
    using Silk.NET.SDL;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using Point = Hexa.NET.Mathematics.Point2;

    public static unsafe class Mouse
    {
        private static readonly MouseButton[] buttons = Enum.GetValues<MouseButton>();
        private static readonly string[] buttonNames = Enum.GetNames<MouseButton>();

#nullable disable
        private static Sdl sdl;
#nullable enable

        private static readonly Dictionary<MouseButton, MouseButtonState> states = new();
        private static readonly MouseMotionEventArgs motionEventArgs = new();
        private static readonly MouseButtonEventArgs buttonEventArgs = new();
        private static readonly MouseWheelEventArgs wheelEventArgs = new();

        private static Point pos;
        private static Vector2 delta;
        private static Vector2 deltaWheel;

        internal static void Init()
        {
            sdl = App.sdl;
            pos = default;
            sdl.GetMouseState(ref pos.X, ref pos.Y);

            uint state = sdl.GetMouseState(null, null);
            uint maskLeft = unchecked(1 << (int)MouseButton.Left - 1);
            uint maskMiddle = unchecked(1 << (int)MouseButton.Middle - 1);
            uint maskRight = unchecked(1 << (int)MouseButton.Right - 1);
            uint maskX1 = unchecked(1 << (int)MouseButton.X1 - 1);
            uint maskX2 = unchecked(1 << (int)MouseButton.X2 - 1);
            states.Add(MouseButton.Left, (MouseButtonState)(state & maskLeft));
            states.Add(MouseButton.Middle, (MouseButtonState)(state & maskMiddle));
            states.Add(MouseButton.Right, (MouseButtonState)(state & maskRight));
            states.Add(MouseButton.X1, (MouseButtonState)(state & maskX1));
            states.Add(MouseButton.X2, (MouseButtonState)(state & maskX2));
        }

        public static Vector2 Global
        {
            get
            {
                int x, y;
                sdl.GetGlobalMouseState(&x, &y);
                return new Vector2(x, y);
            }
        }

        public static Vector2 Position => pos;

        public static Vector2 Delta => delta;

        public static Vector2 DeltaWheel => deltaWheel;

        public static IReadOnlyList<MouseButton> Buttons => buttons;

        public static IReadOnlyList<string> ButtonNames => buttonNames;

        public static IDictionary<MouseButton, MouseButtonState> States => states;

        public static event EventHandler<MouseMotionEventArgs>? Moved;

        public static event EventHandler<MouseButtonEventArgs>? ButtonDown;

        public static event EventHandler<MouseButtonEventArgs>? ButtonUp;

        public static event EventHandler<MouseWheelEventArgs>? Wheel;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDown(MouseButton button)
        {
            return states[button] == MouseButtonState.Down;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUp(MouseButton button)
        {
            return states[button] == MouseButtonState.Down;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnButtonDown(MouseButtonEvent mouseButtonEvent)
        {
            MouseButton button = (MouseButton)mouseButtonEvent.Button;
            states[button] = MouseButtonState.Down;
            buttonEventArgs.Button = button;
            buttonEventArgs.State = MouseButtonState.Down;
            buttonEventArgs.Clicks = mouseButtonEvent.Clicks;
            ButtonDown?.Invoke(null, buttonEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnButtonUp(MouseButtonEvent mouseButtonEvent)
        {
            MouseButton button = (MouseButton)mouseButtonEvent.Button;
            states[button] = MouseButtonState.Up;
            buttonEventArgs.Button = button;
            buttonEventArgs.State = MouseButtonState.Up;
            buttonEventArgs.Clicks = mouseButtonEvent.Clicks;
            ButtonUp?.Invoke(null, buttonEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnMotion(MouseMotionEvent mouseButtonEvent)
        {
            if (mouseButtonEvent.Xrel == 0 && mouseButtonEvent.Yrel == 0)
            {
                return;
            }

            delta += new Vector2(mouseButtonEvent.Xrel, mouseButtonEvent.Yrel);
            motionEventArgs.RelX = delta.X;
            motionEventArgs.RelY = delta.Y;
            motionEventArgs.X = pos.X;
            motionEventArgs.Y = pos.Y;
            Moved?.Invoke(null, motionEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnWheel(MouseWheelEvent mouseWheelEvent)
        {
            deltaWheel += new Vector2(mouseWheelEvent.X, mouseWheelEvent.Y);
            wheelEventArgs.Wheel = new Vector2(mouseWheelEvent.X, mouseWheelEvent.Y);
            wheelEventArgs.Direction = (MouseWheelDirection)mouseWheelEvent.Direction;
            Wheel?.Invoke(null, wheelEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Flush()
        {
            sdl.GetMouseState(ref pos.X, ref pos.Y);
            delta = Vector2.Zero;
            deltaWheel = Vector2.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ScreenToWorld(Matrix4x4 proj, Matrix4x4 viewInv, Viewport viewport)
        {
            var vx = (2.0f * (pos.X - viewport.X) / viewport.Width - 1.0f) / proj.M11;
            var vy = (-2.0f * (pos.Y - viewport.Y) / viewport.Height + 1.0f) / proj.M22;
            Vector3 rayDirViewSpace = new(vx, vy, 1);
            Vector3 rayDir = Vector3.TransformNormal(rayDirViewSpace, viewInv);
            return Vector3.Normalize(rayDir);
        }
    }
}