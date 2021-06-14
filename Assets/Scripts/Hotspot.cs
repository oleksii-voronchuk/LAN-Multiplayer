using Assets.Scripts.Network;
using Assets.Scripts.QR.Generator;
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
        [SerializeField] private Image qr;

        private IEnumerator Start()
        {
            yield return null;
            var profile = NetworkAdapter.GetProfile();
            var msg = $"{profile.SSID}:{profile.Key}";
            debug.text = msg;
            Debug.Log(debug.text);
            qr.sprite = QRGenerator.Generate(msg);
        }

    }
}
