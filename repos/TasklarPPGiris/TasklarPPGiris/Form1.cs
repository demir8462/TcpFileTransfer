namespace TasklarPPGiris
{
    public partial class Form1 : Form
    {
        // TASKLAR ISLERDIR AYRI THREADLAR DEGIL. BIR THREADDA
        // AYNI ANDA BIRDEN FAZLA TASK GERCEKLESITIRLEBILIR ANCAK
        // ZAMANLAMA CALISTIKLARI THREADIN ZAMANLAMASIDIR
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        void consoleyaz(string str)
        {
            textBox4.AppendText(str+Environment.NewLine);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            consoleyaz("TASKLAR AYARLANIYOR..");
            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    consoleyaz("Task1:" + i.ToString());

                }


            });
            
            Task task2 = new Task(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    consoleyaz("Task2:" + i.ToString());

                }
            });
            task2.Start();
            Task.WaitAll(task2);
            for (int i = 0; i < 1000; i++)
            {
                consoleyaz("Mainthr:" + i.ToString());
            }
        }
    }
}