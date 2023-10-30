using System;
using System.Diagnostics;
using System.Linq;
using OculusKiller.Utilities;

namespace OculusKiller.Core
{
    public static class VRServerMonitor
    {
        public static void MonitorVRServer(string vrServerPath, string oculusPath)
        {
            if (string.IsNullOrEmpty(vrServerPath))
            {
                ErrorLogger.LogError(new Exception("Invalid vrserver path."));
                return;
            }

            var vrServerProcess = Process.GetProcessesByName("vrserver").FirstOrDefault(process => process.MainModule.FileName == vrServerPath);
            if (vrServerProcess != null)
            {
                ErrorLogger.Log("Monitoring vrserver process.");
                vrServerProcess.EnableRaisingEvents = true;

                vrServerProcess.Exited += (sender, e) =>
                {
                    ErrorLogger.Log("vrserver process exited.");
                    KillOculusServer(oculusPath);
                };

                ErrorLogger.Log($"vrserver process status: {vrServerProcess.Responding}");
                ErrorLogger.Log($"vrserver process start time: {vrServerProcess.StartTime}");
                ErrorLogger.Log($"vrserver process total processor time: {vrServerProcess.TotalProcessorTime}");

                vrServerProcess.WaitForExit();
            }
            else
            {
                ErrorLogger.LogError(new Exception("vrserver process not found."));
            }
        }

        private static void KillOculusServer(string oculusPath)
        {
            var ovrServerProcess = Process.GetProcessesByName("OVRServer_x64").FirstOrDefault(process => process.MainModule.FileName == oculusPath);
            if (ovrServerProcess != null)
            {
                ovrServerProcess.Kill();
                ovrServerProcess.WaitForExit();
            }
            else
            {
                ErrorLogger.LogError(new Exception("Oculus runtime not found..."));
            }
        }
    }
}
