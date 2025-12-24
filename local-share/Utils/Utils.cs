using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows.Media.Imaging;

namespace local_share.Utils
{
    public static class Utils
    {
        public static string GetLocalIPAddress()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up)
                    continue;

                // ❌ bỏ mạng ảo
                if (ni.Description.Contains("Hyper-V") ||
                    ni.Description.Contains("Virtual") ||
                    ni.Description.Contains("WSL") ||
                    ni.Description.Contains("Docker"))
                    continue;

                // ✅ chỉ lấy Ethernet / WiFi
                if (ni.NetworkInterfaceType != NetworkInterfaceType.Wireless80211 &&
                    ni.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                    continue;

                var ipProps = ni.GetIPProperties();

                // 🔥 BẮT BUỘC có gateway (đang dùng mạng)
                if (!ipProps.GatewayAddresses.Any())
                    continue;

                foreach (var addr in ipProps.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return addr.Address.ToString(); // ✅ IP LAN thật
                    }
                }
            }

            return "127.0.0.1";
        }

        public static BitmapImage GenerateQr()
        {
            string localIP = GetLocalIPAddress();
            string host = "http://" + localIP + ":5000"; 
            using var generator = new QRCodeGenerator();
            using var data = generator.CreateQrCode(host, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(data);
            using Bitmap bitmap = qrCode.GetGraphic(20);

            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;

            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = ms;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();

            return image;
        }
    }
}
