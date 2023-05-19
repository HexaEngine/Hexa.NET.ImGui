namespace HexaEngine.Core.Input
{
    using Silk.NET.SDL;
    using System.Runtime.CompilerServices;

    public static unsafe class Gamepads
    {
        private static readonly List<Gamepad> gamepads = new();
        private static readonly Dictionary<int, Gamepad> idToGamepads = new();

        public static IReadOnlyList<Gamepad> Controllers => gamepads;

        public static IReadOnlyDictionary<int, Gamepad> IdToGamepad => idToGamepads;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Init()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AddController(ControllerDeviceEvent even)
        {
            Gamepad gamepad = new(even.Which);
            gamepads.Add(gamepad);
            idToGamepads.Add(gamepad.Id, gamepad);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void RemoveController(ControllerDeviceEvent even)
        {
            Gamepad gamepad = idToGamepads[even.Which];
            gamepads.Remove(gamepad);
            idToGamepads.Remove(even.Which);
            gamepad.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Remapped(ControllerDeviceEvent even)
        {
            idToGamepads[even.Which].OnRemapped();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AxisMotion(ControllerAxisEvent even)
        {
            idToGamepads[even.Which].OnAxisMotion(even);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ButtonDown(ControllerButtonEvent even)
        {
            idToGamepads[even.Which].OnButtonDown(even);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ButtonUp(ControllerButtonEvent even)
        {
            idToGamepads[even.Which].OnButtonUp(even);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void TouchPadDown(ControllerTouchpadEvent even)
        {
            idToGamepads[even.Which].OnTouchPadDown(even);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void TouchPadMotion(ControllerTouchpadEvent even)
        {
            idToGamepads[even.Which].OnTouchPadDown(even);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void TouchPadUp(ControllerTouchpadEvent even)
        {
            idToGamepads[even.Which].OnTouchPadUp(even);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SensorUpdate(ControllerSensorEvent even)
        {
            idToGamepads[even.Which].OnSensorUpdate(even);
        }
    }
}