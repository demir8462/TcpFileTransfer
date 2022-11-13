using System.IO;
using System.Security.Cryptography;

namespace TCPFileTransfer
{
    public partial class Form1 : Form
    {
        private static TextBox tx;
        Connector connector = new Connector();
        FileStream filestream;
        public static string SAVE_PATH;
        public static Label bytesRecivedLabel,recivingFileSizeLabel;
        public long SENDFILESIZE;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            tx = textBox4;
            bytesRecivedLabel = label11;
            recivingFileSizeLabel = label13;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public void updateBytesSend(long sent)
        {
            label4.Text = sent.ToString() + "BYTES - "+ ((float)(sent/1024/1024))+ "MB \t "+(int)(sent / SENDFILESIZE * 100)+"+%";
            setProgressBar((int)(sent / SENDFILESIZE * 100));
        }
        public static void updateRecivedBytes(long recived)
        {
            bytesRecivedLabel.Text = recived.ToString() + "BYTES - " + ((float)(recived / 1024 / 1024)) + "MB \t " + (int)(recived / Connector.RECIEVE_FILE_SIZE * 100) + "+%";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                connector.baglantiAl(int.Parse(textBox3.Text));
            });
        }
        public static void writeConsole(string text)
        {
            tx.AppendText(text);
            tx.AppendText(Environment.NewLine);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void setProgressBar(int value)
        {
            if (progressBar1.Value == value)
                return;
            else
                progressBar1.Value = value;
        }
        
        
        private void button4_Click(object sender, EventArgs e)
        {
            writeConsole("Baðlantý Saðlanýyor ["+textBox2.Text+"]:"+textBox3.Text);
            if(connector.baglan(textBox2.Text,int.Parse(textBox3.Text)))
            {
                writeConsole("BAGLANTI KURULDU !");
            }else
            {
                writeConsole("BAGLANTI KURULAMADI !");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            filestream = File.Open(textBox1.Text, FileMode.Open);
            SENDFILESIZE = new FileInfo(filestream.Name).Length;
            if (connector.dosyaYollaIstek(filestream.Name,SENDFILESIZE))
            {
                MessageBox.Show("Istek kabul edildi !");
                Task.Run(() =>
                {
                    sendTheFile();
                });
            }else
            {
                MessageBox.Show("Istek red edildi !");
            }
        }
        public void sendTheFile()
        {
            
            byte[] buffer = new byte[Connector.BUFFERSIZE];
            int bytesread = 0;
            long totalBytesSend=0;
            while ((bytesread = filestream.Read(buffer, 0, Connector.BUFFERSIZE)) > 0)
            {
                totalBytesSend += bytesread;
                writeConsole("BYTESREAD FROM DISC : " + bytesread);
                connector.writeBytes(buffer);
                updateBytesSend(totalBytesSend);
                if (bytesread % Connector.BUFFERSIZE != 0)
                    break;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            connector.dosyaBekle();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            textBox5.Text = saveFileDialog1.FileName;
            if (!File.Exists(saveFileDialog1.FileName))
                File.Create(saveFileDialog1.FileName).Close();
            SAVE_PATH = textBox5.Text;
        }
    }
}