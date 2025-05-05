using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.Studio.Blog
{
    public class Blog
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string blogUrl = "https://studio.compaclass.com/en/blog";
        private void InitDriver(bool headless = false)
        {
            ChromeOptions options = new();

            if (headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-gpu");
            }

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        [SetUp]
        public void SetUp()
        {

            // Gọi InitDriver với tham số headless = false (mặc định)
            // Thay đổi thành true nếu muốn chạy ở chế độ headless
            InitDriver(true);
        }

        //Truy cập Blog từ Discovery 
        [Test]
        public void BlogInDiscovery()
        {
            driver.Navigate().GoToUrl("https://compaclass.com/learn/home");
            Login();
            Thread.Sleep(1000);
            IWebElement discovery = wait.Until(d => d.FindElement(By.CssSelector("a[href='/discovery']")));
            discovery.Click();
            Thread.Sleep(2000);
            IWebElement blog = driver.FindElement(By.XPath("//a[contains(text(),'JavaScript có còn cần thiết trong thời đại AI và N')]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", blog);
            Thread.Sleep(1000);
            blog.Click();
            Thread.Sleep(2000);
        }
        //Truy cập qua tab Blog
        [Test]
        public void BlogTab()
        {
            driver.Navigate().GoToUrl("https://compaclass.com/learn/home");
            Login();
            Thread.Sleep(1000);
            IWebElement blog = wait.Until(d => d.FindElement(By.CssSelector("a[href='/blog']")));
            blog.Click();
            Thread.Sleep(2000);
        }
        [Test]
        public void CreateNew()
        {

            driver.Navigate().GoToUrl(blogUrl);
            Login();
            Thread.Sleep(2000);
            IWebElement Create = driver.FindElement(By.XPath("//button[normalize-space()='New blog']"));
            Create.Click();
            Thread.Sleep(1000);
        }

        [Test]
        public void AddContent()
        {

        }

        public void Login()
        {
            Thread.Sleep(2000);
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
