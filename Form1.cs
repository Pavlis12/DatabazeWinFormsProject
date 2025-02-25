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
            public string ID { get; set; }
            public string Nazev { get; set; }
            public string Popis { get; set; }
            public string MK { get; set; }
            public string Kategorie { get; set; }
            public string Pocet { get; set; }
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
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void prBtn_Click_1(object sender, EventArgs e)
        {
            Pridat();
        }
        
        private void Pridat()
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
            OdebratPocet();
        }

        private void OdebratPocet()
        {
            string hledaneID = odbIDtxt.Text.Trim();

            foreach (DataRow row in table.Rows)
            {

                if (row["ID"].ToString() == hledaneID)
                {
                    if (int.TryParse(odbPocetTxt.Text, out int novyPocet))
                    {


                        if (int.TryParse(row["Počet"].ToString(), out int aktualniPocet))
                        {
                            if (novyPocet > aktualniPocet)
                            {
                                MessageBox.Show($"Nelze jít do záporných hodnot{novyPocet} > {aktualniPocet}");
                                break;
                            }
                            row["Počet"] = aktualniPocet - novyPocet;
                            Prepsat();
                        }
                    }
                }
            }

        }

        private void OdsBtn_Click_1(object sender, EventArgs e)
        {
            Ostranit();
        }

        private void Prepis()
        {
            StreamWriter writer = new StreamWriter(path, false);
            string zaznam = "";
            writer.WriteLine(zaznam);
            writer.Close();
        }

        private void Ostranit()
        {
            if (!string.IsNullOrEmpty(odsIDtxt.Text))
            {
                Prepis();
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
            Hledat();
           
        }

        private void Hledat()
        {
            string hledatID = hledIdTxt.Text;
            string hledatKat = hledKatTxt.Text;
            string filtr = "";

            if (!string.IsNullOrEmpty(hledatID))
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
            string zaznam = PridIDTxt.Text + ";" + PridNazTxt.Text + ";" + PridPopTxt.Text + ";" + PridMKTxt.Text + ";" + PridKatTxt.Text + ";" + PridPocetTxt.Text;
            writer.WriteLine(zaznam);
            writer.Close();
        }
        
        private void Prepsat()
        {
            StreamWriter writer = new StreamWriter(path, false);
            foreach (DataRow row in table.Rows)
            {
                string zaznam = $"{row["ID"]};{row["Název"]};{row["Popis"]};{row["M/K"]};{row["Kategorie"]};{row["Počet"]}";
                writer.WriteLine(zaznam);
            }
            writer.Close();
        }

        private void NacistBtn_Click(object sender, EventArgs e)
        {
            Nacist();
        }

        private void Nacist()
        {
            StreamReader reader = new StreamReader(path, true);
            table.Clear();
            string radek;
            List<Zaznam> produkty = new List<Zaznam>();
            while ((radek = reader.ReadLine()) != null)
            {
               
                string[] hodnoty = radek.Split(';');
                if (hodnoty.Length == 6)
                {
                    
                    DataRow row = table.NewRow();
                    row["ID"] = hodnoty[0];
                    row["Název"] = hodnoty[1];
                    row["Popis"] = hodnoty[2];
                    row["M/K"] = hodnoty[3][0]; 
                    row["Kategorie"] = hodnoty[4];
                    row["Počet"] = int.TryParse(hodnoty[5], out int pocet) ? pocet : 0;

                    
                    table.Rows.Add(row);
                }
                 

            }
            reader.Close();
            dataGridView1.DataSource = table;
           
        }

        private void PridatPocBtn_Click(object sender, EventArgs e)
        {
            PridatPocet();
        }

        private void PridatPocet()
        {
            string hledaneID = odbIDtxt.Text.Trim();

            foreach (DataRow row in table.Rows)
            {

                if (row["ID"].ToString() == hledaneID)
                {
                    if (int.TryParse(odbPocetTxt.Text, out int novyPocet))
                    {


                        if (int.TryParse(row["Počet"].ToString(), out int aktualniPocet))
                        {

                            row["Počet"] = aktualniPocet + novyPocet;
                            Prepsat();
                        }
                    }
                }
            }
        }
    }
    }



