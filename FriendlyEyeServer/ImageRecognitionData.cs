using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendlyEyeServer
{
    class ImageRecognitionData
    {
        public string ext;
        public string path;

        public ImageRecognitionData(string ext, string path)
        {
            this.ext = ext;
            this.path = path;
        }
    }
}
