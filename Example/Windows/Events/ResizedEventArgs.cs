namespace HexaEngine.Core.Windows.Events
{
    public class ResizedEventArgs : RoutedEventArgs
    {
        public ResizedEventArgs()
        {
        }

        public ResizedEventArgs(int oldWidth, int oldHeight, int newWidth, int newHeight)
        {
            OldWidth = oldWidth;
            OldHeight = oldHeight;
            NewWidth = newWidth;
            NewHeight = newHeight;
        }

        public int OldWidth { get; internal set; }

        public int OldHeight { get; internal set; }

        public int NewWidth { get; internal set; }

        public int NewHeight { get; internal set; }
    }
}