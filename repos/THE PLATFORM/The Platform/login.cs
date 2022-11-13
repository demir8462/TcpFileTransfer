using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_Platform;
namespace The_Platform
{
    public partial class login : Form
    {
        public static string username,password;
        public static Form reg;
        public string getusername()
        {
            return username;
        }
        public string getpassword()
        {
            return password;
        }
        
        
        public login()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            username = textBox1.Text;
            password = textBox2.Text;
            try
            {
                MessageBox.Show("Logged In :"+Program.db_kisi_cek(username, password).Rows[0]["username"].ToString()+"SUCSESSFULLY !");
                Program.Panel = new UserPanel();
                Program.Panel.Show();
                this.Hide();
            }
            catch(Exception ee)
            {
                MessageBox.Show("Username or password is wrong !");
            }
            
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            reg = new Register();
            reg.Show();
        }
        
        private void Login_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            label1.Font = new Font("HACKED",15);
        }
    }
}
