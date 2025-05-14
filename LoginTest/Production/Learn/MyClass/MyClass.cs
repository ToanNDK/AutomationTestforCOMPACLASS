using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.Learn.MyClass
{
    [TestFixture]
    [Category("LearnClass")]
    public class ClassTest
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string homeUrl = "https://compaclass.com/vn/academy/kpim";


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


        //Test 1 : Truy cập class trên thanh sidebar
        [Test]
        public void sidebarTest()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Thread.Sleep(3000);
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[contains(text(),'Đăng nhập')]"));
            loginBtn.Click();
            Thread.Sleep(2000);
            Login();
            Assert.That(driver.Url.Contains(homeUrl), Is.True);
            IWebElement testclass = driver.FindElement(By.XPath("//a[@href='/learn/class']"));
            testclass.Click();
            Thread.Sleep(1000);
            Assert.That(driver.Url.Contains("https://compaclass.com/learn/class"), Is.True);
            Thread.Sleep(2000);
        }
        //Test 2 : Truy cập class 
        [Test]
        public void classTest()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Thread.Sleep(3000);
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[contains(text(),'Đăng nhập')]"));
            loginBtn.Click();
            Thread.Sleep(2000);
            Login();
            Assert.That(driver.Url.Contains(homeUrl), Is.True);
            IWebElement testclass = driver.FindElement(By.XPath("//a[@href='/learn/class']"));
            testclass.Click();
            Thread.Sleep(3000);
            Assert.That(driver.Url.Contains("https://compaclass.com/learn/class"), Is.True);
            Thread.Sleep(3000);
            IWebElement classes = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Cơ Bản - DMVN')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, 200);");
            classes.Click();
            Thread.Sleep(3000);
        }

        //Test 3: Bấm các tab trên scroll bar
        [Test]
        public void scrollBarTest()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Thread.Sleep(3000);

            // Đăng nhập và vào lớp học
            driver.FindElement(By.XPath("//button[contains(text(),'Đăng nhập')]")).Click();
            Thread.Sleep(2000);
            Login();
            driver.FindElement(By.XPath("//a[@href='/learn/class']")).Click();
            Thread.Sleep(2000);
            IWebElement classes = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Cơ Bản - DMVN')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", classes);
            Thread.Sleep(4000);

            classes.Click();
            Thread.Sleep(2000);
            IWebElement member = driver.FindElement(By.XPath("//a[normalize-space()='Thành viên']"));
            member.Click();
            Thread.Sleep(2000);
            IWebElement post = driver.FindElement(By.XPath("//a[normalize-space()='Bài đăng']"));
            post.Click();
            Thread.Sleep(2000);
            IWebElement qna = driver.FindElement(By.XPath("//a[normalize-space()='Hỏi & Đáp']"));
            qna.Click();
            Thread.Sleep(2000);
            IWebElement grade = driver.FindElement(By.XPath("//a[normalize-space()='Điểm số']"));
            grade.Click();
            Thread.Sleep(2000);
            IWebElement feedback = driver.FindElement(By.XPath("//a[normalize-space()='Đánh giá']"));
            feedback.Click();
            Thread.Sleep(2000);
            IWebElement certificate = driver.FindElement(By.XPath("//a[normalize-space()='Chứng chỉ']"));
            certificate.Click();
            Thread.Sleep(2000);

        }
        // Test 4: Kiểm tra chuyển đến hồ sơ học viên từ tab "Thành viên"
        [Test]
        public void memberProfileTest()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Thread.Sleep(3000);

            // Đăng nhập và vào lớp học
            driver.FindElement(By.XPath("//button[contains(text(),'Đăng nhập')]")).Click();
            Thread.Sleep(2000);
            Login();
            driver.FindElement(By.XPath("//a[@href='/learn/class']")).Click();
            Thread.Sleep(2000);
            IWebElement classes = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Cơ Bản - DMVN')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(100, 200);");
            Thread.Sleep(2000);

            classes.Click();
            Thread.Sleep(2000);


            IWebElement memberTab = driver.FindElement(By.XPath("//a[normalize-space()='Thành viên']"));
            memberTab.Click();
            Thread.Sleep(2000);

            // Chọn học viên đầu tiên trong danh sách
            IWebElement firstMember = driver.FindElement(By.XPath("//div[contains(@class, 'flex')]//a[contains(@href, '/learn/public-profile')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", firstMember);
            Thread.Sleep(2000);
            firstMember.Click();
            Thread.Sleep(2000);


            Assert.That(driver.Url.Contains("/learn/public-profile/"), Is.True, "Không chuyển đến trang hồ sơ học viên!");



        }
        //Test 5: Click nút "Thêm thành viên" trong tab Thành viên
        [Test]
        public void AddMember()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Thread.Sleep(3000); // Đợi trang load

            // Đăng nhập
            driver.FindElement(By.XPath("//button[contains(text(),'Đăng nhập')]")).Click();
            Thread.Sleep(2000);
            Login();
            driver.FindElement(By.XPath("//a[@href='/learn/class']")).Click();
            Thread.Sleep(2000);

            // Tìm lớp học
            IWebElement classes = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Cơ Bản - DMVN')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", classes);
            Thread.Sleep(2000);
            classes.Click();
            Thread.Sleep(2000);


            IWebElement memberTab = driver.FindElement(By.XPath("//a[normalize-space()='Thành viên']"));
            memberTab.Click();
            Thread.Sleep(2000);

            IWebElement addMember = driver.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > main:nth-child(2) > div:nth-child(1) > div:nth-child(2) > section:nth-child(1) > div:nth-child(1) > button:nth-child(2)"));
            Actions actions = new(driver);
            actions.MoveToElement(addMember).Click().Perform();

            Thread.Sleep(2000);

            IWebElement txtEmail = driver.FindElement(By.XPath("//input[@placeholder='Địa chỉ email...']"));
            txtEmail.SendKeys("lozy564@gmail.com");
            Thread.Sleep(3000);

        }


        // Test 6: Click vào nút SVG và chọn "Xem hồ sơ"
        [Test]
        public void ClickSvgAndViewProfile()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Thread.Sleep(3000);


            driver.FindElement(By.XPath("//button[contains(text(),'Đăng nhập')]")).Click();
            Thread.Sleep(2000);
            Login();
            driver.FindElement(By.XPath("//a[@href='/learn/class']")).Click();
            Thread.Sleep(2000);


            IWebElement classes = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Cơ Bản - DMVN')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(100, 200);");
            Thread.Sleep(2000);
            classes.Click();
            Thread.Sleep(2000);

            // Chuyển đến tab "Thành viên"
            IWebElement memberTab = driver.FindElement(By.XPath("//a[normalize-space()='Thành viên']"));
            memberTab.Click();
            Thread.Sleep(2000);

            IWebElement addMember = driver.FindElement(By.CssSelector("div div div div div div div:nth-child(1) div:nth-child(2) div:nth-child(2) div:nth-child(1) div:nth-child(1) div:nth-child(2) button:nth-child(1) svg"));
            Actions actions = new(driver);
            actions.MoveToElement(addMember).Click().Perform();

            Thread.Sleep(2000);
            IWebElement profile = driver.FindElement(By.XPath("//span[contains(text(),'Xem hồ sơ')]"));
            profile.Click();
            Thread.Sleep(2000);
        }

        // Test 7: Click vào nút SVG và chọn "Xem điểm"
        [Test]
        public void ClickSvgAndViewProfileMark()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Thread.Sleep(3000);


            driver.FindElement(By.XPath("//button[contains(text(),'Đăng nhập')]")).Click();
            Thread.Sleep(2000);
            Login();
            driver.FindElement(By.XPath("//a[@href='/learn/class']")).Click();
            Thread.Sleep(2000);


            IWebElement classes = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Cơ Bản - DMVN')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(100, 200);");
            Thread.Sleep(4000);
            classes.Click();
            Thread.Sleep(2000);

            // Chuyển đến tab "Thành viên"
            IWebElement memberTab = driver.FindElement(By.XPath("//a[normalize-space()='Thành viên']"));
            memberTab.Click();
            Thread.Sleep(2000);

            IWebElement addMember = driver.FindElement(By.CssSelector("div div div div div div div:nth-child(1) div:nth-child(2) div:nth-child(2) div:nth-child(1) div:nth-child(1) div:nth-child(2) button:nth-child(1) svg"));
            Actions actions = new(driver);
            actions.MoveToElement(addMember).Click().Perform();

            Thread.Sleep(2000);
            IWebElement profile = driver.FindElement(By.XPath("//span[contains(text(),'Xem điểm')]"));
            profile.Click();
            Thread.Sleep(2000);
        }
        //Test 8: Xem Bài tập của 1 thành viên
        [Test]
        public void testAssignment()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Thread.Sleep(3000);


            driver.FindElement(By.XPath("//button[contains(text(),'Đăng nhập')]")).Click();
            Thread.Sleep(2000);
            Login();
            driver.FindElement(By.XPath("//a[@href='/learn/class']")).Click();
            Thread.Sleep(2000);


            IWebElement classes = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Cơ Bản - DMVN')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(100, 200);");
            Thread.Sleep(4000);
            classes.Click();
            Thread.Sleep(2000);
            IWebElement assignment = driver.FindElement(By.XPath("//a[normalize-space()='Bài tập']"));
            assignment.Click();
            Thread.Sleep(2000);
            IWebElement score = driver.FindElement(By.XPath("//tbody/tr[12]/td[6]/div[1]/button[1]"));
            score.Click();
            Thread.Sleep(2000);
        }


        public void Login()
        {
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

