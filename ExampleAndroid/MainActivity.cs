namespace ExampleAndroid
{
    using Android.OS;
    using Android.Views.InputMethods;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImGui.Utilities;
    using Hexa.NET.ImGui.Widgets;
    using Hexa.NET.ImGui.Widgets.Dialogs;
    using Hexa.NET.ImNodes;

    [Activity(Label = "ExampleAndroid", MainLauncher = true, Icon = "@mipmap/ic_launcher", Theme = "@style/AppTheme")]
    public unsafe class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;
            base.OnCreate(savedInstanceState);

            var guiContext = ImGui.CreateContext();
            ImGui.SetCurrentContext(guiContext);

            ImNodes.SetImGuiContext(guiContext);
            var nodesContext = ImNodes.CreateContext();
            ImNodes.SetCurrentContext(nodesContext);
            ImNodes.StyleColorsDark(ImNodes.GetStyle());

            var io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;         // Enable Docking
            io.ConfigViewportsNoAutoMerge = false;
            io.ConfigViewportsNoTaskBarIcon = false;

            float density = Resources.DisplayMetrics.Density;

            ImGuiFontBuilder builder = new();
            builder
                .SetOption(conf => conf.SizePixels = 18 * density)
                .AddDefaultFont()
                .Build();

            ImGui.GetStyle().ScaleAllSizes(density);

            WidgetManager.Init();

            WidgetManager.Register<MainWindow>(true, true);
            OpenFileDialog dialog = new();
            dialog.Show();

            CustomGLSurfaceView renderView = new(this);
            SetContentView(renderView);

            renderView.Holder.AddCallback(new SurfaceHolderCallback());
        }

        public static MainActivity Instance { get; private set; }

        public void ShowSoftKeyboardInput()
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService)!;
            imm.ShowSoftInput(null, ShowFlags.Forced, null);
        }
    }

    public class MainWindow : ImWindow
    {
        private string s = "";

        protected override string Name { get; } = "Main";

        public override void DrawContent()
        {
            ImGui.Text("Hello World!");

            ImGui.InputText("TextInput", ref s, 1024);

            ImNodes.BeginNodeEditor();
            ImNodes.EndNodeEditor();
        }
    }
}