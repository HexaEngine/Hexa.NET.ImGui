namespace ExampleD3D11
{
    using System;

    public class ResizedEventArgs : EventArgs
    {
        public ResizedEventArgs(int width, int height, int oldWidth, int oldHeight)
        {
            Width = width;
            Height = height;
            OldWidth = oldWidth;
            OldHeight = oldHeight;
        }

        public int Width { get; }

        public int Height { get; }

        public int OldWidth { get; }

        public int OldHeight { get; }
    }
}