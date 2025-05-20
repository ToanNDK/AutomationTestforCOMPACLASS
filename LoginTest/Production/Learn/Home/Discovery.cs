using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace TestCompa.Production.Learn.Discovery
{
    [TestFixture]
    [Category("Discovery")]
    public class Discovery
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "https://compaclass.com/learn/home";
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
        public void Login()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            //IWebElement emailInput = driver.FindElement(By.Id("email"));
            IWebElement emailInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("email")));
            emailInput.SendKeys("lozik480@gmail.com");

            //IWebElement passwordInput = driver.FindElement(By.Id("password"));
            IWebElement passwordInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("password")));
            passwordInput.SendKeys("Toanking2k3*");

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();

        }

        [TearDown]

        public void TearDown()
        {
            driver.Quit();
        }

        //Test truy cập discovery
        [Test]
        public void NavigateToDiscovery()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            IWebElement discoverybtn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[normalize-space()='Khám phá']")));
            discoverybtn.Click();
            Thread.Sleep(2000);
            Assert.That(driver.Url, Does.Contain("https://compaclass.com/discovery"), "No navigate to discovery!");
        }

        //Xem tất cả các khóa học free
        [Test]
        public void ViewCourseFree()
        {

            NavigateToDiscovery();
            IWebElement viewAll = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//a[@href='/vn/discovery/free-courses']")));
            viewAll.Click();
            Thread.Sleep(2000);

            Assert.That(driver.Url, Does.Contain("https://compaclass.com/vn/discovery/free-courses"), "No navigate to view all free courses!");

        }
        //Xem tất cả khóa học phỏ biến
        [Test]
        public void ViewCourse()
        {

            NavigateToDiscovery();
            IWebElement viewAll = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//a[@href='/vn/discovery/popular-courses']")));
            viewAll.Click();
            Thread.Sleep(2000);

            Assert.That(driver.Url, Does.Contain("https://compaclass.com/vn/discovery/popular-courses"), "No navigate to view all courses!");
        }
        //Xem các lớp học sắp tới
        [Test]
        public void ViewClass()
        {
            InitDriver(false);
            NavigateToDiscovery();
            IWebElement viewAll = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible
                (By.XPath("//a[@href='/vn/discovery/upcoming-classes']")));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", viewAll);
            Thread.Sleep(500);
            viewAll.Click();
            Thread.Sleep(2000);

            Assert.That(driver.Url, Does.Contain("https://compaclass.com/vn/discovery/upcoming-classes"), "No navigate to view all upcoming class!");
        }
        //Chọn các Tag -> Hiển thị 
        [Test]
        public void Tag()
        {
            NavigateToDiscovery();
            IWebElement tag = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible
                (By.XPath("//button[normalize-space()='Power BI']")));
            tag.Click();
            Thread.Sleep(500);
            Assert.That(driver.Url, Does.Contain("https://compaclass.com/discovery?tag=1"), "Unable to click tag!");

        }
    }

}
