﻿using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Reads a multipart form data stream and returns the filename, content type and contents as a stream.
/// </summary>
namespace FriendlyEyeServer
{
    public class MultipartParser
    {
        public MultipartParser(Stream stream)
        {   
            this.Parse(stream, Encoding.UTF8);
        }

        public MultipartParser(Stream stream, Encoding encoding)
        {
            this.Parse(stream, encoding);
        }

        private void Parse(Stream stream, Encoding encoding)
        {
            this.Success = false;

            // Read the stream into a byte array
            byte[] data = ToByteArray(stream);

            // Copy to a string for header parsing
            string content = encoding.GetString(data);

            // The first line should contain the delimiter
            int delimiterEndIndex = content.IndexOf("\r\n");

            if (delimiterEndIndex > -1)
            {
                string delimiter = content.Substring(0, content.IndexOf("\r\n"));
                if(delimiter.Length==0)     // sometimes stream starts with a [cr][lf]..
                {
                    delimiter = content.Substring(2, content.Substring(2).IndexOf("\r\n"));
                }

                // Look for name
                Regex re = new Regex(@"(?<=clientname\=\"")(.*?)(?=\"")");
                Match match = re.Match(content);
                if(match.Success)
                {
                    Clientname = match.Value;
                }
                // Look for address
                re = new Regex(@"(?<=address\=\"")(.*?)(?=\"")");
                match = re.Match(content);
                if (match.Success)
                {
                    Address = match.Value;
                }
                // Look for telephone
                re = new Regex(@"(?<=telephone\=\"")(.*?)(?=\"")");
                match = re.Match(content);
                if (match.Success)
                {
                    Telephone = match.Value;
                }
                // Look for purpose
                re = new Regex(@"(?<=purpose\=\"")(.*?)(?=\"")");
                match = re.Match(content);
                if (match.Success)
                {
                    Purpose = match.Value;
                }                // Look for imageset_number
                re = new Regex(@"(?<=imageset_number\=\"")(.*?)(?=\"")");
                match = re.Match(content);
                if (match.Success)
                {
                    ImagesetNumber = Convert.ToInt32(match.Value);
                }
                // Look for sequence_number
                re = new Regex(@"(?<=sequence_number\=\"")(.*?)(?=\"")");
                match = re.Match(content);
                if (match.Success)
                {
                    FrameNumber = Convert.ToInt32(match.Value);
                }

                // Look for Content-Type
                re = new Regex(@"(?<=Content\-Type:)(.*?)(?=\r\n\r\n)");
                Match contentTypeMatch = re.Match(content);

                // Look for filename
                re = new Regex(@"(?<=filename\=\"")(.*?)(?=\"")");
                Match filenameMatch = re.Match(content);

                // Did we find the required values?
                if (contentTypeMatch.Success && filenameMatch.Success)
                {
                    // Set properties
                    this.ContentType = contentTypeMatch.Value.Trim();
                    this.Filename = filenameMatch.Value.Trim();

                    // Get the start & end indexes of the file contents
                    int startIndex = contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length;

                    byte[] delimiterBytes = encoding.GetBytes("\r\n" + delimiter);
                    int endIndex = IndexOf(data, delimiterBytes, startIndex);

                    int contentLength = endIndex - startIndex;

                    // Extract the file contents from the byte array
                    byte[] fileData = new byte[contentLength];

                    Buffer.BlockCopy(data, startIndex, fileData, 0, contentLength);

                    this.FileContents = fileData;
                    this.Success = true;
                }
            }
        }

        private int IndexOf(byte[] searchWithin, byte[] serachFor, int startIndex)
        {
            int index = 0;
            int startPos = Array.IndexOf(searchWithin, serachFor[0], startIndex);

            if (startPos != -1)
            {
                while ((startPos + index) < searchWithin.Length)
                {
                    if (searchWithin[startPos + index] == serachFor[index])
                    {
                        index++;
                        if (index == serachFor.Length)
                        {
                            return startPos;
                        }
                    }
                    else
                    {
                        startPos = Array.IndexOf<byte>(searchWithin, serachFor[0], startPos + index);
                        if (startPos == -1)
                        {
                            return -1;
                        }
                        index = 0;
                    }
                }
            }

            return -1;
        }

        private byte[] ToByteArray(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public bool Success
        {
            get;
            private set;
        }

        public string ContentType
        {
            get;
            private set;
        }

        public string Filename
        {
            get;
            private set;
        }

        public byte[] FileContents
        {
            get;
            private set;
        }

        public string Clientname
        {
            get;
            private set;
        }

        public string Address
        {
            get;
            private set;
        }

        public string Telephone
        {
            get;
            private set;
        }

        public int ImagesetNumber
        {
            get;
            private set;
        }

        public int FrameNumber
        {
            get;
            private set;
        }

        public string Purpose
        {
            get;
            private set;
        }
    }
}
