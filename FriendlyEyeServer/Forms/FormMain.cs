﻿using System;
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

        public FormMain()
        {
            InitializeComponent();
            SetupTimers();
        }

        private void SetupTimers()
        {
            // Create a timer with a 10 msec interval.
            updateScreenTimer = new System.Windows.Forms.Timer();
            updateScreenTimer.Interval = 10;
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

        }
    }
}
