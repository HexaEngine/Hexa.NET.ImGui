namespace ExampleD3D11.Input
{
    using ExampleD3D11.Input.Events;
    using Hexa.NET.SDL2;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class Keyboard
    {
        private static readonly Key[] keys = (Key[])Enum.GetValues(typeof(Key));
        private static readonly string[] keyNames = new string[keys.Length];
        private static readonly Dictionary<Key, KeyState> states = new();
        private static readonly KeyboardEventArgs keyboardEventArgs = new();
        private static readonly KeyboardCharEventArgs keyboardCharEventArgs = new();

        public static IReadOnlyList<Key> Keys => keys;

        public static IReadOnlyList<string> KeyNames => keyNames;

        public static IReadOnlyDictionary<Key, KeyState> States => states;

        internal static unsafe void Init()
        {
            int numkeys;
            byte* pKeys = SDL.GetKeyboardState(&numkeys);

            for (int i = 0; i < keys.Length; i++)
            {
                Key key = keys[i];
                keyNames[i] = SDL.GetKeyNameS((int)key);
                var scancode = (Key)SDL.GetScancodeFromKey((int)key);
                states.Add(key, (KeyState)pKeys[(int)scancode]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnKeyDown(SDLKeyboardEvent keyboardEvent)
        {
            Key keyCode = (Key)SDL.GetKeyFromScancode(keyboardEvent.Keysym.Scancode);
            states[keyCode] = KeyState.Down;
            keyboardEventArgs.KeyState = KeyState.Down;
            keyboardEventArgs.KeyCode = keyCode;
            keyboardEventArgs.Scancode = keyboardEvent.Keysym.Scancode;
            KeyDown?.Invoke(null, keyboardEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnKeyUp(SDLKeyboardEvent keyboardEvent)
        {
            Key keyCode = (Key)SDL.GetKeyFromScancode(keyboardEvent.Keysym.Scancode);
            states[keyCode] = KeyState.Up;
            keyboardEventArgs.KeyState = KeyState.Up;
            keyboardEventArgs.KeyCode = keyCode;
            keyboardEventArgs.Scancode = keyboardEvent.Keysym.Scancode;
            KeyUp?.Invoke(null, keyboardEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnTextInput(SDLTextInputEvent textInputEvent)
        {
            unsafe
            {
                keyboardCharEventArgs.Char = *(char*)&textInputEvent.Text_0;
            }
            TextInput?.Invoke(null, keyboardCharEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Flush()
        {
        }

        public static event EventHandler<KeyboardEventArgs>? KeyDown;

        public static event EventHandler<KeyboardEventArgs>? KeyUp;

        public static event EventHandler<KeyboardCharEventArgs>? TextInput;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUp(Key n)
        {
            return states[n] == KeyState.Up;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDown(Key n)
        {
            return states[n] == KeyState.Down;
        }

        public static SDLKeymod GetModState()
        {
            return SDL.GetModState();
        }
    }
}