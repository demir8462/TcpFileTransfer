
using OpenQA.Selenium.Chrome;

namespace SELENIUMCARTCURT
{
    public partial class Form1 : Form
    {
        public string URL = "https://www.instagram.com/accounts/login/";
        ChromeDriver drv;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            drv = new ChromeDriver();
            drv.Navigate().GoToUrl(URL);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            drv.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"loginForm\"]/div/div[1]/div/label/input")).SendKeys(textBox2.Text);
            drv.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"loginForm\"]/div/div[2]/div/label/input")).SendKeys(textBox3.Text);
            drv.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"loginForm\"]/div/div[3]/button")).Click();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            drv.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"mount_0_0_Px\"]/div/div/div/div[1]/div/div/div/div[1]/section/nav/div[2]/div/div/div[3]/div/div[2]/a/svg")).Click();
        }
    }
}