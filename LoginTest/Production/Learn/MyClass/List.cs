using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;

namespace TestCompa.Production.Learn.MyClassList
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
        // 1. Test chức năng tìm kiếm với từ khóa ("powerbi", "microsoft")

        [Test, Order(1)]
        public void searchClass()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();

            // Đợi trang load và click vào nút để chuyển trang
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();  // Nhấn vào để chuyển trang

            // Đợi trang mới tải xong
            wait.Until(d => d.Url.Contains("/learn/class"));

           
            IWebElement searchContainer = driver.FindElement(By.XPath("//div[contains(@class, 'flex items-center md:items-end')]"));
            IWebElement searchInput = searchContainer.FindElement(By.TagName("input"));
            //searchInput.SendKeys("power bi");
            searchInput.SendKeys("microsoft");

            searchInput.SendKeys(Keys.Enter);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,400)");


            Thread.Sleep(10000);
        }
        [Test, Order(2)]
        public void searchClassWithSuggest()
        {
            searchClass();

            IWebElement searchContainer = driver.FindElement(By.XPath("//div[contains(@class, 'flex items-center md:items-end')]"));
            IWebElement searchInput = searchContainer.FindElement(By.TagName("input"));
            searchInput.Click();
            searchInput.Clear();
            Thread.Sleep(2000);
            searchInput.Click();
            Thread.Sleep(2000);
            IWebElement element = driver.FindElement(By.CssSelector("div.flex.items-center.gap-2 p"));
            element.Click();
            Thread.Sleep(4000);
        }
        [Test, Order(3)]
        public void searchClassDelete()
        {
            searchClass();
            IWebElement delete = driver.FindElement(By.XPath("//button[@class='absolute right-3']//*[name()='svg']"));
            delete.Click();
            Thread.Sleep(3000);

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
