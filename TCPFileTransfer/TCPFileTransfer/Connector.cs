using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TCPFileTransfer
{
    public enum TYPE { SERVER, CLIENT };

    public class Connector
    {
        public static int BUFFERSIZE = 1024*1024;
        byte[] paket = new byte[BUFFERSIZE];
        public static int bytesread;
        public static long TOTALBYTETOREAD,RECIEVE_FILE_SIZE;
        TcpListener listener;
        TcpClient client;
        NetworkStream stream;

        BinaryFormatter bf = new BinaryFormatter();
        SpecialDataPackages iletisimPaketi = new SpecialDataPackages();
        bool BLOCK_READING_STREAM;
        TYPE type;
        public void writeBytes(byte[] paket)
        {
            stream.Write(paket,0,BUFFERSIZE);
            stream.Flush();
        }
        public byte[] getBytes()
        {
            bytesread = stream.Read(paket, 0, BUFFERSIZE);
            if(bytesread > 0)
                Form1.writeConsole("BytesRead : " + bytesread);
            return paket;
        }
        public void baglantiAl(int port)
        {
            Form1.writeConsole("BAĞLANTI BEKLENIYOR : "+port);
            listener = new TcpListener(port);
            listener.Start();
            stream = new NetworkStream(listener.AcceptSocket());
            Form1.writeConsole("BAĞLANTI KURULDU ! : " + stream.Socket.RemoteEndPoint);
        }
        public bool baglan(string ip, int port)
        {
            client = new TcpClient(ip, port);
            stream = client.GetStream();
            return client.Connected;
        }
        public bool dosyaYollaIstek(string name,long size)
        {
            iletisimPaketi.name = name;
            iletisimPaketi.size = size;
            iletisimPaketi.tip = SpecialDataPackages.type.DOSYAYOLLA;
            BLOCK_READING_STREAM = true;
            bf.Serialize(stream,iletisimPaketi);
            readSpecial();
            if (iletisimPaketi.tip == SpecialDataPackages.type.CEVAP && iletisimPaketi.cevap)
            {
                Form1.writeConsole("ONAY GELDI !");
            }
            else
                Form1.writeConsole("ONAY GELMEDI PU !");
            BLOCK_READING_STREAM = false;
            return iletisimPaketi.cevap;
        }
        public void dosyaBekle()
        {
            readSpecial();
            if(iletisimPaketi.tip == SpecialDataPackages.type.DOSYAYOLLA)
            {
                DialogResult dialogResult = MessageBox.Show("Dosya Istegı :" + iletisimPaketi.name + ":" + iletisimPaketi.size, "ISTEK", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    iletisimPaketi.cevap = true;
                }
                else if (dialogResult == DialogResult.No)
                {
                    iletisimPaketi.cevap = false;
                }
                iletisimPaketi.tip = SpecialDataPackages.type.CEVAP;
                bf.Serialize(stream, iletisimPaketi);
                TOTALBYTETOREAD = iletisimPaketi.size;
                RECIEVE_FILE_SIZE = iletisimPaketi.size;
                Form1.recivingFileSizeLabel.Text = (RECIEVE_FILE_SIZE/1024/1024).ToString()+"MB";
                Task.Run(() =>
                {
                    Form1.writeConsole("SOKETTEN VERI OKUMA BAŞLADI !");
                    startReadingFile();
                });
            }
        }
        void startReadingFile()
        {
            try
            {
                FileStream filestream;
                filestream = File.OpenWrite(Form1.SAVE_PATH);
                byte[] buffer = new byte[BUFFERSIZE];
                int bytesRead = -1;
                while (true)
                {
                    if(TOTALBYTETOREAD < BUFFERSIZE)
                        bytesread = stream.Read(buffer, 0, (int)TOTALBYTETOREAD);
                    else
                        bytesread = stream.Read(buffer, 0, buffer.Length);
                    TOTALBYTETOREAD -= bytesread;
                    Form1.writeConsole("SOKETTEN VERI OKUNDU :" + bytesread + " KALAN : "+TOTALBYTETOREAD);
                    filestream.Write(buffer, 0, bytesread);
                    Form1.updateRecivedBytes(RECIEVE_FILE_SIZE - TOTALBYTETOREAD);
                    if (0 == TOTALBYTETOREAD)
                    {
                        Form1.writeConsole("DOSYA TAMAMEN ALINDI !");
                        filestream.Flush();
                        filestream.Close();
                        break;
                    }
                }
            }
            catch(Exception e)
            {
                Form1.writeConsole(e.Message);
            }
            
        }
        public bool readSpecial()
        {
            while(true)
            {
                if ((iletisimPaketi= (SpecialDataPackages)bf.Deserialize(stream)) == null)
                    continue;
                else
                {
                    return true;
                }
            }
        }
    }
    [Serializable]
    class SpecialDataPackages
    {
        public enum type {DOSYAYOLLA ,CEVAP};
        public string name;
        public long size;
        public bool cevap;
        public type tip;
    }
}
