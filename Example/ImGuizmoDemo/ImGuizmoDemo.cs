namespace Example.ImGuizmoDemo
{
    using HexaEngine.Core;
    using HexaEngine.Core.Graphics;
    using HexaEngine.Core.Graphics.Buffers;
    using HexaEngine.Core.Input;
    using HexaEngine.Core.Windows.Events;
    using HexaEngine.ImGuiNET;
    using HexaEngine.ImGuizmoNET;
    using HexaEngine.Mathematics;
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    public class ImGuizmoDemo
    {
        private IGraphicsDevice device;

        public ImGuizmoDemo()
        {
        }

        private CameraTransform camera = new();
        private ConstantBuffer<Matrix4x4> cameraBuffer;
        private Vector3 sc = new(2, 0, 0);
        private const float speed = 2;
        private bool first = true;

        private ImGuizmoOperation operation = ImGuizmoOperation.Translate;
        private ImGuizmoMode mode = ImGuizmoMode.Local;

        private Viewport SourceViewport = new(1920, 1080);
        private Viewport Viewport;
        private bool gimbalGrabbed;
        private Matrix4x4 cube = Matrix4x4.Identity;
        private bool overGimbal;

        public unsafe void Init(IGraphicsDevice device)
        {
            var sw = Application.MainWindow.SwapChain;
            this.device = device;
            cameraBuffer = new(device, CpuAccessFlags.Write);
            Application.MainWindow.SwapChain.Resized += Resized;
            SourceViewport = new(sw.Width, sw.Height);
        }

        private void Resized(object? sender, ResizedEventArgs e)
        {
            SourceViewport = new(e.NewWidth, e.NewHeight);
        }

        public unsafe void Draw()
        {
            ImGui.PushStyleColor(ImGuiCol.WindowBg, Vector4.Zero);
            if (!ImGui.Begin("Demo ImGuizmo", null, ImGuiWindowFlags.MenuBar))
            {
                ImGui.PopStyleColor(1);
                ImGui.End();
                return;
            }
            ImGui.PopStyleColor(1);

            var io = ImGui.GetIO();

            HandleInput();
            DrawMenuBar();

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

            var view = camera.View;
            var proj = camera.Projection;
            ImGuizmo.SetDrawlist();
            ImGuizmo.Enable(true);
            ImGuizmo.SetOrthographic(false);
            ImGuizmo.SetRect(position.X, position.Y, size.X, size.Y);

            var transform = cube;

            ImGuizmo.SetID(0);

            if (ImGuizmo.Manipulate((float*)&view, (float*)&proj, operation, mode, (float*)&transform))
            {
                gimbalGrabbed = true;
                cube = transform;
            }

            if (!ImGuizmo.IsUsing() && gimbalGrabbed)
            {
                gimbalGrabbed = false;
            }
            overGimbal = ImGuizmo.IsOver();

            ImGui.Text($"IsOver: {overGimbal}");

            Matrix4x4 matrix = Matrix4x4.Identity;
            ImGuizmo.DrawGrid((float*)&view, (float*)&proj, (float*)&matrix, 10);
            ImGuizmo.DrawCubes((float*)&view, (float*)&proj, (float*)&transform, 1);

            ImGui.End();
        }

        private void HandleInput()
        {
            if (ImGui.IsWindowHovered())
            {
                if (ImGui.IsMouseDown(ImGuiMouseButton.Middle) || Keyboard.IsDown(Key.LCtrl) || first)
                {
                    Vector2 delta = Vector2.Zero;
                    if (Mouse.IsDown(MouseButton.Middle))
                    {
                        delta = Mouse.Delta;
                    }

                    float wheel = 0;
                    if (Keyboard.IsDown(Key.LCtrl))
                    {
                        wheel = Mouse.DeltaWheel.Y;
                    }

                    // Only update the camera's position if the mouse got moved in either direction
                    if (delta.X != 0f || delta.Y != 0f || wheel != 0f || first)
                    {
                        sc.X += sc.X / 2 * -wheel;

                        // Rotate the camera left and right
                        sc.Y += -delta.X * Time.Delta * speed;

                        // Rotate the camera up and down
                        // Prevent the camera from turning upside down (1.5f = approx. Pi / 2)
                        sc.Z = Math.Clamp(sc.Z + delta.Y * Time.Delta * speed, -MathF.PI / 2, MathF.PI / 2);

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

            if (ImGui.BeginMenu("options"))
            {
                if (ImGui.RadioButton("Translate", operation == ImGuizmoOperation.Translate))
                {
                    operation = ImGuizmoOperation.Translate;
                }

                if (ImGui.RadioButton("Rotate", operation == ImGuizmoOperation.Rotate))
                {
                    operation = ImGuizmoOperation.Rotate;
                }

                if (ImGui.RadioButton("Scale", operation == ImGuizmoOperation.Scale))
                {
                    operation = ImGuizmoOperation.Scale;
                }

                if (ImGui.RadioButton("Local", mode == ImGuizmoMode.Local))
                {
                    mode = ImGuizmoMode.Local;
                }

                ImGui.SameLine();
                if (ImGui.RadioButton("World", mode == ImGuizmoMode.World))
                {
                    mode = ImGuizmoMode.World;
                }

                ImGui.EndMenu();
            }

            ImGui.EndMenuBar();
        }
    }
}