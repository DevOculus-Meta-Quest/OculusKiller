using OculusKiller.Utilities;
using System;

namespace OculusKiller.Core
{
    internal class ProcessMonitor
    {
        // Method to monitor the VR processes
        public static void MonitorProcesses(string vrServerPath, string oculusPath)
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

                    // Once the vrserver process exits, log the event
                    ErrorLogger.Log("vrserver process has exited. Terminating SteamVR and Oculus Air Link.");
                }
                else
                {
                    // If the vrserver process is not found, log the event
                    ErrorLogger.Log("vrserver process not found.");
                }
            }
            catch (Exception e)
            {
                // Log any exceptions that occur
                ErrorLogger.LogError(e);
            }
            finally
            {
                // Terminate SteamVR and Oculus Air Link processes
                ProcessUtilities.TerminateProcess("vrserver");
                ProcessUtilities.TerminateProcess("OVRServer_x64");
                ErrorLogger.Log("Terminated SteamVR and Oculus Air Link processes.");
            }
        }
    }
}