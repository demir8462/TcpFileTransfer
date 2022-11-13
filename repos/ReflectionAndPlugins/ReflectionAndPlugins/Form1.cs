using System.Reflection;
using IPlugin;
namespace ReflectionAndPlugins
{

    public partial class Form1 : Form
    {
        List<IPLUGIN> PLUGINS = new List<IPLUGIN>();
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string dll in Directory.GetFiles("plugins", "*.dll"))
            {
                addToConsole(dll);
                Assembly asm = Assembly.LoadFrom(dll);
                foreach (Type item in asm.GetTypes())
                {
                    if (item.GetInterface("IPLUGIN") != null)
                    {
                        IPLUGIN PLUGIN = Activator.CreateInstance(item) as IPLUGIN;
                        PLUGIN.PROP.winform = this;
                        PLUGIN.PROP.func = addToConsole;
                        addToConsole("[LOAD]" + PLUGIN.Name + " :" + PLUGIN.Desc);
                        PLUGINS.Add(PLUGIN);
                    }
                }
            }
            LoadPlugins();


        }
        void addToConsole(string txt)
        {
            textBox1.AppendText(txt);
            textBox1.AppendText(Environment.NewLine);
        }
        void LoadPlugins()
        {
            foreach (IPLUGIN item in PLUGINS)
            {
                addToConsole("[START]" + item.Name);
                item.Run();
            }
        }
    }

}
   

