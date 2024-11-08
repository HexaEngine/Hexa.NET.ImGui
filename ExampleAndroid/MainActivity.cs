namespace ExampleAndroid
{
    using Android.OS;
    using Android.Views;
    using Android.Views.InputMethods;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImGui.Utilities;

    [Activity(Label = "ExampleAndroid", MainLauncher = true, Icon = "@mipmap/ic_launcher", Theme = "@style/AppTheme")]
    public unsafe class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;
            base.OnCreate(savedInstanceState);

            var guiContext = ImGui.CreateContext();
            ImGui.SetCurrentContext(guiContext);
            ImGuiFontBuilder builder = new();
            builder
                .SetOption(conf => conf.SizePixels = 33)
                .AddDefaultFont()
                .Build();

            ImGui.GetStyle().ScaleAllSizes(4.0f);

            CustomGLSurfaceView renderView = new(this);
            SetContentView(renderView);

            renderView.Holder.AddCallback(new SurfaceHolderCallback());
        }

        public static MainActivity Instance { get; private set; }

        public void ShowSoftKeyboardInput()
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
            imm.ToggleSoftInput(ShowFlags.Forced, 0);
        }
    }
}