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
    public partial class Form3 : Form
    {
        OracleConnection conn = new OracleConnection("Data source = orcl1; user id = mario; password = titans;");
        bool unique = false;
        bool pass = false;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            conn.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string id = textBox1.Text.ToString();
            string name = nameTB.Text.ToString();
            string ps = passwordTB.Text;
            OracleCommand create = new OracleCommand();
            if (pass == true && unique == true)
            {
                create.Connection = conn;
                create.CommandText = "insert into system_user values(:id ,:name,:ps, 'f')";
                create.Parameters.Add("id", id);
                create.Parameters.Add("name", name);
                create.Parameters.Add("ps", ps);
                create.ExecuteNonQuery();
                MessageBox.Show("account created");
            }
            else MessageBox.Show("Please fill the blanks with valid data");
            conn.Dispose();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string id = textBox1.Text.ToString();
            OracleCommand check = new OracleCommand();
            check.Connection = conn;
            check.Parameters.Add("id", id);
            check.CommandText = "select count(user_id) from system_user where user_id = :id";
            OracleDataReader dr = check.ExecuteReader();
            string cnt = "0";
            while (dr.Read()) cnt = dr[0].ToString();
            if (cnt == "0") { unique = true; label3.Text = "Verified"; }
            else label3.Text = "ID already exists, try choosing a unique name";
            dr.Close();
        }

        private void passwordTB_TextChanged(object sender, EventArgs e)
        {
            string ps = passwordTB.Text;
            if (ps.Length < 6 || ps.Length > 16) label6.Text = "Must be 6-16 characters";
            else { label6.Text = "verified"; label6.ForeColor = Color.DarkCyan; pass = true;  }
        }
    }
}
