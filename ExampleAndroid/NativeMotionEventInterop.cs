namespace ExampleAndroid
{
    using Hexa.NET.ImGui.Backends.Android;
    using System.Runtime.InteropServices;

    public static unsafe class NativeMotionEventInterop
    {
        [DllImport("android")]
        public static extern AInputEvent* AMotionEvent_fromJava(IntPtr env, IntPtr motionEvent);

        [DllImport("android")]
        public static extern AInputEvent* AKeyEvent_fromJava(IntPtr env, IntPtr motionEvent);
    }
}