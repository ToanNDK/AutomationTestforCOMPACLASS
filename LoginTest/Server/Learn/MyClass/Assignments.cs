using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Learn.Assignments
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
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [SetUp]
        public void Setup()
        {
            // Gọi InitDriver với tham số headless = false (mặc định)
            // Thay đổi thành true nếu muốn chạy ở chế độ headless
            InitDriver(true);
            driver.Navigate().GoToUrl("http://10.10.10.30/Auth/SignIn");
        }
        // 1. Test chức năng điều hướng tới Assignment

        [Test, Order(1)]
        public void Assignments()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Login();


            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(3000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            Thread.Sleep(5000);
            IWebElement tab = driver.FindElement(By.XPath("//button[normalize-space()='2']"));
            tab.Click();
            Thread.Sleep(2000);
            IWebElement testclass = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Cơ Bản')]"));
            js.ExecuteScript("arguments[0].scrollIntoView(true);", testclass);
            Thread.Sleep(2000);
            testclass.Click();
            Thread.Sleep(5000);
            IWebElement assign = driver.FindElement(By.XPath("//a[text()='Bài tập']"));
            assign.Click();
            Thread.Sleep(2000);




        }
        //2. test điều hướng trang chấm điểm
        [Test, Order(2)]
        public void AssignmentsMark()
        {
            Assignments();
            IWebElement mark = driver.FindElement(By.XPath("//button[text()='Chấm điểm']"));
            mark.Click();
            Thread.Sleep(5000);

        }
        //3. test ghi điểm bài làm
        [Test, Order(3)]
        public void MarkAssignments()
        {
            AssignmentsMark();
            IWebElement student = driver.FindElement(By.XPath("//tbody/tr[1]/td[1]"));
            student.Click();
            Thread.Sleep(5000);
            IWebElement mark = driver.FindElement(By.CssSelector("input[type='number'][min='0'][max='100']"));
            mark.Click();
            mark.Clear();

            Random random = new();
            int randomNumber = random.Next(0, 101);

            mark.SendKeys(randomNumber.ToString());

            IWebElement submit = driver.FindElement(By.XPath("//button[contains(text(),'Lưu')]"));
            submit.Click();
            Thread.Sleep(5000);
        }
        //4. test feedback
        [Test, Order(4)]
        public void feedbackAssignments()
        {
            MarkAssignments();
            Thread.Sleep(5000);
            IWebElement feedback = driver.FindElement(By.XPath("//textarea[@name='feedbackComment' and @maxlength='1000']"));
            feedback.Click();
            feedback.Clear();
            Random rd = new();
            feedback.SendKeys($"Excellent{rd}!");
            Thread.Sleep(5000);
            IWebElement submit = driver.FindElement(By.XPath("//button[contains(text(),'Lưu')]"));
            submit.Click();
            Thread.Sleep(5000);
        }
        //5.Nộp bài tập

        [Test, Order(5)]
        public void submit()
        {
            driver.Navigate().GoToUrl("http://10.10.10.30/learn/class/test-IIPbL/activity/activity-1-untitled-h0W3o");
            Thread.Sleep(5000);
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("lozik480@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Toanking2k3*");

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();
            Thread.Sleep(5000);

            IWebElement inputField = driver.FindElement(By.CssSelector("input#assignment-link"));
            inputField.SendKeys("Link");

            IWebElement fileInput = driver.FindElement(By.Id("file-input"));
            fileInput.SendKeys(@"D:\KPIM\ĐỒ ÁN TT\Thông tin Quản lý Final.pdf");
            Thread.Sleep(3000);
        }
        public void Login()
        {
            Thread.Sleep(5000);
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
