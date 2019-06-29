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
    public partial class Form1 : Form
    {
        OracleConnection conn = new OracleConnection("Data source = orcl1; user id = mario; password = titans;");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show(DateTime.Now.ToString());
            conn.Open();
            OracleCommand com = new OracleCommand();
            com.Connection = conn;
            //com.CommandText = "insert into system_user values('0001','mario','titans', 'f                     ')";
            //com.ExecuteNonQuery();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

       
      

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string userid = nameTB.Text.ToString();
            string password = passwordTB.Text.ToString();

            if (userid == "" || password == "") notice.Text = "Please enter both user ID and password";
            else
            {
                OracleCommand login = new OracleCommand();
                login.Connection = conn;
                login.Parameters.Add("id", userid);
                login.Parameters.Add("pass", password);
                login.CommandText = "select count(user_id) from system_user where user_id= :id and pass_word= :pass";
                OracleDataReader rd = login.ExecuteReader();
                string count = "0";
                while (rd.Read()) count = rd[0].ToString();
                if (count == "0")  notice.Text = "incorrect username or password";
                else if (count == "1")
                {
                    OracleCommand logged = new OracleCommand();
                    logged.Connection = conn;
                    logged.Parameters.Add("same", userid);
                    logged.CommandText = "select USERNAME,STATUS from system_user where user_id= :same";
                    OracleDataReader rdr = logged.ExecuteReader();
                    string nm = "none";
                    string status = "none";
                    Form2 form2;
                    while (rdr.Read())
                    {
                        nm = rdr[0].ToString();
                        status = rdr[1].ToString();
                    }
                    form2 = new Form2(userid, nm,status);
                    form2.Show();
                    rdr.Close();
                    //form2.Close();
                    //conn.Dispose();
                    //form2.Close();
                }
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //conn.Dispose();
            Form3 f = new Form3();
            f.Show();
            //f.Close();
        }
    }
}
