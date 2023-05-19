namespace HexaEngine.Core.Input
{
    using Silk.NET.SDL;
    using System.Runtime.CompilerServices;

    public static class Joysticks
    {
        private static readonly List<Joystick> joysticks = new();
        private static readonly Dictionary<int, Joystick> idToJoystick = new();

        public static IReadOnlyList<Joystick> Sticks => joysticks;

        public static IReadOnlyDictionary<int, Joystick> IdToJoystick => idToJoystick;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AddJoystick(JoyDeviceEvent even)
        {
            Joystick joystick = new(even.Which);
            joysticks.Add(joystick);
            idToJoystick.Add(joystick.Id, joystick);
        }

        internal static void OnAxisMotion(JoyAxisEvent even)
        {
            idToJoystick[even.Which].OnAxisMotion(even);
        }

        internal static void OnBallMotion(JoyBallEvent even)
        {
            idToJoystick[even.Which].OnBallMotion(even);
        }

        internal static void OnButtonDown(JoyButtonEvent even)
        {
            idToJoystick[even.Which].OnButtonDown(even);
        }

        internal static void OnButtonUp(JoyButtonEvent even)
        {
            idToJoystick[even.Which].OnButtonUp(even);
        }

        internal static void OnHatMotion(JoyHatEvent even)
        {
            idToJoystick[even.Which].OnHatMotion(even);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void RemoveJoystick(JoyDeviceEvent even)
        {
            Joystick joystick = idToJoystick[even.Which];
            joysticks.Remove(joystick);
            idToJoystick.Remove(even.Which);
            joystick.Dispose();
        }
    }
}