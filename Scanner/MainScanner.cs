using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebServiceScanner.Scanner
{
    public class MainScanner
    {
        public static List<Image> Scan()
        {
            var f = WIAScanner
                .Scan(
                WIAScanner.GetDevices().ToList()[0].DeviceID,
                1,
                WIAScanQuality.Preview,
                WIAPageSize.A4
                );

            return f;
        }

        public static IEnumerable<WIADeviceInfo> ScannerList()
        {
            return WIAScanner.GetDevices();
        }
    }
}