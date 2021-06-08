using UnityEngine;
using UnityEngine.Android;

namespace Assets.Scripts.Network.WIFI
{
    public class AndroidWiFiController : WiFiController
    {
        private readonly string[] permissions = new string[]
        {
            "android.permission.INTERNET",
            "android.permission.ACCESS_WIFI_STATE",
            "android.permission.CHANGE_WIFI_STATE",
            "android.permission.ACCESS_NETWORK_STATE"
        };

        public override void Enable()
        {
            Invoke("TurnOn");
        }

        public override void Connect(string ssid, string password)
        {
            Invoke("Connect", ssid, password);
        }

        private void CheckPermissions()
        {
            foreach (var permission in permissions)
            {
                if (!Permission.HasUserAuthorizedPermission(permission))
                {
                    Permission.RequestUserPermission(permission);
                }
            }
        }

        private void Invoke(string methodName, params object[] args)
        {
            CheckPermissions();
            using var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using var androidPlugin = new AndroidJavaObject("com.olvor.WifiPlugin.WifiPlugin", currentActivity);
            androidPlugin.Call(methodName, args);
        }
    }
}
