using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using SeleniumExtras.WaitHelpers;

namespace TestCompa.Server.Learn.Grade
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
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        private void Login()
        {
            try
            {
                wait.Until(d => d.FindElement(By.Id("email"))).SendKeys("info@kpim.vn");
                driver.FindElement(By.Id("password")).SendKeys("KPIM@123");
                driver.FindElement(By.XPath("//button[text()='SIGN IN']")).Click();
                wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']"))); // Chờ trang load xong
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Đã đăng nhập.");
            }
        }

        private void NavigateToGrade()
        {
            Login();

            // Chờ và bấm vào menu class
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[href='/learn/class']"))).Click();

            // Cuộn xuống cuối trang
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

            // Chờ và bấm vào trang 2
            IWebElement navigatePage2 = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[normalize-space()='2']")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", navigatePage2);
            Thread.Sleep(500);
            navigatePage2.Click();

            // Chờ nội dung trang 2 load xong trước khi tiếp tục
            Thread.Sleep(2000); // Hoặc có thể chờ phần tử trong trang 2 xuất hiện thay vì dùng `Thread.Sleep`

            // Chờ và chọn lớp "Power BI Cơ Bản"
            IWebElement testclass = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(),'Power BI Cơ Bản')]")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", testclass);
            Thread.Sleep(500);
            testclass.Click();

            // Chờ và chọn tab "Điểm số"
            IWebElement assign = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[text()='Điểm số']")));
            assign.Click();
        }


        [Test, Order(1)]
        public void Assignments()
        {
            NavigateToGrade();
        }

        [Test, Order(2)]
        public void AssignmentsMark()
        {
            NavigateToGrade();
            Thread.Sleep(1000);
            IWebElement mark = wait.Until(d => d.FindElement(By.XPath("//a[contains(text(),'Trương Minh Vương')]")));
            mark.Click();
        }

        [Test, Order(3)]
        public void ChapterView()
        {
            AssignmentsMark();
            Thread.Sleep(2000);
            IWebElement btnView = wait.Until(d => d.FindElement(By.XPath("//button[@type='button']//span[contains(text(),'Tất cả các chương')]")));
            btnView.Click();

            IReadOnlyCollection<IWebElement> labels = wait.Until(d => d.FindElements(By.XPath("//label")));
            if (labels.Count > 0)
            {
                labels.ElementAt(new Random().Next(labels.Count)).Click();
            }
        }

        [Test, Order(4)]
        public void AssignmentView()
        {
            AssignmentsMark();
            IWebElement btnView = wait.Until(d => d.FindElement(By.XPath("//button[@type='button']//span[contains(text(),'Loại bài tập')]")));
            btnView.Click();

            IReadOnlyCollection<IWebElement> labels = wait.Until(d => d.FindElements(By.XPath("//label")));
            if (labels.Count > 0)
            {
                labels.ElementAt(new Random().Next(labels.Count)).Click();
            }
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
