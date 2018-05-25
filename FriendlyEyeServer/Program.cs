using System;
using System.Windows.Forms;
using FriendlyEyeServer.Forms;
using System.ServiceModel.Web;
using System.Web.Services.Description;
using System.ServiceModel.Description;
using System.ServiceModel;

namespace FriendlyEyeServer
{
    public static class Program
    {
        public static FormMain formMain;

        //        public static FormMain FormMain { get => formMain; set => formMain = value; }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (FormMain formMain = new FormMain())
            {
                FriendlyEyeServerService friendlyEyeServerService = new FriendlyEyeServerService();
                formMain.FriendlyEyeServerService = friendlyEyeServerService;
                WebServiceHost host = new WebServiceHost(friendlyEyeServerService, new Uri("http://localhost:8000"));
//                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IFriendlyEyeServerService), new WebHttpBinding(), "");
                ServiceDebugBehavior stp = host.Description.Behaviors.Find<ServiceDebugBehavior>();
                ServiceBehaviorAttribute sba = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                sba.InstanceContextMode = InstanceContextMode.Single;
                stp.HttpHelpPageEnabled = false;
                host.Open();

                Application.Run(formMain);

                host.Close();
            }
        }
    }
}
