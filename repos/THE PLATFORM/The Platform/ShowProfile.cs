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
    public partial class ShowProfile : Form
    {
        public ShowProfile()
        {
            InitializeComponent();
        }

        private void ShowProfile_Load(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(130, 92, 92);
            label1.Text = Program.tablo.Rows[0]["USERNAME"].ToString();
            REGISTERED.Text = Program.tablo.Rows[0]["REGISTERED"].ToString();
            LONLINE.Text = Program.tablo.Rows[0]["LONLINE"].ToString();
            pictureBox1.Image = Image.FromFile(Program.DIRECTORY+"\\user\\pic\\"+ Program.tablo.Rows[0]["PICTURE"].ToString()+".png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
