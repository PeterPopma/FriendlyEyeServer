using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FriendlyEyeServer
{
    class KPNClient
    {
        static HttpClient client = new HttpClient();
        const string HOST_URL = "http://localhost:8000"; //"http://192.168.1.103:8000";

        public KPNClient()
        {
            client.BaseAddress = new Uri(HOST_URL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public string AnalyzeImage(string imageUrl)
        {
            string result = "";
            HttpResponseMessage response = null;
            string access_token = "";
            try
            {
                /* curl -X POST \
                     'https://api-prd.kpn.com/oauth/client_credential/accesstoken?grant_type=client_credentials' \
                     -H 'content-type: application/x-www-form-urlencoded' \
                     -d 'client_id=APP_CONSUMER_KEY&client_secret=APP_CONSUMER_SECRET'
                */
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", "OheiFTNnQDAiIorhTVaBRcVhSt7hYWKy"), new KeyValuePair<string, string>("client_secret", "MvgAJadHESAghMpy")
                });
                response = client.PostAsync("https://api-prd.kpn.com/oauth/client_credential/accesstoken?grant_type=client_credentials", content).GetAwaiter().GetResult();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Cannot connect to KPN host!");
                return "";
            }

            string text = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            // Look for access token
            Regex re = new Regex(@"(?<=access_token\"" : \"")(.*?)(?=\"")");
            Match match = re.Match(text);
            if (match.Success)
            {
                access_token = match.Value;
            }

            // www.gravityone.nl/uploads/Peter_462414_1.png
            client.DefaultRequestHeaders.Accept.Clear();
       //     client.DefaultRequestHeaders.Accept.Add(
       //         new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("authorization", "BearerToken " + access_token);

            ImageRecognitionBody imageRecognitionBody = new ImageRecognitionBody();
            imageRecognitionBody.data.Add(new ImageRecognitionData("png", imageUrl));
            string jsonString = imageRecognitionBody.ToJSON();
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            try
            {

             //   client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                response = client.PostAsync("https://api-prd.kpn.com/ai/image-classifier/v1/classify", jsonContent).GetAwaiter().GetResult();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Cannot connect to KPN host!");
                return "";
            }

            text = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            // Look for access token
        //    re = new Regex(@"(?<=classification\"": \"")(.*?)(?=\"")");
            MatchCollection matches = Regex.Matches(text, @"(?<=classification\"": \"")(.*?)(?=\"")");
            foreach(Match myMatch in matches)
            {
                result += myMatch.Value + ", ";
            }
            if(result.Length>2)
            {
                result = result.Substring(0, result.Length - 2);       // strip last comma
            }

            return result;
        }

    }
}
