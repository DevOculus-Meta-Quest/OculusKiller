﻿using OculusKiller.Utilities;
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

        private static bool DidUserExitSteamVR(Process vrServerProcess, Process vrDashboardProcess)
        {
            try
            {
                // Check if the exit code is 0, which usually indicates a normal exit
                if (vrServerProcess.ExitCode == 0 && vrDashboardProcess.ExitCode == 0)
                {
                    return true; // User likely exited SteamVR normally
                }
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, isCritical: false);
            }

            // Additional timing analysis can be implemented here if needed
            // For example, checking the runtime of the processes, user activity, etc.

            return false; // Default assumption is that the user did not exit normally
        }
    }
}