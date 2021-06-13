using Assets.Scripts.Extensions;
using System;
using System.Diagnostics;
using System.Linq;

namespace Assets.Scripts.Network
{
    public static class NetworkAdapter
    {
        public static string StartHotspot(WLANProfile wlanProfile)
        {
            StopHotspot();
            var addressees = GetIPs();
            var commandProcess = StartCommandProcess();
            commandProcess.StandardInput.WriteLine($"netsh wlan set hostednetwork mode=allow ssid={wlanProfile.SSID} key={wlanProfile.Key}");
            commandProcess.StandardInput.WriteLine("netsh wlan start hostednetwork");
            StopCommandProcess(commandProcess);
            var newAddressees = GetIPs();
            return newAddressees.Except(addressees).LastOrDefault()?.Trim(' ');
        }

        public static void StopHotspot()
        {
            var commandProcess = StartCommandProcess();
            commandProcess.StandardInput.WriteLine("netsh wlan stop hostednetwork");
            StopCommandProcess(commandProcess);
        }

        private static Process StartCommandProcess()
        {
            var processStartInfo = new ProcessStartInfo
            {
                Verb = "runas",
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            return Process.Start(processStartInfo);
        }

        private static void StopCommandProcess(Process commandProcess)
        {
            commandProcess.StandardInput.Close(); // line added to stop process from hanging on ReadToEnd()
            Console.WriteLine(commandProcess.StandardOutput.ReadToEnd());
        }

        public static string[] GetIPs()
        {
            var commandProcess = StartCommandProcess();
            commandProcess.StandardInput.WriteLine("netsh interface ip show address | findstr \"IP Address\"");
            commandProcess.StandardInput.Close();
            var addressees = commandProcess.StandardOutput.ReadToEnd().Replace("IP Address:", string.Empty);
            StopCommandProcess(commandProcess);
            return addressees.Split('\n');
        }

        public static WLANProfile GetProfile()
        {
            var profile = new WLANProfile();
            var commandProcess = StartCommandProcess();
            commandProcess.StandardInput.WriteLine("cls");
            commandProcess.StandardInput.WriteLine("netsh wlan show interfaces");
            commandProcess.StandardInput.Close();
            profile.SSID = commandProcess.StandardOutput.ReadToEnd().FindSSID();
            StopCommandProcess(commandProcess);

            var keyCommandProcess = StartCommandProcess();
            keyCommandProcess.StandardInput.WriteLine($"netsh wlan show profile {profile.SSID} key=clear");
            keyCommandProcess.StandardInput.Close();
            profile.Key = keyCommandProcess.StandardOutput.ReadToEnd().FindSSIDKey();
            StopCommandProcess(keyCommandProcess);

            return profile;
        }
    }

    public struct WLANProfile
    {
        public string SSID;
        public string Key;

        public WLANProfile(string ssid, string key)
        {
            SSID = ssid;
            Key = key;
        }
    }
}
