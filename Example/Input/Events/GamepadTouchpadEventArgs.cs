namespace HexaEngine.Core.Input.Events
{
    public class GamepadTouchpadEventArgs : EventArgs
    {
        public GamepadTouchpadEventArgs()
        {
        }

        public GamepadTouchpadEventArgs(int finger, FingerState state, float x, float y, float pressure)
        {
            Finger = finger;
            State = state;
            X = x;
            Y = y;
            Pressure = pressure;
        }

        public int Finger { get; internal set; }

        public FingerState State { get; internal set; }

        public float X { get; internal set; }

        public float Y { get; internal set; }

        public float Pressure { get; internal set; }
    }
}