namespace ExampleAndroid
{
    using Android.Views;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImGui.Backends.Android;
    using System.Runtime.InteropServices;

    internal unsafe class SurfaceHolderCallback : Java.Lang.Object, ISurfaceHolderCallback
    {
        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            // Handle changes to the surface
        }

        [DllImport("android")]
        public static extern IntPtr ANativeWindow_fromSurface(IntPtr jniEnv, IntPtr surface);

        [DllImport("android")]
        public static extern void ANativeWindow_release(IntPtr window);

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            IntPtr surfaceHandle = holder.Surface.Handle;
            IntPtr nativeWindow = ANativeWindow_fromSurface(
                Android.Runtime.JNIEnv.Handle,
                surfaceHandle
            );

            var context = ImGui.GetCurrentContext();
            ImGuiImplAndroid.SetCurrentContext(context);
            ImGuiImplAndroid.Init(new((ANativeWindow*)nativeWindow));
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            // Clean up resources related to the surface
        }
    }
}