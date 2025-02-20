using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        class Zaznam
        {
            string nazev { get; set; }
        }
        private DataTable table;
        string path = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Nastaveni();
        }

        private void Nastaveni()
        {
            path = Application.StartupPath;
            path = path + "\\data.txt";
            dataGridView1.ReadOnly = true;
            table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Název", typeof(string));
            table.Columns.Add("Popis", typeof(string));
            table.Columns.Add("Kategorie", typeof(string));
            table.Columns.Add("M/K", typeof(char));
            table.Columns.Add("Počet", typeof(int));
            dataGridView1.DataSource = table;
        }

        private void prBtn_Click_1(object sender, EventArgs e)
        {
            if (int.TryParse(PridPocetTxt.Text, out int pocet))
            {
                Ulozit();
                if (!string.IsNullOrEmpty(PridMKTxt.Text) && PridMKTxt.Text.Length == 1)
                {
                    char mk = PridMKTxt.Text[0];

                    table.Rows.Add(PridIDTxt.Text, PridNazTxt.Text,
                                   PridPopTxt.Text, PridKatTxt.Text, mk, pocet);

                    PridIDTxt.Text = string.Empty;
                    PridKatTxt.Text = string.Empty;
                    PridMKTxt.Text = string.Empty;
                    PridNazTxt.Text = string.Empty;
                    PridPocetTxt.Text = string.Empty;
                    PridPopTxt.Text = string.Empty;
                }
                
            }
        }

       

        private void OdBtn_Click_1(object sender, EventArgs e)
        {
            
                string hledaneID = odbIDtxt.Text.Trim();

            foreach (DataRow row in table.Rows)
                    {

                        if (row["ID"].ToString() == hledaneID) 
                        {
                            if(int.TryParse(odbPocetTxt.Text, out int novyPocet))
                              {
                      
                    
                             if (int.TryParse(row["Počet"].ToString(), out int aktualniPocet))
                            {
                                row["Počet"] = aktualniPocet - novyPocet; 
                                       
                            }
                        }
                }
            }
                  
        }

        private void OdsBtn_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(odsIDtxt.Text))
            {

                string hledanNazev = odsIDtxt.Text.Trim().Replace("'", "''");
                DataRow[] vymazat = table.Select($"Convert(ID, 'System.String') LIKE '{hledanNazev}'");
                if (vymazat.Length > 0)
                {
                    foreach (DataRow row in vymazat)
                    {
                        row.Delete();
                    }
                    table.AcceptChanges();


                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = table;
                }

            }
        }

        private void hledBtn_Click_1(object sender, EventArgs e)
        {
            string hledatID = hledIdTxt.Text;
            string hledatKat = hledKatTxt.Text;
            string filtr = "";

            if (string.IsNullOrEmpty(hledatID))
            {
                filtr += $"ID = '{hledatID}'";
            }

            if (!string.IsNullOrEmpty(hledatKat))
            {

                if (!string.IsNullOrEmpty(filtr))
                {
                    filtr += " AND ";
                }
                filtr += $"Kategorie = '{hledatKat}'";
            }

            if (!string.IsNullOrEmpty(filtr))
            {
                DataRow[] radky = table.Select(filtr);

                if (radky.Length > 0)
                {
                    DataTable filtred = table.Clone();

                    foreach (DataRow row in radky)
                    {
                        filtred.ImportRow(row);
                    }

                    dataGridView1.DataSource = filtred;

                }
            }
            else
            {
                dataGridView1.DataSource = table;
            }
        }

        private void ZpetBtn_Click_1(object sender, EventArgs e)
        {
            dataGridView1.DataSource = table;
        }
        private void Ulozit()
        {
            StreamWriter writer = new StreamWriter(path, true);
            string zaznam = PridIDTxt.Text + ";" + PridNazTxt.Text + ";" + PridPopTxt.Text + ";" + PridMKTxt.Text + ";" + PridKatTxt.Text + ";"+PridPocetTxt.Text;
            writer.WriteLine(zaznam);
            writer.Close();

        }
    }
   
}


