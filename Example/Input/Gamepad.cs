namespace HexaEngine.Core.Input
{
    using HexaEngine.Core.Input.Events;
    using Silk.NET.SDL;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Text;

    public unsafe class Gamepad : IDisposable
    {
        private static readonly GamepadAxis[] axes = Enum.GetValues<GamepadAxis>();
        private static readonly string[] axisNames = new string[axes.Length];
        private static readonly GamepadButton[] buttons = Enum.GetValues<GamepadButton>();
        private static readonly string[] buttonNames = new string[buttons.Length];

        private static readonly GamepadSensorType[] sensorTypes = Enum.GetValues<GamepadSensorType>();

        private readonly Sdl sdl;
        private readonly int id;
        private readonly GameController* controller;
        internal readonly Silk.NET.SDL.Joystick* joystick;
        private readonly Dictionary<GamepadAxis, short> axisStates = new();
        private readonly Dictionary<GamepadButton, GamepadButtonState> buttonStates = new();
        private readonly Dictionary<GamepadSensorType, GamepadSensor> sensors = new();
        private readonly List<GamepadTouchpad> touchpads = new();
        private readonly Haptic? haptic;
        private readonly List<string> mappings = new();

        private readonly GamepadRemappedEventArgs remappedEventArgs = new();
        private readonly GamepadAxisMotionEventArgs axisMotionEventArgs = new();
        private readonly GamepadButtonEventArgs buttonEventArgs = new();

        private readonly string guid;
        private short deadzone = 8000;

        static Gamepad()
        {
            var sdl = Application.sdl;
            for (int i = 0; i < axes.Length; i++)
            {
                axisNames[i] = sdl.GameControllerGetStringForAxisS(Helper.ConvertBack(axes[i]));
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttonNames[i] = sdl.GameControllerGetStringForButtonS(Helper.ConvertBack(buttons[i]));
            }
        }

        public Gamepad(int id)
        {
            sdl = Application.sdl;
            controller = sdl.GameControllerOpen(id);
            joystick = sdl.GameControllerGetJoystick(controller);
            this.id = sdl.JoystickInstanceID(joystick);
            var axes = Enum.GetValues<GamepadAxis>();
            for (int i = 0; i < axes.Length; i++)
            {
                if (sdl.GameControllerHasAxis(controller, Helper.ConvertBack(axes[i])) == SdlBool.True)
                {
                    axisStates.Add(axes[i], 0);
                }
            }
            var buttons = Enum.GetValues<GamepadButton>();
            for (int i = 0; i < buttons.Length; i++)
            {
                if (sdl.GameControllerHasButton(controller, Helper.ConvertBack(buttons[i])) == SdlBool.True)
                {
                    buttonStates.Add(buttons[i], GamepadButtonState.Up);
                }
            }

            var touchpadCount = sdl.GameControllerGetNumTouchpads(controller);
            for (int i = 0; i < touchpadCount; i++)
            {
                touchpads.Add(new(i, controller));
            }

            var sensorTypes = Enum.GetValues<GamepadSensorType>();
            for (int i = 0; i < sensorTypes.Length; i++)
            {
                if (sdl.GameControllerHasSensor(controller, Helper.ConvertBack(sensorTypes[i])) == SdlBool.True)
                {
                    sensors.Add(sensorTypes[i], new(controller, sensorTypes[i]));
                }
            }

            var mappingCount = sdl.GameControllerNumMappings();
            for (int i = 0; i < mappingCount; i++)
            {
                mappings.Add(sdl.GameControllerMappingForIndexS(i));
            }

            if (sdl.JoystickIsHaptic(joystick) == 1)
            {
                haptic = Haptic.OpenFromGamepad(this);
            }

            var guid = sdl.JoystickGetGUID(joystick);
            var buffer = Alloc<byte>(33);
            sdl.JoystickGetGUIDString(guid, buffer, 33);
            var size = StringSizeNullTerminated(buffer);
            var value = Encoding.ASCII.GetString(buffer, size - 1);
            Free(buffer);
            this.guid = value;
        }

        public static IReadOnlyList<GamepadAxis> Axes => axes;

        public static IReadOnlyList<string> AxisNames => axisNames;

        public static IReadOnlyList<GamepadButton> Buttons => buttons;

        public static IReadOnlyList<string> ButtonNames => buttonNames;

        public static IReadOnlyList<GamepadSensorType> SensorTypes => sensorTypes;

        public int Id => id;

        public string Name => sdl.GameControllerNameS(controller);

        public ushort Vendor => sdl.GameControllerGetVendor(controller);

        public ushort Product => sdl.GameControllerGetProduct(controller);

        public ushort ProductVersion => sdl.GameControllerGetProductVersion(controller);

        public string Serial => sdl.GameControllerGetSerialS(controller);

        public string GUID => guid;

        public bool IsAttached => sdl.GameControllerGetAttached(controller) == SdlBool.True;

        public bool IsHaptic => sdl.JoystickIsHaptic(joystick) == 1;

        public bool HasLED => sdl.GameControllerHasLED(controller) == SdlBool.True;

        public GamepadType Type => Helper.Convert(sdl.GameControllerGetType(controller));

        public short Deadzone { get => deadzone; set => deadzone = value; }

        public int PlayerIndex { get => sdl.GameControllerGetPlayerIndex(controller); set => sdl.GameControllerSetPlayerIndex(controller, value); }

        public string Mapping => sdl.GameControllerMappingS(controller);

        public IReadOnlyDictionary<GamepadAxis, short> AxisStates => axisStates;

        public IReadOnlyDictionary<GamepadButton, GamepadButtonState> ButtonStates => buttonStates;

        public IReadOnlyDictionary<GamepadSensorType, GamepadSensor> Sensors => sensors;

        public IReadOnlyList<GamepadTouchpad> Touchpads => touchpads;

        public IReadOnlyList<string> Mappings => mappings;

        public Haptic? Haptic => haptic;

        public event EventHandler<GamepadRemappedEventArgs>? Remapped;

        public event EventHandler<GamepadAxisMotionEventArgs>? AxisMotion;

        public event EventHandler<GamepadButtonEventArgs>? ButtonDown;

        public event EventHandler<GamepadButtonEventArgs>? ButtonUp;

        public bool HasButton(GamepadButton button)
        {
            return buttonStates.ContainsKey(button);
        }

        public bool HasAxis(GamepadAxis axis)
        {
            return axisStates.ContainsKey(axis);
        }

        public bool HasSensor(GamepadSensorType sensor)
        {
            return sensors.ContainsKey(sensor);
        }

        public bool IsDown(GamepadButton button)
        {
            return buttonStates[button] == GamepadButtonState.Down;
        }

        public bool IsUp(GamepadButton button)
        {
            return buttonStates[button] == GamepadButtonState.Up;
        }

        public void Rumble(ushort lowFreq, ushort highFreq, uint durationMs)
        {
            sdl.GameControllerRumble(controller, lowFreq, highFreq, durationMs);
        }

        public void RumbleTriggers(ushort rightRumble, ushort leftRumble, uint durationMs)
        {
            sdl.GameControllerRumbleTriggers(controller, rightRumble, leftRumble, durationMs);
        }

        public void SetLED(Vector4 color)
        {
            sdl.GameControllerSetLED(controller, (byte)(color.X * 255), (byte)(color.Y * 255), (byte)(color.Z * 255));
        }

        public void SetLED(byte red, byte green, byte blue)
        {
            sdl.GameControllerSetLED(controller, red, green, blue);
        }

        public void AddMapping(string mapping)
        {
            sdl.GameControllerAddMapping(mapping);
            mappings.Add(mapping);
        }

        internal void OnRemapped()
        {
            remappedEventArgs.Mapping = Mapping;
            Remapped?.Invoke(this, remappedEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnAxisMotion(ControllerAxisEvent even)
        {
            var axis = Helper.Convert((GameControllerAxis)even.Axis);
            if (Math.Abs((int)even.Value) < deadzone)
            {
                even.Value = 0;
            }

            if (even.Value == axisStates[axis])
            {
                return;
            }

            axisStates[axis] = even.Value;
            axisMotionEventArgs.Axis = axis;
            axisMotionEventArgs.Value = even.Value;
            AxisMotion?.Invoke(this, axisMotionEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnButtonDown(ControllerButtonEvent even)
        {
            var button = Helper.Convert((GameControllerButton)even.Button);
            buttonStates[button] = GamepadButtonState.Down;
            buttonEventArgs.Button = button;
            buttonEventArgs.State = GamepadButtonState.Down;
            ButtonDown?.Invoke(this, buttonEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnButtonUp(ControllerButtonEvent even)
        {
            var button = Helper.Convert((GameControllerButton)even.Button);
            buttonStates[button] = GamepadButtonState.Up;
            buttonEventArgs.Button = button;
            buttonEventArgs.State = GamepadButtonState.Up;
            ButtonUp?.Invoke(this, buttonEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnTouchPadDown(ControllerTouchpadEvent even)
        {
            touchpads[even.Touchpad].OnTouchPadDown(even);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnTouchPadMotion(ControllerTouchpadEvent even)
        {
            touchpads[even.Touchpad].OnTouchPadMotion(even);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnTouchPadUp(ControllerTouchpadEvent even)
        {
            touchpads[even.Touchpad].OnTouchPadUp(even);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnSensorUpdate(ControllerSensorEvent even)
        {
            sensors[Helper.Convert((SensorType)even.Sensor)].OnSensorUpdate(even);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            foreach (var sensor in sensors)
            {
                sensor.Value?.Dispose();
            }
            sdl.GameControllerClose(controller);
        }
    }
}