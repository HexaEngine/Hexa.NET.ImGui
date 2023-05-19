namespace HexaEngine.Core.Debugging
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public static class CrashLogger
    {
        public static void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Trace.Listeners.Add(new DebugListener($"logs/app-{DateTime.Now:yyyy-dd-M--HH-mm-ss}.log"));
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
                var fileInfo = new FileInfo($"logs/crash-{DateTime.Now:yyyy-dd-M--HH-mm-ss}.log");
                fileInfo.Directory?.Create();
                File.AppendAllText(fileInfo.FullName, e.ExceptionObject.ToString());
            }
        }
    }
}