namespace ExampleFramework.ImGuiDemo
{
    using Hexa.NET.ImGui;
    using System;
    using System.Numerics;

    public class ImGuiDemo
    {
        private ImGuiTreeNodeFlags base_flags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.OpenOnDoubleClick | ImGuiTreeNodeFlags.SpanAvailWidth;
        private bool align_label_with_current_x_position = false;
        private bool test_drag_and_drop = false;

        private int selection_mask = 1 << 2;

        private void HelpMarker(string desc)
        {
            ImGui.TextDisabled("(?)");
            if (ImGui.IsItemHovered(ImGuiHoveredFlags.DelayShort) && ImGui.BeginTooltip())
            {
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted(desc);
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
        }

        private static int clicked = 0;
        private static bool check = true;
        private static int e = 0;

        private static int counter = 0;

        private static int item_current = 0;

        private static string str0 = "Hello, world!";

        private static string str1 = "";

        private static int i0 = 123;

        private static float f0 = 0.001f;

        private static double d0 = 999999.00000001;

        private static float f1 = 1.0E-9f;

        private static int i1 = 50, i2 = 42;

        private static Vector3 vec4a = new(0.10f, 0.20f, 0.30f);

        private static float f2 = 1.00f, f3 = 0.0067f;

        private static int i3 = 0;

        private static float f4 = 0.123f, f5 = 0.0f;

        private static float angle = 0.0f;

        private enum Element
        { Fire, Earth, Air, Water, COUNT };

        private static int elem = (int)Element.Fire;

        private static Vector3 col1 = new(1.0f, 0.0f, 0.2f);
        private static Vector4 col2 = new(0.4f, 0.7f, 0.0f, 0.5f);

        private static int item_current0 = 1;

        private static float[] arr = { 0.6f, 0.1f, 1.0f, 0.5f, 0.92f, 0.1f, 0.2f };

        public void Draw()
        {
            if (!ImGui.Begin("Demo ImGui"))
            {
                ImGui.End();
                return;
            }

            IMGUI_DEMO_MARKER("Widgets/Basic");

            if (ImGui.TreeNode("Basic"u8))
            {
                IMGUI_DEMO_MARKER("Widgets/Basic/Button");

                if (ImGui.Button("Button"))
                    clicked++;
                if ((clicked & 1) != 0)
                {
                    ImGui.SameLine();
                    ImGui.Text("Thanks for clicking me!");
                }

                IMGUI_DEMO_MARKER("Widgets/Basic/Checkbox");

                ImGui.Checkbox("checkbox", ref check);

                IMGUI_DEMO_MARKER("Widgets/Basic/RadioButton");

                ImGui.RadioButton("radio a", ref e, 0); ImGui.SameLine();
                ImGui.RadioButton("radio b", ref e, 1); ImGui.SameLine();
                ImGui.RadioButton("radio c", ref e, 2);

                // Color buttons, demonstrate using PushID() to add unique identifier in the ID stack, and changing style.
                IMGUI_DEMO_MARKER("Widgets/Basic/Buttons (Colored)");
                for (int i = 0; i < 7; i++)
                {
                    if (i > 0)
                        ImGui.SameLine();
                    ImGui.PushID(i);
                    ImGui.PushStyleColor(ImGuiCol.Button, ImGui.HSV(i / 7.0f, 0.6f, 0.6f).Value);
                    ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ImGui.HSV(i / 7.0f, 0.7f, 0.7f).Value);
                    ImGui.PushStyleColor(ImGuiCol.ButtonActive, ImGui.HSV(i / 7.0f, 0.8f, 0.8f).Value);
                    ImGui.Button("Click");
                    ImGui.PopStyleColor(3);
                    ImGui.PopID();
                }

                // Use AlignTextToFramePadding() to align text baseline to the baseline of framed widgets elements
                // (otherwise a Text+SameLine+Button sequence will have the text a little too high by default!)
                // See 'Demo->Layout->Text Baseline Alignment' for details.
                ImGui.AlignTextToFramePadding();
                ImGui.Text("Hold to repeat:");
                ImGui.SameLine();

                // Arrow buttons with Repeater
                IMGUI_DEMO_MARKER("Widgets/Basic/Buttons (Repeating)");

                float spacing = ImGui.GetStyle().ItemInnerSpacing.X;

                if (ImGui.ArrowButton("##left", ImGuiDir.Left)) { counter--; }
                ImGui.SameLine(0.0f, spacing);
                if (ImGui.ArrowButton("##right", ImGuiDir.Right)) { counter++; }

                ImGui.SameLine();
                ImGui.Text($"{counter}");

                ImGui.Separator();
                ImGui.LabelText("label", "Value");

                {
                    // Using the _simplified_ one-liner Combo() api here
                    // See "Combo" section for examples of how to use the more flexible BeginCombo()/EndCombo() api.
                    IMGUI_DEMO_MARKER("Widgets/Basic/Combo");
                    string[] items = { "AAAA", "BBBB", "CCCC", "DDDD", "EEEE", "FFFF", "GGGG", "HHHH", "IIIIIII", "JJJJ", "KKKKKKK" };

                    ImGui.Combo("combo", ref item_current, items, IM_ARRAYSIZE(items));
                    ImGui.SameLine(); HelpMarker(
                        "Using the simplified one-liner Combo API here.\nRefer to the \"Combo\" section below for an explanation of how to use the more flexible and general BeginCombo/EndCombo API.");
                }

                {
                    // To wire InputText() with std::string or any other custom string type,
                    // see the "Text Input > Resize Callback" section of this demo, and the misc/cpp/imgui_stdlib.h file.
                    IMGUI_DEMO_MARKER("Widgets/Basic/InputText");

                    ImGui.InputText("input text", ref str0, 128);
                    ImGui.SameLine(); HelpMarker(
                        "USER:\n"

                        + "Hold SHIFT or use mouse to select text.\n"

                        + "CTRL+Left/Right to word jump.\n"

                        + "CTRL+A or Double-Click to select all.\n"

                        + "CTRL+X,CTRL+C,CTRL+V clipboard.\n"

                        + "CTRL+Z,CTRL+Y undo/redo.\n"

                        + "ESCAPE to revert.\n\n"

                        + "PROGRAMMER:\n"

                        + "You can use the ImGuiInputTextFlags_CallbackResize facility if you need to wire InputText() "

                        + "to a dynamic string type. See misc/cpp/imgui_stdlib.h for an example (this is not demonstrated "

                        + "in imgui_demo.cpp).");

                    ImGui.InputTextWithHint("input text (w/ hint)", "enter text here", ref str1, 128);

                    IMGUI_DEMO_MARKER("Widgets/Basic/InputInt, InputFloat");

                    ImGui.InputInt("input int", ref i0);

                    ImGui.InputFloat("input float", ref f0, 0.01f, 1.0f, "%.3f");

                    ImGui.InputDouble("input double", ref d0, 0.01f, 1.0f, "%.8f");

                    ImGui.InputFloat("input scientific", ref f1, 0.0f, 0.0f, "%e");
                    ImGui.SameLine(); HelpMarker(
                        "You can input value using the scientific notation,\n"

                        + "  e.g. \"1e+8\" becomes \"100000000\".");

                    ImGui.InputFloat3("input float3", ref vec4a);
                }

                {
                    IMGUI_DEMO_MARKER("Widgets/Basic/DragInt, DragFloat");

                    ImGui.DragInt("drag int", ref i1, 1);
                    ImGui.SameLine(); HelpMarker(
                        "Click and drag to edit value.\n"

                        + "Hold SHIFT/ALT for faster/slower edit.\n"

                        + "Double-click or CTRL+click to input value.");

                    ImGui.DragInt("drag int 0..100", ref i2, 1, 0, 100, "%d%%", ImGuiSliderFlags.AlwaysClamp);

                    ImGui.DragFloat("drag float", ref f2, 0.005f);
                    ImGui.DragFloat("drag small float", ref f3, 0.0001f, 0.0f, 0.0f, "%.06f ns");
                }

                {
                    IMGUI_DEMO_MARKER("Widgets/Basic/SliderInt, SliderFloat");

                    ImGui.SliderInt("slider int", ref i3, -1, 3);
                    ImGui.SameLine(); HelpMarker("CTRL+click to input value.");

                    ImGui.SliderFloat("slider float", ref f4, 0.0f, 1.0f, "ratio = %.3f");
                    ImGui.SliderFloat("slider float (log)", ref f5, -10.0f, 10.0f, "%.4f", ImGuiSliderFlags.Logarithmic);

                    IMGUI_DEMO_MARKER("Widgets/Basic/SliderAngle");

                    ImGui.SliderAngle("slider angle", ref angle);

                    // Using the format string to display a name instead of an integer.
                    // Here we completely omit '%d' from the format string, so it'll only display a name.
                    // This technique can also be used with DragInt().
                    IMGUI_DEMO_MARKER("Widgets/Basic/Slider (enum)");

                    string[] elems_names = { "Fire", "Earth", "Air", "Water" };
                    string elem_name = elem >= 0 && elem < (int)Element.COUNT ? elems_names[elem] : "Unknown";
                    ImGui.SliderInt("slider enum", ref elem, 0, (int)Element.COUNT - 1, elem_name);

                    ImGui.SameLine(); HelpMarker("Using the format string parameter to display a name instead of the underlying integer.");
                }

                {
                    IMGUI_DEMO_MARKER("Widgets/Basic/ColorEdit3, ColorEdit4");

                    ImGui.ColorEdit3("color 1", ref col1);
                    ImGui.SameLine(); HelpMarker(
                        "Click on the color square to open a color picker.\n"

                        + "Click and hold to use drag and drop.\n"

                        + "Right-click on the color square to show options.\n"

                        + "CTRL+click on individual component to input value.\n");

                    ImGui.ColorEdit4("color 2", ref col2);
                }

                {
                    // Using the _simplified_ one-liner ListBox() api here
                    // See "List boxes" section for examples of how to use the more flexible BeginListBox()/EndListBox() api.
                    IMGUI_DEMO_MARKER("Widgets/Basic/ListBox");
                    string[] items = { "Apple", "Banana", "Cherry", "Kiwi", "Mango", "Orange", "Pineapple", "Strawberry", "Watermelon" };

                    ImGui.ListBox("listbox", ref item_current0, items, IM_ARRAYSIZE(items), 4);
                    ImGui.SameLine(); HelpMarker(
                        "Using the simplified one-liner ListBox API here.\nRefer to the \"List boxes\" section below for an explanation of how to use the more flexible and general BeginListBox/EndListBox API.");
                }

                {
                    // Tooltips
                    IMGUI_DEMO_MARKER("Widgets/Basic/Tooltips");
                    ImGui.AlignTextToFramePadding();
                    ImGui.Text("Tooltips:");

                    ImGui.SameLine();
                    ImGui.Button("Button");
                    if (ImGui.IsItemHovered())
                        ImGui.SetTooltip("I am a tooltip");

                    ImGui.SameLine();
                    ImGui.Button("Fancy");
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.Text("I am a fancy tooltip");

                        ImGui.PlotLines("Curve", ref arr[0], IM_ARRAYSIZE(arr));
                        ImGui.Text($"Sin(time) = {MathF.Sin((float)ImGui.GetTime())}");
                        ImGui.EndTooltip();
                    }

                    ImGui.SameLine();
                    ImGui.Button("Delayed");
                    if (ImGui.IsItemHovered(ImGuiHoveredFlags.DelayNormal)) // Delay best used on items that highlight on hover, so this not a great example!
                        ImGui.SetTooltip("I am a tooltip with a delay.");

                    ImGui.SameLine();
                    HelpMarker(
                        "Tooltip are created by using the IsItemHovered() function over any kind of item.");
                }

                ImGui.TreePop();
            }

            ImGui.TreePop();

            ImGui.End();
        }

        private int IM_ARRAYSIZE<T>(T[] items)
        {
            return items.Length;
        }

        private void IMGUI_DEMO_MARKER(string v)
        {
        }
    }
}