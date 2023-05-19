namespace HexaEngine.Core.Graphics
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public readonly struct RendererProfiler
    {
        private readonly Dictionary<object, double> stages;
        private readonly Dictionary<object, long> startTimeStamps;

        public RendererProfiler(int initialStageCount)
        {
            stages = new Dictionary<object, double>(initialStageCount);
            startTimeStamps = new Dictionary<object, long>(initialStageCount);
        }

        public double this[object stage]
        {
            get
            {
                if (stages.TryGetValue(stage, out var value))
                {
                    return value;
                }

                return -1.0;
            }
        }

        public void Start(object o)
        {
            startTimeStamps.Add(o, Stopwatch.GetTimestamp());
        }

        public void End(object o)
        {
            long timestamp = Stopwatch.GetTimestamp();
            if (!stages.TryGetValue(o, out var value))
            {
                value = 0.0;
            }

            stages[o] = value + (timestamp - startTimeStamps[o]) / (double)Stopwatch.Frequency;
            startTimeStamps.Remove(o);
        }

        public void Set(object o, double value)
        {
            stages[o] = value;
            startTimeStamps.Remove(o);
        }

        public void Clear()
        {
            stages.Clear();
        }
    }
}