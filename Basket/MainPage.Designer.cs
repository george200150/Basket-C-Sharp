namespace Basket
{
    partial class MainPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sHome = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sAway = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sNrAvailableSeats = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sNrSoldSeats = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.locuriCBox = new System.Windows.Forms.TextBox();
            this.numeCBox = new System.Windows.Forms.TextBox();
            this.locuriBox = new System.Windows.Forms.Label();
            this.numeBox = new System.Windows.Forms.Label();
            this.aId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.aArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.aLocatie = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.aOra = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.aNrAvailableSeats = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listaMeciuri = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sId
            // 
            this.sId.Text = "Id";
            this.sId.Width = 0;
            // 
            // sHome
            // 
            this.sHome.Text = "Home";
            this.sHome.Width = 93;
            // 
            // sAway
            // 
            this.sAway.Text = "Away";
            this.sAway.Width = 79;
            // 
            // sData
            // 
            this.sData.Text = "Data";
            this.sData.Width = 92;
            // 
            // sNrAvailableSeats
            // 
            this.sNrAvailableSeats.Text = "Locuri disponibile";
            this.sNrAvailableSeats.Width = 108;
            // 
            // locuriCBox
            // 
            this.locuriCBox.Location = new System.Drawing.Point(210, 374);
            this.locuriCBox.Margin = new System.Windows.Forms.Padding(2);
            this.locuriCBox.Name = "locuriCBox";
            this.locuriCBox.Size = new System.Drawing.Size(127, 20);
            this.locuriCBox.TabIndex = 3;
            // 
            // numeCBox
            // 
            this.numeCBox.Location = new System.Drawing.Point(210, 340);
            this.numeCBox.Margin = new System.Windows.Forms.Padding(2);
            this.numeCBox.Name = "numeCBox";
            this.numeCBox.Size = new System.Drawing.Size(127, 20);
            this.numeCBox.TabIndex = 2;
            // 
            // locuriBox
            // 
            this.locuriBox.AutoSize = true;
            this.locuriBox.Location = new System.Drawing.Point(107, 378);
            this.locuriBox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.locuriBox.Name = "locuriBox";
            this.locuriBox.Size = new System.Drawing.Size(95, 13);
            this.locuriBox.TabIndex = 5;
            this.locuriBox.Text = "Numar locuri dorite";
            // 
            // numeBox
            // 
            this.numeBox.AutoSize = true;
            this.numeBox.Location = new System.Drawing.Point(107, 340);
            this.numeBox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.numeBox.Name = "numeBox";
            this.numeBox.Size = new System.Drawing.Size(91, 13);
            this.numeBox.TabIndex = 4;
            this.numeBox.Text = "Nume cumparator";
            // 
            // aId
            // 
            this.aId.Text = "aId";
            this.aId.Width = 0;
            // 
            // aArtist
            // 
            this.aArtist.Text = "Home";
            this.aArtist.Width = 144;
            // 
            // aLocatie
            // 
            this.aLocatie.Text = "Away";
            this.aLocatie.Width = 168;
            // 
            // aOra
            // 
            this.aOra.Text = "Data";
            this.aOra.Width = 98;
            // 
            // aNrAvailableSeats
            // 
            this.aNrAvailableSeats.Text = "Locuri disponibile";
            this.aNrAvailableSeats.Width = 116;
            // 
            // listaMeciuri
            // 
            this.listaMeciuri.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.aId,
            this.aArtist,
            this.aLocatie,
            this.aOra,
            this.aNrAvailableSeats});
            this.listaMeciuri.HideSelection = false;
            this.listaMeciuri.Location = new System.Drawing.Point(358, 33);
            this.listaMeciuri.Margin = new System.Windows.Forms.Padding(2);
            this.listaMeciuri.Name = "listaMeciuri";
            this.listaMeciuri.Size = new System.Drawing.Size(533, 241);
            this.listaMeciuri.TabIndex = 12;
            this.listaMeciuri.UseCompatibleStateImageBehavior = false;
            this.listaMeciuri.View = System.Windows.Forms.View.Details;
            this.listaMeciuri.SelectedIndexChanged += new System.EventHandler(this.listaMeciuri_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(173, 403);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 19);
            this.button1.TabIndex = 14;
            this.button1.Text = "cumpara";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(85, 136);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 19);
            this.button2.TabIndex = 13;
            this.button2.Text = "filtreaza";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(440, 403);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 19);
            this.button3.TabIndex = 15;
            this.button3.Text = "log out";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(173, 136);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(56, 19);
            this.button4.TabIndex = 16;
            this.button4.Text = "refresh";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 472);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listaMeciuri);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.locuriBox);
            this.Controls.Add(this.numeBox);
            this.Controls.Add(this.locuriCBox);
            this.Controls.Add(this.numeCBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainPage";
            this.Text = "mainPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColumnHeader sId;
        private System.Windows.Forms.ColumnHeader sHome;
        private System.Windows.Forms.ColumnHeader sAway;
        private System.Windows.Forms.ColumnHeader sData;
        private System.Windows.Forms.ColumnHeader sNrAvailableSeats;
        private System.Windows.Forms.ColumnHeader sNrSoldSeats;
        private System.Windows.Forms.TextBox locuriCBox;
        private System.Windows.Forms.TextBox numeCBox;
        private System.Windows.Forms.Label locuriBox;
        private System.Windows.Forms.Label numeBox;
        private System.Windows.Forms.ColumnHeader aId;
        private System.Windows.Forms.ColumnHeader aArtist;
        private System.Windows.Forms.ColumnHeader aLocatie;
        private System.Windows.Forms.ColumnHeader aOra;
        private System.Windows.Forms.ColumnHeader aNrAvailableSeats;
        private System.Windows.Forms.ListView listaMeciuri;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}