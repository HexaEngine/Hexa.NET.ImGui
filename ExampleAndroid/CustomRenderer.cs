namespace ExampleAndroid
{
    using Android.Opengl;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImGui.Backends.Android;
    using Hexa.NET.ImGui.Backends.OpenGL3;
    using Hexa.NET.ImGui.Widgets;
    using Hexa.NET.ImNodes;
    using Javax.Microedition.Khronos.Opengles;

    public class CustomRenderer : Java.Lang.Object, GLSurfaceView.IRenderer
    {
        private static bool WantTextInputLast = false;

        public void OnDrawFrame(IGL10? gl)
        {
            var io = ImGui.GetIO();

            if (io.WantTextInput && !WantTextInputLast)
                MainActivity.Instance.ShowSoftKeyboardInput();
            WantTextInputLast = io.WantTextInput;

            // Clear the screen
            GLES20.GlClear(GLES20.GlColorBufferBit | GLES20.GlDepthBufferBit);

            ImGuiImplOpenGL3.NewFrame();
            ImGuiImplAndroid.NewFrame();
            ImGui.NewFrame();

            WidgetManager.Draw();

            ImGui.Render();
            ImGui.EndFrame();

            ImGuiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());
        }

        public void OnSurfaceChanged(IGL10? gl, int width, int height)
        {
            // Set the viewport to the new size
            GLES20.GlViewport(0, 0, width, height);
        }

        public void OnSurfaceCreated(IGL10? gl, Javax.Microedition.Khronos.Egl.EGLConfig? config)
        {
            var context = ImGui.GetCurrentContext();
            ImGuiImplOpenGL3.SetCurrentContext(context);
            ImGuiImplOpenGL3.Init("#version 300 es");
        }
    }
}