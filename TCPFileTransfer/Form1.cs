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
        public static ProgressBar recProgressBar;
        public int LAST_PROGRESSBAR_VALUE;
        public long SENDFILESIZE,totalBytesSend;
        bool TRANSFERCONTINUES;
        Thread speedCalculaterTHR;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            tx = textBox4;
            bytesRecivedLabel = label11;
            recivingFileSizeLabel = label13;
            recProgressBar = progressBar2;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        void calculateSpeed()
        {
            long lastcount = totalBytesSend;
            while(TRANSFERCONTINUES)
            {
                float MB = (totalBytesSend-lastcount)/1024.0f/1024.0f;
                lastcount = totalBytesSend;
                label16.Text = MB.ToString();
                Thread.Sleep(1000);
            }
        }
        public void updateBytesSend(long sent)
        {
           
            if(LAST_PROGRESSBAR_VALUE != SENDFILESIZE)
            {
                LAST_PROGRESSBAR_VALUE = (int)((float)(sent) / (float)(SENDFILESIZE) * 100);
                
            }
            label4.Text = sent.ToString() + "BYTES - " + ((float)(sent / 1024 / 1024)) + "MB \t " + LAST_PROGRESSBAR_VALUE + "%";
            progressBar1.Value = LAST_PROGRESSBAR_VALUE;
            
        }
        public static void updateRecivedBytes(long recived)
        {
            bytesRecivedLabel.Text = recived.ToString() + "BYTES - " + ((float)(recived / 1024 / 1024)) + "MB \t " + (int)((float)recived / Connector.RECIEVE_FILE_SIZE * 100) + "% - "+Connector.BUFFERSIZE/1024/1024;
            recProgressBar.Value = (int)((float)recived / Connector.RECIEVE_FILE_SIZE * 100);
          
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
            label15.Text = getMd5OfFile(openFileDialog1.FileName);
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
        public static string getMd5OfFile(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    string a = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                    stream.Close();
                    return a;
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            filestream = File.Open(textBox1.Text, FileMode.Open);
            SENDFILESIZE = new FileInfo(filestream.Name).Length;
            button2.Enabled = false;
            pictureBox1.Visible = true;
            if (connector.dosyaYollaIstek(filestream.Name,SENDFILESIZE,label15.Text,textBox6.Text))
            {
                MessageBox.Show("Istek kabul edildi !");
                Task.Run(() =>
                {
                    sendTheFile();
                });
            }else
            {
                MessageBox.Show("Istek red edildi !");
                pictureBox2.Visible = true;
            }
        }
        public void sendTheFile()
        {
            
            byte[] buffer = new byte[Connector.BUFFERSIZE];
            int bytesread = 0;
            totalBytesSend=0;
            speedCalculaterTHR = new Thread(new ThreadStart(calculateSpeed));
            speedCalculaterTHR.Start();
            TRANSFERCONTINUES = true;
            while ((bytesread = filestream.Read(buffer, 0, Connector.BUFFERSIZE)) > 0)
            {
                totalBytesSend += bytesread;
                writeConsole("BYTESREAD FROM DISC : " + bytesread+ "BYTES LEFT : "+(SENDFILESIZE-totalBytesSend).ToString());
                try
                {
                    connector.writeBytes(buffer);
                    
                }catch(Exception e)
                {
                    writeConsole(e.Message);
                    TRANSFERCONTINUES = false;
                    break;
                }
                updateBytesSend(totalBytesSend);
                if (bytesread % Connector.BUFFERSIZE != 0)
                    break;
            }
            TRANSFERCONTINUES = false;
            filestream.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Connector.PASS = textBox6.Text;
                connector.dosyaBekle();
            });
            button7.Enabled = false;
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