using UnityEngine;
using ZXing;

namespace Assets.Scripts.QR.Reader
{
    public class CodeReader
    {
        private readonly BarcodeReader reader = new BarcodeReader();
        private Color32[] data;

        public string Read(WebCamTexture webCamTexture)
        {
            if (data == null)
            {
                data = new Color32[webCamTexture.width * webCamTexture.height];
            }
            webCamTexture.GetPixels32(data);

            var result = reader.Decode(data, webCamTexture.width, webCamTexture.height);
            return result != null ? result.Text : string.Empty;
        }
    }
}
