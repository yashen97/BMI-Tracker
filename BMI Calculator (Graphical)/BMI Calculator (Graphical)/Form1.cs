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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        //Converting the weight to Kg
        private double ConvertTokg(double weight)
        {
            if(radioButton4.Checked==true)
            {
                weight = Convert.ToDouble(textBox3.Text);
            }
            else if(radioButton5.Checked==true)
            {
                weight = Convert.ToDouble(textBox3.Text);
                weight *= 0.4535924;
            }
            return weight;
        }

        //Converting the height to cm
        private double ConvertTocm(double s1, double s2)
        {
            
           if(radioButton1.Checked == true)
            {
                int ft = 0;
                    
                if(s2>=12)
                {
                    while(s2>=12)
                    {
                        ft++;
                        s2 -= 12;
                    }

                    s1 += ft;
                }

                //inch to cm
                double cm1 = s2 * 2.54;
                double cm2 = s1 * 30.48;
                return cm1 + cm2;
            }

           if(radioButton2.Checked == true)
            {
                return s1;
            }
           else
            {
                return s1 * 100;
            }
        }

        //Controls of GroupBox(height)
        private void height()
        {
            if (radioButton1.Checked == true)
            {
                textBox2.Show(); label2.Show(); label1.Text = "Feet";
                textBox1.Clear(); textBox2.Clear();
            }
            else if (radioButton2.Checked == true)
            {
                textBox2.Hide(); label2.Hide(); label1.Text = "cm";
                textBox1.Clear(); textBox2.Clear();
            }
            else if (radioButton3.Checked == true)
            {
                textBox2.Hide(); label2.Hide(); label1.Text = "Meters";
                textBox1.Clear(); textBox2.Clear();
            }
        }

        //Controls of GroupBox(weight)
        private void weight()
        {
            if (radioButton4.Checked == true)
            {
                label3.Text = "Kg";
                textBox3.Clear();
            }
            else if (radioButton5.Checked == true)
            {
                label3.Text = "lbs";
                textBox3.Clear();
            }
        }

        //Enable to type only Digits and one Dot
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar !='.'))
            {
                e.Handled = true;
            }
            if((e.KeyChar =='.') && ((sender as TextBox).Text.IndexOf('.') > -1 ))
            {
                e.Handled = true;
            }
        }

        //Enable to type only Digits
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //Enable to type only Digits and one Dot
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar !='.'))
            {
                e.Handled = true;
            }

            if((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        //BMI calculate
        private double bmical(double h, double w)
        {
            return w / (Math.Pow(h,2));
            
        }

        //BMI Statement
        private string state(double bmi)
        {
            if(bmi < 15)
            {
                label5.ForeColor = Color.Purple;
                label5.BackColor = Color.Red;
                return "Very Severely Underweight";
            }
            else if(bmi < 16)
            {
                label5.ForeColor = Color.Blue;
                label5.BackColor = Color.OrangeRed;
                return "Severely Underweight";
            }
            else if(bmi < 18.5)
            {   
                label5.ForeColor = Color.LightCyan;
                return "Under Weight";
            }
            else if(bmi <= 24.9)
            {
                label5.ForeColor = Color.Green;
                return "Normal Weight";
            }
            else if(bmi <= 29.9)
            {
                label5.ForeColor = Color.Yellow;
                return "Over Weight";
            }
            else if(bmi <= 34.9)
            {
                label5.ForeColor = Color.Orange;
                return "Obesity (Class 1)";
            }
            else if (bmi <= 39.9)
            {
                label5.BackColor = Color.White;
                label5.ForeColor = Color.OrangeRed;
                return "Obesity (Class 2)";
            }
            else
            {
                label5.ForeColor = Color.Red;
                label5.BackColor = Color.Yellow;
                return "Obesity (Class 3)";
            }

        }
        //Calculate Button Click
        private void button1_Click(object sender, EventArgs e)
        {   
            if(textBox1.Text=="" || textBox3.Text=="")
            {
                MessageBox.Show("Please Fill the Required Fields");
            }
            else if(radioButton1.Checked==true && textBox2.Text=="")
            {
                MessageBox.Show("Inch value can't be kept empty");
            }
            else
            {
                //Calculate weight to Kg
                double weight = Convert.ToDouble(textBox3.Text);
                double kg = ConvertTokg(weight);


                //Calculate height to m
                double height1 = Convert.ToDouble(textBox1.Text);
                double height2;

                if (textBox2.Text == "")
                {
                    height2 = 0;
                }
                else
                {
                    height2 = Convert.ToDouble(textBox2.Text);
                }
                double m = ConvertTocm(height1, height2) / 100;

                //Generate BMI Value
                double bmi = bmical(m, kg);
                bmi = Math.Round(bmi, 3);

                
                groupBox3.Show();
                label4.Text = bmi.ToString();
                label5.Text = state(bmi);

                button2.Show();

                int value = RandomNumber();

                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Database\BMI.mdf;Integrated Security=True;Connect Timeout=30"); //sql connection adapter
                con.Open();
                string qry = "INSERT INTO track values ('"+value+"','"+textBox4.Text.Trim()+"','"+textBox5.Text+"','"+m+"','"+kg+"','"+bmi+"','"+label5.Text.Trim()+"')"; //connection string
                try
                {
                    SqlCommand cmd = new SqlCommand(qry, con);
                    cmd.ExecuteNonQuery();

                }
                catch(Exception es)
                {
                    MessageBox.Show(es.Message);
                }
                finally
                {
                    con.Close();
                }

                
                this.chart1.Series["BMI"].Points.AddXY(textBox4.Text,bmi);
                chart1.Show();
            }

        }

        //Radio Button Click Actions
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            height();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            height();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            height();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            weight();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            weight();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); groupBox3.Hide();
            button1.Show();
            radioButton1.Checked = true; radioButton4.Checked = true;
            textBox4.Clear();textBox5.Clear();
            chart1.Hide();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           
        }

        public int RandomNumber()
        {
            Random random = new Random();
            return random.Next(0, 10000);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            history obj = new history();
            this.Hide();
            obj.Show();
        }
    }
}
