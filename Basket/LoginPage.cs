using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basket.services;
//using Dapper;

namespace Basket
{
    internal partial class LoginPage : Form
    {
        private MasterService masterService;


        public LoginPage(MasterService masterService)
        {
            InitializeComponent();
            this.masterService = masterService;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = usernameBox.Text;
            string password = passwordBox.Text;
            if (username != null && password != null)
            {
                if (masterService.FindClientByCredentials(username, password) != null)
                {
                    MainPage main = new MainPage(masterService, this);
                    main.Show();
                    this.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Nu exista cont cu aceste date de logare! Va rugam incercati din nou!");
                }
            }
        }
    }
}
