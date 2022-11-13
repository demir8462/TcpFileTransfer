using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace MasterChatServer
{
    public class Server
    {
        public static TcpListener listener;
        public static List<SClient> clientler = new List<SClient>();
        public static List<SClient> REMOVEclients = new List<SClient>();
        public static List<ChatRoom> odalar = new List<ChatRoom>();
        public static List<ChatRoom> Rodalar = new List<ChatRoom>();
        public static int ALLROOMCOUNT = 0;
        public static object o = new object(), odao = new object();
        public static Thread odakontrolTHR;
        public static void startListen(int port)
        {
            listener = new TcpListener(port);
            listener.Start();
            odakontrolTHR = new Thread(new ThreadStart(odaListKontrol)) ;
            odakontrolTHR.Start();
            while(true)
            {
                lock (o)
                {
                    clientler.Add(new SClient(listener.AcceptSocket()));
                    Form1.writeConsole("baglantı geldi..");
                }
            }
        }
        public static void removeClientControl(SClient c)
        {
            lock(o)
            {
                clientler.Remove(c);
                REMOVEclients.Remove(c);
            }
        }
        public static void removeClientEkle(SClient c)
        {
            
                REMOVEclients.Add(c);
            
        }
        public static void odaListKontrol()
        {
            while (true)
            {
                Form1.odaTemizle();
                lock(odao)
                {
                    foreach (ChatRoom oda in odalar)
                    {

                        if (oda.participantcount == 0)
                        {
                            Rodalar.Add(oda);
                        }
                        else
                        {
                            Form1.odaEkle(oda.ID.ToString(), oda.JOINID, oda.participantcount);
                        }

                    }
                    foreach (ChatRoom item in Rodalar)
                    {
                        odalar.Remove(item);
                    }
                }
                Thread.Sleep(1000);
            }
            // 0 katılımcı varsa listeleeme hatta sil ok ? tmm abi yapiom
        }
    }
}
