namespace HexaEngine.Core.Input
{
    using Silk.NET.SDL;

    public unsafe class Haptic
    {
        private static readonly Sdl sdl = Application.sdl;
        private readonly int id;
        private readonly Silk.NET.SDL.Haptic* haptic;

        private Haptic(Silk.NET.SDL.Haptic* haptic)
        {
            this.haptic = haptic;
            id = sdl.HapticIndex(haptic);
        }

        public int Id => id;

        public string Name => sdl.HapticNameS(id);

        public int AxesCount => sdl.HapticNumAxes(haptic);

        public int EffectsCount => sdl.HapticNumEffects(haptic);

        public int EffectsPlayingCount => sdl.HapticNumEffectsPlaying(haptic);

        public bool RumbleSupported => sdl.HapticRumbleSupported(haptic) == 1;

        public HapticEffectFlags EffectsSupported => (HapticEffectFlags)sdl.HapticQuery(haptic);

        public static Haptic OpenFromGamepad(Gamepad gamepad)
        {
            return new(sdl.HapticOpenFromJoystick(gamepad.joystick));
        }

        public static Haptic OpenFromJoystick(Joystick joystick)
        {
            return new(sdl.HapticOpenFromJoystick(joystick.joystick));
        }

        public static Haptic OpenFromMouse()
        {
            return new(sdl.HapticOpenFromMouse());
        }

        public static Haptic OpenFromIndex(int index)
        {
            return new(sdl.HapticOpen(index));
        }
    }
}