namespace ExampleFramework.ImGuizmoDemo
{
    using ExampleFramework;
    using ExampleFramework.ImGuiDemo;
    using ExampleFramework.Input;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImGuizmo;
    using Hexa.NET.Mathematics;
    using System;
    using System.Numerics;

    public class ImGuizmoDemo
    {
        public ImGuizmoDemo()
        {
        }

        private CameraTransform camera = new();
        private Vector3 sc = new(10, 0.5f, 0.5f);

        private const float speed = 2;
        private bool first = true;

        private string[] operationNames = Enum.GetNames<ImGuizmoOperation>();
        private ImGuizmoOperation[] operations = Enum.GetValues<ImGuizmoOperation>();

        private string[] modeNames = Enum.GetNames<ImGuizmoMode>();
        private ImGuizmoMode[] modes = Enum.GetValues<ImGuizmoMode>();

        private ImGuizmoOperation operation = ImGuizmoOperation.Universal;
        private ImGuizmoMode mode = ImGuizmoMode.Local;

        private Viewport SourceViewport = new(1920, 1080);
        private Viewport Viewport;
        private bool gimbalGrabbed;
        private Matrix4x4 cube = Matrix4x4.Identity;
        private bool overGimbal;

        public unsafe void Init()
        {
            App.MainWindow.Resized += Resized;
            SourceViewport = new(App.MainWindow.Width, App.MainWindow.Height);
        }

        private void Resized(object? sender, ResizedEventArgs e)
        {
            SourceViewport = new(e.Width, e.Height);
        }

        public void Draw()
        {
            // IMPORTANT: If you want to render your scene through the window, set the background color to transparent, make sure to calculate the viewport after it to avoid misalignment
            ImGui.PushStyleColor(ImGuiCol.WindowBg, Vector4.Zero);
            if (!ImGui.Begin("Demo ImGuizmo", ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoMove))
            {
                ImGui.PopStyleColor(1);
                ImGui.End();
                return;
            }
            ImGui.PopStyleColor(1);

            if (!ImGui.IsWindowDocked())
            {
                var node = ImGuiP.DockBuilderGetCentralNode(ImGuiManager.DockSpaceId);
                ImGuiP.DockBuilderDockWindow("Demo ImGuizmo", node.ID);
            }

            HandleInput();
            DrawMenuBar();

            // IMPORTANT: Calculate the viewport, so the gizmo is always in the center of the window, to avoid misalignment
            var position = ImGui.GetWindowPos();
            var size = ImGui.GetWindowSize();
            float ratioX = size.X / SourceViewport.Width;
            float ratioY = size.Y / SourceViewport.Height;
            var s = Math.Min(ratioX, ratioY);
            var w = SourceViewport.Width * s;
            var h = SourceViewport.Height * s;
            var x = position.X + (size.X - w) / 2;
            var y = position.Y + (size.Y - h) / 2;
            Viewport = new Viewport(x, y, w, h);

            // Get the view and projection matrix from the camera
            var view = camera.View;
            var proj = camera.Projection;

            // IMPORTANT: Set the drawlist and enable ImGuizmo and set the rect, before using any ImGuizmo functions
            ImGuizmo.SetDrawlist();
            ImGuizmo.Enable(true);
            ImGuizmo.SetOrthographic(false);
            ImGuizmo.SetRect(position.X, position.Y, size.X, size.Y);

            var transform = cube;

            // Draw the grid and the cube
            Matrix4x4 matrix = Matrix4x4.Identity;
            ImGuizmo.DrawGrid(ref view, ref proj, ref matrix, 10);
            ImGuizmo.DrawCubes(ref view, ref proj, ref transform, 1);

            // IMPORTANT: If you use multiple gizmos, you need to set the ID for each gizmo
            ImGuizmo.PushID(0);

            // Call the Manipulate function to manipulate the cube
            if (ImGuizmo.Manipulate(ref view, ref proj, operation, mode, ref transform))
            {
                gimbalGrabbed = true;
                cube = transform;
            }

            // Query gizmo state and update local variables
            if (!ImGuizmo.IsUsing() && gimbalGrabbed)
            {
                gimbalGrabbed = false;
            }
            overGimbal = ImGuizmo.IsOver();

            ImGuizmo.PopID();

            // User-Interface for the gizmo operation modes.
            ImGui.PushItemWidth(100);
            int opIndex = Array.IndexOf(operations, operation);
            if (ImGui.Combo("##Operation", ref opIndex, operationNames, operationNames.Length))
            {
                operation = operations[opIndex];
            }
            int modeIndex = Array.IndexOf(modes, mode);
            if (ImGui.Combo("##Mode", ref modeIndex, modeNames, modeNames.Length))
            {
                mode = modes[modeIndex];
            }
            ImGui.PopItemWidth();

            // Display the gizmo state
            ImGui.Text($"IsOver: {overGimbal}");
            ImGui.Text($"IsUsed: {gimbalGrabbed}");

            ImGui.End();
        }

        private void HandleInput()
        {
            if (ImGui.IsWindowHovered())
            {
                bool mouseMiddlePressed = ImGui.IsMouseDown(ImGuiMouseButton.Middle);
                bool lCtrlPressed = ImGui.IsKeyPressed(ImGuiKey.LeftCtrl);
                if (mouseMiddlePressed || lCtrlPressed || first)
                {
                    Vector2 delta = Vector2.Zero;
                    if (mouseMiddlePressed)
                    {
                        delta = Mouse.Delta;
                    }

                    float wheel = 0;
                    if (lCtrlPressed)
                    {
                        wheel = Mouse.DeltaWheel.Y;
                    }

                    // Only update the camera's position if the mouse got moved in either direction
                    if (delta.X != 0f || delta.Y != 0f || wheel != 0f || first)
                    {
                        sc.X += sc.X / 2 * -wheel;

                        // Rotate the camera left and right
                        sc.Y += -delta.X * 0.004f * speed;

                        // Rotate the camera up and down
                        // Prevent the camera from turning upside down (1.5f = approx. Pi / 2)
                        sc.Z = Math.Clamp(sc.Z + delta.Y * 0.004f * speed, -MathF.PI / 2, MathF.PI / 2);

                        first = false;

                        // Calculate the cartesian coordinates
                        Vector3 pos = SphereHelper.GetCartesianCoordinates(sc);
                        var orientation = Quaternion.CreateFromYawPitchRoll(-sc.Y, sc.Z, 0);
                        camera.PositionRotation = (pos, orientation);
                        camera.Recalculate();
                    }
                }
            }
        }

        private void DrawMenuBar()
        {
            if (!ImGui.BeginMenuBar())
            {
                ImGui.EndMenuBar();
                return;
            }

            ImGui.EndMenuBar();
        }
    }
}