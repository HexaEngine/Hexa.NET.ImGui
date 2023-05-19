namespace HexaEngine.Core.Input.Events
{
    public class GamepadTouchpadMotionEventArgs : EventArgs
    {
        public GamepadTouchpadMotionEventArgs()
        {
        }

        public GamepadTouchpadMotionEventArgs(int finger, float x, float y, float pressure)
        {
            Finger = finger;
            X = x;
            Y = y;
            Pressure = pressure;
        }

        public int Finger { get; internal set; }

        public float X { get; internal set; }

        public float Y { get; internal set; }

        public float Pressure { get; internal set; }
    }
}