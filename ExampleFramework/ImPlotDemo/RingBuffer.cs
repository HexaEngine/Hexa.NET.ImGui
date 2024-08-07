namespace ExampleFramework.ImPlotDemo
{
    public unsafe struct RingBuffer
    {
        private readonly float[] values;
        private readonly int length;
        private int tail = 0;
        private int head;

        public RingBuffer(int length)
        {
            values = new float[length];
            this.length = length;
        }

        public float[] Values => values;

        public int Length => length;

        public int Tail => tail;

        public int Head => head;

        public void Add(float value)
        {
            if (value < 0)
                value = 0;
            values[tail] = value;

            tail++;

            if (tail == length)
            {
                tail = 0;
            }
            if (tail < 0)
            {
                tail = length - 1;
            }

            head = (tail - length) % length;

            if (head < 0)
                head += length;
        }
    }
}