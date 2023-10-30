using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OculusKiller.Utilities;

namespace OculusKiller.Core
{
    internal class VRServerMonitor
    {
        public static async void MonitorVRServer(string vrServerPath, string oculusPath)
        {
            while (true) // Continuous monitoring
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

                    // Waiting for a short duration before rechecking the process
                    await Task.Delay(5000);
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e);
                    await Task.Delay(10000); // Wait longer if an error occurs
                }
            }
        }
    }
}