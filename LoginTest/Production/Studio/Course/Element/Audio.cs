using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.CourseBuilder.Audio
{
    [TestFixture]
    [Category("Element")]
    public class Audio
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "https://studio.compaclass.com/";
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
        //Test 1: Test PDF (UC-S)
        [Test]
        public void PDFDragNDrop()
        {
            courseBuilder();
            IWebElement title = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//p[starts-with(normalize-space(), 'AudioBlock')]")));
            title.Click();
            IWebElement elementTab = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]")));
            elementTab.Click();

            IWebElement textElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[img[@alt='Audio Block Icon']]")));

            IWebElement targetElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]")));

            Actions actions = new(driver);
            actions.DragAndDrop(textElement, targetElement).Perform();

            Thread.Sleep(2000);
        }
        //Test 2: Add content (UC-S)
        [Test]
        public void addContent()
        {
            PDFDragNDrop();

            IWebElement addPdf = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[.//p[text()='Click to add audio']]")));
            addPdf.Click();

            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            inputFile.SendKeys(@"C:\Users\Hello\Videos\Screen Recordings\Screen Recording 2025-04-26 123449.mp4");
            Thread.Sleep(1000);
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
