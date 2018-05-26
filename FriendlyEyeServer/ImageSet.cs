using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendlyEyeServer
{
    class ImageSet
    {
        string hints;
        string name;
        string address;
        string telephone;
        int approvals;
        int denials;
        int id;
        List<ImageFrame> images = new List<ImageFrame>();
        List<int> reviewerIDs = new List<int>();

        public ImageSet(string name, string address, string telephone, int id)
        {
            this.name = name;
            this.address = address;
            this.telephone = telephone;
            this.id = id;
        }

        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public int Approvals { get => approvals; set => approvals = value; }
        public int Denials { get => denials; set => denials = value; }
        public List<ImageFrame> Images { get => images; set => images = value; }
        public int ID { get => id; set => id = value; }
        public List<int> ReviewerIDs { get => reviewerIDs; set => reviewerIDs = value; }
        public string Hints { get => hints; set => hints = value; }

        public int ReviewCount() { return approvals + denials; }
    }
}
