using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Drawing;
using System.Net;
using System.Web.Helpers;
using System.Web.Script.Services;
using WebServiceScanner.Scanner;
using System.IO;
using System.Threading.Tasks;

namespace WebServiceScanner
{
    /// <summary>
    /// Summary description for ScannerService
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ScannerService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetData(int g, int h)
        {
            return JsonSerializer.Serialize(new { city = h + g });
        }
        
        //[WebMethod]
        //public List<Image> GetImages()
        //{
        //    var images = Scanner.MainScanner.Scan();
        //    return images;
        //}


        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public object Scanner(bool isFeeder)
        {
            var images = new List<byte[]>();
            var scanner = true;
            if(scanner)
            {
                using (var ms = new MemoryStream())
                {
                    //var Scannedimages = MainScanner.Scan();

                    var Scannedimages = isFeeder? Feeder.Scan() : MainScanner.Scan();
                    foreach (var image in Scannedimages)
                    {
                        image.Save(ms, image.RawFormat);
                        images.Add(ms.ToArray());
                    }
                };
                return JsonSerializer.Serialize(new { images });
            }

            var someUrl = new List<string>();
            someUrl.Add("https://images-na.ssl-images-amazon.com/images/I/61s0IaMcKtL._AC_SX679_.jpg");
            someUrl.Add("https://images-na.ssl-images-amazon.com/images/I/618cA0sAAML._AC_SL1500_.jpg");

            using (var webClient = new WebClient())
            {
                foreach (var url in someUrl)
                {
                    images.Add(webClient.DownloadData(url));
                }
                return JsonSerializer.Serialize(new { images });
            }
            
        }


        [WebMethod]
        public object ScannerList()
        {
            return MainScanner.ScannerList();
        }

    }
}
