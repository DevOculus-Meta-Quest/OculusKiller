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
                ErrorLogger.Log("Application started.");

                string oculusPath = PathUtilities.GetOculusPath();
                if (string.IsNullOrEmpty(oculusPath))
                {
                    ErrorLogger.LogError(new Exception("Oculus path not found."));
                    return;
                }
                ErrorLogger.Log($"Oculus path: {oculusPath}");

                var steamPaths = PathUtilities.GetSteamPaths();
                if (steamPaths == null)
                {
                    ErrorLogger.LogError(new Exception("Steam paths not found."));
                    return;
                }

                string startupPath = steamPaths.Item1;
                string vrServerPath = steamPaths.Item2;
                ErrorLogger.Log($"Steam startup path: {startupPath}");
                ErrorLogger.Log($"Steam VR server path: {vrServerPath}");

                // Using ProcessUtilities to start the SteamVR process
                Process steamProcess = ProcessUtilities.StartProcess(startupPath);
                if (steamProcess == null)
                {
                    return;
                }

                ErrorLogger.Log("Started SteamVR using vrstartup.exe");

                await Task.Delay(5000);

                VRServerMonitor.MonitorVRServer(vrServerPath, oculusPath);
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(e, isCritical: true);
            }
        }
    }
}