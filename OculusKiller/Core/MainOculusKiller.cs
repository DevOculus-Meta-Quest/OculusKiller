using OculusKiller.Utilities;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OculusKiller.Core
{
    internal class MainOculusKiller
    {
        public static async Task Main()
        {
            try
            {
                // Logging the start of the application
                ErrorLogger.Log("Application started.");

                // Retrieving and validating the Oculus path
                string oculusPath = PathUtilities.GetOculusPath();
                if (string.IsNullOrEmpty(oculusPath))
                {
                    ErrorLogger.LogError(new Exception("Oculus path not found."), isCritical: false);
                    return;
                }
                ErrorLogger.Log($"Oculus path: {oculusPath}");

                // Retrieving and validating the Steam paths
                var steamPaths = PathUtilities.GetSteamPaths();
                if (steamPaths == null)
                {
                    ErrorLogger.LogError(new Exception("Steam paths not found."), isCritical: false);
                    return;
                }

                // Starting the SteamVR process and validating it
                string startupPath = steamPaths.Item1;
                string vrServerPath = steamPaths.Item2;
                ErrorLogger.Log($"Steam startup path: {startupPath}");
                ErrorLogger.Log($"Steam VR server path: {vrServerPath}");

                Process steamProcess = ProcessUtilities.StartProcess(startupPath);
                if (steamProcess == null)
                {
                    ErrorLogger.LogError(new Exception("Failed to start SteamVR process."), isCritical: false);
                    return;
                }

                ErrorLogger.Log("Started SteamVR using vrstartup.exe");

                // Waiting for the SteamVR process to start
                await Task.Delay(5000);

                // Monitoring the VR server
                ProcessMonitor.MonitorProcesses(vrServerPath, oculusPath);
            }
            catch (Exception e)
            {
                // Handling and logging exceptions
                ErrorLogger.LogError(e, isCritical: true);
            }
        }
    }
}