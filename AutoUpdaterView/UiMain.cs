using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;
using AutoUpdaterView.Internals;

namespace AutoUpdaterView
{
    public partial class UiMain : Form
    {
        private const string FilePath = @".\UpdaterConfig.json";
        private string _processKill;
        private readonly string _output;

        public UiMain()
        {
            _output = ConfigurationManager.AppSettings["OutputPath"] ?? @".\";
            InitializeComponent();
        }

        private void UiMain_Load(object sender, EventArgs e)
        {
            var ser = new DataContractJsonSerializer(typeof (JsonFile));            
            var json = (JsonFile) ser.ReadObject(File.OpenRead(FilePath));
            _processKill = json.ProcessKill;
            KillProcess();

            TmrMain.Enabled = true;
        }

        private void KillProcess()
        {
            var tmp = Process.GetProcesses();
            var list = tmp.Where(p => p.ProcessName.Equals(_processKill)).Select(x => x.Id).ToList();
            foreach (var id in list)
            {
                Process.GetProcessById(id).Kill();
            }
        }

        private void UnzipAll()
        {
            try
            {
                if (!Directory.Exists(_output)) Directory.CreateDirectory(_output);

                using (var unzip = new Unzip("update.zip"))
                {
                    unzip.ExtractToDirectory(_output);
                }
            }
            catch (Exception ex)
            {
                StreamWriter sw;
                using (sw = new StreamWriter(@".\AutoUpdaterView.log.txt", true))
                {
                    sw.Write(new string('=', 45));
                    sw.Write($"{DateTime.Now.ToLongDateString()}");
                    sw.WriteLine(new string('=', 45));
                    sw.WriteLine($"TIPO : {ex.GetType().FullName}");
                    sw.WriteLine($"MENSAGEM : {ex.Message}");
                }
            }
        }

        private void TmrMain_Tick(object sender, EventArgs e)
        {
            TmrMain.Enabled = false;

            UnzipAll();

            Process.Start($@"./{_processKill}.exe");
            Environment.Exit(Environment.ExitCode);            
        }
    }
}
