using MissionPlanner.Utilities;
using MissionPlanner.Controls;
using System;
using System.Windows.Forms;

namespace TelemetryLogger
{
    public partial class TelemetryLoggerConfigView : UserControl, IActivate
    {
        public TelemetryLoggerConfigView()
        {
            InitializeComponent();
        }

        public void Activate()
        {
            txt_log_dir.Text = TelemetryLoggerPlugin.GetLogDir();

            var frequencyParsed = decimal.Parse(Settings.Instance["TelemetryLogFrequency"]);
            frequency.Value = frequencyParsed;
        }


        private void OnLogDirBrowseClicked(object sender, EventArgs e)
        {
            var ofd = new FolderBrowserDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txt_log_dir.Text = ofd.SelectedPath;
            }
        }

        private void OnLogDirTextChanged(object sender, EventArgs e)
        {
            string path = txt_log_dir.Text;
            if (!string.IsNullOrEmpty(path) && System.IO.Directory.Exists(path))
            {
                Settings.Instance["TelemetryLogDir"] = path;
            }
        }

        private void OnFrequencyChanged(object sender, EventArgs e)
        {
            Settings.Instance["TelemetryLogFrequency"] = frequency.Value.ToString();
        }

    }
}
