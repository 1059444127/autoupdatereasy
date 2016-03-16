using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using AutoUpdaterView.Resources;

namespace AutoUpdaterView
{
    public partial class UiMain : Form
    {
        private const string FilePath = @".\UpdaterConfig.json";
        private Dictionary<string, object> _jsonFile;

        public UiMain()
        {
            InitializeComponent();
        }

        private void UiMain_Load(object sender, EventArgs e)
        {
            _jsonFile = (Dictionary<string, object>)new JavaScriptSerializer().DeserializeObject(File.ReadAllText(FilePath));
            _jsonFile["output"] = ConfigurationManager.AppSettings["OutputPath"] ?? _jsonFile["output"];

            TmrMain.Enabled = true;
        }

        private void UnzipAll()
        {
            if (!Directory.Exists(_jsonFile["output"].ToString())) Directory.CreateDirectory(_jsonFile["output"].ToString());
            using (var unzip = new Unzip($@".\{_jsonFile["packageFileName"]}"))
            {
                unzip.ExtractToDirectory(_jsonFile["output"].ToString());
            }
        }

        private void TmrMain_Tick(object sender, EventArgs e)
        {
            TmrMain.Enabled = false;
            try
            {                
                KillProcess();
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Errors.KillProcessException, Errors.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(Environment.ExitCode);
            }

            try
            {
                LblMessage.Text = Messages.Extracting;
                UnzipAll();
            }
            catch (Exception)
            {
                MessageBox.Show(Errors.Fail_Extracting_Files, Errors.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                RestartProcess();
                Environment.Exit(Environment.ExitCode);
            }            
            if (string.IsNullOrWhiteSpace(_jsonFile["processStart"].ToString())) return;
            Process.Start($@".\{_jsonFile["processStart"]}");
            Environment.Exit(Environment.ExitCode);
        }

        private void RestartProcess()
        {
            LblMessage.Text = Messages.Restarting;
            var processStart = _jsonFile["processStart"]?.ToString();
            if (processStart == null) return;
            var path = $@".\{processStart}";
            if (!File.Exists(path)) return;
            Process.Start(path);
        }

        private void KillProcess()
        {
            LblMessage.Text = string.Format(Messages.KillProcessX, Program.MainProcessName ?? "---");
            if (string.IsNullOrWhiteSpace(Program.MainProcessName)) return;
            var processes = Process.GetProcessesByName(Program.MainProcessName);
            if (processes.Length == 0) return;
            foreach (var process in processes)
            {
                process.Kill();
            }
            for (var i = 0; i < 10; i++)
            {
                var ps = Process.GetProcessesByName(Program.MainProcessName);
                if (ps.Length != 0)
                {
                    Thread.Sleep(1000);
                }
                else return;
            }
            throw new TimeoutException();
        }
    }
}
