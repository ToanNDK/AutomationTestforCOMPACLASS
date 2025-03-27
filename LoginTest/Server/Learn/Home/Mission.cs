using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;


namespace TestCompa.Server.Learn.Mission
{
    public class Mission
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "http://10.10.10.30/learn/mission";
        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
        }

        //Test 1: Kiểm tra truy cập trang mission khi chưa đăng nhập -> Chuyển về trang đăng nhập
        [Test]
        public void accessMissionWithoutLogin()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            string currentUrl = driver.Url;
            Assert.IsTrue(currentUrl.Contains("http://10.10.10.30/Auth/SignIn"));
        }
        //Test 2: Kiểm tra truy cập trang mission khi đã đăng nhập -> Chuyển về trang Mission
        [Test]
        public void accessMissionLogin()
        {
            driver.Navigate().GoToUrl(devUrl);
            string currentUrl = driver.Url;
            Thread.Sleep(2000);
            Login();
            Assert.IsTrue(driver.Url.Contains(devUrl));
        }

        // Test 3: Truy cập trang mission -> Nhận phần thưởng 
        [Test]
        public void recievedMission()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            Assert.IsTrue(driver.Url.Contains(devUrl));
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
            Assert.IsTrue(driver.Url.Contains(devUrl));

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
