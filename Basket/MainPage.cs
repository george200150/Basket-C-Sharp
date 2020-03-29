using Basket.domain;
using Basket.services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Basket
{
    internal partial class MainPage : Form
    {
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet set = new DataSet();
        MasterService masterService;
        LoginPage loginPage;

        public MainPage(MasterService masterService, LoginPage loginPage)
        {
            InitializeComponent();
            this.masterService = masterService;
            this.loginPage = loginPage;
            this.listaMeciuri.FullRowSelect = true;
            PopulateMeciTable();

        }


        private List<MeciDTO> ToDTO(IEnumerable<Meci> meciList)
        {
            List<MeciDTO> dtos = new List<MeciDTO>();
            foreach (Meci meci in meciList)
            {
                String id = meci.id;
                int bilete = meci.numarBileteDisponibile;
                TipMeci tip = meci.tip;
                DateTime date = meci.date;
                Echipa home = masterService.FindOneEchipa(meci.home);
                Echipa away = masterService.FindOneEchipa(meci.away);
                MeciDTO dto = new MeciDTO(id, home, away, date, tip, bilete);
                dtos.Add(dto);
            }
            return dtos;
        }

        private void FilterNotSoldOut()
        {
            listaMeciuri.Items.Clear();
            IEnumerable<Meci> all = masterService.FindAllMeci();

            List<MeciDTO> allDTO = ToDTO(all);

            allDTO = allDTO.OrderBy(x => x.numarBilete).Reverse().ToList();

            foreach (MeciDTO s in allDTO)
            {
                if (s.numarBilete > 0)
                {
                    var row = new string[] { s.id, s.homeString, s.awayString, s.date.ToShortDateString(), s.numarBileteSauSoldOut };
                    var lvi = new ListViewItem(row);
                    listaMeciuri.Items.Add(lvi);
                    lvi.Tag = s;
                }
            }
        }

        private void PopulateMeciTable()
        {
            listaMeciuri.Items.Clear();
            IEnumerable<Meci> all = masterService.FindAllMeci();

            IEnumerable<MeciDTO> allDTO = ToDTO(all);

            foreach (MeciDTO s in allDTO)
            {
                if (s.numarBilete > 0)
                {
                    var row = new string[] { s.id, s.homeString, s.awayString, s.date.ToShortDateString(), s.numarBileteSauSoldOut};
                    var lvi = new ListViewItem(row);
                    listaMeciuri.Items.Add(lvi);
                    lvi.Tag = s;
                }
                else
                {
                    var row = new string[] { s.id, s.homeString, s.awayString, s.date.ToShortDateString(), s.numarBileteSauSoldOut };
                    ListViewItem lvi = new ListViewItem(row);
                    lvi.ForeColor = Color.Red;
                    listaMeciuri.Items.Add(lvi);
                    lvi.Tag = s;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            this.loginPage.Enabled = true;
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

                    MeciDTO meciul = (MeciDTO)item.Tag;

                    if (meciul.numarBilete < locuri)
                    {
                        MessageBox.Show("Nu sunt destule bilete disponibile!");
                    }
                    else
                    {
                        Meci meciNou = new Meci(meciul.id, meciul.home.id, meciul.away.id, meciul.date, meciul.tip, meciul.numarBilete - locuri);
                        this.masterService.UpdateMeci(meciNou);
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

        private void listaMeciuri_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
