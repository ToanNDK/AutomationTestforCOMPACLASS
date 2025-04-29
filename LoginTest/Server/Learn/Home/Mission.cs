using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace TestCompa.Server.Learn.Mission
{
    public class Mission
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "http://10.10.10.30/learn/mission";
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

        //Test 1: Kiểm tra truy cập trang mission khi chưa đăng nhập -> Chuyển về trang đăng nhập
        [Test]
        public void AccessMissionWithoutLogin()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            string currentUrl = driver.Url;
            Assert.That(currentUrl.Contains("http://10.10.10.30/Auth/SignIn"), Is.True);
        }
        //Test 2: Kiểm tra truy cập trang mission khi đã đăng nhập -> Chuyển về trang Mission
        [Test]
        public void AccessMissionLogin()
        {
            driver.Navigate().GoToUrl(devUrl);

            Thread.Sleep(2000);
            Login();
            Assert.That(driver.Url, Does.Contain(devUrl));
        }

        // Test 3: Truy cập trang mission -> Nhận phần thưởng 
        [Test]
        public void recievedMission()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            Assert.That(driver.Url.Contains(devUrl), Is.True);
            IWebElement btnClaim = driver.FindElement(By.XPath("//button[span[text()='Nhận Thưởng']]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(100, 200);");
            Thread.Sleep(5000);
            btnClaim.Click();
            Thread.Sleep(3000);

        }
        //Test 4: Truy cập trang Mission -> Nhận tất cả phần thưởng 
        [Test]
        public void recivedAll()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            Login();
            Assert.That(driver.Url.Contains(devUrl), Is.True);

            IWebElement btnClaim = driver.FindElement(By.XPath("//button[span[text()='Nhận Tất cả']]"));
            btnClaim.Click();
            Thread.Sleep(5000);
            IWebElement btnConfirm = driver.FindElement(By.XPath("//button[contains(@class,'px-9 lg:px-14 bg-green-l30 flex justify-center items-center')]"));
            btnConfirm.Click();
            Thread.Sleep(5000);

            IWebElement btnWeek = driver.FindElement(By.XPath("//button[@id='headlessui-tabs-tab-:r4:']"));

            Thread.Sleep(2000);
            btnWeek.Click();
            Thread.Sleep(3000);
            btnClaim.Click();
            Thread.Sleep(3000);
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
