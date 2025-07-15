namespace ExampleFramework.ImPlotDemo
{
    using ExampleFramework;
    using Hexa.NET.ImGui;
    using Hexa.NET.ImPlot;
    using System.Numerics;

    public class ImPlotDemo
    {
        public RingBuffer Frame = new(512);

        public void Draw()
        {
            const int shade_mode = 2;
            const float fill_ref = 0;
            double fill = shade_mode == 0 ? -double.PositiveInfinity : shade_mode == 1 ? double.PositiveInfinity : fill_ref;

            if (!ImGui.Begin("Demo ImPlot"))
            {
                ImGui.End();
                return;
            }

            Frame.Add(Time.Delta * 1000);

            ImPlot.SetNextAxesToFit();
            if (ImPlot.BeginPlot("Frame", new Vector2(-1, 0), ImPlotFlags.NoInputs))
            {
                ImPlot.PushStyleVar(ImPlotStyleVar.FillAlpha, 0.25f);
                ImPlot.PlotShaded("Total", ref Frame.Values[0], Frame.Length, fill, 1, 0, ImPlotShadedFlags.None, Frame.Head);
                ImPlot.PopStyleVar();

                ImPlot.PlotLine("Total", ref Frame.Values[0], Frame.Length, 1, 0, ImPlotLineFlags.None, Frame.Head);
                ImPlot.EndPlot();
            }

            ImGui.End();
        }
    }
}