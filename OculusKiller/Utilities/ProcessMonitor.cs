using System;
using System.Diagnostics;
using System.Linq;
using OculusKiller.Utilities;

namespace OculusKiller.Core
{
    internal class ProcessMonitor
    {
        private const int MaxRetries = 3; // Maximum number of consecutive restart attempts

        public static void MonitorProcesses(string vrServerPath, string oculusPath)
        {
            int retryCount = 0;

            while (retryCount < MaxRetries) // Monitoring loop with retry limit
            {
                try
                {
                    var vrServerProcess = Process.GetProcessesByName("vrserver")
                        .FirstOrDefault(process => process.MainModule.FileName == vrServerPath);

                    var vrDashboardProcess = Process.GetProcessesByName("vrdashboard").FirstOrDefault();

                    if (vrServerProcess != null && vrDashboardProcess != null)
                    {
                        ErrorLogger.Log("Monitoring vrserver and vrdashboard processes.");

                        vrServerProcess.WaitForExit(); // Wait for the vrserver process to exit
                        vrDashboardProcess.WaitForExit(); // Wait for the vrdashboard process to exit

                        // Check exit codes to determine if processes crashed
                        if (vrServerProcess.ExitCode != 0 || vrDashboardProcess.ExitCode != 0)
                        {
                            ErrorLogger.Log("vrserver or vrdashboard process crashed. Restarting...");
                            // Restarting the SteamVR process
                            ProcessUtilities.StartProcess(vrServerPath);
                            retryCount++;
                        }
                        else
                        {
                            ErrorLogger.Log("vrserver and vrdashboard processes exited normally.");
                            break; // Exit the loop if processes terminated normally
                        }
                    }
                    else
                    {
                        ErrorLogger.Log("vrserver or vrdashboard process not found. Starting...");
                        // Restarting the SteamVR process
                        ProcessUtilities.StartProcess(vrServerPath);
                        retryCount++;
                    }
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
