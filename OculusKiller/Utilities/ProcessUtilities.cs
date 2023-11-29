using System;
using System.Diagnostics;

namespace OculusKiller.Utilities
{
    public static class ProcessUtilities
    {
        public static Process StartProcess(string processPath)
        {
            try
            {
                Process process = Process.Start(processPath);
                if (process == null || process.HasExited)
                {
                    ErrorLogger.LogError(new Exception($"Failed to start process: {processPath}"));
                    return null;
                }

                ErrorLogger.Log($"Successfully started process: {processPath}");
                return process;
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(new Exception($"Error starting process: {processPath}. Error: {e.Message}"));
                return null;
            }
        }

        public static void TerminateProcess(string processName)
        {
            foreach (var process in Process.GetProcessesByName(processName))
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                    ErrorLogger.Log($"Terminated process: {process.ProcessName}");
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(new Exception($"Error terminating process: {process.ProcessName}. Error: {e.Message}"));
                }
            }
        }
    }
}