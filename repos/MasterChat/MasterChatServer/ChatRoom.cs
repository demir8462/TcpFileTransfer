using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCLIB;
namespace MasterChatServer
{
    public class ChatRoom
    {
        public int ID;
        public string JOINID,PASS;
        public List<SClient> participants = new List<SClient>();
        public List<Plugin> plugins = new List<Plugin>();   
        public int participantcount;
        public static object o = new object(),mesajo = new object();
        public ChatRoom(string PASS)
        {
            this.PASS = PASS;
            Server.ALLROOMCOUNT++;
            ID = Server.ALLROOMCOUNT;
            
            Task.Factory.StartNew(MesageRouter);
        }
        public void MesageRouter()
        {
            while (true)
            {
               lock(Server.odao)
                {
                    try
                    {
                        foreach (SClient sender in participants)
                        {

                            try
                            {
                                foreach (SClient members in participants)
                                {
                                    if (members == sender)
                                        continue;
                                    lock (mesajo)
                                    {
                                        foreach (Mesaj msj in sender.mesajlar)
                                        {
                                            members.mesajAt(msj.msj, msj.ARGB);
                                        }
                                    }
                                }
                                sender.mesajlar.Clear();
                            }
                            catch (Exception e)
                            {
                                Form1.writeConsole(e.Message);
                                continue;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Form1.writeConsole(e.Message);
                        continue;
                    }
                }
            }
                
            
        }
    }
}
