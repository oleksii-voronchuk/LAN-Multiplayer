using System;
using System.Diagnostics;

namespace Assets.Scripts.Network
{
    public static class NetworkAdapter
    {

        public static void StartHotspot(string ssid, string password)
        {
            var commandProcess = StartCommandProcess();
            commandProcess.StandardInput.WriteLine($"netsh wlan set hostednetwork mode=allow ssid={ssid} key={password}");
            commandProcess.StandardInput.WriteLine("netsh wlan start hostednetwork");
            StopCommandProcess(commandProcess);
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
    }
}
