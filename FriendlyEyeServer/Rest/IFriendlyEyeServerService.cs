using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "newwarning?reviewerid={reviewerID}", BodyStyle = WebMessageBodyStyle.Bare)]
        XElement GetNewWarning(int reviewerID);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "warningimages?id={ID}", BodyStyle = WebMessageBodyStyle.Bare)]
        XElement GetWarningImages(int ID);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "warningimage?id={ID}&frame_number={frameNumber}", BodyStyle = WebMessageBodyStyle.Bare)]
        string GetWarningImage(int ID, int frameNumber);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "review?id={ID}&reviewerid={reviewerID}&value={value}", BodyStyle = WebMessageBodyStyle.Bare)]
        XElement PostReview(int ID, int reviewerID, int value);
    }
}
