namespace ExampleWin32D3D9
{
    using System;
    using System.Runtime.InteropServices;

    public unsafe class Win32
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint CreateWindowEx(
     int dwExStyle,
     char* lpClassName,
     byte* lpWindowName,
     int dwStyle,
     int x, int y,
     int nWidth, int nHeight,
     nint hWndParent,
     nint hMenu,
     nint hInstance,
     nint lpParam);

        [DllImport("user32.dll")]
        public static extern nint DefWindowProc(nint hWnd, uint msg, nuint wParam, nint lParam);

        [DllImport("user32.dll")]
        public static extern bool DestroyWindow(nint hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(nint hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(nint hWnd);

        [DllImport("user32.dll")]
        public static extern nint GetMessage(out MSG lpMsg, nint hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern bool PeekMessage(ref MSG lpMsg, nint hwnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage(ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern nint DispatchMessage(ref MSG lpMsg);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern short RegisterClassA(ref WNDCLASS lpWndClass);

        [DllImport("kernel32.dll")]
        public static extern nint GetModuleHandle(string lpModuleName);

        public const int WS_OVERLAPPEDWINDOW = 0x00CF0000;
        public const int SW_SHOW = 5;
        public const uint WM_DESTROY = 0x0002;
        public const uint WM_QUIT = 0x0012;

        public const uint PM_REMOVE = 0x0001;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public nint hwnd;
        public uint message;
        public nint wParam;
        public nint lParam;
        public uint time;
        public POINT pt;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct WNDCLASS
    {
        public uint style;
        public nint lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public nint hInstance;
        public nint hIcon;
        public nint hCursor;
        public nint hbrBackground;
        public char* lpszMenuName;
        public char* lpszClassName;
    }

    public delegate nint WindowProcCallback(nint hWnd, uint msg, nuint wParam, nint lParam);
}