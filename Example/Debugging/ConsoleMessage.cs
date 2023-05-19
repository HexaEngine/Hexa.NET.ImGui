namespace HexaEngine.Core.Debugging
{
    using System;

    public struct LogMessage
    {
        public LogSeverity Severity;
        public string Message;
        public string Timestamp;

        public LogMessage(LogSeverity severity, string source, string message) : this()
        {
            Severity = severity;
            Message = $"[{source}]: {message}";
            Timestamp = DateTime.Now.ToShortTimeString();
        }
    }
}