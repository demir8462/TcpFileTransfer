using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Platform
{
    public partial class Register : Form
    {
        String gender = "G_PIC";
        public Register()
        {
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            label1.Font = new Font("HACKED", 15);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0)
            {
                MessageBox.Show("Fill all places !", "WARNING");
            }else
            {
                if (textBox2.Text != textBox3.Text)
                {
                    MessageBox.Show("Passwords aren't same !");
                }else
                {
                    if (checkBox2.Checked)
                        gender = "B_PIC";
                    Program.mysql_komut_islet("INSERT INTO users(USERNAME,PASSW,REGISTERED,PICTURE,LONLINE) VALUES('" + textBox1.Text + "','" + textBox2.Text + "','" + DateTime.Now + "','" + gender + "','never')");
                    MessageBox.Show("Registered Sucsessfully !", "DONE");
                    this.Hide();
                }
                
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox1.Checked;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox2.Checked;
        }
    }
}
