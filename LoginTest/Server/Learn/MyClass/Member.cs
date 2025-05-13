using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Learn.MyClassMember
{
    public class ClassTests
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string homeUrl = "http://10.10.10.30/learn/home";


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
            wait = new(driver, TimeSpan.FromSeconds(10));
        }

        [SetUp]
        public void Setup()
        {
            // Gọi InitDriver với tham số headless = false (mặc định)
            // Thay đổi thành true nếu muốn chạy ở chế độ headless
            InitDriver(true);
            driver.Navigate().GoToUrl("http://10.10.10.30/Auth/SignIn");
        }
        // 1. Test chức năng điều hướng tới Member


        [Test, Order(1)]
        public void memberClass()
        {
            InitDriver(false);
            driver.Navigate().GoToUrl(homeUrl);
            Login();


            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(3000);
            IWebElement testclass = driver.FindElement(By.XPath("//a[contains(text(),'ClassTest')]"));
            ScrollToElement(testclass);
            testclass.Click();

            Thread.Sleep(2000);

            IWebElement member = driver.FindElement(By.XPath("//a[text()='Thành viên']"));
            member.Click();
            Thread.Sleep(2000);

        }
        [Test, Order(2)]
        public void memberList()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Login();
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(3000);


            IWebElement testclass = driver.FindElement(By.XPath("//a[contains(text(),'ClassTest')]"));
            ScrollToElement(testclass);
            Thread.Sleep(4000);
            testclass.Click();
            Thread.Sleep(5000);
            IWebElement member = driver.FindElement(By.XPath("//a[text()='Thành viên']"));
            member.Click();
            Thread.Sleep(5000);
            IWebElement hocVienButton = driver.FindElement(By.CssSelector("button.flex.items-center.gap-2"));
            hocVienButton.Click();
            Thread.Sleep(3000);
        }
        [Test, Order(3)]
        public void hoverCard()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Login();
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(3000);
            IWebElement testclass = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Test')]"));
            ScrollToElement(testclass);
            testclass.Click();
            Thread.Sleep(5000);
            IWebElement member = driver.FindElement(By.XPath("//a[text()='Thành viên']"));
            member.Click();
            Thread.Sleep(5000);
            //Hover
            Actions action = new(driver);
            IList<IWebElement> profileElements = driver.FindElements(By.CssSelector("div.col-span-2.flex.gap-2.items-center a[href*='/learn/public-profile']"));
            foreach (var profile in profileElements)
            {
                action.MoveToElement(profile).Perform();
                Thread.Sleep(500);
            }


            Thread.Sleep(5000);
        }

        public void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});", element);
        }
        public void Login()
        {
            Thread.Sleep(5000);
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
