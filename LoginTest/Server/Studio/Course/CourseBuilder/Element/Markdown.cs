using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.CourseBuilder.Markdown
{
    [TestFixture]
    [Category("Element")]
    public class Markdown
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

        public void studioTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = driver.FindElement(By.XPath("//button[.//span[text()='Course']]"));
            course.Click();

            Thread.Sleep(5000);
        }

        public void courseBuilder()
        {
            studioTest();
            IWebElement tab = driver.FindElement(By.XPath("//button[normalize-space()='1']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView(true);", tab);
            Thread.Sleep(3000);
            tab.Click();
            Thread.Sleep(5000);

            string expectedText = "CourseTest";


            IWebElement h2Element = driver.FindElement(By.XPath($"//h3[contains(text(),'{expectedText}')]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", h2Element);
            Thread.Sleep(3000);
            h2Element.Click();

            Thread.Sleep(3000);
        }
        //Test 1: Test Markdown (UC-S164)
        [Test]
        public void markdownDragNDrop()
        {
            courseBuilder();
            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'MarkdownBlock')]"));
            title.Click();
            Thread.Sleep(1000);
            IWebElement elementTab = driver.FindElement(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]"));
            elementTab.Click();
            Thread.Sleep(3000);

            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Markdown Block Icon']]"));

            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            Actions actions = new(driver);
            actions.DragAndDrop(textElement, targetElement).Perform();

            Thread.Sleep(4000);
        }
        //Test 2: Add content (UC-S165)
        [Test]
        public void addContent()
        {
            markdownDragNDrop();
            IWebElement click = driver.FindElement(By.XPath("//div[@class='relative ']"));
            click.Click();
            Thread.Sleep(2000);
            IWebElement txt = driver.FindElement(By.XPath("//textarea[@placeholder='Enter Markdown text...']"));
            txt.Click();
            Thread.Sleep(2000);
            txt.SendKeys("Add Content");
            Thread.Sleep(2000);
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
