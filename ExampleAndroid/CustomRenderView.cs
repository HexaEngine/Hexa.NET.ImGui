namespace ExampleAndroid
{
    using Android.Content;
    using Android.Opengl;
    using Android.Runtime;
    using Android.Views;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImGui.Backends.Android;
    using static Java.Interop.JniEnvironment;

    public unsafe class CustomGLSurfaceView : GLSurfaceView
    {
        public CustomGLSurfaceView(Context context) : base(context)
        {
            // Set the OpenGL ES version
            SetEGLContextClientVersion(3); // Use 3 for OpenGL ES 3.0

            Focusable = true;
            FocusableInTouchMode = true;
            Clickable = true;

            // Set the custom renderer
            SetRenderer(new CustomRenderer());
            RenderMode = Rendermode.Continuously;
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent? e)
        {
            int unicodeChar = e!.UnicodeChar;
            if (unicodeChar > 0 && !char.IsControl((char)unicodeChar))
            {
                ImGuiIOPtr io = ImGui.GetIO();
                io.AddInputCharacter((uint)unicodeChar);
            }
            IntPtr env = JNIEnv.Handle; // Get the JNIEnv handle
            AInputEvent* nativeEvent = NativeMotionEventInterop.AKeyEvent_fromJava(env, e.Handle);

            ImGuiImplAndroid.HandleInputEvent(nativeEvent);
            return base.OnKeyDown(keyCode, e);
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent? e)
        {
            IntPtr env = JNIEnv.Handle; // Get the JNIEnv handle
            AInputEvent* nativeEvent = NativeMotionEventInterop.AKeyEvent_fromJava(env, e.Handle);

            ImGuiImplAndroid.HandleInputEvent(nativeEvent);
            return base.OnKeyUp(keyCode, e);
        }

        public override bool OnHoverEvent(MotionEvent? e)
        {
            IntPtr env = JNIEnv.Handle; // Get the JNIEnv handle
            AInputEvent* nativeEvent = NativeMotionEventInterop.AMotionEvent_fromJava(env, e.Handle);

            ImGuiImplAndroid.HandleInputEvent(nativeEvent);
            return base.OnHoverEvent(e);
        }

        public override bool OnTouchEvent(MotionEvent? e)
        {
            IntPtr env = JNIEnv.Handle; // Get the JNIEnv handle
            AInputEvent* nativeEvent = NativeMotionEventInterop.AMotionEvent_fromJava(env, e.Handle);

            ImGuiImplAndroid.HandleInputEvent(nativeEvent);
            return base.OnTouchEvent(e);
        }

        public override bool OnGenericMotionEvent(MotionEvent? e)
        {
            IntPtr env = JNIEnv.Handle; // Get the JNIEnv handle
            AInputEvent* nativeEvent = NativeMotionEventInterop.AMotionEvent_fromJava(env, e.Handle);

            ImGuiImplAndroid.HandleInputEvent(nativeEvent);
            return base.OnGenericMotionEvent(e);
        }
    }
}