using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace SERILESTIRMESERVER
{
   
    public partial class Form1 : Form
    {
        TcpClient tcpClient;
        TcpListener tcpListener;
        NetworkStream stream;
        Socket socket;
        StreamWriter writer;
        StreamReader reader;
        BinaryFormatter bf = new BinaryFormatter();
        Insan insan;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            tcpListener = new TcpListener(int.Parse(textBox5.Text));
            tcpListener.Start();
            socket = tcpListener.AcceptSocket();
            label5.Text = "baglandý !";
            stream = new NetworkStream(socket);
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            tcpClient = new TcpClient(textBox4.Text, int.Parse(textBox5.Text));
            if(tcpClient.Connected)
            {
                stream = tcpClient.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*List<string> list = new List<string>();
            list.Clear();
            list.Add(textBox1.Text);
            list.Add(textBox2.Text);
            list.Add(textBox3.Text);*/
            insan = new Insan();
            insan.Name =textBox1.Text;
            insan.Job = textBox2.Text;
            insan.Age = int.Parse(textBox3.Text);
            bf.Serialize(stream, insan);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*List<string> list = (List<string>)bf.Deserialize(stream);
            textBox1.Text = list[0];
            textBox2.Text = list[1];
            textBox3.Text = list[2];*/
            insan = (Insan)bf.Deserialize(stream);
            textBox1.Text = insan.Name;
            textBox2.Text = insan.Job;
            textBox3.Text = insan.Age.ToString();
        }
    }
}