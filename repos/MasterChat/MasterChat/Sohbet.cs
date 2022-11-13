using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterChat
{
    public partial class Sohbet : Form
    {
        public static Color ownTextColor = Color.Black;
        public static RichTextBox mesajTxt;
        public static bool AllowColorfulTexts = false;
        public static string[] katilimcilar = { };
        public Thread katilimcithr;
        public Sohbet()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            mesajTxt = richTextBox1;
            katilimcithr = new Thread(new ThreadStart(katilimciGuncelle));
            katilimcithr.Start();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 10)
            {
                MessageBox.Show("Mesajınız en az 10 karaekter olmalıdır !", "SPAM ATIYOR OLABİLİR MİSİN SALİH ?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Client.MesajYolla(Client.nick+":"+textBox1.Text);
            mesajEkle("YOU:" +textBox1.Text,ownTextColor);
            textBox1.Text = "";
            /*Client.getKatilimcilar();
            listView1.Items.Clear();
            foreach (string item in Client.PARTICIPANTS)
            {
                listView1.Items.Add(new ListViewItem(item));
            }*/
        }
        public static void mesajEkle(string txt,Color c)
        {
            mesajTxt.SelectionColor = c;
            mesajTxt.AppendText(txt);
            mesajTxt.AppendText(Environment.NewLine);
        }

        private void Sohbet_Load(object sender, EventArgs e)
        {
            this.Text = Client.nick;
            listView2.Items.Clear();
            foreach (MCPlugin.MCPLUGIN item in Client.plugins)
            {
                string[] lvi = { item.Name};
                listView2.Items.Add(new ListViewItem(lvi));
            }
        }
        public static void ScrollGuncelle()
        {
            mesajTxt.SelectionStart = mesajTxt.Text.Length;
            mesajTxt.ScrollToCaret();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            colorDialog1.ShowDialog();
            ownTextColor = colorDialog1.Color;
            panel1.BackColor = ownTextColor;
            textBox1.ForeColor = ownTextColor;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            AllowColorfulTexts = checkBox1.Checked;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true;
               
                return;
            }
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(this, e);
            }
            
        }
        public static void BEEPSOUND()
        {
            SystemSounds.Hand.Play();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Client.sendBeep();
        }
        public void katilimciGuncelle()
        {
            while (Client.odabagli && Client.connected)
            {
                Client.getKatilimcilar();
                listView1.Items.Clear();
                foreach (string item in katilimcilar)
                {
                    listView1.Items.Add(new ListViewItem(item));
                }
                Thread.Sleep(1000);
            }
        }
    }
}
