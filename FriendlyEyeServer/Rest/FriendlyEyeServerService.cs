using FriendlyEyeServer.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FriendlyEyeServer
{
    public class FriendlyEyeServerService : IFriendlyEyeServerService
    {
        List<ImageSet> imageSets = new List<ImageSet>();
        const int MAX_NUMBER_OF_REVIEWS = 2;       
        const int MINIMUM_FRAMES_RECEIVED = 10;

        internal List<ImageSet> ImageSets { get => imageSets; set => imageSets = value; }

        public FriendlyEyeServerService()
        {
        }

        public XElement NewImage(Stream stream)
        {
            MultipartParser parser = new MultipartParser(stream);
            if (parser.Success)
            {
                // Save the file
                SaveFile(parser.Filename + ".png", parser.FileContents);
                string hints = "";
/*
                // Upload the first file through FTP
                if (parser.FrameNumber == 1)
                {
                    using (WebClient client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential("u16882p12377", "Boarnsyl3");
                        client.UploadFile("ftp://ftp.gravityone.nl/domains/gravityone.nl/public_html/uploads/" + parser.Filename + ".png", "STOR", "C:\\SavedImages\\" + parser.Filename + ".png");
                    }

                    // Analyse image through KPN
                    hints = new KPNClient().AnalyzeImage("http://www.gravityone.nl/uploads/" + parser.Filename + ".png");

                }
*/
                // Store the image in memory
                FindOrCreateImageSet(parser, hints);

            }

            XElement root = new XElement("Result");
            string result = "success";
            XAttribute attribResult = new XAttribute("result", result);
            root.Add(result);
            return root;
        }

        // Obtain a new warning from the queue
        public XElement GetNewWarning(int reviewerID)
        {
            for (int k = 0; k < ImageSets.Count; k++)
            {
                ImageSet imageSet = ImageSets[k];
                if(imageSet.ReviewCount() < MAX_NUMBER_OF_REVIEWS && !imageSet.ReviewerIDs.Contains(reviewerID) && imageSet.Images.Count>=MINIMUM_FRAMES_RECEIVED)
                {
                    imageSet.ReviewerIDs.Add(reviewerID);
                    XElement root = new XElement("image");
                    XAttribute attribId = new XAttribute("id", imageSet.ID.ToString());
                    root.Add(attribId);
                    XAttribute attribNumFrames = new XAttribute("num_frames", imageSet.Images.Count);
                    root.Add(attribNumFrames);
                    if (imageSet.Hints != null)
                    {
                        XAttribute attribHints = new XAttribute("hints", imageSet.Hints);
                        root.Add(attribHints);
                    }
                    XAttribute attribPurpose = new XAttribute("purpose", imageSet.Purpose);
                    root.Add(attribPurpose);
                    XAttribute attribFilename = new XAttribute("filename", imageSet.Name + "_" + imageSet.ID + "_1.png");
                    root.Add(attribFilename);
                    return root;
                }
            }

            return new XElement("Nothing");
        }

        // Collect data from a warning (that has been taken from the queue)
        public XElement GetWarningImages(int ID)
        {
            ImageSet imageSet = ImageSets.Find(o => o.ID.Equals(ID));
            XElement root = new XElement("images");

            if (imageSet == null)
            {
                return new XElement("Nothing");
            }
            for ( int k=0; k<imageSet.Images.Count; k++)
            {
                ImageFrame frame = imageSet.Images[k];
                XElement frameElement = new XElement("frame");
                frameElement.SetAttributeValue("frame_number", frame.FrameNumber);
                frameElement.SetValue(new System.Data.Linq.Binary(frame.ImageData));
                root.Add(frameElement);
            }

            return root;
        }

        private MemoryStream CopyFileToMemory(string path)
        {
            MemoryStream ms = new MemoryStream();
            FileStream fs = new FileStream(path, FileMode.Open);
            fs.Position = 0;
            fs.CopyTo(ms);
            fs.Close();
            fs.Dispose();
            return ms;
        }

        // Collect data from a warning (that has been taken from the queue)
        public string GetWarningImage(int ID, int frameNumber)
        {
            ImageSet imageSet = ImageSets.Find(o => o.ID.Equals(ID));
           // XElement root = new XElement("image");
/*
            if (imageSet == null)
            {
               return new XElement("Nothing");
            }*/
            ImageFrame frame = imageSet.Images.Find(o => o.FrameNumber.Equals(frameNumber));
            string data = Convert.ToBase64String(frame.ImageData);
            // root.Value = data;
            byte[] newdata = Convert.FromBase64String(data);
            return data;//root;
        }


        // Send review about a warning
        public XElement PostReview(int ID, int reviewerID, int value)
        {
            XElement root = new XElement("Result");
            ImageSet imageSet = imageSets.Find(o => o.ID.Equals(ID));
            if (value == 0)
                imageSet.Denials++;
            else
                imageSet.Approvals++;
            XAttribute attribYes = new XAttribute("yes_votes", imageSet.Approvals);
            root.Add(attribYes);
            XAttribute attribNo = new XAttribute("no_votes", imageSet.Denials);
            root.Add(attribNo);

            if(imageSet.ReviewCount()>=MAX_NUMBER_OF_REVIEWS)
            {
                FormCallPolice formCallPolice = new FormCallPolice();
                formCallPolice.richTextBoxPoliceAPI.Text = "POST api.politie.nl/incident \r\n { \r\n name : \"" + imageSet.Name + "\"\r\n address : \"" + imageSet.Address + "\"\r\n telephone : \"" + imageSet.Telephone + "\"\r\n}";
                formCallPolice.ShowDialog();
            }

            return root;
        }

        private void SaveFile(string filename, byte[] fileContents)
        {
            try
            {
                File.WriteAllBytes("C:\\SavedImages\\" + filename, fileContents);
            } catch (DirectoryNotFoundException)
            {
                
            }
        }

        private void FindOrCreateImageSet(MultipartParser parser, string hints)
        {
            foreach(ImageSet imageSet in ImageSets)
            {
                if(imageSet.ID.Equals(parser.ImagesetNumber))
                {
                    imageSet.Images.Add(new ImageFrame(parser.FileContents, parser.FrameNumber));
                    if (hints.Length > 0)
                    {
                        imageSet.Hints = hints;
                    }
                    return;
                }
            }

            // not found; create new
            ImageSet newImageSet = new ImageSet(parser.Clientname, parser.Address, parser.Telephone, parser.ImagesetNumber, parser.Purpose);
            newImageSet.Images.Add(new ImageFrame(parser.FileContents, parser.FrameNumber));
            if (hints.Length > 0)
            {
                newImageSet.Hints = hints;
            }
            ImageSets.Add(newImageSet);
        }
    }
}
