using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.Learn.Posts
{
    [TestFixture]
    [Category("Class")]
    public class ClassTests
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string homeUrl = "https://compaclass.com/learn/home";


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
            driver.Navigate().GoToUrl("https://auth.compaclass.com/Auth/SignIn");
        }
        // 1. Test chức năng điều hướng tới Post
        [Test, Order(1)]
        public void Posts()
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
            IWebElement testclass = driver.FindElement(By.XPath("//a[contains(text(),'Class Test')]"));
            testclass.Click();
            Thread.Sleep(5000);
            IWebElement assign = driver.FindElement(By.XPath("//a[text()='Bài đăng']"));
            assign.Click();
            Thread.Sleep(2000);




        }
        //2. Thêm Post
        [Test, Order(2)]
        public void addPost()
        {
            Posts();
            IWebElement addPost = driver.FindElement(By.CssSelector("button.bg-primary.rounded-2xl.flex.items-center.text-white"));
            addPost.Click();
            Thread.Sleep(5000);
            IWebElement Title = driver.FindElement(By.CssSelector("textarea[placeholder='Chủ đề của bài đăng']"));
            Title.SendKeys("Title 12345");
            Thread.Sleep(2000);
            IWebElement Content = driver.FindElement(By.CssSelector("textarea[placeholder='Nội dung của bài đăng']"));
            Content.SendKeys("abcxyz123456");
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(2000);
        }
        //3. Sửa Post
        [Test, Order(3)]
        public void editPosts()
        {
            addPost();
            Thread.Sleep(3000);
            IWebElement svg = driver.FindElement(By.CssSelector("button.outline-none.cursor-pointer.group"));
            svg.Click();
            Thread.Sleep(2000);
            IWebElement edit = driver.FindElement(By.XPath("//button[span[text()='Chỉnh sửa bài đăng']]"));
            edit.Click();
            IWebElement Title = driver.FindElement(By.CssSelector("textarea[placeholder='Chủ đề của bài đăng']"));
            Title.Click();
            Title.Clear();
            Title.SendKeys("New Title!");
            Thread.Sleep(2000);
            IWebElement Content = driver.FindElement(By.CssSelector("textarea[placeholder='Nội dung của bài đăng']"));
            Content.Click();
            Content.Clear();
            Content.SendKeys("New Content!");
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(2000);



        }
        //4. Xóa bài đăng
        [Test, Order(4)]
        public void deletePosts()
        {
            addPost();
            Thread.Sleep(3000);
            IWebElement svg = driver.FindElement(By.CssSelector("button.outline-none.cursor-pointer.group"));
            svg.Click();
            Thread.Sleep(2000);
            IWebElement delete = driver.FindElement(By.XPath("//button[span[text()='Xóa bài đăng']]"));
            delete.Click();
            Thread.Sleep(4000);
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Xóa']"));
            confirm.Click();

            Thread.Sleep(2000);

        }
        //5.React bài đăng
        [Test, Order(5)]
        public void reactPosts()
        {
            addPost();
            IWebElement react = driver.FindElement(By.CssSelector("button.flex.items-center.gap-2.group"));
            react.Click();
            Thread.Sleep(5000);
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
