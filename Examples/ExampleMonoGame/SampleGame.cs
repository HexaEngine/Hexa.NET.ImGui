using System;
using Hexa.NET.ImGui;
using Hexa.NET.Utilities.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Num = System.Numerics;

namespace MonoGameExample;

/// <summary>
/// Simple Monogame + ImGui example
/// </summary>
public class SampleGame : Game
{
    private GraphicsDeviceManager _graphics;
    private ImGuiRenderer _imGuiRenderer;

    private Texture2D _xnaTexture;
    private ImTextureRef _imGuiTexture;

    public SampleGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.PreferMultiSampling = true;

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _imGuiRenderer = new ImGuiRenderer(this);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Texture loading example

        // First, load the texture as a Texture2D (can also be done using the content pipeline)
        _xnaTexture = CreateTexture(GraphicsDevice, 300, 150, pixel =>
        {
            int red = (pixel % 300) / 2;
            return new Color(red, 1, 1);
        });

        // Then, bind it to an ImGui-friendly pointer that we can use during regular ImGui.** calls.
        _imGuiTexture = _imGuiRenderer.BindTexture(_xnaTexture);

        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(clear_color.X, clear_color.Y, clear_color.Z));

        // Call BeforeLayout first to set things up
        _imGuiRenderer.BeforeLayout(gameTime);

        // Draw our UI
        ImGuiLayout();

        // Call AfterLayout now to finish up and draw all the things
        _imGuiRenderer.AfterLayout();

        base.Draw(gameTime);
    }

    // Direct port of the example at https://github.com/ocornut/imgui/blob/master/examples/example_sdl2_opengl2/main.cpp
    private float f = 0.0f;

    private bool show_test_window = false;
    private bool show_another_window = false;
    private Num.Vector3 clear_color = new Num.Vector3(114f / 255f, 144f / 255f, 154f / 255f);
    private byte[] _textBuffer = new byte[100];

    protected virtual void ImGuiLayout()
    {
        // 1. Show a simple window
        // Tip: if we don't call ImGui.Begin()/ImGui.End() the widgets appears in a window automatically called "Debug"
        {
            ImGui.Text("Hello, world!"u8);
            ImGui.SliderFloat("float"u8, ref f, 0.0f, 1.0f, string.Empty);
            ImGui.ColorEdit3("clear color", ref clear_color);
            if (ImGui.Button("Test Window"u8))
            {
                show_test_window = !show_test_window;
            }

            if (ImGui.Button("Another Window"u8))
            {
                show_another_window = !show_another_window;
            }

            // Use StrBuilder for dynamic text instead of string.Format
            // see: 
            unsafe
            {
                byte* buffer = stackalloc byte[128];
                StrBuilder builder = new(buffer, 128);
                builder.Reset();
                builder.Append("Application average "u8);
                builder.Append(1000f / ImGui.GetIO().Framerate, 3);
                builder.Append(" ms/frame ("u8);
                builder.Append(ImGui.GetIO().Framerate, 1);
                builder.Append(" FPS)"u8);
                builder.End();
                ImGui.Text(builder);
            }

            ImGui.InputText("Text input"u8, ref _textBuffer[0], 100);

            ImGui.Text("Texture sample"u8);
            // Updated to use ImTextureRef - the API should be the same
            ImGui.Image(_imGuiTexture, new Num.Vector2(300, 150), Num.Vector2.Zero, Num.Vector2.One);
        }

        // 2. Show another simple window, this time using an explicit Begin/End pair
        if (show_another_window)
        {
            ImGui.SetNextWindowSize(new Num.Vector2(200, 100), ImGuiCond.FirstUseEver);
            ImGui.Begin("Another Window"u8, ref show_another_window);
            ImGui.Text("Hello"u8);
            ImGui.End();
        }

        // 3. Show the ImGui test window. Most of the sample code is in ImGui.ShowTestWindow()
        if (show_test_window)
        {
            ImGui.SetNextWindowPos(new Num.Vector2(650, 20), ImGuiCond.FirstUseEver);
            ImGui.ShowDemoWindow(ref show_test_window);
        }
    }

    public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
    {
        //initialize a texture
        var texture = new Texture2D(device, width, height);

        //the array holds the color for each pixel in the texture
        Color[] data = new Color[width * height];
        for (var pixel = 0; pixel < data.Length; pixel++)
        {
            //the function applies the color according to the specified pixel
            data[pixel] = paint(pixel);
        }

        //set the color
        texture.SetData(data);

        return texture;
    }

    protected override void UnloadContent()
    {
        // Clean up ImGui resources
        if (_imGuiTexture.TexID != ImTextureID.Null)
        {
            _imGuiRenderer.UnbindTexture(_imGuiTexture);
        }

        _imGuiRenderer?.Dispose();

        base.UnloadContent();
    }
}
