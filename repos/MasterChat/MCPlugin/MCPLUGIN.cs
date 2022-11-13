using System.Drawing;

namespace MCPlugin
{
    public interface MCPLUGIN
    {
        // THESE PROPERTIE'S VALUES HAVE TO BE DEFINED ON OVERRIDE
        public string Name { get;}
        public string Version { get;}
        public string Desc { get;}
        public bool Essential { get;}
        public EventManager manager { get;}
        public void Run();

    }
    public class EventManager
    {
        public enum EVENTTYPE { PAKETAL, PAKETYOLLA ,MESAJAL,MESAJYOLLA}
        public Dictionary<EVENTTYPE,EventHandler> events = new Dictionary<EVENTTYPE,EventHandler>();   
        public bool regEvent(EventHandler e,EVENTTYPE tip)
        {
            events.Add(tip, e);
            return true;
        }
        
    }
    public class EventInfo : EventArgs
    {
        public string mesaj;
        public int ARGB;
        public bool cancelevent;
    }
}