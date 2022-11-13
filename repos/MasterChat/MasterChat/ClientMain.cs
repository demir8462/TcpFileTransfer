using System.Reflection;
namespace MasterChat
{
    public partial class ClientMain : Form
    {
        Sohbet sohbetform;
        public ClientMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Client.loadPlugins();
            findPluginsPreConnect();
        }
        void findPluginsPreConnect()
        {
            listView1.Items.Clear();
            foreach ( MCPlugin.MCPLUGIN plugin in Client.plugins)
            {
                string[] LVI = { plugin.Name,plugin.Essential.ToString()};
                listView1.Items.Add(new ListViewItem(LVI));
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Client.connect(textBox1.Text, int.Parse(textBox2.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Client.createRoom(textBox5.Text,textBox3.Text);
            Client.odajoinid = Client.Gpaket.JOINID;
            Client.nick = textBox3.Text;
            sohbetform = new Sohbet();
            sohbetform.Show();
            Task.Run(() =>
            {
                Client.MesajAl();
            });
        }

        private void ClientMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Client.joinRoom(textBox4.Text,textBox5.Text,textBox3.Text))
                return;
            Client.odajoinid = textBox4.Text;
            Client.nick = textBox3.Text;
            sohbetform = new Sohbet();
            sohbetform.Show();
            Task.Run(() =>
            {
                Client.MesajAl();
            });
        }
    }
}