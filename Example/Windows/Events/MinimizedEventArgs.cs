namespace HexaEngine.Core.Windows.Events
{
    using HexaEngine.Core.Windows;

    public class MinimizedEventArgs : RoutedEventArgs
    {
        public WindowState OldState { get; internal set; }

        public WindowState NewState { get; internal set; }
    }
}