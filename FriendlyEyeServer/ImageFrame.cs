using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendlyEyeServer
{
    class ImageFrame
    {
        byte[] imageData;
        int frameNumber;

        public ImageFrame(byte[] imageData, int frameNumber)
        {
            this.imageData = imageData;
            this.frameNumber = frameNumber;
        }

        public byte[] ImageData { get => imageData; set => imageData = value; }
        public int FrameNumber { get => frameNumber; set => frameNumber = value; }
    }
}
