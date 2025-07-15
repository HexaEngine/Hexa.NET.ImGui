namespace ExampleFramework
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

        public bool Handled { get; set; }
    }
}