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
    public partial class Form4 : Form
    {
        string spotid, id;
        OracleConnection conn = new OracleConnection("Data source = orcl1; user id = mario; password = titans;");
        
        public Form4(string spotidi,string idi)
        {
            InitializeComponent();
            spotid = spotidi; id = idi;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            conn.Open();

            radioButton2.Checked = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = false;
      
            label1.Text = "Phone number: ";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            label1.Text = "Credit Number: ";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!="" && textBox2.Text!="")
            {
                if (radioButton2.Checked == true)
                {
                    string cnum = textBox1.Text.ToString();
                    string pin = textBox2.Text.ToString();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.Parameters.Add("cnumi", cnum);
                    cmd.Parameters.Add("pini", pin);
                    cmd.CommandText = "select count(cnum) from credit_info where cnum = :cnumi and pin = :pini";
                    OracleDataReader rd = cmd.ExecuteReader();
                    string cnt = "0";
                    while (rd.Read())
                    {
                        cnt = rd[0].ToString();
                    }
                    rd.Close();
                    //MessageBox.Show(cnt);
                    if (cnt == "1")
                    {
                        MessageBox.Show("Your place has been reserved");
                        MessageBox.Show("Your Parking spot's ID is " + spotid);
                        // update status and insert in reservation table
                        OracleCommand c = new OracleCommand();
                        c.Connection = conn;
                        c.Parameters.Add("id", id);
                        c.CommandText = "update system_user  set status='r' where user_id = :id";
                        c.ExecuteNonQuery();


                        OracleCommand cvr = new OracleCommand();
                        cvr.Connection = conn;
                        cvr.Parameters.Add("plidi", spotid);
                        cvr.CommandText = "update place set STATUS ='r' where place_id = :plidi";
                        cvr.ExecuteNonQuery();

                        OracleCommand cr = new OracleCommand();
                        cr.Connection = conn;
                        cr.Parameters.Add("uidii", id);
                        cr.Parameters.Add("sidii", spotid);
                        string n = DateTime.Now.ToString();
                        cr.Parameters.Add("sdateii",n);
                      
                        cr.CommandText = "insert into reservation values(:uidii, :sidii, :sdateii)";
                        cr.ExecuteNonQuery();

                    }
    
                }
                else if (radioButton2.Checked==true)
                {

                    OracleCommand c = new OracleCommand();
                    c.Connection = conn;
                    c.Parameters.Add("id", id);
                    c.CommandText = "update system_user  set status='r' where user_id = :id";
                    c.ExecuteNonQuery();


                    OracleCommand cvr = new OracleCommand();
                    cvr.Connection = conn;
                    cvr.Parameters.Add("plidi", spotid);
                    cvr.CommandText = "update place set STATUS ='r' where place_id = :plidi";
                    cvr.ExecuteNonQuery();

                    OracleCommand cr = new OracleCommand();
                    cr.Connection = conn;
                    cr.Parameters.Add("uidii", id);
                    cr.Parameters.Add("sidii", spotid);
                    string n = DateTime.Now.ToString();
                    cr.Parameters.Add("sdateii", n);

                    cr.CommandText = "insert into reservation values(:uidii, :sidii, :sdateii)";
                    cr.ExecuteNonQuery();
                }
            }
            
            conn.Close();
        }
    }
}
