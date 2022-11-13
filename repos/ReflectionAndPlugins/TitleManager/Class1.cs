using IPlugin;
namespace TitleManager
{
    public class TitleManager : IPLUGIN
    {
        public ClassProp PROP { get; set; } 
        public string Name { get; set; }
        public string Desc { get; set; }
        Thread thr;
        public TitleManager()
        {
            Name = "TitleManager";
            Desc = "Sets random strings as Form title";
            PROP = new ClassProp();
        }
        public void Run()
        {
            thr = new Thread(new ThreadStart(setTitle));
            thr.Start();
        }
        void setTitle()
        {
            Random rand = new Random();
            string[] words = { "#@", "bRoKe", "glAsS", "Fk!", "$$%!!" };
            while (true)
            {
                PROP.setTitle(words[rand.Next(0,5)]+ words[rand.Next(0, 5)]);
                Thread.Sleep(1000);
            }
        }
    }
}