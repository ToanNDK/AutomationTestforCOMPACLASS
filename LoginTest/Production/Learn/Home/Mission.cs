using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.Learn
{
    public class Mission
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "https://compaclass.com/learn/mission";
        private readonly string email = "info@kpim.vn";
        private readonly string password = "KPIM@123";

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
        /*[Test]
        public void AccessMissionWithoutLogin()
        {
            InitDriver(false);
            driver.Navigate().GoToUrl(devUrl);

            string currentUrl = driver.Url;
            Thread.Sleep(3000);
            Assert.That(currentUrl, Does.Contain("https://auth.compaclass.com/Auth/SignIn"));
            Thread.Sleep(500);
        }*/

        //Test 2: Kiểm tra truy cập trang mission khi đã đăng nhập -> Chuyển về trang Mission
        [Test]
        public void AccessMissionLogin()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            string currentUrl = driver.Url;
            Assert.That(currentUrl, Does.Contain("https://auth.compaclass.com/Auth/SignIn"));
            Login();
            Assert.That(driver.Url, Does.Contain(devUrl));
        }

        // Test 3: Truy cập trang mission -> Nhận phần thưởng 
        [Test]
        public void ReceiveMission()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            Login();
            Assert.That(driver.Url, Does.Contain(devUrl));
            Thread.Sleep(15000);

            IWebElement btnClaim = driver.FindElement(By.XPath("//button[span[text()='Nhận Thưởng']]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnClaim);
            btnClaim.Click();
            Thread.Sleep(3000);
        }

        //Test 4: Truy cập trang Mission -> Nhận tất cả phần thưởng 
        [Test]
        public void ReceiveAll()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            Login();
            Assert.That(driver.Url, Does.Contain(devUrl));
            Thread.Sleep(2000);

            /* // Bấm nút "Nhận tất cả"
            IWebElement btnClaim = driver.FindElement(By.XPath("//button[span[text()='Nhận Tất cả']]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnClaim);
            btnClaim.Click();
            Thread.Sleep(4000); 

            
            IWebElement btnReward = driver.FindElement(By.XPath("//button[span[text()='Nhận thưởng']]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnReward);
            Thread.Sleep(2000); // Chờ một chút trước khi click
            btnReward.Click();
            Thread.Sleep(4000); */

            // Bấm sang tab "Hàng tuần"
            IWebElement btnWeek = driver.FindElement(By.XPath("//button[.//div[contains(text(), 'Hàng tuần')]]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnWeek);
            Thread.Sleep(2000);
            btnWeek.Click();
            Thread.Sleep(2000);

            // Bấm lại "Nhận tất cả" trong tab "Hàng tuần"
            IWebElement btnClaimAgain = driver.FindElement(By.XPath("//button[@class='group/btn items-center justify-center gap-2 whitespace-nowrap font-medium ring-offset-white transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 hover:bg-gray-100 hover:text-dark h-10 w-fit py-0 text-base hidden lg:block px-[40px] rounded-[5px] border-[2px] border-white text-white']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnClaimAgain);
            Thread.Sleep(2000);
            btnClaimAgain.Click();

            Thread.Sleep(2000);
        }

        //Test 5: Truy cập trang Mission -> Nhận từng phần thưởng 
        [Test]
        public void ReceiveIndividualReward()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            Login();
            Assert.That(driver.Url, Does.Contain(devUrl));
            Thread.Sleep(2000);

            // Bấm sang tab "Hàng tuần"
            IWebElement btnWeek = driver.FindElement(By.XPath("//button[.//div[contains(text(), 'Hàng tuần')]]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnWeek);
            Thread.Sleep(2000);
            btnWeek.Click();
            Thread.Sleep(2000);

            IWebElement btnClaim = driver.FindElement(By.XPath("//button[span[text()='Nhận Thưởng']]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnClaim);
            Thread.Sleep(2000);
            btnClaim.Click();

            Thread.Sleep(2000);
        }

        private void Login()
        {
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys(email);

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys(password);

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