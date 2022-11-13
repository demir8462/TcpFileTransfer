using System.Windows.Forms;
namespace IPlugin
{
    public interface IPLUGIN
    {
        
        public ClassProp PROP { get; set; }
        string Name { get; set; }
        string Desc { get; set; }
        void Run();
    }
    public class ClassProp
    {
        public Form winform;
        public delegate void writeConsole(string text);
        public writeConsole func;
        Color getBackColor()
        {
            return winform.BackColor;
        }
        public void setBackColor(Color color)
        {
            winform.BackColor = color; 
        }
        public string getTitle()
        {
            return winform.Text;
        }
        public void setTitle(string text)
        {
            winform.Text = text;
        }
    }
}