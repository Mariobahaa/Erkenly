using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Parking_Garage_Automation
{
    public partial class Form2 : Form
    {
        OracleConnection conn = new OracleConnection("Data source = orcl1; user id = mario; password = titans;");
        string id,name,status;
        List<string> addresses = new List<string>();
        int shown = 0;
        string spotid;float costph;
        public Form2(string idi, string namei, string statusi)
        {
            InitializeComponent();
            id = idi; name = namei; status = statusi;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                string reg = textBox1.Text.ToString();
                OracleCommand search = new OracleCommand();
                search.Connection = conn;
                search.Parameters.Add("reg", reg);
                search.CommandText = "select address from place where region = :reg and status = 'f'";
                OracleDataReader rd = search.ExecuteReader();
                int av = 0;
                while(rd.Read())
                {
                    addresses.Add(rd[0].ToString());
                    av++;
                }
                rd.Close();
                if (av > 0)
                {
                    StringBuilder f = new StringBuilder();
                    f.Append("http://maps.google.com/maps?q=");
                    f.Append(addresses[0]);
                    webBrowser1.Navigate(f + "," + "+");

                    OracleCommand s = new OracleCommand();
                    s.Connection = conn;
                    s.Parameters.Add("addr", addresses[0]);
                    s.CommandText = "select place_id, costph from place where address= :addr";
                    //MessageBox.Show(addresses[0]);
                    OracleDataReader rdi = s.ExecuteReader();
                    while (rdi.Read())
                    {
                        spotid = rdi[0].ToString();
                        label3.Text = "Spot ID: " + rdi[0].ToString();
                        label1.Text = "Price/hour: " + rdi[1];
                        costph = Convert.ToSingle(rdi[1]);
                    }
                    rdi.Close();
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (shown < addresses.Count-1)
            {
                shown++;
                StringBuilder find = new StringBuilder();
                find.Append("http://maps.google.com/maps?q=");
                find.Append(addresses[shown]);
                webBrowser1.Navigate(find + "," + "+");

                OracleCommand srch = new OracleCommand();
                srch.Connection = conn;
                srch.Parameters.Add("addre", addresses[shown]);
                MessageBox.Show("available spot: " + addresses[shown]);
                srch.CommandText = "select place_id, costph from place where address = :addre";
                OracleDataReader rdr1 = srch.ExecuteReader();
                while (rdr1.Read())
                {
                    spotid = rdr1[0].ToString();
                    label3.Text = "Spot ID: " + rdr1[0].ToString();
                    label1.Text = "Price/hour: " + rdr1[1];
                    costph = Convert.ToSingle(rdr1[1]);
                }
                rdr1.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (shown > 0)
            {
                shown--;
                StringBuilder find = new StringBuilder();
                find.Append("http://maps.google.com/maps?q=");
                find.Append(addresses[shown]);
                webBrowser1.Navigate(find + "," + "+");

                OracleCommand srchi = new OracleCommand();
                srchi.Connection = conn;
                srchi.Parameters.Add("addrr", addresses[shown]);
                MessageBox.Show("available spot: " + addresses[shown]);
                srchi.CommandText = "select place_id, costph from place where address = :addrr";
                OracleDataReader rdr2 = srchi.ExecuteReader();
                while (rdr2.Read())
                {
                    spotid = rdr2[0].ToString();
                    label3.Text = "Spot ID: " + rdr2[0].ToString();
                    label1.Text = "Price/hour: " + rdr2[1];
                    costph = Convert.ToSingle(rdr2[1]);
                }
                rdr2.Close();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (status == "f")
            {
                Form4 f = new Form4(spotid, id);
                f.Show();
            }
            else MessageBox.Show("You've already reserved a parking spot!");
        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            Form5 f = new Form5(id);
            f.Show();
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            conn.Open();
        }
    }
}
