using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Studio
{
    public class HomeCourse
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "http://10.10.10.30:3000/";
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
        //Test 0: Truy cập trang 
        [Test]
        public void studioTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            IWebElement course = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > aside:nth-child(1) > div:nth-child(1) > a:nth-child(3) > button:nth-child(1)"));
            course.Click();
            Thread.Sleep(5000);
        }
        //Test 1: bấm nút tạo khóa học mới
        [Test]
        public void btnNewCourse()
        {
            studioTest();
            IWebElement create = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > div:nth-child(2) > article:nth-child(1) > section:nth-child(1) > div:nth-child(2) > button:nth-child(2)"));
            Actions action = new(driver);
            action.DoubleClick(create).Perform();
            Thread.Sleep(5000);
        }
        //Test 2: Tạo khóa học mới thành công
        [Test]
        public void createNewCourse()
        {
            btnNewCourse();
            IWebElement btnCreate = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > button:nth-child(1)"));
            btnCreate.Click();
            Thread.Sleep(5000);
            IWebElement name = driver.FindElement(By.XPath("//input[@name='name']"));
            name.SendKeys("Test Course 123");
            IWebElement thumbnailUrl = driver.FindElement(By.XPath("//input[@name='thumbnail']"));
            thumbnailUrl.SendKeys("https://picsum.photos/200/300");
            IWebElement description = driver.FindElement(By.XPath("//textarea[@name='description']"));
            description.SendKeys("Course Description");
            IWebElement price = driver.FindElement(By.XPath("//input[@name='price']"));
            price.SendKeys("2500");
            IWebElement hours = driver.FindElement(By.XPath("//input[@name='estimateHours']"));
            hours.SendKeys("2");
            IWebElement bannerUrl = driver.FindElement(By.XPath("//input[@name='banner']"));
            bannerUrl.SendKeys("https://picsum.photos/id/870/200/300?grayscale&blur=2");
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(5000);
        }

        public void Login()
        {
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
