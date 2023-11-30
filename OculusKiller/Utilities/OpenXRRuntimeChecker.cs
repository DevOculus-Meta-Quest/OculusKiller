using Microsoft.Win32;
using OculusKiller.Utilities;
using System;
using System.Collections.Generic;

namespace OculusKiller.RuntimeManagement
{
    public static class OpenXRRuntimeChecker
    {
        private const string OpenXRKeyPath = @"SOFTWARE\Khronos\OpenXR\1";

        public static void CheckAndLogRuntimes()
        {
            try
            {
                // Log available runtimes
                var availableRuntimes = GetAvailableRuntimes();
                ErrorLogger.Log("Available OpenXR Runtimes:");
                foreach (var runtime in availableRuntimes)
                {
                    ErrorLogger.Log(runtime);
                }

                // Log active runtime
                var activeRuntime = GetActiveRuntime();
                ErrorLogger.Log($"Active OpenXR Runtime: {activeRuntime}");
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e);
            }
        }

        private static List<string> GetAvailableRuntimes()
        {
            var runtimes = new List<string>();
            using (var key = Registry.LocalMachine.OpenSubKey($@"{OpenXRKeyPath}\AvailableRuntimes"))
            {
                if (key != null)
                {
                    foreach (var valueName in key.GetValueNames())
                    {
                        // The value names are the actual file paths
                        runtimes.Add(valueName);
                    }
                }
            }
            return runtimes;
        }

        private static string GetActiveRuntime()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(OpenXRKeyPath))
            {
                if (key != null)
                {
                    return key.GetValue("ActiveRuntime") as string;
                }
            }
            return "No active runtime found.";
        }
    }
}
