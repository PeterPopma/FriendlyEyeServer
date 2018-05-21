using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FriendlyEyeServer
{
    [ServiceContract]
    public interface IFriendlyEyeServerService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "newimage", BodyStyle = WebMessageBodyStyle.Bare)]
        XElement NewImage(Stream stream);
    }
}
