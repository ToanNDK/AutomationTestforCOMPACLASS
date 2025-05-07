using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.CourseBuilder.Overview
{
    public class CourseOverview
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string StudioUrl = "http://10.10.10.30:3000/";
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

        public void StudioTest()
        {
            driver.Navigate().GoToUrl(StudioUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[.//span[text()='Course']]")));
            course.Click();

            Thread.Sleep(2000);
        }


        public void CourseBuilder()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));

            // chọn Course test
            StudioTest();

            // Đợi nút tab "1" hiển thị và scroll đến
            IWebElement tab = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("//button[normalize-space()='1']")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", tab);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(tab)).Click();

            // Đợi tiêu đề khoá học hiển thị
            string expectedText = "CourseTest";
            IWebElement h2Element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath($"//h3[contains(text(),'{expectedText}')]")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", h2Element);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(h2Element)).Click();
        }

        //Test 1: Add Description
        [Test]
        public void AddDescription()
        {

            CourseBuilder();
            IWebElement description = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//textarea[@id='description']")));
            description.Clear();
            description.SendKeys("Description Test");
            Thread.Sleep(2000);
        }
        //Test 2 : Add Infomation
        [Test]
        public void AddInfomation()
        {
            CourseBuilder();
            IWebElement info = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//input[@class='flex w-full bg-white rounded-md border ring-offset-white border-gray-l20 file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-darkGray focus-visible:outline-none focus-visible:ring-0 disabled:cursor-not-allowed disabled:opacity-50 focus-visible:border-primary-l40 h-10 px-3 py-2 text-sm flex-1 opacity-70']")));
            info.SendKeys("INFOMATION Test");
            Thread.Sleep(1000);
        }

        //Test 3: About the course 
        [Test]
        public void AboutTheCourse()
        {
            CourseBuilder();
            //nhập content vào ckeditor
            IWebElement ckeditor = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div.ck-editor__editable")));
            ckeditor.Clear();
            ckeditor.SendKeys("INFOMATION Test");
            Thread.Sleep(1000);
        }
        //Test 3.1 About the course (Heading) 
        [Test]
        public void HeadingAboutTheCourse()
        {
            AboutTheCourse();
            Thread.Sleep(2000);
            IWebElement heading = driver.FindElement(By.XPath("//button[@role='combobox' and @type='button']"));
            heading.Click();
            Thread.Sleep(2000);
            IList<IWebElement> headingOptions = driver.FindElements(By.CssSelector("span.flex-1"));

            Random random = new();
            int randomIndex = random.Next(headingOptions.Count);

            IWebElement randomHeading = headingOptions[randomIndex];
            randomHeading.Click();

        }
        //Test 3.2 Bold, Italic, Underline, Strikethrough 
        [Test]
        public void TextFormat()
        {
            AboutTheCourse();
            IWebElement ckeditor = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div.ck-editor__editable")));
            ckeditor.SendKeys(Keys.Control + 'a');
            Thread.Sleep(1000);
            ckeditor.SendKeys(Keys.Control + 'b');
            Thread.Sleep(1000);
            ckeditor.SendKeys(Keys.Control + 'u');
            Thread.Sleep(1000);
            ckeditor.SendKeys(Keys.Control + 'i');
            Thread.Sleep(1000);
            IWebElement strikethrough = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".lucide.lucide-strikethrough")));
            strikethrough.Click();
            Thread.Sleep(2000);
        }
        //Test 4: Quote 
        [Test]
        public void Quote()
        {
            AboutTheCourse();
            IWebElement Quote = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".lucide.lucide-quote")));
            Quote.Click();
            IWebElement List = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".lucide.lucide-list")));
            List.Click();
            Thread.Sleep(2000);
        }
        //Test 5: Add Teacher 
        [Test]
        public void AddTeacher()
        {
            CourseBuilder();
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(2000);
            IWebElement add = driver.FindElement(By.CssSelector("svg.lucide.lucide-plus.text-primary"));
            add.Click();
            Thread.Sleep(2000);

            IWebElement teacherEmail = driver.FindElement(By.XPath("//input[@placeholder='Email address or name']"));
            teacherEmail.SendKeys("lozy564");
            Thread.Sleep(1000);
            IWebElement confirm = driver.FindElement(By.XPath("//div[contains(@class,'flex flex-col text-sm')]"));
            confirm.Click();
            Thread.Sleep(2000);

        }

        public void Login()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            //IWebElement emailInput = driver.FindElement(By.Id("email"));
            IWebElement emailInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("email")));
            emailInput.SendKeys("info@kpim.vn");

            //IWebElement passwordInput = driver.FindElement(By.Id("password"));
            IWebElement passwordInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("password")));
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
