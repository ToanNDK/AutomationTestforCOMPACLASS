using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;

namespace TestCompa.Server.Learn.MyClassProgress
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
        //  2. Test chức năng điều hướng: Nhấn Tab Overview

        [Test, Order(1)]
        public void testOverTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();

            // Đợi trang load và click vào nút để chuyển trang
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();  // Nhấn vào để chuyển trang
            Thread.Sleep(3000);
            IWebElement overview = wait.Until(d => d.FindElement(By.XPath("//div[contains(@class,'flex flex-col gap-2 lg:grid sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-3 lg:gap-4 xl:grid-cols-4 2xl:grid-cols-5')]//div[1]//div[1]//a[1]")));
            overview.Click();
            Thread.Sleep(5000);
            

        }
        //3. Test thông tin chi tiết về mục lục nội dung học, giới thiệu lớp học, phần trăm tiến độ...

        [Test, Order(2)]
        public void testScrollbar()
        {
            testOverTest(); // Gọi test trước đó để vào trang chính

            Thread.Sleep(5000); // Đợi trang load hoàn toàn

            // Tìm tất cả các tab trong thanh điều hướng
            var tabs = driver.FindElements(By.CssSelector("div.flex.gap-8 a"));

            foreach (var tab in tabs)
            {
                Console.WriteLine("Đang bấm vào: " + tab.Text);

                // Cuộn đến phần tử (tránh bị che mất)
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", tab);
                Thread.Sleep(500); 

                // Click vào tab
                tab.Click();
                Thread.Sleep(2000); // Đợi nội dung tab load xong
            }
        }
        [Test,Order(3)]
        public void contiuneLearn()
        {
            driver.Navigate().GoToUrl("http://10.10.10.30/learn/home");
            Login();
            Assert.IsTrue(driver.Url.Contains("http://10.10.10.30/learn/home"));
            Thread.Sleep(5000);
            IWebElement continueLearn = driver.FindElement(By.XPath("//a[span[text()='Tiếp tục học']]"));
            continueLearn.Click();
            Thread.Sleep(10000);
            Assert.IsTrue(driver.Url.Contains("http://10.10.10.30/vn/learn/course"));
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
