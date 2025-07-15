namespace ExampleFramework.NodeEditor.Pins
{
    using ExampleFramework.NodeEditor;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImNodes;
    using System.Globalization;
    using System.Numerics;

    public class FloatPin : Pin, IDefaultValuePin
    {
        private readonly string name;
        private readonly string nameY;
        private readonly string nameZ;
        private readonly string nameW;
        private readonly PinFlags flags;

        public FloatPin(int id, string name, ImNodesPinShape shape, PinKind kind, PinType type, uint maxLinks = uint.MaxValue, PinFlags flags = PinFlags.None) : base(id, name, shape, kind, type, maxLinks)
        {
            this.name = $"{name}##Value{Id.ToString(CultureInfo.InvariantCulture)}";
            nameY = $"##Value1{Id.ToString(CultureInfo.InvariantCulture)}";
            nameZ = $"##Value2{Id.ToString(CultureInfo.InvariantCulture)}";
            nameW = $"##Value3{Id.ToString(CultureInfo.InvariantCulture)}";
            this.flags = flags;
        }

        public FloatPin(int id, string name, ImNodesPinShape shape, PinKind kind, PinType type, Vector4 value, uint maxLinks = uint.MaxValue, PinFlags flags = PinFlags.None) : base(id, name, shape, kind, type, maxLinks)
        {
            this.name = $"{name}##Value{Id.ToString(CultureInfo.InvariantCulture)}";
            nameY = $"##Value1{Id.ToString(CultureInfo.InvariantCulture)}";
            nameZ = $"##Value2{Id.ToString(CultureInfo.InvariantCulture)}";
            nameW = $"##Value3{Id.ToString(CultureInfo.InvariantCulture)}";
            this.flags = flags;
            ValueX = value.X;
            ValueY = value.Y;
            ValueZ = value.Z;
            ValueW = value.W;
        }

        public float ValueX;
        public float ValueY;
        public float ValueZ;
        public float ValueW;

        public string GetDefaultValue()
        {
            return Type switch
            {
                PinType.Float => ValueX.ToString(CultureInfo.InvariantCulture),
                PinType.Float2 => $"float2({ValueX.ToString(CultureInfo.InvariantCulture)},{ValueY.ToString(CultureInfo.InvariantCulture)})",
                PinType.Float3 => $"float3({ValueX.ToString(CultureInfo.InvariantCulture)},{ValueY.ToString(CultureInfo.InvariantCulture)},{ValueZ.ToString(CultureInfo.InvariantCulture)})",
                PinType.Float4 => $"float4({ValueX.ToString(CultureInfo.InvariantCulture)},{ValueY.ToString(CultureInfo.InvariantCulture)},{ValueZ.ToString(CultureInfo.InvariantCulture)},{ValueW.ToString(CultureInfo.InvariantCulture)})",
                _ => "0",
            };
        }

        protected override void DrawContent()
        {
            if (Kind == PinKind.Input && Links.Count == 0)
            {
                ImGui.PushItemWidth(100);

                if (Type == PinType.Float && flags == PinFlags.None)
                {
                    ImGui.InputFloat(name, ref ValueX);
                }
                if (Type == PinType.Float && flags == PinFlags.Slider)
                {
                    ImGui.SliderFloat(name, ref ValueX, 0, 1);
                }
                if (Type == PinType.Float2 && flags == PinFlags.None)
                {
                    ImGui.InputFloat(name, ref ValueX);
                    ImGui.InputFloat(nameY, ref ValueY);
                }
                if (Type == PinType.Float2 && flags == PinFlags.Slider)
                {
                    var val = new Vector2(ValueX, ValueY);
                    if (ImGui.SliderFloat2(name, ref val, 0, 1))
                    {
                        ValueX = val.X;
                        ValueY = val.Y;
                    }
                }
                if (Type == PinType.Float3 && flags == PinFlags.None)
                {
                    ImGui.InputFloat(name, ref ValueX);
                    ImGui.InputFloat(nameY, ref ValueY);
                    ImGui.InputFloat(nameZ, ref ValueZ);
                }
                if (Type == PinType.Float3 && flags == PinFlags.ColorEdit)
                {
                    var val = new Vector3(ValueX, ValueY, ValueZ);
                    if (ImGui.ColorEdit3(name, ref val, ImGuiColorEditFlags.DisplayHex | ImGuiColorEditFlags.Hdr))
                    {
                        ValueX = val.X;
                        ValueY = val.Y;
                        ValueZ = val.Z;
                    }
                }
                if (Type == PinType.Float3 && flags == PinFlags.ColorPicker)
                {
                    var val = new Vector3(ValueX, ValueY, ValueZ);
                    if (ImGui.ColorPicker3(name, ref val, ImGuiColorEditFlags.DisplayHex | ImGuiColorEditFlags.Hdr))
                    {
                        ValueX = val.X;
                        ValueY = val.Y;
                        ValueZ = val.Z;
                    }
                }
                if (Type == PinType.Float4 && flags == PinFlags.Slider)
                {
                    var val = new Vector3(ValueX, ValueY, ValueZ);
                    if (ImGui.SliderFloat3(name, ref val, 0, 1))
                    {
                        ValueX = val.X;
                        ValueY = val.Y;
                        ValueZ = val.Z;
                    }
                }
                if (Type == PinType.Float4 && flags == PinFlags.None)
                {
                    ImGui.InputFloat(name, ref ValueX);
                    ImGui.InputFloat(nameY, ref ValueY);
                    ImGui.InputFloat(nameZ, ref ValueZ);
                    ImGui.InputFloat(nameW, ref ValueW);
                }
                if (Type == PinType.Float4 && flags == PinFlags.ColorEdit)
                {
                    var val = new Vector4(ValueX, ValueY, ValueZ, ValueW);
                    if (ImGui.ColorEdit4(name, ref val, ImGuiColorEditFlags.DisplayHex | ImGuiColorEditFlags.Hdr))
                    {
                        ValueX = val.X;
                        ValueY = val.Y;
                        ValueZ = val.Z;
                        ValueW = val.W;
                    }
                }
                if (Type == PinType.Float4 && flags == PinFlags.ColorPicker)
                {
                    var val = new Vector4(ValueX, ValueY, ValueZ, ValueW);
                    if (ImGui.ColorPicker4(name, ref val, ImGuiColorEditFlags.DisplayHex | ImGuiColorEditFlags.Hdr))
                    {
                        ValueX = val.X;
                        ValueY = val.Y;
                        ValueZ = val.Z;
                        ValueW = val.W;
                    }
                }
                if (Type == PinType.Float4 && flags == PinFlags.Slider)
                {
                    var val = new Vector4(ValueX, ValueY, ValueZ, ValueW);
                    if (ImGui.SliderFloat4(name, ref val, 0, 1))
                    {
                        ValueX = val.X;
                        ValueY = val.Y;
                        ValueZ = val.Z;
                        ValueW = val.W;
                    }
                }

                ImGui.PopItemWidth();
            }
            else
            {
                base.DrawContent();
            }
        }
    }
}