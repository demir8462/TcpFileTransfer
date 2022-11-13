using MCLIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MCPlugin;
using System.Reflection;

namespace MasterChat
{
    public class Client
    {
        static TcpClient client = new TcpClient();
        static NetworkStream stream;
        static BinaryFormatter bf = new BinaryFormatter();
        public static IPaket paket = new IPaket(),Gpaket = new IPaket();
        public static bool connected, odabagli;
        public static string odajoinid,nick;
        public static string[] PARTICIPANTS = {};
        public static event EventHandler paketAl,paketYolla,EmesajAl,EmesajYolla;
        public static object evento = new object(); // bos
        public static Dictionary<EventManager.EVENTTYPE, EventHandler> eventler = new Dictionary<EventManager.EVENTTYPE, EventHandler>();
        public static List<MCPLUGIN> plugins = new List<MCPLUGIN>();
        public static MCPlugin.EventInfo paketInfo= new MCPlugin.EventInfo(), GpaketInfo= new MCPlugin.EventInfo();
        static object paketLock = new object();
        public static bool connect(string ip="127.0.0.1",int port=2525)
        {
            try
            {
                client.Connect(ip, port);
                stream = client.GetStream();
                connected = true;
                // BAGLANTI SAGLANIR ISE BOS EVENTLER ATILIYOR
                eventler.Add(EventManager.EVENTTYPE.PAKETAL,paketAl);
                eventler.Add(EventManager.EVENTTYPE.PAKETYOLLA, paketYolla);
                eventler.Add(EventManager.EVENTTYPE.MESAJYOLLA, EmesajYolla);
                eventler.Add(EventManager.EVENTTYPE.MESAJAL, EmesajAl);
                eventler[EventManager.EVENTTYPE.PAKETAL] = EmptyEvent;
                eventler[EventManager.EVENTTYPE.PAKETYOLLA] = EmptyEvent;
                eventler[EventManager.EVENTTYPE.MESAJYOLLA] = EmptyEvent;
                eventler[EventManager.EVENTTYPE.MESAJAL] = EmptyEvent;
                loadEvents();
                return true;
            }catch(Exception e)
            {
                return false;
            }

        }
       
        static void EmptyEvent(object s,EventArgs e)
        {

        }
        public static void loadPlugins()
        {

            foreach (string item in Directory.GetFiles("plugins", "*.dll"))
            {
                Assembly asm = Assembly.LoadFrom(item);
                foreach (Type tip in asm.GetTypes())
                {
                    if(tip.GetInterface("MCPLUGIN") != null)
                    {
                        plugins.Add((MCPLUGIN)Activator.CreateInstance(tip));
                    }
                }
            }
            
            
        }
        public static void loadEvents()
        {
            foreach (MCPLUGIN item in plugins)
            {
                Task.Run(() =>
                {
                    item.Run();
                });
                Task.Run(() =>
                {
                    foreach (var item in item.manager.events)
                    {
                        if (eventler[item.Key] != null)
                        {
                            eventler[item.Key] += item.Value;
                        }
                    }
                });
            }
        }
        public static bool sendPackage(IPaket paket)
        {
            try
            {
                eventler[EventManager.EVENTTYPE.PAKETYOLLA](evento, paketInfo);
                paketInfo.cancelevent = false;
                if(paketInfo.cancelevent)
                {
                    paketInfo.cancelevent = false;
                    return false;
                }
                paket.msj = paketInfo.mesaj;
                paket.nick = nick;
                paket.ARGB = paketInfo.ARGB;
                lock(paketLock)
                {
                    bf.Serialize(stream, paket);
                }
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
        public static bool getPackage()
        {
            try
            {
                Gpaket = (IPaket)bf.Deserialize(stream);
                eventler[EventManager.EVENTTYPE.PAKETAL](evento, GpaketInfo);
                return (Gpaket != null);
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        public static bool createRoom(string pass,string nickk)
        {
            paket.type = IPaket.PAKETTYPE.ODAKUR;
            paket.roompass = pass;
            nick = nickk;
            paket.nick = nickk;
            foreach  (MCPLUGIN plugin in plugins)
            {
                if (!plugin.Essential)
                    continue;
                Plugin p = new Plugin();
                p.name = plugin.Name;
                p.desc = plugin.Desc;
                p.essential = plugin.Essential;
                paket.pluginler.Add(p);
            }
            sendPackage(paket);
            while (connected)
            {
                if(getPackage())
                {
                    if(Gpaket.type == IPaket.PAKETTYPE.CEVAP)
                    {
                        if(Gpaket.cevap)
                        {

                            odabagli = true;
                            return true;
                        }else
                        {
                            // KODLAR
                            MessageBox.Show(Gpaket.detay,"Tüh",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static bool joinRoom(string id,string pass,string nickk)
        {
            paket.type = IPaket.PAKETTYPE.ODABAGLAN;
            paket.JOINID = id;
            paket.roompass = pass;
            nick = nickk;
            paket.nick = nickk;
            foreach (MCPLUGIN plugin in plugins)
            {
                if (!plugin.Essential)
                    continue;
                Plugin p = new Plugin();
                p.name = plugin.Name;
                p.desc = plugin.Desc;
                p.essential = plugin.Essential;
                paket.pluginler.Add(p);
            }
            sendPackage(paket);
            while (connected)
            {
                if (getPackage())
                {
                    if (Gpaket.type == IPaket.PAKETTYPE.CEVAP)
                    {
                        if (Gpaket.cevap)
                        {

                            odabagli = true;
                            return true;
                        }
                        else
                        {
                            // KODLAR
                            MessageBox.Show(Gpaket.detay, "Tüh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        public static void clientOl()
        {
            connected = false;
            stream.Close();
            client.Close();
        }
        public static void MesajYolla(string msj)
        {
            
            paketInfo.mesaj = msj;
            paket.type = IPaket.PAKETTYPE.MESAJ;
            paketInfo.ARGB = Sohbet.ownTextColor.ToArgb();
            eventler[EventManager.EVENTTYPE.MESAJYOLLA](evento, paketInfo);
            sendPackage(paket);
        }
        public static void MesajAl()
        {
            while (true)
            {
                if(getPackage())
                {
                    if(Gpaket.type == IPaket.PAKETTYPE.MESAJ)
                    {
                        GpaketInfo.mesaj = Gpaket.msj;
                        GpaketInfo.ARGB = Gpaket.ARGB;
                        eventler[EventManager.EVENTTYPE.MESAJAL](evento, GpaketInfo);
                        if(Sohbet.AllowColorfulTexts)
                        {
                            Sohbet.mesajEkle(GpaketInfo.mesaj, Color.FromArgb(GpaketInfo.ARGB));
                            Sohbet.ScrollGuncelle();
                        }
                        else
                            Sohbet.mesajEkle(GpaketInfo.mesaj, Color.Black);
                    }else if (Gpaket.type == IPaket.PAKETTYPE.SENDBEEP)
                    {
                        Sohbet.BEEPSOUND();
                    }
                    else if (Gpaket.type == IPaket.PAKETTYPE.GETPARTICIPANTS)
                    {
                        Sohbet.katilimcilar = Gpaket.katilimcilar;
                    }
                }
            }
        }

        public static void getKatilimcilar()
        {
            paket.type = IPaket.PAKETTYPE.GETPARTICIPANTS;
            sendPackage(paket);
        }
        public static void sendBeep()
        {
            paket.type = IPaket.PAKETTYPE.SENDBEEP;
            sendPackage(paket);
        }
    }
}
