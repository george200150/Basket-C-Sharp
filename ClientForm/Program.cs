using System;
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
          
            LoginWindow win = new LoginWindow();
            Application.Run(win);
        }
    }
}

