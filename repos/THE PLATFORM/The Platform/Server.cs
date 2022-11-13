using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
namespace The_Platform
{
    public class Mesaj_Alici
    {
        private int clientnumber = 0;
        private Chat cht;
        private Thread thread_message;
        List<string> command_list;
        private bool devam;
        public Mesaj_Alici(Chat cht)
        {
            this.cht = cht;
            devam = true;
            clientnumber = Chat.HOW_MANY_CLIENT_CONNECTED;
            thread_message = new Thread(get_MessagesS);
            thread_message.Start();
        }
        private void get_MessagesS()
        {
            
            while (devam)
            {
                try
                {
                    Chat.g_msg = Chat.g_akis[clientnumber].ReadLine();
                    if (Chat.g_msg == null || Chat.g_msg.Length == 0)
                    {
                        continue;
                    }
                    
                    if (Chat.start_with(Chat.g_msg, "[SCOMMAND] ADD USERNAME"))
                    {
                        command_list = Chat.g_msg.Split(' ').ToList();
                        cht.add_to_users(command_list[3]);
                        Chat.USERS.Add(command_list[3]);
                    }
                    else
                    {
                        cht.add_to_chat(Chat.g_msg);
                    }
                    for (int i = 0; i < Chat.c_akis.Count; i++)
                    {
                        if (i != clientnumber)
                        {
                            Chat.c_akis[i].WriteLine(Chat.g_msg);
                            Chat.c_akis[i].Flush();
                        }
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    continue;
                }
            }
        }
        public void stop_thread()
        {
            devam = false;
        }
    }
    public class Server
    {
        public static TcpListener listener;
        public static Socket server_soket;
        NetworkStream stream;
        private int port;
        public Server(int port)
        {
            this.port = port;
            listener = new TcpListener(port);
        }
        public NetworkStream wait_client()
        {
            listener.Start();
            Chat.USERS.Add(UserPanel.username);
            server_soket =listener.AcceptSocket();
            stream = new NetworkStream(server_soket);
            return stream;
        }
    }
}
