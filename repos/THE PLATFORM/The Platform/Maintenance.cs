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
    public partial class Maintenance : Form
    {
        public Maintenance()
        {
            InitializeComponent();
        }

        private void Maintenance_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(122,45,45);
            label2.Font = new Font("HACKED", 19);
            pictureBox1.Image = Image.FromFile(Program.DIRECTORY+"\\pic\\maintenance.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
