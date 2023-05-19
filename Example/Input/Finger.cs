namespace HexaEngine.Core.Input
{
    public struct Finger
    {
        public FingerState State;
        public float X;
        public float Y;
        public float Pressure;

        public Finger(FingerState state, float x, float y, float pressure)
        {
            State = state;
            X = x;
            Y = y;
            Pressure = pressure;
        }
    }
}