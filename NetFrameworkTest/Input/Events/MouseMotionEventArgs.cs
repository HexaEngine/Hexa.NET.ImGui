namespace ExampleD3D11.Input.Events
{
    public class MouseMotionEventArgs : EventArgs
    {
        public MouseMotionEventArgs()
        {
        }

        public float X { get; internal set; }
        public float Y { get; internal set; }
        public float RelX { get; internal set; }
        public float RelY { get; internal set; }
    }
}