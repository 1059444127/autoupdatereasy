using System;
using System.Windows.Forms;

namespace AutoUpdaterView
{
    public static class Program
    {
        public static string MainProcessName { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {            
            if (args != null && args.Length == 1)
            {
                MainProcessName = args[0];
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UiMain());
        }
    }
}
