using OculusKiller.Utilities;
using System;
using System.Diagnostics;
using System.Threading;

namespace OculusKiller.Core
{
    internal class ProcessMonitor
    {
        private const int MaxRetries = 3; // Maximum number of consecutive restart attempts
        private const int RetryDelay = 5000; // Delay between retries in milliseconds

        // Method to monitor the VR processes
        public static void MonitorProcesses(string vrServerPath, string oculusPath)
        {
            int retryCount = 0; // Counter for the number of retries

            // Loop for monitoring with a maximum retry limit
            while (retryCount < MaxRetries)
            {
                try
                {
                    // Find the vrserver process by its path
                    var vrServerProcess = PathUtilities.FindProcessByPath("vrserver", vrServerPath);

                    // Check if the vrserver process is found
                    if (vrServerProcess != null)
                    {
                        ErrorLogger.Log("Monitoring vrserver process.");

                        // Wait for the vrserver process to exit
                        vrServerProcess.WaitForExit();

                        // Check if the user intentionally exited SteamVR
                        if (DidUserExitSteamVR(vrServerProcess))
                        {
                            ErrorLogger.Log("User has intentionally exited SteamVR.");
                            break; // Exit the loop as the user has intentionally exited
                        }
                        else
                        {
                            // If the process crashed, attempt to restart it
                            ErrorLogger.Log("vrserver process crashed. Restarting...");
                            ProcessUtilities.StartProcess(vrServerPath);
                            retryCount++;
                        }
                    }
                    else
                    {
                        // If the vrserver process is not found, start it
                        ErrorLogger.Log("vrserver process not found. Starting...");
                        ProcessUtilities.StartProcess(vrServerPath);
                        retryCount++;
                    }
                }
                catch (Exception e)
                {
                    // Log any exceptions that occur
                    ErrorLogger.LogError(e);
                    retryCount++;
                }

                // Delay before the next retry
                Thread.Sleep(RetryDelay);
            }

            // If the maximum retry limit is reached, terminate SteamVR and Oculus Air Link
            if (retryCount >= MaxRetries)
            {
                ErrorLogger.Log("Maximum retry limit reached. Terminating SteamVR and Oculus Air Link.");
                ProcessUtilities.TerminateProcess("vrserver");
                ProcessUtilities.TerminateProcess("OVRServer_x64");
            }
        }

        // Method to determine if the user intentionally exited SteamVR
        private static bool DidUserExitSteamVR(Process vrServerProcess)
        {
            try
            {
                // Check if the vrserver process was started by this application and has exited
                if (vrServerProcess.StartInfo.FileName != string.Empty && vrServerProcess.HasExited)
                {
                    // Return true if the process exited normally (ExitCode is 0)
                    return vrServerProcess.ExitCode == 0;
                }
            }
            catch (Exception e)
            {
                // Log any exceptions that occur, but do not treat them as critical
                ErrorLogger.LogError(e, isCritical: false);
            }

            // Assume it's not a normal exit if any exception occurs or conditions are not met
            return false;
        }
    }
}