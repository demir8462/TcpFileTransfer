using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
namespace The_Platform
{
    public partial class Chat : Form
    {
        public static int HOW_MANY_CLIENT_CONNECTED = 0;
        NetworkStream stream;
        public static List<StreamWriter> c_akis = new List<StreamWriter> { };
        public static List<StreamReader> g_akis = new List<StreamReader> { };
        public static List<string> USERS = new List<string> { };
        List<string> command_list;
        List<Mesaj_Alici> message_getters;
        public static String g_msg;
        Thread server_thread,client_chat;
        public Chat()
        {
            InitializeComponent();
        }
        public void baglantilari_al()
        {
            while(UserPanel.isserver)
            {
                stream = UserPanel.server.wait_client();
                g_akis.Add(new StreamReader(stream));
                c_akis.Add(new StreamWriter(stream));
                message_getters.Add(new Mesaj_Alici(this));
                HOW_MANY_CLIENT_CONNECTED += 1;
                for (int i = 0; i < USERS.Count; i++)
                {
                    c_akis[HOW_MANY_CLIENT_CONNECTED - 1].WriteLine("[SCOMMAND] ADD USERNAME " + USERS[i]);
                }
                c_akis[HOW_MANY_CLIENT_CONNECTED - 1].Flush();
            }
            
        }
        private void Chat_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            button1.BackgroundImage = Image.FromFile(Program.DIRECTORY+"\\pic\\send.png");
            if(UserPanel.isserver)
            {
                chatting.Items.Add("[SERVER] Server Başlatılıyor..");
                chatting.Items.Add("[SERVER] Ilk bağlantı bekleniyor..");
                message_getters = new List<Mesaj_Alici> { };
                server_thread = new Thread(baglantilari_al);
                server_thread.Start();
                
            }
            else
            {
                client_chat = new Thread(get_Messages);
                client_chat.Start();
            }
            
        }
        
        private void get_Messages()
        {
            
            while (true)
            {
                g_msg = Client.g_akis.ReadLine();
                if (g_msg == null)
                    continue;
                if (g_msg.Length == 0)
                    continue;
                if(start_with(g_msg,"[SCOMMAND] ADD USERNAME"))
                {
                    command_list = g_msg.Split(' ').ToList();
                    users.Items.Add(command_list[3]);
                }else
                    add_to_chat(g_msg);
            }
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(UserPanel.isserver)
            {
                UserPanel.isserver = false;
                for(int i =0;i<g_akis.Count;i++)
                {
                    g_akis[i].Close();
                    c_akis[i].Close();
                    message_getters[i].stop_thread();
                }
                Server.listener.Stop();
                Server.server_soket.Close();
            }else
            {
                Client.c_akis.Close();
                Client.g_akis.Close();
                Client.client.Close();
            }
            Application.Exit();
        }

        public void add_to_chat(string msg)
        {
            chatting.Items.Add(msg);
        }
        public void add_to_users(string nick)
        {
            users.Items.Add(nick);
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
                return;
            if(UserPanel.isserver)
            {
                for (int i = 0; i < c_akis.Count; i++)
                {
                    c_akis[i].WriteLine(UserPanel.username + "-->" + textBox1.Text);
                    c_akis[i].Flush();
                }
            }else
            {
                Client.c_akis.WriteLine(UserPanel.username+"-->"+textBox1.Text);
                Client.c_akis.Flush();
            }
            add_to_chat(UserPanel.username + "-->" + textBox1.Text);
            textBox1.Text = "";
        }
        public static bool start_with(string text,string with)
        {
            if (with.Length > text.Length)
                return false;
            for(int i =0;i<with.Length;i++)
            {
                if (text[i] != with[i])
                    return false;
            }
            return true;
        }
    }
}
