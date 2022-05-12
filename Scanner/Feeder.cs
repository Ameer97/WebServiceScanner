using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using WIA;

namespace WebServiceScanner.Scanner
{
    class Feeder
    {
        const string wiaFormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";
        const string wiaFormatJPG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";
        const string wiaFormatPNG = "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}";
        const string wiaFormatTIFF = "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}";



        private static void SetupADF(Device wia, bool duplex)
        {
            //string adf = string.Empty;
            var list = new List<string>();
            foreach (WIA.Property deviceProperty in wia.Properties)
            {
                //list.Add(deviceProperty.Name + " ------ " + deviceProperty.get_Value());
                if (deviceProperty.Name == "Document Handling Select") //or PropertyID == 3088
                {
                    int value = duplex ? 0x004 : 0x001;
                    deviceProperty.set_Value(value);
                }

                if (deviceProperty.PropertyID == 3096) //or PropertyID == 3088 for pages
                {
                    deviceProperty.set_Value(1);
                }

                if (deviceProperty.Name.Equals("Horizontal Extent"))
                {
                    var tempNewProperty = 619;
                    deviceProperty.set_Value(tempNewProperty);
                }
                if (deviceProperty.Name.Equals("Vertical Extent"))
                {
                    var tempNewProperty = 876;
                    deviceProperty.set_Value(tempNewProperty);
                }
            }
            //File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//properties.txt", string.Join(Environment.NewLine, list));

        }

        public static List<Image> Scan()
        {
            List<Image> images = new List<Image>();
            List<String> tmp_imageList = new List<String>();

            bool hasMorePages = true;
            bool useAdf = true;
            bool duplex = false;

            int pages = 0;

            string fileName = null;
            string fileName_duplex = null;

            WIA.DeviceManager manager = null;
            WIA.Device device = null;
            WIA.DeviceInfo device_infoHolder = null;
            WIA.Item item = null;
            WIA.ICommonDialog wiaCommonDialog = null;

            manager = new WIA.DeviceManager();

            // select the correct scanner using the provided scannerId parameter
            foreach (WIA.DeviceInfo info in manager.DeviceInfos)
            {
                device_infoHolder = info;
                break;
            }

            while (hasMorePages)
            {
                wiaCommonDialog = new WIA.CommonDialog();

                // Connect to scanner
                device = device_infoHolder.Connect();

                if (device.Items[1] != null)
                {
                    item = device.Items[1] as WIA.Item;

                    try
                    {
                        if ((useAdf) || (duplex))
                            SetupADF(device, duplex); //Sets the right properties in WIA

                        WIA.ImageFile image = null;
                        WIA.ImageFile image_duplex = null;

                        // scan image                
                        image = (WIA.ImageFile)wiaCommonDialog.ShowTransfer(item, wiaFormatTIFF, false);

                        if (duplex)
                        {
                            image_duplex = (ImageFile)wiaCommonDialog.ShowTransfer(item, wiaFormatPNG, false);
                        }

                        // save (front) image to temp file
                        fileName = Path.GetTempFileName();
                        tmp_imageList.Add(fileName);
                        File.Delete(fileName);
                        image.SaveFile(fileName);
                        image = null;

                        // add file to images list
                        images.Add(Image.FromFile(fileName));

                        if (duplex)
                        {
                            fileName_duplex = Path.GetTempFileName();
                            tmp_imageList.Add(fileName_duplex);
                            File.Delete(fileName_duplex);
                            image_duplex.SaveFile(fileName_duplex);
                            image_duplex = null;

                            // add file_duplex to images list
                            images.Add(Image.FromFile(fileName_duplex));
                        }

                        if (useAdf || duplex)
                        {
                            hasMorePages = false; //Returns true if the feeder has more pages
                            pages++;
                        }
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        wiaCommonDialog = null;
                        manager = null;
                        item = null;
                        device = null;
                    }
                }
            }
            device = null;
            return images;
        }
    }
}