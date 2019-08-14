using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BMI_Calculator__Graphical_
{
    public partial class history : Form
    {
        public history()
        {
            InitializeComponent();
        }

        private void history_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Database\BMI.mdf;Integrated Security=True;Connect Timeout=30");
            string qry = "Select Name,Age,Height,Weight,BMI,State from track ";
            SqlDataAdapter adp = new SqlDataAdapter(qry,con);
            DataSet set = new DataSet();
            adp.Fill(set, "track");
            dataGridView1.DataSource = set.Tables["track"];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 obj = new Form1();
            this.Hide();
            obj.Show();
        }
    }
}
