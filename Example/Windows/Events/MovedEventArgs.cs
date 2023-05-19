namespace HexaEngine.Core.Windows.Events
{
    public class MovedEventArgs : RoutedEventArgs
    {
        public int OldX { get; internal set; }

        public int OldY { get; internal set; }

        public int NewX { get; internal set; }

        public int NewY { get; internal set; }
    }
}