using Model.domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thrift.Protocol;
using Thrift.Transport;

namespace ClientForm
{
    partial class MainPage : Form
    {
        public static Boolean needsUpdate { get; set; }
        private List<Meci> meciuriData;
        private int observerServerPort;
        private Client myClient;


        public MainPage(int observerServerPort, Client myClient)
        {
            this.myClient = myClient;
            InitializeComponent();
            this.observerServerPort = observerServerPort;
            Task.Run(() => checkForUpdate());


            TTransport transport = new TSocket("localhost", 9091);
            TProtocol protocol = new TBinaryProtocol(transport);
            transport.Open();

            TransformerService.Client client = new TransformerService.Client(protocol);
            List<MeciDTO> dtos = client.findAllMeci();
            meciuriData = retreive(dtos);
            transport.Close();

            MessageServer messageServer = new MessageServer(observerServerPort); // start mini-server as observer on the client

            PopulateMeciTable();
            this.locuriCBox.Text = "0";
            this.listaMeciuri.FullRowSelect = true;
        }


        private void checkForUpdate()
        {
            while (true)
            {
                Thread.Sleep(250);
                if (needsUpdate)
                {
                    needsUpdate = false;
                    checkForUpdates();
                }
            }
        }

        private void checkForUpdates(Boolean isUpdate = false)
        {
            Debug.WriteLine("loadProbeTable isUpdate: " + isUpdate);

            if (listaMeciuri.InvokeRequired)
            {
                listaMeciuri.Invoke(new MethodInvoker(delegate
                {
                    checkForUpdatesImpl(isUpdate);
                }));
            }
            else
            {
                checkForUpdatesImpl(isUpdate);
            }
        }


        private void checkForUpdatesImpl(bool isUpdate)
        {
            if (isUpdate)
            {
                PopulateMeciTable();
            }
            else
            {
                TTransport transport = new TSocket("localhost", 9091);
                TProtocol protocol = new TBinaryProtocol(transport);
                transport.Open();

                TransformerService.Client client = new TransformerService.Client(protocol);
                var dtos = client.findAllMeci();
                transport.Close();
                meciuriData = retreive(dtos);

                PopulateMeciTable();
            }
        }


        private List<Meci> retreive(List<MeciDTO> dtos)
        {
            var meciuri = new List<Meci>();
            foreach (var dto in dtos)
            {
                DateTime date = new DateTime(dto.Date);
                // alternative ToObject with (int) dto.Tip
                TipMeci tip = (TipMeci)Enum.Parse(typeof(TipMeci), dto.Tip.ToString());
                Meci meci = new Meci(dto.Id, dto.Home, dto.Away, date, tip, dto.NumarBileteDisponibile);
                meciuri.Add(meci);
            }
            return meciuri;
        }
        

        public void logout()
        {
            Application.Exit();
            this.Close();
        }


        private void FilterNotSoldOut()
        {
            listaMeciuri.Items.Clear();
            TTransport transport = new TSocket("localhost", 9091);
            TProtocol protocol = new TBinaryProtocol(transport);
            transport.Open();

            TransformerService.Client client = new TransformerService.Client(protocol);
            var dtos = client.findAllMeciWithTickets().OrderBy(x => x.NumarBileteDisponibile).Reverse().ToList();
            transport.Close();

            var all = retreive(dtos);

            foreach (Meci s in all)
            {
                var row = new string[] { s.id, s.home, s.away, s.date.ToShortDateString(), s.numarBileteDisponibile.ToString() };
                var lvi = new ListViewItem(row);
                listaMeciuri.Items.Add(lvi);
                lvi.Tag = s;
            }
        }



        private void PopulateMeciTable()
        {
            listaMeciuri.Items.Clear();
            


            TTransport transport = new TSocket("localhost", 9091);
            TProtocol protocol = new TBinaryProtocol(transport);
            transport.Open();

            TransformerService.Client client = new TransformerService.Client(protocol);
            var dtos = client.findAllMeci();
            transport.Close();

            var all = retreive(dtos);
            
            foreach (Meci s in all)
            {
                if (s.numarBileteDisponibile > 0)
                {
                    var row = new string[] { s.id, s.home, s.away, s.date.ToShortDateString(), s.numarBileteDisponibile.ToString() };
                    var lvi = new ListViewItem(row);
                    listaMeciuri.Items.Add(lvi);
                    lvi.Tag = s;

                }
                else
                {
                    var row = new string[] { s.id, s.home, s.away, s.date.ToShortDateString(), "SOLD OUT" };
                    ListViewItem lvi = new ListViewItem(row);
                    lvi.ForeColor = Color.Red;
                    listaMeciuri.Items.Add(lvi);
                    lvi.Tag = s;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.logout();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FilterNotSoldOut();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PopulateMeciTable();
        }




        private void button1_Click(object sender, EventArgs e)
        {
            String nume = this.numeCBox.Text;
            String bilete = this.locuriCBox.Text;
            int locuri = -1;
            int.TryParse(bilete, out locuri);

            if (nume.Length > 0 && bilete.Length > 0 && locuri != -1)
            {
                if (this.listaMeciuri.SelectedItems.Count > 0)
                {
                    ListViewItem item = this.listaMeciuri.SelectedItems[0];

                    Meci selectedMatch = (Meci)item.Tag;

                    if (selectedMatch.numarBileteDisponibile < locuri)
                    {
                        MessageBox.Show("Nu sunt destule bilete disponibile!");
                    }
                    else
                    {
                        selectedMatch.numarBileteDisponibile = selectedMatch.numarBileteDisponibile - locuri;

                        TTransport transport = new TSocket("localhost", 9091);
                        TProtocol protocol = new TBinaryProtocol(transport);
                        transport.Open();

                        TransformerService.Client client = new TransformerService.Client(protocol);
                        TipMeciDTO tipdto = (TipMeciDTO)Enum.ToObject(typeof(TipMeciDTO), selectedMatch.tip);
                        MeciDTO mdto = new MeciDTO
                        {
                            Id = selectedMatch.id,
                            Home = selectedMatch.home,
                            Away = selectedMatch.away,
                            Date = selectedMatch.date.Ticks,
                            Tip = tipdto,
                            NumarBileteDisponibile = selectedMatch.numarBileteDisponibile
                        };
                        ClientDTO cdto = new ClientDTO
                        {
                            Id = myClient.id,
                            Nume = myClient.nume,
                            Password = myClient.password,
                            Ip = myClient.host,
                            Port = myClient.port
                        };
                        client.ticketsSold(mdto, cdto);
                        transport.Close();

                        checkForUpdates(true);
                        PopulateMeciTable();

                        MessageBox.Show("Ati cumparat biletele!");
                    }
                }
                else
                {
                    MessageBox.Show("Nu ati selectat un meci din lista!");
                }
            }
            else
            {
                MessageBox.Show("Nu ati introdus bine datele!");
            }
        }
    }
}
