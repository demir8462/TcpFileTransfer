using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Security.Policy;

namespace YERISTASYON
{
    public class PortReader
    {
        string name;
        int baudrate, databitscount;
        Parity p;
        StopBits b;
        SerialPort sp;
        Thread bagkontrol;
       
        public static DateTime lasttime;
        bool bagKKoptu;
        public PortReader(string name,int baudrate,int databits,Parity p,StopBits b)
        {
            this.name = name;
            this.baudrate = baudrate;
            this.databitscount = databits;
            this.p = p;
            this.b = b;
            bagkontrol = new Thread(new ThreadStart(bagKontrolF));
            bagkontrol.Start();
        }
        public void bagKontrolF()
        {
            while(true)
            {
                if(!SerialPort.GetPortNames().Contains(name))
                {
                    bagKoptu();
                }else
                {
                    if(bagKKoptu)
                    {
                        bagKKoptu = false;
                        Thread.Sleep(600);
                        openPort();
                    }
                }
            }
        }
        public void bagKoptu()
        {
            bagKKoptu = true;
            try
            {
                sp.Close();
            }catch(Exception e)
            {
                Form1.writeConsole("BAGLANTI KOPTU[EXCP] ISOPEN:" + sp.IsOpen.ToString());
            }
        }
        public void PortVeriGeldi(object sender, EventArgs e)
        {
            Form1.writeConsole("");
            Form1.writeConsole("Veri Geldi !");
            Form1.writeConsole(sp.ReadExisting());
            lasttime = DateTime.Now;
       
        }
        
        public void regEvent(SerialDataReceivedEventHandler e)
        {
            sp.DataReceived += e;
            Form1.writeConsole("EVENT REGISTERED: "+e.ToString());
        }
        public bool openPort()
        {
            sp = new SerialPort(name, baudrate, p, databitscount, b);
            regEvent(PortVeriGeldi);
            sp.Open();
            Form1.writeConsole("PORT ACILDI :"+sp.IsOpen.ToString());
            
            return sp.IsOpen;
        }
        public static string[] getComs()
        {
            return SerialPort.GetPortNames();
        }
        
    }
}
