using Services;
using System;
using Networking.network;
using System.Windows.Forms;


namespace ClientForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          
            IServices server = new ServicesRpcProxy("127.0.0.1", 55555);
            LoginWindow win = new LoginWindow(server);
            Application.Run(win);
        }
    }
}

