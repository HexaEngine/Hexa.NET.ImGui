namespace ExampleD3D11.Input.Events
{
    using ExampleD3D11.Input;
    using Hexa.NET.SDL2;

    public class KeyboardEventArgs : EventArgs
    {
        public KeyboardEventArgs()
        {
        }

        public KeyboardEventArgs(Key keyCode, KeyState keyState, SDLScancode scancode)
        {
            KeyCode = keyCode;
            KeyState = keyState;
            Scancode = scancode;
        }

        public Key KeyCode { get; internal set; }

        public KeyState KeyState { get; internal set; }

        public SDLScancode Scancode { get; internal set; }
    }
}