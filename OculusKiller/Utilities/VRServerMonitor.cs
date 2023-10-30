using System;
using System.Diagnostics;
using System.Linq;
using OculusKiller.Utilities;

namespace OculusKiller.Core
{
    internal class VRServerMonitor
    {
        private const int MaxRetries = 3; // Maximum number of consecutive restart attempts

        public static void MonitorVRServer(string vrServerPath, string oculusPath)
        {
            int retryCount = 0;

            while (retryCount < MaxRetries) // Monitoring loop with retry limit
            {
                try
                {
                    var vrServerProcess = Process.GetProcessesByName("vrserver")
                        .FirstOrDefault(process => process.MainModule.FileName == vrServerPath);

                    if (vrServerProcess != null)
                    {
                        ErrorLogger.Log("Monitoring vrserver process.");
                        vrServerProcess.WaitForExit(); // Wait for the process to exit

                        ErrorLogger.Log("vrserver process exited. Restarting...");
                    }
                    else
                    {
                        ErrorLogger.Log("vrserver process not found. Starting...");
                    }

                    // Restarting the SteamVR process
                    ProcessUtilities.StartProcess(vrServerPath);
                    retryCount++;
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e);
                    retryCount++;
                }
            }

            // After reaching the maximum retry limit, terminate SteamVR and Oculus Air Link
            ErrorLogger.Log("Maximum retry limit reached. Terminating SteamVR and Oculus Air Link.");
            ProcessUtilities.TerminateProcess("vrserver");
            ProcessUtilities.TerminateProcess("OVRServer_x64");
        }
    }
}