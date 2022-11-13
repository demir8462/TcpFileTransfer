using System.Net.Sockets;

namespace TYLER_DURDEN
{
    public partial class Form1 : Form
    {
        string filepath;
        string IP;
        short PORT;
        TcpListener listener;
        TcpClient client;
        NetworkStream stream;
        Socket socket;
        StreamWriter writer;
        StreamReader reader;
        byte[] buffer;
                
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // SAVE FÝLE
        {
            saveFileDialog1.ShowDialog();
            filepath = saveFileDialog1.FileName;
            textBox1.Text = filepath;
        }

        private void button2_Click(object sender, EventArgs e) // SELECT FÝLE
        {
            openFileDialog1.ShowDialog();
            filepath = openFileDialog1.FileName;
            textBox1.Text = filepath;
        }

        private void button3_Click(object sender, EventArgs e) // SENT File
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            IP = textBox2.Text;
            PORT = short.Parse(textBox3.Text);
            sunucuKur();
        }
     

        private void button5_Click(object sender, EventArgs e)
        {
            IP = textBox2.Text;
            PORT = short.Parse(textBox3.Text);
            sunucuyaBaglan();
        }
        void sunucuyaBaglan()
        {
            client = new TcpClient(IP,PORT);
            if(client != null && client.Connected)
            {
                MessageBox.Show("BAÐLANDI");
            }
        }
        void sunucuKur()
        {
            listener = new TcpListener(PORT);
            listener.Start();
            socket = listener.AcceptSocket();
            stream = new NetworkStream(socket);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            MessageBox.Show("BAÐLANDI !");
        }
    }
}