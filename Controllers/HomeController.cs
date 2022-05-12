using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;

namespace WebServiceScanner.Controllers
{
    public class HomeController : Controller
    {
        public object Index()
        {
            return View();
        }


        public ActionResult Scann()
        {
            return View();

        }


        //public ActionResult GetImages2()
        //{
        //    var images = Scanner.MainScanner.Scan()[0];
        //    return File(ImageToByteArray(images), "image/jpeg");

        //}

        private byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }




    }
}
