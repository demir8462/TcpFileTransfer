using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.IO.Ports;
namespace YERISTASYON
{
    public partial class Form1 : Form
    {
        public static TextBox console;
        public PortReader pr;
        float[] xz = {0,0 };
        float angle = 90;
        
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            console = textBox1;
            ModifyProgressBarColor.SetState(progressBar1,2);
            loadComs();
        }
        public static void writeConsole(string a)
        {
            console.AppendText(a);
            console.AppendText(Environment.NewLine);
        }
        
        public void loadComs()
        {
            listBox1.Items.Clear(); 
            foreach (string item in PortReader.getComs())
            {
                listBox1.Items.Add(item);
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox5.Image = RotateImage(pictureBox5.Image, 1);
            

        }
        private Bitmap RotateImage(Image img,float angle)
        {
            
            Bitmap rbmp = new Bitmap(img.Width, img.Height);
            rbmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            Graphics g = Graphics.FromImage(rbmp);
            g.TranslateTransform(rbmp.Width/2, rbmp.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-rbmp.Width / 2, -rbmp.Height / 2);
            g.DrawImage(img, new PointF(0, 0));
            return rbmp;
        }
        public void setrotation(float degree)
        {
            pictureBox5.Image= RotateImage(pictureBox5.Image, angle-degree);
            if (degree > angle)
            {
                angle += Math.Abs(angle - degree);
            }else
            {
                angle -= Math.Abs(angle - degree);
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItems.Count != 1)
                    return;
                pr = new PortReader(listBox1.SelectedItem.ToString(), 9600, 8,Parity.None, StopBits.One);
                pr.openPort();
            }catch(Exception ex)
            {
                writeConsole("BAGLANTIDA SORUN OLUSTU : " + ex.Message.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            loadComs();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double a = (180/Math.PI)*Math.Abs(Math.Atan(float.Parse(textBox6.Text) / float.Parse(textBox5.Text)));
            label41.Text = ((180 / Math.PI) * Math.Atan(float.Parse(textBox6.Text) / float.Parse(textBox5.Text))).ToString();
            if (float.Parse(textBox6.Text) > 0)
            {
                // Z COMPONENTI POZITIF YONDE
                if(float.Parse(textBox5.Text) > 0)
                {
                    // +X YONUNDE BILESEN
                    setrotation((float)a);
                }else
                {
                    // -X YONUNDE
                    setrotation((float)(180-a));
                }
            }else
            { // -Z YONUNDE
                if (float.Parse(textBox5.Text) > 0)
                {
                    // +X YONUNDE BILESEN
                    setrotation((float)(360 - a));

                }
                else
                {
                    // -X YONUNDE
                    setrotation((float)(180 + a));
                }
            }
            

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }
}