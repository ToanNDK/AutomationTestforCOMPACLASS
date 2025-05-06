using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Learn.Home.UserSetting
{

    public class UserSetting
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "http://10.10.10.30/learn/user-setting?tab=notification";
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
            InitDriver(false);
        }
        [Test]
        public void InfomationUser()
        {

            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement info = driver.FindElement(By.XPath("//button[.//h2[text()='Thông tin hồ sơ']]"));
            info.Click();
            Thread.Sleep(1000);
        }

        //Change title, Skill, About me, 
        [Test]
        public void ChangeText()
        {
            InfomationUser();

            IWebElement title = driver.FindElement(By.XPath("//input[@placeholder='Tiêu đề hiển thị của bạn']"));
            title.SendKeys("Title");
            Thread.Sleep(500);

            IWebElement skill = driver.FindElement(By.XPath("//input[@placeholder='Thêm kỹ năng']"));
            skill.SendKeys("Skill");
            Thread.Sleep(500);

            skill.SendKeys(Keys.Enter);
            Thread.Sleep(500);

            IWebElement aboutme = driver.FindElement(By.XPath("//textarea[@placeholder='Giới thiệu về bản thân']"));
            aboutme.SendKeys("Aboutme");
            Thread.Sleep(500);
        }

        // change telephone
        [Test]
        public void ChangeTelephone()
        {
            InfomationUser();
            IWebElement sdt = driver.FindElement(By.XPath("//input[@placeholder='Số điện thoại']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", sdt);
            Thread.Sleep(1000);
            sdt.SendKeys("08181891");
            Thread.Sleep(1000);
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//span[@class='text-red-d10 text-sm text-red']")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập sai định dạng sdt!");
            sdt.SendKeys("23");
            Thread.Sleep(1000);
        }
        //bật các toggle 
        [Test]
        public void Toggle()
        {
            InfomationUser();
            // Lấy tất cả các toggle button dựa vào class chung
            var toggles = driver.FindElements(By.XPath("//button[contains(@class, 'switch')]"));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            for (int i = 0; i < toggles.Count; i++)
            {
                try
                {
                    var toggle = toggles[i];

                    // Scroll đến toggle (tránh bị ẩn)
                    js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", toggle);
                    Thread.Sleep(500); // chờ animation scroll

                    // Click vào toggle
                    toggle.Click();
                    Thread.Sleep(200);
                }
                catch (StaleElementReferenceException)
                {
                    toggles = driver.FindElements(By.XPath("//button[contains(@class, 'switch')]"));
                    i--; // 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Không click được toggle thứ {i + 1}: {ex.Message}");
                }
            }
        }
        //Order

        [Test]
        public void CheckOrder()
        {

            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement info = driver.FindElement(By.XPath("//button[.//h2[text()='Giao dịch của tôi']]"));
            info.Click();
            Thread.Sleep(1000);
        }
        public void Login()
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
