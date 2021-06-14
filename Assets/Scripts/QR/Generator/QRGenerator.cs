using UnityEngine;
using ZXing;
using ZXing.Common;

namespace Assets.Scripts.QR.Generator
{
    public static class QRGenerator
    {
        public static Sprite Generate(string msg)
        {
            var qrWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 256,
                    Width = 256,
                }
            };
            var pixels = qrWriter.Write(msg);
            var texture = new Texture2D(qrWriter.Options.Width, qrWriter.Options.Height);
            texture.SetPixels32(pixels);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
    }
}
