using System;
using System.Diagnostics;
using System.Linq;

namespace Assets.Scripts.Network
{
    public static class NetworkAdapter
    {

        public static string StartHotspot(string ssid, string password)
        {
            StopHotspot();
            var addressees = GetIPs();
            var commandProcess = StartCommandProcess();
            commandProcess.StandardInput.WriteLine($"netsh wlan set hostednetwork mode=allow ssid={ssid} key={password}");
            commandProcess.StandardInput.WriteLine("netsh wlan start hostednetwork");
            StopCommandProcess(commandProcess);
            var newAddressees = GetIPs();
            return newAddressees.Except(addressees).LastOrDefault();
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
                UseShellExecute = false
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
    }
}
