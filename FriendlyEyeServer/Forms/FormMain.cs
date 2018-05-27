using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FriendlyEyeServer.Forms
{
    public partial class FormMain : Form
    {
        private static System.Windows.Forms.Timer updateScreenTimer;
        private FriendlyEyeServerService friendlyEyeServerService;

        public FriendlyEyeServerService FriendlyEyeServerService { get => friendlyEyeServerService; set => friendlyEyeServerService = value; }

        public FormMain()
        {
            InitializeComponent();
            SetupTimers();
        }

        private void SetupTimers()
        {
            // Create a timer with a 100 msec interval.
            updateScreenTimer = new System.Windows.Forms.Timer();
            updateScreenTimer.Interval = 100;
            updateScreenTimer.Tick += new EventHandler(OnTimedEventUpdateScreen);
            updateScreenTimer.Start();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop the timer
            updateScreenTimer.Enabled = false;
        }

        private void OnTimedEventUpdateScreen(object sender, EventArgs eArgs)
        {
            richTextBoxAlerts.Clear();
            richTextBoxAlerts.Text = CreateAlertsOverview();
        }

        private string CreateAlertsOverview()
        {
            string result = "";
            foreach(ImageSet imageSet in friendlyEyeServerService.ImageSets)
            {
                result += "<" + imageSet.ID + "> " + imageSet.Name + " - " + imageSet.Address + " - " + imageSet.Telephone + " - " + imageSet.Approvals + ":" + imageSet.Denials + "\r\n"; 
            }

            return result;
        }
    }
}
