using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderWidget
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            bool appNotRunning;
            try
            {
                Mutex mtx = new Mutex(false, "FolderWidget", out appNotRunning);
                if (appNotRunning)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.ThreadException += Application_ThreadException;
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                    clsManageComunication.GetServerWorker.RunWorkerAsync();
                    Application.Run(new frmMain());
                }
                else
                {
                    clsManageComunication.TransmitDataToServer(string.Empty);
                    return;
                }
            }
            catch (Exception ex)
            {
                clsFileManager.WriteError("program, Error:" + ex.Message, ex.StackTrace);
                MessageBox.Show("Fatal Error Occurred");
                Application.Exit();
            }
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (e.ExceptionObject as Exception);
            clsFileManager.WriteError("main, Error:" + ex.Message, ex.StackTrace);
            MessageBox.Show("Fatal Error Occurred");
            Application.Exit();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            clsFileManager.WriteError("from Client, Error:" + e.Exception.Message, e.Exception.StackTrace);
            MessageBox.Show("Fatal Error Occurred");
            Application.Exit();
        }
    }


}



