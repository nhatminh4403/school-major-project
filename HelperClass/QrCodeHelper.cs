using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace school_major_project.HelperClass
{
    public static class QrCodeHelper
    {
        public static byte[] GenerateQrCode(string content, int width = 300, int height = 300)
        {
            QRCodeWriter qRCodeWriter = new QRCodeWriter();
            var hints = new Dictionary<EncodeHintType, Object>
            {
                { EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H },
                { EncodeHintType.MARGIN, 2 },
                { EncodeHintType.CHARACTER_SET, "UTF-8" }
            };

            var bitMatrix = qRCodeWriter.encode(
                content,
                BarcodeFormat.QR_CODE,
                width,
                height,
                hints
            );

            using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb))
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Color color = bitMatrix[x, y] ? Color.Black : Color.White;
                        bitmap.SetPixel(x, y, color);
                    }
                }
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }
    }
} 