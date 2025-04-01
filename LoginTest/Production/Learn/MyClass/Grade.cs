using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;

namespace TestCompa.Production.Learn.Grade
{
    public class ClassTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "https://compaclass.com/learn/home";
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
        public void Grade()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(5000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            Thread.Sleep(5000);
            IWebElement testclass = wait.Until(d => d.FindElement(By.XPath("//a[contains(text(),'Class Test')]")));
            testclass.Click();
            Thread.Sleep(5000);
            IWebElement grade = driver.FindElement(By.XPath("//a[text()='Điểm số']"));
            grade.Click();
            Thread.Sleep(2000);




        }
        //2. test điều hướng trang Xem điểm số
        [Test, Order(2)]
        public void gradeMark()
        {
            Grade();
            IWebElement mark = driver.FindElement(By.XPath("//a[contains(text(),'KPIM Academy')]"));
            mark.Click();
            Thread.Sleep(5000);

        }

        //3. Xem điểm theo chương
        [Test, Order(3)]
        public void chapterView()
        {
            Grade();
            Thread.Sleep(2000);
            IWebElement btnView = driver.FindElement(By.XPath("//span[@class='flex-1 text-start font-bold text-white line-clamp-1 ']"));
            btnView.Click();
            Thread.Sleep(2000);
            IReadOnlyCollection<IWebElement> labels = btnView.FindElements(By.XPath("//label"));
            if (labels.Count > 0)
            {
                Random rand = new Random();
                int index = rand.Next(labels.Count);
                labels.ElementAt(index).Click();
            }

            Thread.Sleep(2000);
        }
        //4. Xem điểm theo bài tập
        [Test, Order(4)]
        public void assignmentView()
        {
            Grade();
            IWebElement user = driver.FindElement(By.XPath("//a[normalize-space()='KPIM Academy']"));
            user.Click();
            Thread.Sleep(3000);
            IWebElement btnView = driver.FindElement(By.XPath("//button[@type='button']//span[contains(text(),'Loại bài tập')]"));
            btnView.Click();
            Thread.Sleep(2000);
            IReadOnlyCollection<IWebElement> labels = btnView.FindElements(By.XPath("//label"));
            if (labels.Count > 0)
            {
                Random rand = new Random();
                int index = rand.Next(labels.Count);
                labels.ElementAt(index).Click();
                Thread.Sleep(2000);
            }

            Thread.Sleep(2000);
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
