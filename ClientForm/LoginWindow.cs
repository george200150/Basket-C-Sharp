using Model.domain;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Windows.Forms;
using Thrift.Protocol;
using Thrift.Transport;

namespace ClientForm
{
    internal partial class LoginWindow : Form
    {
        public static int observerServerPort;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private int getAvailablePort(int startPort)
        {
            int port = startPort;
            bool isNotBlocked = false;

            while (!isNotBlocked)
            {
                isNotBlocked = true;

                using (TcpClient tcpClient = new TcpClient())
                {
                    try
                    {
                        tcpClient.Connect("127.0.0.1", port);
                        Console.WriteLine("OPENED PORT: {0}", port);
                        isNotBlocked = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("closed port: {0}", port);
                    }
                }

                if (isNotBlocked)
                {
                    return port;
                }
                else
                {
                    port++;
                }
            }
            return -1;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string username = usernameBox.Text;
            string password = passwordBox.Text;
            if (username != null && password != null)
            {
                TTransport transport = new TSocket("localhost", 9091);
                TProtocol protocol = new TBinaryProtocol(transport);
                transport.Open();

                TransformerService.Client client = new TransformerService.Client(protocol);
                observerServerPort = getAvailablePort(9092);
                Debug.WriteLine("port for client's server: " + observerServerPort);
                String response = client.login(username, password, "localhost", observerServerPort);
                Console.WriteLine("CSharp client received: {0}", response);
                transport.Close();

                if (!response.Equals("success"))
                {
                    throw new Exception("authentification failed");
                }
                Random random = new Random();
                string id = random.NextDouble().ToString();
                Client myClient = new Client(id, username, password, "localhost", observerServerPort);
                MainPage form2 = new MainPage(observerServerPort, myClient);
                form2.Text = "Form for " + username;
                form2.Show();
                this.Hide();
            }
        }
    }
}
