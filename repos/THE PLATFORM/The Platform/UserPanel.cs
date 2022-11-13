using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace The_Platform
{
    public partial class UserPanel : Form
    {
        public static Server server;
        public static Client client;
        public static String username;
        public static bool isserver = false;
        public UserPanel()
        {
            InitializeComponent();
        }
        private void check_files()
        {
            // SISTEM ONLINE CALIŞTIĞINDA BU KONTROL SİSTEMİ ONLİNE RESMİ KAYDEDİCEK !
            if (!Directory.Exists(Program.DIRECTORY + "\\user"))
            {
                Directory.CreateDirectory(Program.DIRECTORY + "\\user");
                Directory.CreateDirectory(Program.DIRECTORY + "\\pic");
                Directory.CreateDirectory(Program.DIRECTORY + "\\user\\pic");
            }
            else if (!Directory.Exists(Program.DIRECTORY + "\\user\\pic"))
            {
                Directory.CreateDirectory(Program.DIRECTORY + "user\\pic");
            }
        }
        private void setpp(string pp)
        {
            if (pp == "B_PIC")
                pictureBox1.Image = Image.FromFile(Program.DIRECTORY + "\\user\\pic\\b_pic.png");
            else if (pp == "G_PIC")
                pictureBox1.Image = Image.FromFile(Program.DIRECTORY + "\\user\\pic\\b_pic.png");
            else
                pictureBox1.Image = Image.FromFile(pp);
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            Program.db_kisi_cek(Prompt.ShowPrompt("USERNAME :", "Profile View"));
            Program.showp = new ShowProfile();
            Program.showp.Show();
        }

        private void UserPanel_Load(object sender, EventArgs e)
        {
            check_files();
            label1.Font = new Font("HACKED", 18);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            start_profile();
        }
        public void start_profile()
        {
            username = Program.tablo.Rows[0]["username"].ToString();
            label_name.Text = username;
            label_registered.Text = Program.tablo.Rows[0]["REGISTERED"].ToString();
            setpp(Program.tablo.Rows[0]["PICTURE"].ToString());
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                Program.mysql_komut_islet("UPDATE users SET LONLINE='NOW' where USERNAME='" + username + "';");
            else
                Program.mysql_komut_islet("UPDATE users SET LONLINE='" + DateTime.Now + "' where USERNAME='" + username + "';");
        }

        private void UserPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.mysql_komut_islet("UPDATE users SET LONLINE='" + DateTime.Now + "' where USERNAME='" + username + "';");
            Application.Exit();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            isserver = true;
            server = new Server(Int32.Parse(Prompt.ShowPrompt("Port:","SERVER")));
            Program.chat = new Chat();
            Program.chat.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            isserver = false;
            /*DO SOMETHING TO CONNECT */
            /* OPEN THE CHAT ROOM */
            client = new Client(Prompt.ShowPrompt("Ip:", "CLIENT"), Int32.Parse(Prompt.ShowPrompt("Port:", "CLIENT")));
            Program.chat = new Chat();
            Program.chat.Show();
        }

  
    }
}
