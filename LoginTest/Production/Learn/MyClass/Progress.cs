using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.MyClassProgress
{
    [TestFixture]
    [Category("LearnClass")]
    public class ClassTests
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string homeUrl = "https://compaclass.com/learn/home";


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
        public void Setup()
        {
            // Gọi InitDriver với tham số headless = false (mặc định)
            // Thay đổi thành true nếu muốn chạy ở chế độ headless
            InitDriver(true);
            driver.Navigate().GoToUrl("https://auth.compaclass.com/Auth/SignIn");
        }
        //  2. Test chức năng điều hướng: Nhấn Tab Overview

        [Test, Order(1)]
        public void testOverTest()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Login();

            // Đợi trang load và click vào nút để chuyển trang
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();  // Nhấn vào để chuyển trang
            Thread.Sleep(3000);
            IWebElement overview = wait.Until(d => d.FindElement(By.XPath("//div[contains(@class,'flex flex-col gap-2 lg:grid sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-3 lg:gap-4 xl:grid-cols-4 2xl:grid-cols-5')]//div[1]//div[1]//a[1]")));
            overview.Click();
            Thread.Sleep(2000);


        }
        //3. Test thông tin chi tiết về mục lục nội dung học, giới thiệu lớp học, phần trăm tiến độ...

        [Test, Order(2)]
        public void testScrollbar()
        {
            testOverTest(); // Gọi test trước đó để vào trang chính

            Thread.Sleep(2000); // Đợi trang load hoàn toàn

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
        [Test, Order(3)]
        public void contiuneLearn()
        {
            driver.Navigate().GoToUrl("https://compaclass.com/learn/home");
            Login();
            Assert.That(driver.Url.Contains("https://compaclass.com/learn/home"), Is.True);
            Thread.Sleep(2000);
            IWebElement continueLearn = driver.FindElement(By.XPath("//a[span[text()='Tiếp tục học']]"));
            continueLearn.Click();
            Thread.Sleep(10000);
            Assert.That(driver.Url.Contains("https://compaclass.com/vn/learn/course"), Is.True);
        }

        public void Login()
        {
            Thread.Sleep(2000);
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("info@kpim.vn");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Kpim@2025");

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
