using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaxBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            using (var mutex = new Mutex(false, "ServerB.exe"))
            {
                if (!mutex.WaitOne(TimeSpan.FromSeconds(3), false))
                {
                    MessageBox.Show(
                        "Программа уже запущена!",
                        "Программа уже запущена!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Logger.Initialize();
                Application.Run(new MainForm());
            }
        }
    }
}
