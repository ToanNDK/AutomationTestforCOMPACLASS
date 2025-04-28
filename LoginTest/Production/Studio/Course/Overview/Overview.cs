using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace TestCompa.Production.CourseBuilder.Overview
{
    [TestFixture]
    [Category("Login")]
    public class Overview
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string StudioUrl = "https://studio.compaclass.com";

        private IWebElement WaitUntilVisible(By locator, int timeoutSeconds = 10)
        {
            var localWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            return localWait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        private IWebElement WaitUntilClickable(By locator, int timeoutSeconds = 10)
        {
            var localWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            return localWait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

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
            InitDriver(true);
        }

        public void StudioTest()
        {
            driver.Navigate().GoToUrl(StudioUrl);
            Login();
            var course = WaitUntilClickable(By.XPath("//button[.//span[text()='Course']]"));
            course.Click();
            WaitUntilVisible(By.XPath("//button[normalize-space()='1']"));
        }

        public void CourseBuilder()
        {
            StudioTest();
            var tab = WaitUntilClickable(By.XPath("//button[normalize-space()='1']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", tab);
            tab.Click();

            string expectedText = "CourseTest";
            var h2Element = WaitUntilClickable(By.XPath($"//h3[contains(text(),'{expectedText}')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", h2Element);
            h2Element.Click();
        }

        [Test]
        public void AddDescription()
        {
            CourseBuilder();
            var description = WaitUntilVisible(By.XPath("//textarea[@id='description']"));
            description.Clear();
            description.SendKeys("Description Test");
        }

        [Test]
        public void AddInfomation()
        {
            CourseBuilder();
            var info = WaitUntilVisible(By.XPath("//input[@class='flex w-full bg-white rounded-md border ring-offset-white border-gray-l20 file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-darkGray focus-visible:outline-none focus-visible:ring-0 disabled:cursor-not-allowed disabled:opacity-50 focus-visible:border-primary-l40 h-10 px-3 py-2 text-sm flex-1 opacity-70']"));
            info.SendKeys("INFOMATION Test");
        }

        [Test]
        public void DeleteInfomation()
        {
            AddInfomation();
            var delete = WaitUntilClickable(By.CssSelector("button:has(svg.lucide.lucide-minus.text-red)"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", delete);
            delete.Click();
        }

        [Test]
        public void AboutTheCourse()
        {
            CourseBuilder();
            var ckeditor = WaitUntilVisible(By.CssSelector("div.ck-editor__editable"));
            ckeditor.Clear();
            ckeditor.SendKeys("INFOMATION Test");
        }

        [Test]
        public void HeadingAboutTheCourse()
        {
            AboutTheCourse();
            var heading = WaitUntilClickable(By.XPath("//button[@role='combobox' and @type='button']"));
            heading.Click();
            var headingOptions = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("span.flex-1")));
            var random = new Random();
            headingOptions[random.Next(headingOptions.Count)].Click();
        }

        [Test]
        public void TextFormat()
        {
            AboutTheCourse();
            var ckeditor = WaitUntilVisible(By.CssSelector("div.ck-editor__editable"));
            ckeditor.SendKeys(Keys.Control + 'a');
            ckeditor.SendKeys(Keys.Control + 'b');
            ckeditor.SendKeys(Keys.Control + 'u');
            ckeditor.SendKeys(Keys.Control + 'i');
            var strikethrough = WaitUntilClickable(By.CssSelector(".lucide.lucide-strikethrough"));
            strikethrough.Click();
        }

        [Test]
        public void Quote()
        {
            AboutTheCourse();
            var quote = WaitUntilClickable(By.CssSelector(".lucide.lucide-quote"));
            quote.Click();
            var list = WaitUntilClickable(By.CssSelector(".lucide.lucide-list"));
            list.Click();
        }

        [Test]
        public void AddTeacher()
        {
            CourseBuilder();
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            var add = WaitUntilClickable(By.CssSelector("svg.lucide.lucide-plus.text-primary"));
            add.Click();
            var teacherEmail = WaitUntilVisible(By.XPath("//input[@placeholder='Email address or name']"));
            teacherEmail.SendKeys("lozy564");
            var confirm = WaitUntilClickable(By.XPath("//div[contains(@class,'flex flex-col text-sm')]"));
            confirm.Click();
        }

        [Test]
        public void ProfileTeacher()
        {
            AddTeacher();
            string originalTab = driver.CurrentWindowHandle;
            var profile = WaitUntilClickable(By.XPath("//a[normalize-space()='TeacherAssistant']"));
            profile.Click();
            wait.Until(d => d.WindowHandles.Count > 1);
            foreach (var tab in driver.WindowHandles)
            {
                if (tab != originalTab)
                {
                    driver.SwitchTo().Window(tab);
                    break;
                }
            }
            wait.Until(d => d.Url.Contains("compaclass.com/learn/public-profile"));
            Assert.That(driver.Url.StartsWith("https://compaclass.com/learn/public-profile"), "URL is not correct after switching to profile tab.");
            driver.Close();
            driver.SwitchTo().Window(originalTab);
        }

        public void Login()
        {
            var emailInput = WaitUntilVisible(By.Id("email"));
            emailInput.SendKeys("info@kpim.vn");

            var passwordInput = WaitUntilVisible(By.Id("password"));
            passwordInput.SendKeys("KPIM@123");

            var loginButton = WaitUntilClickable(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
