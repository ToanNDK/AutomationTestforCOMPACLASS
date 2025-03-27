using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace TestCompa
{
    public class HomeTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://10.10.10.30");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        //Test 1: Test nút Bắt đầu khám phá
        [Test]

        public void NavigationToAcademyKPIM()
        {
            Thread.Sleep(2000);
            IWebElement startButton = wait.Until(d => d.FindElement(By.XPath("//button[span and normalize-space(span)='Bắt đầu khám phá']")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", startButton);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", startButton);
            Thread.Sleep(2000);
            wait.Until(d => d.Url == "http://10.10.10.30/vn/academy/kpim");
            Assert.AreEqual("http://10.10.10.30/vn/academy/kpim", driver.Url, "Không chuyển hướng thành công");

        }
        //Test 2: Test các nút trên sidebar
        [Test]
        public void btnSidebar()
        {
            Thread.Sleep(2000);
            IWebElement startButton = wait.Until(d => d.FindElement(By.XPath("//button[span and normalize-space(span)='Bắt đầu khám phá']")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", startButton);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", startButton);
            Thread.Sleep(2000);
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[contains(text(),'Đăng nhập')]"));
            loginBtn.Click();
            Thread.Sleep(2000);
            Login();
            Thread.Sleep(2000);
            IWebElement course = driver.FindElement(By.XPath("//a[@href='/learn/course']"));
            course.Click();
            Thread.Sleep(2000);
            IWebElement classes = driver.FindElement(By.XPath("//a[@href='/learn/class']"));
            classes.Click();
            Thread.Sleep(2000);
            IWebElement leaderboard = driver.FindElement(By.XPath("//a[@href='/learn/leaderboard']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", leaderboard);
            Thread.Sleep(2000);
            leaderboard.Click();
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(4000);
            IWebElement instruction = driver.FindElement(By.XPath("//a[@href='/learn/instruction']"));
            instruction.Click();
            Thread.Sleep(2000);
        }

        //Test 3: Bấm khóa học trên sidebar -> Bấm nút Khám phá khóa học mới
        [Test]
        public void btnDiscoveryCourse()
        {
            Thread.Sleep(2000);
            IWebElement startButton = wait.Until(d => d.FindElement(By.XPath("//button[span and normalize-space(span)='Bắt đầu khám phá']")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", startButton);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", startButton);
            Thread.Sleep(2000);
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[contains(text(),'Đăng nhập')]"));
            loginBtn.Click();
            Thread.Sleep(2000);
            Login();
            IWebElement course = driver.FindElement(By.XPath("//a[@href='/learn/course']"));
            course.Click();
            Thread.Sleep(2000);
            IWebElement btnCourseDiscovery = driver.FindElement(By.XPath("//button[contains(text(),'Khám phá khóa học mới')]"));
            btnCourseDiscovery.Click();
            Thread.Sleep(2000);
        }
        //Test 4: Bấm chọn mua khóa học 0đ
        [Test]
        public void purchase0Course()
        {
            Thread.Sleep(2000);
            IWebElement startButton = wait.Until(d => d.FindElement(By.XPath("//button[span and normalize-space(span)='Bắt đầu khám phá']")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", startButton);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", startButton);
            Thread.Sleep(2000);
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[contains(text(),'Đăng nhập')]"));
            loginBtn.Click();
            Thread.Sleep(2000);
            Login();
            Thread.Sleep(2000);
            IWebElement course0 = driver.FindElement(By.XPath("//a[contains(text(), 'Power BI Fundamental')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", course0);
            Thread.Sleep(5000);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", course0);
            Thread.Sleep(5000);
            Assert.IsTrue(driver.Url.Contains("http://10.10.10.30/vn/learn/course/power-bi-fundamental/overview"));
            var registerCourse = driver.FindElement(By.CssSelector("button.group\\/btn"));

            if (registerCourse != null)
            {
                string buttonText = registerCourse.Text.Trim();

                if (buttonText == "Ghi danh")
                {
                    registerCourse.Click();
                    Assert.IsTrue(driver.Url.Contains("http://10.10.10.30/learn/checkout"));
                    Thread.Sleep(5000);
                }
                else if (buttonText == "Đi đến học")
                {
                    registerCourse.Click();
                    Assert.IsTrue(driver.Url.Contains("http://10.10.10.30/vn/learn/course"));
                    Thread.Sleep(5000);

                }
            }
            else
            {
                Assert.Fail("Không tìm thấy nút đăng ký hoặc học.");
            }
            Thread.Sleep(5000);



        }
        public void Login()
        {
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("lozik480@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Toanking2k3*");

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();

        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
