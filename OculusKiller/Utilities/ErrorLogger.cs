using System;
using System.IO;
using System.Windows.Forms;

namespace OculusKiller.Utilities
{
    public static class ErrorLogger
    {
        private static string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OculusKiller", "OculusKiller.log");

        public static void Log(string message)
        {
            InitializeLogging();
            File.AppendAllText(logPath, DateTime.Now + ": " + message + "\n");
        }

        public static void LogError(Exception exception, bool isCritical = false)
        {
            string errorMessage = $"An error occurred: {exception.Message}";
            Log(errorMessage);

            if (isCritical)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void InitializeLogging()
        {
            if (!Directory.Exists(Path.GetDirectoryName(logPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            }
        }
    }
}