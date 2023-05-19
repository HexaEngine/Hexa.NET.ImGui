namespace HexaEngine.Core.Input
{
    using HexaEngine.Core.Input.Events;
    using Silk.NET.SDL;

    public unsafe class GamepadTouchpad
    {
        private readonly Sdl sdl;
        private readonly int id;
        private readonly GameController* controller;
        private readonly Finger[] fingerStates;

        private readonly GamepadTouchpadEventArgs touchpadEventArgs = new();
        private readonly GamepadTouchpadMotionEventArgs motionEventArgs = new();

        public GamepadTouchpad(int id, GameController* controller)
        {
            sdl = Application.sdl;
            this.id = id;
            this.controller = controller;
            var fingerCount = sdl.GameControllerGetNumTouchpadFingers(controller, id);
            fingerStates = new Finger[fingerCount];
            for (int i = 0; i < fingerCount; i++)
            {
                byte state;
                float x;
                float y;
                float pressure;
                sdl.GameControllerGetTouchpadFinger(controller, id, i, &state, &x, &y, &pressure);
                fingerStates[i] = new(state == Sdl.Pressed ? FingerState.Down : FingerState.Up, x, y, pressure);
            }
        }

        public int Id => id;

        public int FingerCount => fingerStates.Length;

        public event EventHandler<GamepadTouchpadEventArgs>? TouchPadDown;

        public event EventHandler<GamepadTouchpadMotionEventArgs>? TouchPadMotion;

        public event EventHandler<GamepadTouchpadEventArgs>? TouchPadUp;

        internal void OnTouchPadDown(ControllerTouchpadEvent even)
        {
            var state = fingerStates[even.Finger];
            state.State = FingerState.Down;
            state.X = even.X;
            state.Y = even.Y;
            state.Pressure = even.Pressure;
            fingerStates[even.Finger] = state;
            touchpadEventArgs.State = FingerState.Down;
            touchpadEventArgs.Finger = even.Finger;
            touchpadEventArgs.X = even.X;
            touchpadEventArgs.Y = even.Y;
            touchpadEventArgs.Pressure = even.Pressure;
            TouchPadDown?.Invoke(this, touchpadEventArgs);
        }

        internal void OnTouchPadMotion(ControllerTouchpadEvent even)
        {
            var state = fingerStates[even.Finger];
            state.X = even.X;
            state.Y = even.Y;
            state.Pressure = even.Pressure;
            fingerStates[even.Finger] = state;
            motionEventArgs.Finger = even.Finger;
            motionEventArgs.X = even.X;
            motionEventArgs.Y = even.Y;
            motionEventArgs.Pressure = even.Pressure;
            TouchPadMotion?.Invoke(this, motionEventArgs);
        }

        internal void OnTouchPadUp(ControllerTouchpadEvent even)
        {
            var state = fingerStates[even.Finger];
            state.State = FingerState.Up;
            state.X = even.X;
            state.Y = even.Y;
            state.Pressure = even.Pressure;
            fingerStates[even.Finger] = state;

            touchpadEventArgs.State = FingerState.Up;
            touchpadEventArgs.Finger = even.Finger;
            touchpadEventArgs.X = even.X;
            touchpadEventArgs.Y = even.Y;
            touchpadEventArgs.Pressure = even.Pressure;
            TouchPadUp?.Invoke(this, touchpadEventArgs);
        }

        public bool IsDown(int finger)
        {
            return fingerStates[finger].State == FingerState.Down;
        }

        public bool IsUp(int finger)
        {
            return fingerStates[finger].State == FingerState.Up;
        }

        public Finger GetFinger(int index)
        {
            return fingerStates[index];
        }

        public void Flush()
        {
            for (int i = 0; i < fingerStates.Length; i++)
            {
                byte state;
                float x;
                float y;
                float pressure;
                sdl.GameControllerGetTouchpadFinger(controller, id, i, &state, &x, &y, &pressure);
                fingerStates[i] = new(state == Sdl.Pressed ? FingerState.Down : FingerState.Up, x, y, pressure);
            }
        }
    }
}