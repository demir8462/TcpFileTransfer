using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace The_Platform
{
    static class Program
    {
        public static MySqlDataAdapter adapter = new MySqlDataAdapter();
        public static DataTable tablo = new DataTable();
        public static Form logn,Panel,showp,chat;
        public static MySqlConnection baglanti;
        private static Form maintenance;
        public static String DIRECTORY = Environment.CurrentDirectory;
        private static void mysql_connect()
        {
            baglanti = new MySqlConnection("Server=localhost;Database=users;uid=yazilim;Password=@547320@Demir@;");
            try
            {
                baglanti.Open();
                Application.Run(logn);
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                maintenance = new Maintenance();
                Application.Run(maintenance);
                
            }
        }
        public static bool mysql_komut_islet(string command)
        {

            new MySqlCommand(command, baglanti).ExecuteNonQuery();
            return true;
        }
        public static DataTable db_kisi_cek(string username, string pass)
        {
            adapter.SelectCommand = new MySqlCommand("SELECT * FROM users WHERE USERNAME='" + username + "' AND PASSW='" + pass + "';", Program.baglanti);
            adapter.Fill(tablo);
            return tablo;
        }
        public static DataTable db_kisi_cek(string username)
        {
            tablo.Clear();
            adapter.SelectCommand = new MySqlCommand("SELECT USERNAME,REGISTERED,PICTURE,LONLINE FROM users WHERE USERNAME='" + username + "';", Program.baglanti);
            adapter.Fill(tablo);
            try
            {
                if (tablo.Rows[0]["username"] == username)
                    return tablo;
            }catch(Exception ee)
            {
                MessageBox.Show("Kullanıcı Bulunamadı !");
            }
            return tablo;
        }
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            logn = new login();
            mysql_connect();
            
        }
    }
}
