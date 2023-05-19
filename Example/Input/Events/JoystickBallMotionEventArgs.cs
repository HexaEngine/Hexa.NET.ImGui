namespace HexaEngine.Core.Input.Events
{
    public class JoystickBallMotionEventArgs : EventArgs
    {
        public JoystickBallMotionEventArgs()
        {
        }

        public JoystickBallMotionEventArgs(int ball, int relX, int relY)
        {
            Ball = ball;
            RelX = relX;
            RelY = relY;
        }

        public int Ball { get; internal set; }

        public int RelX { get; internal set; }

        public int RelY { get; internal set; }
    }
}