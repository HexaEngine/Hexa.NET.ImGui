namespace HexaEngine.Core.Input
{
    using HexaEngine.Core.Input.Events;
    using Silk.NET.SDL;
    using System.Numerics;
    using System.Text;

    public unsafe class Joystick : IDisposable
    {
        private static readonly Sdl sdl = Application.sdl;
        private readonly int id;
        internal readonly Silk.NET.SDL.Joystick* joystick;
        private readonly Dictionary<int, short> axes = new();
        private readonly Dictionary<int, (int, int)> balls = new();
        private readonly Dictionary<int, JoystickButtonState> buttons = new();
        private readonly Dictionary<int, JoystickHatState> hats = new();

        private readonly JoystickAxisMotionEventArgs axisMotionEventArgs = new();
        private readonly JoystickBallMotionEventArgs ballMotionEventArgs = new();
        private readonly JoystickButtonEventArgs buttonEventArgs = new();
        private readonly JoystickHatMotionEventArgs hatMotionEventArgs = new();

        private readonly string guid;
        private short deadzone = 8000;

        private bool disposedValue;

        public Joystick(int id)
        {
            this.id = id;
            joystick = sdl.JoystickOpen(id);

            var axisCount = sdl.JoystickNumAxes(joystick);
            for (int i = 0; i < axisCount; i++)
            {
                short state;
                sdl.JoystickGetAxisInitialState(joystick, i, &state);
                axes.Add(i, state);
            }

            var ballCount = sdl.JoystickNumBalls(joystick);
            for (int i = 0; i < ballCount; i++)
            {
                int x;
                int y;
                sdl.JoystickGetBall(joystick, i, &x, &y);
                balls.Add(i, new(x, y));
            }

            var buttonCount = sdl.JoystickNumButtons(joystick);
            for (int i = 0; i < buttonCount; i++)
            {
                var state = (JoystickButtonState)sdl.JoystickGetButton(joystick, i);
                buttons.Add(i, state);
            }

            var hatCount = sdl.JoystickNumHats(joystick);
            for (int i = 0; i < hatCount; i++)
            {
                var state = (JoystickHatState)sdl.JoystickGetHat(joystick, i);
                hats.Add(i, state);
            }

            var guid = sdl.JoystickGetGUID(joystick);
            var buffer = Alloc<byte>(33);
            sdl.JoystickGetGUIDString(guid, buffer, 33);
            var size = StringSizeNullTerminated(buffer);
            var value = Encoding.ASCII.GetString(buffer, size - 1);
            Free(buffer);
            this.guid = value;
        }

        public int Id => id;

        public string Name => sdl.JoystickNameS(joystick);

        public ushort Vendor => sdl.JoystickGetVendor(joystick);

        public ushort Product => sdl.JoystickGetProduct(joystick);

        public ushort ProductVersion => sdl.JoystickGetProductVersion(joystick);

        public string Serial => sdl.JoystickGetSerialS(joystick);

        public string Guid => guid;

        public bool IsAttached => sdl.JoystickGetAttached(joystick) == SdlBool.True;

        public bool IsVirtual => sdl.JoystickIsVirtual(id) == SdlBool.True;

        public bool HasLED => sdl.JoystickHasLED(joystick) == SdlBool.True;

        public JoystickType Type => Helper.Convert(sdl.JoystickGetType(joystick));

        public short Deadzone { get => deadzone; set => deadzone = value; }

        public int PlayerIndex { get => sdl.JoystickGetPlayerIndex(joystick); set => sdl.JoystickSetPlayerIndex(joystick, value); }

        public JoystickPowerLevel PowerLevel => Helper.Convert(sdl.JoystickCurrentPowerLevel(joystick));

        public IReadOnlyDictionary<int, short> Axes => axes;

        public IReadOnlyDictionary<int, (int, int)> Balls => balls;

        public IReadOnlyDictionary<int, JoystickButtonState> Buttons => buttons;

        public IReadOnlyDictionary<int, JoystickHatState> Hats => hats;

        public event EventHandler<JoystickAxisMotionEventArgs>? AxisMotion;

        public event EventHandler<JoystickBallMotionEventArgs>? BallMotion;

        public event EventHandler<JoystickButtonEventArgs>? ButtonDown;

        public event EventHandler<JoystickButtonEventArgs>? ButtonUp;

        public event EventHandler<JoystickHatMotionEventArgs>? HatMotion;

        public void Rumble(ushort lowFreq, ushort highFreq, uint durationMs)
        {
            sdl.JoystickRumble(joystick, lowFreq, highFreq, durationMs);
        }

        public void RumbleTriggers(ushort left, ushort right, uint durationMs)
        {
            sdl.JoystickRumbleTriggers(joystick, left, right, durationMs);
        }

        public void SetLED(Vector4 color)
        {
            sdl.JoystickSetLED(joystick, (byte)(color.X * 255), (byte)(color.Y * 255), (byte)(color.Z * 255));
        }

        public void SetLED(byte red, byte green, byte blue)
        {
            sdl.JoystickSetLED(joystick, red, green, blue);
        }

        internal void OnAxisMotion(JoyAxisEvent even)
        {
            if (Math.Abs((int)even.Value) < deadzone)
            {
                even.Value = 0;
            }

            if (even.Value == axes[even.Axis])
            {
                return;
            }

            axes[even.Axis] = even.Value;
            axisMotionEventArgs.Axis = even.Axis;
            axisMotionEventArgs.Value = even.Value;
            AxisMotion?.Invoke(this, axisMotionEventArgs);
        }

        internal void OnBallMotion(JoyBallEvent even)
        {
            balls[even.Ball] = (even.Xrel, even.Yrel);
            ballMotionEventArgs.Ball = even.Ball;
            ballMotionEventArgs.RelX = even.Xrel;
            ballMotionEventArgs.RelY = even.Yrel;
            BallMotion?.Invoke(this, ballMotionEventArgs);
        }

        internal void OnButtonDown(JoyButtonEvent even)
        {
            buttons[even.Button] = JoystickButtonState.Down;
            buttonEventArgs.Button = even.Button;
            buttonEventArgs.State = JoystickButtonState.Down;
            ButtonDown?.Invoke(this, buttonEventArgs);
        }

        internal void OnButtonUp(JoyButtonEvent even)
        {
            buttons[even.Button] = JoystickButtonState.Up;
            buttonEventArgs.Button = even.Button;
            buttonEventArgs.State = JoystickButtonState.Up;
            ButtonUp?.Invoke(this, buttonEventArgs);
        }

        internal void OnHatMotion(JoyHatEvent even)
        {
            hats[even.Hat] = (JoystickHatState)even.Value;
            hatMotionEventArgs.Hat = even.Hat;
            hatMotionEventArgs.State = (JoystickHatState)even.Value;
            HatMotion?.Invoke(this, hatMotionEventArgs);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                sdl.JoystickClose(joystick);
                disposedValue = true;
            }
        }

        ~Joystick()
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