using FixHub.Forms;
using FixHub.Helpers;
using System;
using System.Windows.Forms;

namespace FixHub
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DatabaseInitializer.EnsureSuperAdminReady();
            Application.Run(new frmLogin());
        }
    }
}
