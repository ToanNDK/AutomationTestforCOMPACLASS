using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;

namespace TestCompa.Server.Learn.Attendance
{
    public class ClassTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "http://10.10.10.30/learn/home";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(devUrl);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        // 1. Test chức năng điều hướng tới Grade

        [Test, Order(1)]
        public void Assignments()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(3000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            Thread.Sleep(5000);
            IWebElement testclass = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Test')]"));
            testclass.Click();
            Thread.Sleep(5000);
            IWebElement assign = driver.FindElement(By.XPath("//a[text()='Lịch học']"));
            assign.Click();
            Thread.Sleep(2000);




        }
        //2. test Thêm cuộc họp mới
        [Test, Order(2)]
        public void AssignmentsMark()
        {
            Assignments();
            IWebElement newMeeting = driver.FindElement(By.XPath("//button[.//span[contains(text(), 'Thêm cuộc họp mới')]]"));
            newMeeting.Click();
            Thread.Sleep(5000);

        }

        
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
