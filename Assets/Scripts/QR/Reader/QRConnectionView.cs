using Assets.Scripts.Network;
using Assets.Scripts.Network.WIFI;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.QR.Reader
{
    public class QRConnectionView : MonoBehaviour
    {
        [SerializeField] private RawImage viewFinder;
        [SerializeField] private TextMeshProUGUI resultTMP;

        [SerializeField] private Button confirmButton;
        [SerializeField] private Button rescanButton;

        private WebCamTexture webCamTexture;
        private CodeReader reader;
        private string foundMessage;
        private AndroidWiFiController wifiController = new AndroidWiFiController();

        private void OnEnable()
        {
            confirmButton.onClick.AddListener(Confirm);
            rescanButton.onClick.AddListener(Rescan);
        }

        private void OnDisable()
        {
            confirmButton.onClick.RemoveListener(Confirm);
            rescanButton.onClick.RemoveListener(Rescan);
        }

        private IEnumerator Start()
        {
            wifiController.Enable();
            confirmButton.gameObject.SetActive(false);
            rescanButton.gameObject.SetActive(false);

            if (WebCamTexture.devices.Length == 0) yield break;

            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) yield break;

            var cameraDevice = WebCamTexture.devices.First();
            webCamTexture = new WebCamTexture(cameraDevice.name, (int)viewFinder.rectTransform.rect.width, (int)viewFinder.rectTransform.rect.height);
            viewFinder.texture = webCamTexture;
            webCamTexture.Play();

            reader = new CodeReader();
            foundMessage = null;
            StartCoroutine(TryReadCode());
        }

        private IEnumerator TryReadCode()
        {
            while (true)
            {
                while (webCamTexture == null || !webCamTexture.isPlaying) yield return null;

                var codeResult = reader.Read(webCamTexture);
                if (codeResult != null)
                {
                    foundMessage = codeResult;
                    resultTMP.text = foundMessage;
                }

                if (!string.IsNullOrEmpty(foundMessage))
                {
                    confirmButton.gameObject.SetActive(true);
                    rescanButton.gameObject.SetActive(true);
                    yield break;
                }

                yield return null;
            }
        }

        private void Confirm()
        {
            var strings = foundMessage.Split(':');
            var msg = string.Empty;
            foreach (var s in strings)
            {
                msg += $":{s}:\n";
            }

            var profile = new WLANProfile(strings[0], strings[1]);
            wifiController.Connect(profile.SSID, profile.Key);
            resultTMP.text = msg;
        }

        private void Rescan()
        {
            foundMessage = null;
            confirmButton.gameObject.SetActive(false);
            rescanButton.gameObject.SetActive(false);
            StartCoroutine(TryReadCode());
        }
    }
}
