namespace TXTSCREEN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = "#ALÝDEMÝR#";
            MessageBox.Show(s.Substring(1,s.Length-2));
        }
    }
}