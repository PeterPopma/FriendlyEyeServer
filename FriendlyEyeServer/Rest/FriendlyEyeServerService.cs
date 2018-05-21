using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FriendlyEyeServer
{
    public class FriendlyEyeServerService : IFriendlyEyeServerService
    {
        public FriendlyEyeServerService()
        {
        }
        public XElement NewImage(Stream stream)
        {
            MultipartParser parser = new MultipartParser(stream);
            if (parser.Success)
            {
                // Save the file
                SaveFile(parser.Filename + ".png", parser.ContentType, parser.FileContents);
            }

            XElement root = new XElement("Result");
            string result = "success";
            XAttribute attribResult = new XAttribute("result", result);
            root.Add(result);
            return root;
        }

        private void SaveFile(string filename, string contentType, byte[] fileContents)
        {
            File.WriteAllBytes("C:\\SavedImages\\" + filename, fileContents);
        }
    }
}
