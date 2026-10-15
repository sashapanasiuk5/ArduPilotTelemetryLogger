namespace TelemetryLogger
{
    partial class TelemetryLoggerConfigView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BUT_logdirbrowse = new MissionPlanner.Controls.MyButton();
            this.txt_log_dir = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.frequency = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.frequency)).BeginInit();
            this.SuspendLayout();
            // 
            // BUT_logdirbrowse
            // 
            this.BUT_logdirbrowse.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BUT_logdirbrowse.Location = new System.Drawing.Point(553, 12);
            this.BUT_logdirbrowse.Name = "BUT_logdirbrowse";
            this.BUT_logdirbrowse.Size = new System.Drawing.Size(75, 23);
            this.BUT_logdirbrowse.TabIndex = 98;
            this.BUT_logdirbrowse.Text = "Browse";
            this.BUT_logdirbrowse.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_logdirbrowse.UseVisualStyleBackColor = true;
            this.BUT_logdirbrowse.Click += new System.EventHandler(this.OnLogDirBrowseClicked);
            // 
            // txt_log_dir
            // 
            this.txt_log_dir.Location = new System.Drawing.Point(161, 12);
            this.txt_log_dir.Name = "txt_log_dir";
            this.txt_log_dir.Size = new System.Drawing.Size(386, 22);
            this.txt_log_dir.TabIndex = 97;
            this.txt_log_dir.TextChanged += new System.EventHandler(this.OnLogDirTextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(17, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 16);
            this.label3.TabIndex = 96;
            this.label3.Text = "Log Telemetry Path";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label23.Location = new System.Drawing.Point(17, 50);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(85, 16);
            this.label23.TabIndex = 100;
            this.label23.Text = "Log frequency";
            // 
            // NUM_frequency
            // 
            this.frequency.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frequency.Location = new System.Drawing.Point(115, 49);
            this.frequency.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.frequency.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frequency.Name = "NUM_frequency";
            this.frequency.Size = new System.Drawing.Size(67, 22);
            this.frequency.TabIndex = 99;
            this.frequency.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});

            this.frequency.ValueChanged += new System.EventHandler(this.OnFrequencyChanged);
            // 
            // TelemetryLoggerConfigView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label23);
            this.Controls.Add(this.frequency);
            this.Controls.Add(this.BUT_logdirbrowse);
            this.Controls.Add(this.txt_log_dir);
            this.Controls.Add(this.label3);
            this.Name = "TelemetryLoggerConfigView";
            this.Size = new System.Drawing.Size(655, 150);
            ((System.ComponentModel.ISupportInitialize)(this.frequency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MissionPlanner.Controls.MyButton BUT_logdirbrowse;
        private System.Windows.Forms.TextBox txt_log_dir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown frequency;
    }
}
