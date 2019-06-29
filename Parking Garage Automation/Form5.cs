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
    public partial class Form5 : Form
    {
        OracleConnection conn = new OracleConnection("Data source = orcl1; user id = mario; password = titans;");
        string userID;
        public Form5(string user)
        {
            InitializeComponent();
            userID = user;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            conn.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // get spot id and cost
            OracleCommand info = new OracleCommand();
            info.Connection = conn;
            info.Parameters.Add("userIDii", userID);

            info.CommandText = "select place.costph, place.place_id, reservation.sdate from reservation , place  where reservation.place_id = place.place_id and reservation.user_id = :userIDii";
            OracleDataReader rd = info.ExecuteReader();
            string sdate ="none"; string place_id = "none";
            float costph = 0;
            while(rd.Read()) { costph = Convert.ToSingle(rd[0]);place_id = rd[1].ToString(); sdate = rd[2].ToString(); };

            // calculate cost 
            DateTime st = Convert.ToDateTime(sdate);
            DateTime nw = DateTime.Now;
            System.TimeSpan diff = nw - st;
            float time = Convert.ToSingle(diff.TotalHours.ToString()) ;
            if (time < 1) time = 1;
            float total = time * costph;
            MessageBox.Show("You'll be charged " + total + " EGPs\n thank you for using ERKENLY!");

            // delete reservation, update user and place statuses
            OracleCommand dlt = new OracleCommand();
            dlt.Connection = conn;
            dlt.CommandText = " delete from reservation where user_id = :sameID ";
            dlt.Parameters.Add("sameID", userID);
            dlt.ExecuteNonQuery();

            OracleCommand stu = new OracleCommand();
            stu.Connection = conn;
            stu.Parameters.Add("id", userID);
            stu.CommandText = "update system_user  set status='f' where user_id = :id";
            stu.ExecuteNonQuery();

            OracleCommand stp = new OracleCommand();
            stp.Connection = conn;
            stp.Parameters.Add("plidii", place_id);
            stp.CommandText = "update place set STATUS ='f' where place_id = :plidii";
            stp.ExecuteNonQuery();
            // close connection
            conn.Dispose();
        }
    }
}
