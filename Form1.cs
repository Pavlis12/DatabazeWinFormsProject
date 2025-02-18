using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        private DataTable table;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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

        private void prBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(PridPocetTxt.Text, out int pocet))
            {
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

        private void hledBtn_Click(object sender, EventArgs e)
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

        private void ZpetBtn_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = table;
        }

        private void OdBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(odbIDtxt.Text) && int.TryParse(odbPocetTxt.Text, out int novyPocet))
            {

                string HledanNazev = odbIDtxt.Text;
                foreach (DataRow row in table.Rows)
                {
                    if ((string)row["ID"] == HledanNazev)
                    {
                        row["Počet"] = novyPocet;
                        break;
                    }

                }

            }
        }

        private void OdsBtn_Click(object sender, EventArgs e)
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

       
    }
}