namespace HexaEngine.Core.Input
{
    public static class Helper
    {
        public static Silk.NET.SDL.KeyCode ConvertBack(Key code)
        {
            return (Silk.NET.SDL.KeyCode)code;
        }

        public static Silk.NET.SDL.GameControllerAxis ConvertBack(GamepadAxis gamepadAxis)
        {
            return (Silk.NET.SDL.GameControllerAxis)gamepadAxis;
        }

        public static Silk.NET.SDL.GameControllerButton ConvertBack(GamepadButton gamepadButton)
        {
            return (Silk.NET.SDL.GameControllerButton)gamepadButton;
        }

        public static Key Convert(Silk.NET.SDL.KeyCode code)
        {
            return (Key)code;
        }

        public static GamepadAxis Convert(Silk.NET.SDL.GameControllerAxis axis)
        {
            return (GamepadAxis)axis;
        }

        public static GamepadButton Convert(Silk.NET.SDL.GameControllerButton button)
        {
            return (GamepadButton)button;
        }

        public static GamepadType Convert(Silk.NET.SDL.GameControllerType gameControllerType)
        {
            return (GamepadType)gameControllerType;
        }

        internal static GamepadSensorType Convert(Silk.NET.SDL.SensorType sensorType)
        {
            return (GamepadSensorType)sensorType;
        }

        internal static Silk.NET.SDL.SensorType ConvertBack(GamepadSensorType gamepadSensorType)
        {
            return (Silk.NET.SDL.SensorType)gamepadSensorType;
        }

        internal static JoystickType Convert(Silk.NET.SDL.JoystickType joystickType)
        {
            return (JoystickType)joystickType;
        }

        internal static JoystickPowerLevel Convert(Silk.NET.SDL.JoystickPowerLevel joystickPowerLevel)
        {
            return (JoystickPowerLevel)joystickPowerLevel;
        }
    }
}