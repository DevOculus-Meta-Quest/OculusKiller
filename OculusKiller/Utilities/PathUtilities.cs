using System;
using System.IO;
using System.Web.Script.Serialization;

namespace OculusKiller.Utilities
{
    public static class PathUtilities
    {
        public static Tuple<string, string> GetSteamPaths()
        {
            string openVrPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"openvr\openvrpaths.vrpath");
            if (!File.Exists(openVrPath))
            {
                ErrorLogger.LogError(new Exception("OpenVR Paths file not found... (Has SteamVR been run once?)"));
                return null;
            }

            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string openvrJsonString = File.ReadAllText(openVrPath);
                dynamic openvrPaths = jss.DeserializeObject(openvrJsonString);

                string location = openvrPaths["runtime"][0].ToString();
                string startupPath = Path.Combine(location, @"bin\win64\vrstartup.exe");
                string serverPath = Path.Combine(location, @"bin\win64\vrserver.exe");

                if (!File.Exists(startupPath) || !File.Exists(serverPath))
                {
                    ErrorLogger.LogError(new Exception("SteamVR executables not found... (Has SteamVR been run once?)"));
                    return null;
                }

                return new Tuple<string, string>(startupPath, serverPath);
            }
            catch (Exception e)
            {
                ErrorLogger.LogError(new Exception($"Corrupt OpenVR Paths file found... (Has SteamVR been run once?)\n\nMessage: {e}"));
                return null;
            }
        }

        public static string GetOculusPath()
        {
            string oculusPath = Environment.GetEnvironmentVariable("OculusBase");
            if (string.IsNullOrEmpty(oculusPath))
            {
                ErrorLogger.LogError(new Exception("Oculus installation environment not found..."));
                return null;
            }

            oculusPath = Path.Combine(oculusPath, @"Support\oculus-runtime\OVRServer_x64.exe");
            if (!File.Exists(oculusPath))
            {
                ErrorLogger.LogError(new Exception("Oculus server executable not found..."));
                return null;
            }

            return oculusPath;
        }
    }
}