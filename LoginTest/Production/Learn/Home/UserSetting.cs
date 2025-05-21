using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace TestCompa.Production.Learn.Home.UserSetting
{
    [TestFixture]
    [Category("Home")]
    public class UserSetting
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "https://compaclass.com/learn/user-setting?tab=notification";
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

        //View Public-profile
        [Test]
        public void PublicProfile()
        {
            InfomationUser();
            IWebElement viewbtn = driver.FindElement(By.XPath("//a[.//span[text()='Xem hồ sơ công khai']]"));
            viewbtn.Click();
            Thread.Sleep(500);
            Assert.That(driver.Url, Does.Contain("https://compaclass.com/learn/public-profile"));
            Thread.Sleep(1000);
        }
        //Order
        //OrderDetail
        [Test]
        public void CheckOrder()
        {

            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement info = driver.FindElement(By.XPath("//button[.//h2[text()='Giao dịch của tôi']]"));
            info.Click();
            Thread.Sleep(1000);

            IWebElement Id = driver.FindElement(By.XPath("//p[normalize-space()='54eb69d6-aa7a-4e44-ab0a-55828eaadff0']"));
            Id.Click();
            Thread.Sleep(1000);
        }

        //Navigate to Learn 
        [Test]
        public void NavigateToLearn()
        {
            CheckOrder();
            IWebElement navigate = driver.FindElement(By.CssSelector("a[href*='action=continue_learning']"));
            navigate.Click();
            Thread.Sleep(2000);
            Assert.That(driver.Url, Does.Contain("https://compaclass.com/vn/learn/course"));
            Thread.Sleep(2000);
        }


        public void Login()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement emailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("email")));
            emailInput.SendKeys("lozy564@gmail.com");

            IWebElement passwordInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("password")));
            passwordInput.SendKeys("Toanking2k3*");

            IWebElement loginButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text()='SIGN IN']")));
            loginButton.Click();
        }


        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }


}
