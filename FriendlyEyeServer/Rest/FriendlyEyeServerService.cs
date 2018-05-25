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
        const int MAX_NUMBER_OF_REVIEWS = 3;       
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
                
                // Store the image in memory
                FindOrCreateImageSet(parser);
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
            string result = "success";
            XAttribute attribResult = new XAttribute("result", result);
            root.Add(attribResult);
            return root;
        }

        private void SaveFile(string filename, byte[] fileContents)
        {
            File.WriteAllBytes("C:\\SavedImages\\" + filename, fileContents);
        }

        private void FindOrCreateImageSet(MultipartParser parser)
        {
            foreach(ImageSet imageSet in ImageSets)
            {
                if(imageSet.ID.Equals(parser.ImagesetNumber))
                {
                    imageSet.Images.Add(new ImageFrame(parser.FileContents, parser.FrameNumber));
                    return;
                }
            }

            // not found; create new
            ImageSet newImageSet = new ImageSet(parser.Clientname, parser.Address, parser.Telephone, parser.ImagesetNumber);
            newImageSet.Images.Add(new ImageFrame(parser.FileContents, parser.FrameNumber));
            ImageSets.Add(newImageSet);
        }
    }
}
