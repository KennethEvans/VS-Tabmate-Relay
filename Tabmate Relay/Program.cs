using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TabmateRelay {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainForm = new MainForm();
            if (args.Length > 0 && args[0].Length >=2 &&
                args[0].ToLower().Substring(1,1) == "s") {
                mainForm.Show();
            }
            Application.Run();
        }
    }
}
