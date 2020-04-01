using Model.domain;
using Services;
using System;
using System.Windows.Forms;



namespace ClientForm
{
    internal partial class LoginWindow : Form
    {
        private IServices server;


        public LoginWindow(IServices server)
        {
            InitializeComponent();
            this.server = server;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string username = usernameBox.Text;
            string password = passwordBox.Text;
            if (username != null && password != null)
            {
                try
                {
                    Client client = new Client(username, password);
                    ClientCtrl ctrl = new ClientCtrl(server);
                    MainPage accountController = new MainPage(client, ctrl, this);
                    server.login(client, ctrl);
                    accountController.Show();
                    this.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
