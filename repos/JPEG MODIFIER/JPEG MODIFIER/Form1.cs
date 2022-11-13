using ExifLib;
namespace JPEG_MODIFIER
{
    public partial class Form1 : Form
    {
        public string photopath,tarih,marka,model;
        DateTime dt;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            photopath = openFileDialog1.FileName;
            textBox1.Text = photopath;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            pictureBox1.ImageLocation = photopath;
            pictureBox1.Load();

            ExifReader ER = new ExifReader(photopath);
            if(ER.GetTagValue<String>(ExifTags.Make,out marka))
            {
                label7.Text = marka;
            }
            if (ER.GetTagValue<String>(ExifTags.Model, out model))
            {
                label10.Text = model;
            }
            if (ER.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out dt))
            {
                label8.Text = dt.ToString();
            }
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}