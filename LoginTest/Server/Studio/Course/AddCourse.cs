/*using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Studio
{
    public class AddCourse
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
            Thread.Sleep(3000);
            IWebElement course = driver.FindElement(By.XPath("//button[.//span[text()='Course']]"));
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
        //Test 3: Không nhập name
        [Test]
        public void createNewCoursewithoutName()
        {
            btnNewCourse();
            IWebElement btnCreate = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > button:nth-child(1)"));
            btnCreate.Click();
            Thread.Sleep(5000);
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
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'Name must be at least 2 characters.')]")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập sai thiếu thông tin!");
        }
        //Test 4: Không nhập thumbnail
        [Test]
        public void createNewCoursewithoutThumbnail()
        {
            btnNewCourse();
            IWebElement btnCreate = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > button:nth-child(1)"));
            btnCreate.Click();
            Thread.Sleep(5000);
            IWebElement name = driver.FindElement(By.XPath("//input[@name='name']"));
            name.SendKeys("Test Course 123");
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
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'Please enter a valid URL for the thumbnail.')]")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập sai thiếu thông tin!");
        }
        //Test 5: Không nhập descrition
        [Test]
        public void createNewCoursewithoutDescription()
        {
            btnNewCourse();
            IWebElement btnCreate = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > button:nth-child(1)"));
            btnCreate.Click();
            Thread.Sleep(5000);
            IWebElement name = driver.FindElement(By.XPath("//input[@name='name']"));
            name.SendKeys("Test Course 123");
            IWebElement thumbnailUrl = driver.FindElement(By.XPath("//input[@name='thumbnail']"));
            thumbnailUrl.SendKeys("https://picsum.photos/200/300");
            IWebElement price = driver.FindElement(By.XPath("//input[@name='price']"));
            price.SendKeys("2500");
            IWebElement hours = driver.FindElement(By.XPath("//input[@name='estimateHours']"));
            hours.SendKeys("2");
            IWebElement bannerUrl = driver.FindElement(By.XPath("//input[@name='banner']"));
            bannerUrl.SendKeys("https://picsum.photos/id/870/200/300?grayscale&blur=2");
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(5000);
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'Description must be at least 10 characters.')]")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập sai thiếu thông tin!");
        }
        //Test 6: Không nhập estimateHour
        [Test]
        public void createNewCoursewithoutEstimatedHours()
        {
            btnNewCourse();
            IWebElement btnCreate = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > button:nth-child(1)"));
            btnCreate.Click();
            Thread.Sleep(5000);
            IWebElement name = driver.FindElement(By.XPath("//input[@name='name']"));
            name.SendKeys("Test Course 123");
            IWebElement thumbnailUrl = driver.FindElement(By.XPath("//input[@name='thumbnail']"));
            thumbnailUrl.SendKeys("https://picsum.photos/200/300");
            IWebElement price = driver.FindElement(By.XPath("//input[@name='price']"));
            price.SendKeys("2500");
            IWebElement description = driver.FindElement(By.XPath("//textarea[@name='description']"));
            description.SendKeys("Course Description");
            IWebElement bannerUrl = driver.FindElement(By.XPath("//input[@name='banner']"));
            bannerUrl.SendKeys("https://picsum.photos/id/870/200/300?grayscale&blur=2");
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(5000);
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'Estimated hours must be at least 1.')]")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập sai thiếu thông tin!");
        }
        //Test 7: Không nhập banner
        [Test]
        public void createNewCoursewithoutBanner()
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

            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(5000);

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'Please enter a valid URL for the banner.')]")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập sai thiếu thông tin!");
        }
        //Test 8: Nhập name quá dài
        [Test]
        public void courseNameTooLong()
        {
            btnNewCourse();
            IWebElement btnCreate = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > button:nth-child(1)"));
            btnCreate.Click();
            Thread.Sleep(5000);
            IWebElement name = driver.FindElement(By.XPath("//input[@name='name']"));
            string longname = new('a', 150);
            name.SendKeys(longname);
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

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'Name must be at least 2 and at max 50 characters long.')]")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập sai thiếu thông tin!");
            Thread.Sleep(5000);
        }



        public void Login()
        {
            Thread.Sleep(2000);
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
*/