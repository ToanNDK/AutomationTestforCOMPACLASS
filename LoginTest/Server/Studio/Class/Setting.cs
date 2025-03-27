using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools.V131.FedCm;
using OpenQA.Selenium.Interactions;
using System.Xml.Linq;
using OpenQA.Selenium.BiDi.Modules.Input;
using System.Numerics;

namespace TestCompa.Server.Studio
{
    public class settingClass
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "http://10.10.10.30:3000/";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(devUrl);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        //Test 0: Truy cập trang 
        [Test]
        public void classTest()
        {
            driver.Navigate().GoToUrl(devUrl);

            Login();
            Thread.Sleep(5000);
            IWebElement classes = driver.FindElement(By.CssSelector("svg[width='20'][height='20'][viewBox='0 0 18 22']"));
            classes.Click();
            Thread.Sleep(5000);
        }
        //Test 1: bấm nút 3 chấm -> Setting
        [Test]
        public void btnSettingClass()
        {
            classTest();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,300);");
            Thread.Sleep(2000);
            IWebElement edit = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > div:nth-child(2) > article:nth-child(1) > div:nth-child(2) > section:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > button:nth-child(2) > svg:nth-child(1)"));
            edit.Click();
            Thread.Sleep(5000);
            IWebElement setting = driver.FindElement(By.XPath("//span[@class='whitespace-nowrap text-sm transition-all']"));
            setting.Click();
            Thread.Sleep(5000);
        }

        //Test 2: không nhập Occurrence, duration
        [Test]
        public void Testbtn()
        {
            btnSettingClass();
            IWebElement btnqa = driver.FindElement(By.XPath("//div[span[text()='Q&A']]/following-sibling::div/button"));
            btnqa.Click();
            Thread.Sleep(5000);
            IWebElement btnFeedback = driver.FindElement(By.XPath("//div[span[text()='Feedback']]/following-sibling::div/button"));
            btnFeedback.Click();
            Thread.Sleep(5000);
            IWebElement save = driver.FindElement(By.XPath("//button[normalize-space()='Save']"));
            save.Click();
            Thread.Sleep(5000);
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//span[contains(text(), 'This field cannot be left blank')]")));
            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không hiển thị khi nhập sai thông tin đăng nhập!");

        }

        //Test 3: 

        //Test 2: Tạo lớp mới thành công


        public void Login()
        {
            Thread.Sleep(5000);
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("info@kpim.vn");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("KPIM@123");

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }


}
