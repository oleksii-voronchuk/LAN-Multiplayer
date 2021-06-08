namespace Assets.Scripts.Network.WIFI
{
    public abstract class WiFiController
    {
        public abstract void Enable();
        public abstract void Connect(string ssid, string password);
    }
}
