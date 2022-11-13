using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace The_Platform
{
    public class Client
    {
        public static TcpClient client;
        string ip;
        int port;
        public static StreamReader g_akis;
        public static StreamWriter c_akis;
        public Client(string ip,int port)
        {
            client = new TcpClient(ip,port);
            if (!client.Connected)
                MessageBox.Show("Client Bağlanamadı !");
            else
            {
                connect();
                this.port = port;
                this.ip = ip;
            }

        }
        public void connect()
        {
            g_akis = new StreamReader(client.GetStream());
            c_akis = new StreamWriter(client.GetStream());
            c_akis.WriteLine("[SCOMMAND] ADD USERNAME "+UserPanel.username);
            c_akis.Flush();
        }
    }
}
