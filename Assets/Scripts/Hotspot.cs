using Assets.Scripts.Network.WIFI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Hotspot : MonoBehaviour
    {
        [SerializeField] private string ssid;
        [SerializeField] private string password;

        [SerializeField] private Text debug;

        private IEnumerator Start()
        {
            /*    var ip = NetworkAdapter.StartHotspot(ssid, password);
                debug.text = ip;
                yield return new WaitForSecondsRealtime(30);
                NetworkAdapter.StopHotspot();*/
            AndroidWiFiController wiFiController = new AndroidWiFiController();
            wiFiController.Enable();
            wiFiController.Connect("Meow", "17052013");
            yield return null;
        }

    }
}
