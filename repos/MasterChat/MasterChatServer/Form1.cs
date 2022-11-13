namespace MasterChatServer
{
    public partial class Form1 : Form
    {
        static TextBox console;
        static ListView katilimcilar;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            console = textBox1;
            katilimcilar = listView1;
        }
        public static void writeConsole(string line)
        {
            if(console != null)
            {
                console.AppendText(line);
                console.AppendText(Environment.NewLine);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Server.startListen(2525);
            });
        }
        public static void odaEkle(string id,string joinid,int katilimcisayi)
        {
            string[] LVI = { id, joinid, katilimcisayi.ToString() };
            katilimcilar.Items.Add(new ListViewItem(LVI));
        }
        public static void odaTemizle()
        {
            katilimcilar.Items.Clear();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}