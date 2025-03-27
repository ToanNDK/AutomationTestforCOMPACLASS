using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;


namespace TestCompa.Production.Learn
{
    public class Mission
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "https://compaclass.com/learn/mission";
        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        }

        //Test 1: Kiểm tra truy cập trang mission khi chưa đăng nhập -> Chuyển về trang đăng nhập
        [Test]
        public void accessMissionWithoutLogin()
        {
            driver.Navigate().GoToUrl(devUrl);

            string currentUrl = driver.Url;

            Assert.IsTrue(currentUrl.Contains("https://auth.compaclass.com/Auth/SignIn"));
        }
        //Test 2: Kiểm tra truy cập trang mission khi đã đăng nhập -> Chuyển về trang Mission
        [Test]
        public void accessMissionLogin()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            string currentUrl = driver.Url;
            Assert.IsTrue(currentUrl.Contains("https://auth.compaclass.com/Auth/SignIn"));
            Login();
            Assert.IsTrue(driver.Url.Contains(devUrl));
        }

        // Test 3: Truy cập trang mission -> Nhận phần thưởng 
        [Test]
        public void recievedMission()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            Login();
            Assert.IsTrue(driver.Url.Contains(devUrl));
            Thread.Sleep(15000);

            IWebElement btnClaim = driver.FindElement(By.XPath("//button[span[text()='Nhận Thưởng']]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnClaim);
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
        public void recevied()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            Login();
            Assert.IsTrue(driver.Url.Contains(devUrl));
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

        public void TearDown()
        {
            driver.Quit();
        }
    }
}
