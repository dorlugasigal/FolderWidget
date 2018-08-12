using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderWidget
{


    public static class clsManageComunication
    {

        public static BackgroundWorker m_ServerWorker;
        public static BackgroundWorker GetServerWorker
        {
            get
            {
                if (m_ServerWorker == null)
                {
                    m_ServerWorker = new BackgroundWorker();
                    m_ServerWorker.DoWork += M_ServerWorker_DoWork;
                }
                return m_ServerWorker;
            }
        }



        public static void M_ServerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut, 4);
            StreamReader sr = new StreamReader(pipeServer);
            StreamWriter sw = new StreamWriter(pipeServer);
            do
            {
                try
                {
                    pipeServer.WaitForConnection();
                    pipeServer.WaitForPipeDrain();
                    SendMessage(sr.ReadLine());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    pipeServer.WaitForPipeDrain();
                    if (pipeServer.IsConnected)
                    {
                        pipeServer.Disconnect();
                    }
                }
            } while (true);
        }

        public static void TransmitDataToServer(string data)
        {
            var pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut, PipeOptions.Asynchronous);
            if (pipeClient.IsConnected != true) { pipeClient.Connect(); }
            StreamReader sr = new StreamReader(pipeClient);
            StreamWriter sw = new StreamWriter(pipeClient);
            try
            {
                sw.WriteLine(Guid.NewGuid());
                sw.Flush();
                pipeClient.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public delegate void dlSendMessage(string Message);
        public static event dlSendMessage OnSendMessage;
        public static void SendMessage(string Message)
        {
            if (OnSendMessage != null)
            {
                OnSendMessage(Message);
            }
        }
    }
}
