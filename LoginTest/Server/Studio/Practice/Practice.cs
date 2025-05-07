using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Studio.Practice
{
    public class Blog
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string blogUrl = "http://10.10.10.30:3000/en/practice";
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

        //Truy cập Practice từ Studio 
        [Test]
        public void NavigateToPractice()
        {
            driver.Navigate().GoToUrl(blogUrl);
            Login();
            Thread.Sleep(1000);
        }

        [Test]
        public void BlogTab()
        {
            NavigateToPractice();
            IWebElement CreateNewQuiz = driver.FindElement(By.XPath("//button[normalize-space()='New Practice Quiz']"));
            CreateNewQuiz.Click();
            // Navigate to Practice Page
            Assert.True(driver.Url.Contains("https://studio.compaclass.com/en/practice"));
            Thread.Sleep(1000);
        }
        private void Login()
        {
            Thread.Sleep(2000);
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("lozik480@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Toanking2k3*");

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
