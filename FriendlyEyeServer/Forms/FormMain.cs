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
        DataTable dataTableOverview = new DataTable();

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
            updateScreenTimer.Interval = 200;
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
            CreateAlertsOverview();
        }

        private string CreateAlertsOverview()
        {
            string result = "";
            var source = new BindingSource();
            source.DataSource = friendlyEyeServerService.ImageSets;
            dataGridViewAlerts.DataSource = source;

            foreach (ImageSet imageSet in friendlyEyeServerService.ImageSets)
            {
                result += imageSet.TimeCreated.ToString("MM/dd/yyyy hh:mm:ss") + " - " + imageSet.Name + " - " + imageSet.Address + " - " + imageSet.Telephone + " - " + imageSet.Approvals + ":" + imageSet.Denials + "\r\n"; 
            }

            ResizeDataView(dataGridViewAlerts);

            return result;
        }

        private void ResizeDataView(DataGridView grd)
        {
            grd.ColumnHeadersDefaultCellStyle.BackColor = Color.Thistle;
            grd.EnableHeadersVisualStyles = false;
            grd.RowTemplate.Height = 30;

            //datagrid has calculated it's widths so we can store them
            for (int i = 0; i <= grd.Columns.Count - 1; i++)
            {
                grd.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //store autosized widths
                int colw = grd.Columns[i].Width;
                //remove autosizing
                grd.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                //set width to calculated by autosize
                grd.Columns[i].Width = colw;
            }
        }
    }
}
