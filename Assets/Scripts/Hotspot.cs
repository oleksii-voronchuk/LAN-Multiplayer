using Assets.Scripts.Network;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Hotspot : MonoBehaviour
    {
        [SerializeField] private string ssid;
        [SerializeField] private string password;

        private IEnumerator Start()
        {
            NetworkAdapter.StopHotspot();
            yield return new WaitForSecondsRealtime(5);
            NetworkAdapter.StartHotspot(ssid, password);
            yield return new WaitForSecondsRealtime(10);
            NetworkAdapter.StopHotspot();
        }
    }
}
