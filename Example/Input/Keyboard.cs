namespace HexaEngine.Core.Input
{
    using HexaEngine.Core.Input.Events;
    using Silk.NET.SDL;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class Keyboard
    {
#nullable disable
        private static Sdl sdl;
#nullable enable

        private static readonly Key[] keys = Enum.GetValues<Key>();
        private static readonly string[] keyNames = new string[keys.Length];
        private static readonly Dictionary<Key, KeyState> states = new();
        private static readonly KeyboardEventArgs keyboardEventArgs = new();
        private static readonly KeyboardCharEventArgs keyboardCharEventArgs = new();

        public static IReadOnlyList<Key> Keys => keys;

        public static IReadOnlyList<string> KeyNames => keyNames;

        public static IReadOnlyDictionary<Key, KeyState> States => states;

        internal static unsafe void Init()
        {
            sdl = Application.sdl;
            int numkeys;
            byte* pKeys = sdl.GetKeyboardState(&numkeys);

            for (int i = 0; i < keys.Length; i++)
            {
                Key key = keys[i];
                keyNames[i] = sdl.GetKeyNameS((int)key);
                var scancode = (Key)sdl.GetScancodeFromKey((int)key);
                states.Add(key, (KeyState)pKeys[(int)scancode]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnKeyDown(KeyboardEvent keyboardEvent)
        {
            Key keyCode = (Key)sdl.GetKeyFromScancode(keyboardEvent.Keysym.Scancode);
            states[keyCode] = KeyState.Down;
            keyboardEventArgs.KeyState = KeyState.Down;
            keyboardEventArgs.KeyCode = keyCode;
            keyboardEventArgs.Scancode = keyboardEvent.Keysym.Scancode;
            KeyDown?.Invoke(null, keyboardEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnKeyUp(KeyboardEvent keyboardEvent)
        {
            Key keyCode = (Key)sdl.GetKeyFromScancode(keyboardEvent.Keysym.Scancode);
            states[keyCode] = KeyState.Up;
            keyboardEventArgs.KeyState = KeyState.Up;
            keyboardEventArgs.KeyCode = keyCode;
            keyboardEventArgs.Scancode = keyboardEvent.Keysym.Scancode;
            KeyUp?.Invoke(null, keyboardEventArgs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void OnTextInput(TextInputEvent textInputEvent)
        {
            unsafe
            {
                keyboardCharEventArgs.Char = (char)*textInputEvent.Text;
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

        public static Keymod GetModState()
        {
            return sdl.GetModState();
        }
    }
}