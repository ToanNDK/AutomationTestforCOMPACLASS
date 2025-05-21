using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Class.Setting
{
    public class Setting
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
        public void Setup()
        {
            InitDriver(true);
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        private void Login()
        {
            Thread.Sleep(2000);
            IWebElement emailInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("email")));
            emailInput.SendKeys("info@kpim.vn");

            IWebElement passwordInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("password")));
            passwordInput.SendKeys("KPIM@123");

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();
        }
        //Chuyển tới trang Class
        [Test]
        public void NavigateToClass()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();

            IWebElement classes = wait.Until(d => d.FindElement(By.XPath("//a[normalize-space()='Class']")));
            classes.Click();
            Thread.Sleep(500);

        }

        //Vào class
        [Test]
        public void ClassTest()
        {

            NavigateToClass();
            IWebElement testclass = wait.Until(d => d.FindElement(By.XPath("//h3[normalize-space()='Test']")));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", testclass);
            Thread.Sleep(500);
            testclass.Click();

            Thread.Sleep(1000);
            //Nhấn 3 chấm góc phải
            IWebElement edit = driver.FindElement(By.XPath("//button[@aria-haspopup='listbox' and contains(@class, 'cursor-pointer')]"));
            edit.Click();
            Thread.Sleep(500);

            //Chọn Setting
            IWebElement setting = driver.FindElement(By.XPath("//span[contains(text(),'Class Setting')]"));
            setting.Click();
            Thread.Sleep(500);
        }
        //Thay đổi Thumbnail
        [Test]
        public void ChangeThumbnail()
        {

            ClassTest();
            //Thumbnail web
            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));

            string imgPath = @"C:\Users\Hello\Pictures\TestImage\loginBG.jpg";
            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(imgPath);
            Thread.Sleep(1000);

            //Thumbnail mobile
            IWebElement inputFileThumbnail = driver.FindElement(By.XPath("(//input[@type='file'])[2]"));
            string imgMobileThumbnail = @"C:\Users\Hello\Pictures\TestImage\loginBG.jpg";
            inputFileThumbnail.SendKeys(imgMobileThumbnail);

            Thread.Sleep(1000);
        }

        //đổi slug
        [Test]
        public void ChangeSlug()
        {

            Random rd = new();

            int random = rd.Next(1000, 9999);
            ClassTest();

            IWebElement slug = driver.FindElement(By.XPath("//input[@placeholder='Enter class Slug']"));
            slug.Click();
            slug.SendKeys(Keys.Control + 'a');
            slug.SendKeys(Keys.Backspace);
            Thread.Sleep(500);
            slug.SendKeys($"class-slug-{random}");
            Thread.Sleep(500);

            //Save
            IWebElement save = wait.Until(d => d.FindElement(By.XPath("//button[normalize-space()='Save']")));
            save.Click();
            Thread.Sleep(2000);
        }

        //Số lượng ng đăng ký
        [Test]
        public void Registrations()
        {
            Random rd = new();

            int random = rd.Next(10, 100);
            ClassTest();
            //min
            IWebElement minimum = driver.FindElement(By.XPath("//input[@placeholder='Enter Minimum Registrations']"));
            minimum.Click();
            minimum.SendKeys(Keys.Control + 'a');
            minimum.SendKeys(Keys.Backspace);
            Thread.Sleep(500);
            minimum.SendKeys($"{random}");
            Thread.Sleep(500);
            //max
            IWebElement maximum = driver.FindElement(By.XPath("//input[@placeholder='Enter Maximum Registrations']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", maximum);
            Thread.Sleep(500);
            maximum.Click();
            maximum.SendKeys(Keys.Control + 'a');
            maximum.SendKeys(Keys.Backspace);
            Thread.Sleep(500);
            maximum.SendKeys($"{random}");
            Thread.Sleep(500);

            //Save
            IWebElement save = wait.Until(d => d.FindElement(By.XPath("//button[normalize-space()='Save']")));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", save);
            Thread.Sleep(500);
            save.Click();
            Thread.Sleep(2000);
        }
        //Enrollment Start
        [Test]
        public void EnrollmentStart()
        {

            ClassTest();
            IWebElement enrollmentStartButton = driver.FindElement(By.XPath("//label[text()='Enrollment Start']/ancestor::div[contains(@class, 'flex')]/following-sibling::button"));
            enrollmentStartButton.Click();


            Thread.Sleep(500);
            Random rd = new Random();
            int randomDay = rd.Next(10, 29); // từ ngày 10 đến 28

            // Click vào ngày ngẫu nhiên đó
            IWebElement dayButton = driver.FindElement(By.XPath($"//button[text()='{randomDay}']"));
            dayButton.Click();
            Thread.Sleep(1000);

            IWebElement save = wait.Until(d => d.FindElement(By.XPath("//button[normalize-space()='Save']")));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", save);
            Thread.Sleep(500);
            save.Click();
            Thread.Sleep(2000);
        }

        //Enrollment End 
        [Test]
        public void EnrollmentEnd()
        {
            Random rd = new();
            int random = rd.Next(10, 90);
            ClassTest();
            IWebElement certainDays = wait.Until(d => d.FindElement(By.XPath("//button[@id='AfterCertainDay']")));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", certainDays);
            Thread.Sleep(500);
            certainDays.Click();
            Thread.Sleep(500);

            IWebElement inputDays = driver.FindElement(By.XPath("(//input[@type='number'])[3]"));
            inputDays.SendKeys(Keys.Control + 'a');
            inputDays.SendKeys(Keys.Backspace);
            inputDays.SendKeys($"{random}");
            Thread.Sleep(1000);

            IWebElement save = wait.Until(d => d.FindElement(By.XPath("//button[normalize-space()='Save']")));

            js.ExecuteScript("arguments[0].scrollIntoView(true);", save);
            Thread.Sleep(500);
            save.Click();
            Thread.Sleep(2000);
        }

        //Class Start
        [Test]
        public void ClassStart()
        {
            ClassTest();
            IWebElement classStartButton = driver.FindElement(By.XPath("//label[text()='Class Start']/ancestor::div[contains(@class, 'flex')]/following-sibling::button"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", classStartButton);
            Thread.Sleep(500);
            classStartButton.Click();
            Thread.Sleep(500);

            Random rd = new();
            int randomDay = rd.Next(10, 29); // từ ngày 10 đến 28

            // Click vào ngày ngẫu nhiên đó
            IWebElement dayButton = driver.FindElement(By.XPath($"//button[text()='{randomDay}']"));
            dayButton.Click();
            Thread.Sleep(1000);
        }
        //Class End
        [Test]
        public void ClassEnd()
        {

            ClassTest();
            IWebElement classStartButton = driver.FindElement(By.XPath("//label[text()='Class End']/ancestor::div[contains(@class, 'flex')]/following-sibling::button"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", classStartButton);
            Thread.Sleep(500);
            classStartButton.Click();
            Thread.Sleep(500);

        }
        //Occurrence 
        [Test]
        public void Occurrence()
        {
            ClassTest();
            IWebElement text = wait.Until(d => d.FindElement(By.XPath("//input[@placeholder='Search available days...']")));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", text);
            Thread.Sleep(500);
            text.Click();
            Thread.Sleep(500);
            text.SendKeys("Friday");
            Thread.Sleep(500);


            IWebElement save = wait.Until(d => d.FindElement(By.XPath("//button[normalize-space()='Save']")));

            js.ExecuteScript("arguments[0].scrollIntoView(true);", save);
            Thread.Sleep(500);
            save.Click();
            Thread.Sleep(2000);
        }
        //Toggle QNA, Feedback
        [Test]
        public void ToggleTest()
        {
            InitDriver(false);
            ClassTest();
            IWebElement toggleQNA = wait.Until(d => d.FindElement(By.Id("qa")));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", toggleQNA);
            Thread.Sleep(500);
            toggleQNA.Click();

            IWebElement toggleFeedback = wait.Until(d => d.FindElement(By.Id("feedback")));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", toggleFeedback);
            Thread.Sleep(500);
            toggleFeedback.Click();

            IWebElement save = wait.Until(d => d.FindElement(By.XPath("//button[normalize-space()='Save']")));

            js.ExecuteScript("arguments[0].scrollIntoView(true);", save);
            Thread.Sleep(500);
            save.Click();
            Thread.Sleep(2000);
        }
    }
}