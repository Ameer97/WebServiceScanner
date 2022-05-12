using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Web.Http;
using System.Web.Mvc;

namespace WebServiceScanner.Controllers
{
    public class ValuesController : Controller
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        //public ActionResult GetImages2()
        //{
        //    var images = Scanner.MainScanner.Scan()[0];
        //    return base.File(ImageToByteArray(images), "image/jpeg");

        //}
        //Json(new{ asd="ASD" });
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }




        public object TestFile()
        {
            string someUrl = "https://images-na.ssl-images-amazon.com/images/I/61s0IaMcKtL._AC_SX679_.jpg";
            using (var webClient = new WebClient())
            {
                byte[] imageBytes = webClient.DownloadData(someUrl);
                return Json(JsonSerializer.Serialize(new { image = imageBytes }));
            }

            
        }
    }
}
