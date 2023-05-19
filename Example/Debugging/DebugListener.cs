namespace HexaEngine.Core.Debugging
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    public class DebugListener : TraceListener
    {
#if DEBUG
        private readonly BufferedStream stream;
#endif

        public DebugListener(string file)
        {
#if DEBUG
            var fileInfo = new FileInfo(file);
            fileInfo.Directory?.Create();
            stream = new(File.Create(file));
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
#endif
        }

#if DEBUG

        private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            stream.Flush();
            stream.Close();
        }

        private void Application_ApplicationClosing(object sender, EventArgs e)
        {
            stream.Flush();
            stream.Close();
        }

#endif

        public override void Write(string? message)
        {
            if (message == null)
            {
                return;
            }
#if DEBUG
            stream.Write(Encoding.UTF8.GetBytes(message));
#endif
        }

        public override void WriteLine(string? message)
        {
            if (message == null)
            {
                return;
            }
#if DEBUG
            stream.Write(Encoding.UTF8.GetBytes(message + "\n"));
#endif
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
#if DEBUG
            WriteLine(e.ExceptionObject);
            if (e.IsTerminating)
            {
                stream.Flush();
                stream.Close();
            }
#endif
        }
    }
}