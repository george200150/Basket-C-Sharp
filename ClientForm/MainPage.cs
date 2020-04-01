using Model.domain;
using Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClientForm
{
    partial class MainPage : Form
    {

        private readonly ClientCtrl ctrl;
        private readonly IList<Meci> meciuriData; // removed readonly to be able to remove and add an element each update
        private Client currentUser { get; set; }
        LoginWindow loginWindow;

        public event EventHandler<UserEventArgs> updateEvent; //ctrl calls it when it has received an update

        public MainPage(Client client, ClientCtrl ctrl, LoginWindow loginPage)
        {
            this.ctrl = ctrl;
            meciuriData = ctrl.getAllMeciuri().ToList();
            InitializeComponent();
            this.currentUser = client;
            this.loginWindow = loginPage;
            PopulateMeciTable();
            this.numeCBox.Text = this.currentUser.nume;
            this.locuriCBox.Text = "0";
            this.listaMeciuri.FullRowSelect = true;
            ctrl.updateEvent += userUpdate; // ADD OBSERVER (DELEGATE METHOD TO REFRESH GUI)
        }


        protected virtual void OnUserEvent(UserEventArgs e) // CALL REFRESH GUI
        {
            if (updateEvent == null) return;
            updateEvent(this, e);
            Console.WriteLine("Update Event called");
        }


        public void logout()
        {
            Console.WriteLine("Ctrl logout");
            ctrl.logout(currentUser);
            currentUser = null;
            loginWindow.Enabled = true;
        }


        private void FilterNotSoldOut()
        {
            listaMeciuri.Items.Clear();
            List<Meci> all = ctrl.getAllMeciWithTickets().OrderBy(x => x.numarBileteDisponibile).Reverse().ToList();

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
            IEnumerable<Meci> all = ctrl.getAllMeciuri();

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
                        currentUser.nume = this.numeCBox.Text;
                        this.ctrl.ticketsSold(selectedMatch, currentUser);
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


        //for updating the GUI

        //1. define a method for updating the ListView
        private void updateListView(ListView listView, IList<Meci> newData)
        {
            listView.Items.Clear();

            foreach (Meci s in newData)
            {
                if (s.numarBileteDisponibile > 0)
                {
                    var row = new string[] { s.id, s.home, s.away, s.date.ToShortDateString(), s.numarBileteDisponibile.ToString() };
                    var lvi = new ListViewItem(row);
                    listView.Items.Add(lvi);
                    lvi.Tag = s;
                }
                else
                {
                    var row = new string[] { s.id, s.home, s.away, s.date.ToShortDateString(), "SOLD OUT" };
                    ListViewItem lvi = new ListViewItem(row);
                    lvi.ForeColor = Color.Red;
                    listView.Items.Add(lvi);
                    lvi.Tag = s;
                }
            }
        }

        //2. define a delegate to be called back by the GUI Thread
        public delegate void UpdateListViewCallback(ListView list, IList<Meci> data);

        //3. in the other thread call like this:
        /*
         * list.Invoke(new UpdateListBoxCallback(this.updateListBox), new Object[]{list, data});
         * 
         * */

        public void userUpdate(object sender, UserEventArgs e)
        {

            if (e.UserEventType == UpdateType.TICKETS_SOLD)
            {
                String meciInfo = e.Data.ToString();

                Meci meciUpdated = (Meci)e.Data;
                meciuriData.Remove(meciUpdated);
                meciuriData.Add(meciUpdated);

                Console.WriteLine("[MainWindow] Updated Meci " + meciInfo);
                listaMeciuri.BeginInvoke(new UpdateListViewCallback(this.updateListView), new Object[] { listaMeciuri, meciuriData });
            }
        }
    }
}
