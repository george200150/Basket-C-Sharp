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
        public static int portForClientsServer;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private int getAvailablePort(int startPort)
        {
            int port = startPort;
            bool isAvailable = false;

            while (!isAvailable)
            {
                isAvailable = true;

                using (TcpClient tcpClient = new TcpClient())
                {
                    try
                    {
                        tcpClient.Connect("127.0.0.1", port);
                        Console.WriteLine("Port open" + port);
                        isAvailable = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Port closed" + port);
                    }
                }

                if (isAvailable)
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
                portForClientsServer = getAvailablePort(9092);
                Debug.WriteLine("port for client's server: " + portForClientsServer);
                String response = client.login(username, password, "localhost", portForClientsServer);
                Console.WriteLine("CSharp client received: {0}", response);
                transport.Close();

                if (!response.Equals("success"))
                {
                    throw new Exception("authentification failed");
                }
                Random random = new Random();
                string id = random.NextDouble().ToString();
                Client myClient = new Client(id, username, password, "localhost", portForClientsServer);
                MainPage form2 = new MainPage(portForClientsServer, myClient);
                form2.Text = "Window for " + username;
                form2.Show();
                this.Hide();
            }
        }
    }
}
