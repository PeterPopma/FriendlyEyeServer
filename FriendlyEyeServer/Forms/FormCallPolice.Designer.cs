namespace FriendlyEyeServer.Forms
{
    partial class FormCallPolice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBoxPoliceAPI = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(812, 76);
            this.label1.TabIndex = 0;
            this.label1.Text = "Calling Police Incident API";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.richTextBoxPoliceAPI);
            this.panel1.Location = new System.Drawing.Point(86, 143);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1177, 502);
            this.panel1.TabIndex = 1;
            // 
            // richTextBoxPoliceAPI
            // 
            this.richTextBoxPoliceAPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxPoliceAPI.ForeColor = System.Drawing.Color.RoyalBlue;
            this.richTextBoxPoliceAPI.Location = new System.Drawing.Point(20, 15);
            this.richTextBoxPoliceAPI.Name = "richTextBoxPoliceAPI";
            this.richTextBoxPoliceAPI.Size = new System.Drawing.Size(1141, 458);
            this.richTextBoxPoliceAPI.TabIndex = 0;
            this.richTextBoxPoliceAPI.Text = "POST api.politie.nl/incident { ";
            // 
            // FormCallPolice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 648);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "FormCallPolice";
            this.Text = "FormCallPolice";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.RichTextBox richTextBoxPoliceAPI;
    }
}