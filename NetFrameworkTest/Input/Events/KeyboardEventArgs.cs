﻿namespace ExampleD3D11.Input.Events
{
    using ExampleD3D11.Input;
    using Silk.NET.SDL;

    public class KeyboardEventArgs : EventArgs
    {
        public KeyboardEventArgs()
        {
        }

        public KeyboardEventArgs(Key keyCode, KeyState keyState, Scancode scancode)
        {
            KeyCode = keyCode;
            KeyState = keyState;
            Scancode = scancode;
        }

        public Key KeyCode { get; internal set; }

        public KeyState KeyState { get; internal set; }

        public Scancode Scancode { get; internal set; }
    }
}