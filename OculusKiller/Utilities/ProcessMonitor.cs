using OculusKiller.Utilities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace OculusKiller.Core
{
    internal class ProcessMonitor
    {
        private const int MaxRetries = 3; // Maximum number of consecutive restart attempts
        private const int RetryDelay = 5000; // Delay between retries in milliseconds

        public static void MonitorProcesses(string vrServerPath, string oculusPath)
        {
            int retryCount = 0;

            while (retryCount < MaxRetries) // Monitoring loop with retry limit
            {
                try
                {
                    // Retrieving the vrserver and vrdashboard processes
                    var vrServerProcess = FindProcessByPath("vrserver", vrServerPath);
                    var vrDashboardProcess = Process.GetProcessesByName("vrdashboard").FirstOrDefault();

                    if (vrServerProcess != null && vrDashboardProcess != null)
                    {
                        ErrorLogger.Log("Monitoring vrserver and vrdashboard processes.");

                        // Wait for both processes to exit
                        vrServerProcess.WaitForExit();
                        vrDashboardProcess.WaitForExit();

                        // Check if the user has intentionally exited SteamVR
                        if (DidUserExitSteamVR(vrServerProcess, vrDashboardProcess))
                        {
                            ErrorLogger.Log("User has intentionally exited SteamVR.");
                            break; // Exit the loop as user has intentionally exited
                        }
                        else
                        {
                            ErrorLogger.Log("vrserver or vrdashboard process crashed. Restarting...");
                            ProcessUtilities.StartProcess(vrServerPath);
                            retryCount++;
                        }
                    }
                    else
                    {
                        ErrorLogger.Log("vrserver or vrdashboard process not found. Starting...");
                        ProcessUtilities.StartProcess(vrServerPath);
                        retryCount++;
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e);
                    retryCount++;
                }

                Thread.Sleep(RetryDelay); // Delay before next retry
            }

            // After reaching the maximum retry limit, terminate SteamVR and Oculus Air Link
            if (retryCount >= MaxRetries)
            {
                ErrorLogger.Log("Maximum retry limit reached. Terminating SteamVR and Oculus Air Link.");
                ProcessUtilities.TerminateProcess("vrserver");
                ProcessUtilities.TerminateProcess("OVRServer_x64");
            }
        }

        private static Process FindProcessByPath(string processName, string path)
        {
            return Process.GetProcessesByName(processName)
                          .FirstOrDefault(p =>
                          {
                              try
                              {
                                  return p.MainModule.FileName == path;
                              }
                              catch
                              {
                                  return false;
                              }
                          });
        }

        private static bool DidUserExitSteamVR(Process vrServerProcess)
        {
            try
            {
                // Check if the vrserver process was started by this application
                if (vrServerProcess.StartInfo.FileName != string.Empty)
                {
                    // Check if the vrserver process has exited
                    if (!vrServerProcess.HasExited)
                    {
                        return false; // vrserver process is still running
                    }

                    // Check if the vrserver process exited normally
                    if (vrServerProcess.ExitCode == 0)
                    {
                        return true; // vrserver process exited normally, indicating a user-initiated exit
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                ErrorLogger.LogError(ex, isCritical: false);
                // If an InvalidOperationException is caught, it's likely because the process was not started by this application
                // In this case, we can't determine the exit reason, so we assume it's not a normal user exit
                return false;
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, isCritical: false);
                // For any other exceptions, also assume it's not a normal user exit
                return false;
            }

            // If the vrserver process has exited but not normally, assume it's not a user-initiated exit
            return false;
        }
    }
}