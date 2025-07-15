namespace ExampleD3D11.Input.Events
{
    using ExampleD3D11.Input;

    public class MouseButtonEventArgs : EventArgs
    {
        public MouseButton Button { get; internal set; }

        public MouseButtonState State { get; internal set; }

        public int Clicks { get; internal set; }
    }
}