using System;
using System.IO;
using System.Windows.Forms;

namespace OculusKiller.Utilities
{
    public static class ErrorLogger
    {
        // Path for the log file
        private static readonly string logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "OculusKiller",
            "OculusKiller.log");

        // Maximum size of the log file in bytes (10 MB in this example)
        private const long MaxLogSize = 10 * 1024 * 1024;

        // Method to log general messages
        public static void Log(string message)
        {
            CheckAndRotateLog(); // Check if log rotation is needed
            string formattedMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [INFO] {message}";
            File.AppendAllText(logPath, formattedMessage + Environment.NewLine);
        }

        // Method to log exceptions
        public static void LogError(Exception exception, bool isCritical = false)
        {
            CheckAndRotateLog(); // Check if log rotation is needed
            string errorMessage = FormatExceptionMessage(exception);
            File.AppendAllText(logPath, errorMessage + Environment.NewLine);

            if (isCritical)
            {
                MessageBox.Show(errorMessage, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to format exception messages
        private static string FormatExceptionMessage(Exception exception)
        {
            string message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [ERROR] An error occurred: {exception.Message}\n" +
                             $"Source: {exception.Source}\n" +
                             $"Target Site: {exception.TargetSite}\n" +
                             $"Stack Trace: {exception.StackTrace}";

            // Log inner exception if present
            if (exception.InnerException != null)
            {
                message += $"\nInner Exception: {exception.InnerException.Message}\n" +
                           $"Inner Stack Trace: {exception.InnerException.StackTrace}";
            }

            return message;
        }

        // Ensures the log directory exists
        private static void InitializeLogging()
        {
            string directory = Path.GetDirectoryName(logPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        // Checks the size of the log file and rotates it if necessary
        private static void CheckAndRotateLog()
        {
            InitializeLogging(); // Ensure the log directory exists

            FileInfo logFileInfo = new FileInfo(logPath);
            if (logFileInfo.Exists && logFileInfo.Length > MaxLogSize)
            {
                // Rotate the log file by renaming it with a timestamp
                string newLogPath = logPath + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss") + ".log";
                File.Move(logPath, newLogPath);
            }
        }
    }
}