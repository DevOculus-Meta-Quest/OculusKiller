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

        // Method to log general messages
        public static void Log(string message)
        {
            InitializeLogging();
            string formattedMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [INFO] {message}";
            File.AppendAllText(logPath, formattedMessage + Environment.NewLine);
        }

        // Method to log exceptions
        public static void LogError(Exception exception, bool isCritical = false)
        {
            InitializeLogging();
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
            return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [ERROR] An error occurred: {exception.Message}\n" +
                   $"Stack Trace: {exception.StackTrace}";
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
    }
}