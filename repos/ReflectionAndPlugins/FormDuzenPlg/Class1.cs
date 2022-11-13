using IPlugin;
namespace FormDuzenPlg
{
    public class FormDuzenPlugin : IPLUGIN
    {
        public ClassProp PROP { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        Thread thr;
        Random rand= new Random();
        public void Run()
        {
            thr = new Thread(new ThreadStart(rainbowBackground));
            thr.Start();
        }
        public FormDuzenPlugin()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            Name = "FormDuzenPlugin";
            Desc = "Form rengi vs değiştirir";
            PROP = new ClassProp();
        }
        void rainbowBackground()
        {
            Color[] renkler = { Color.Gray, Color.Honeydew, Color.Red,Color.Magenta,Color.LightYellow,Color.PowderBlue };
            while (true)
            {
                PROP.winform.BackColor = renkler[rand.Next(0, renkler.Length)];
                PROP.func(PROP.winform.BackColor.Name);
                Thread.Sleep(1000);
            }
        }
    }
}