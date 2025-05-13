using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace TestCompa.Server.CourseBuilder.HTML
{
    [TestFixture]
    [Category("Element")]
    public class HTML
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

        public void StudioTest()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[.//span[text()='Course']]")));
            course.Click();

            Thread.Sleep(5000);
        }
        [Test]
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

        //Test 1: Test HTML (UC-S167)
        [Test]
        public void MarkdownDragNDrop()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));

            //truy cập vào CourseTest
            CourseBuilder();
            //Chọn Chapter: HTML Block 
            IWebElement title = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//p[starts-with(normalize-space(), 'HTMLBlock')]")));
            title.Click();
            //Chọn tab Element 
            IWebElement elementTab = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]")));
            elementTab.Click();
            //Kéo thả HTML Block vào trang 
            IWebElement textElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[img[@alt='HTML Block Icon']]")));

            IWebElement targetElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]")));

            Actions actions = new(driver);
            actions.DragAndDrop(textElement, targetElement).Perform();

            Thread.Sleep(3000);
        }
        //Test 2: Add content (UC-S168)
        [Test]
        public void AddContent()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            //Kéo thả HTMLBlock vào
            MarkdownDragNDrop();
            //Chọn block vừa kéo vào
            IWebElement click = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='relative ']")));
            click.Click();
            //Chọn vào vùng text để nhập liệu 
            IWebElement txt = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='cm-line']")));
            txt.Click();
            //Nhập content
            txt.SendKeys("Add Content");
            Thread.Sleep(2000);
        }
        //Test 3: Add code HTML 
        [Test]
        public void AddCodeContent()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            //Kéo thả HTMLBlock vào
            MarkdownDragNDrop();
            //Chọn block vừa kéo vào
            IWebElement click = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='relative ']")));
            click.Click();
            //Chọn vào vùng text để nhập liệu 
            IWebElement txt = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='cm-line']")));
            txt.Click();
            //Nhập content
            txt.SendKeys("<b>CONTENT</b>");
            Thread.Sleep(2000);


        }
        //click vào các btn Panel trên thanh  
        [Test]
        public void BtnPanel()
        {
            AddContent();

            // Tìm tất cả các button trong div có class cụ thể
            IReadOnlyCollection<IWebElement> panelButtons = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(
                By.XPath("//div[@class='flex items-center gap-3 p-2 border-t border-x']//button")));

            // Ép về danh sách để truy cập theo chỉ số
            var buttonList = panelButtons.ToList();

            // Nhấn vào tối đa 3 nút đầu tiên
            for (int i = 0; i < Math.Min(3, buttonList.Count); i++)
            {
                IWebElement button = buttonList[i];
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", button);
                Thread.Sleep(500);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", button);
                Thread.Sleep(1000);
            }
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
