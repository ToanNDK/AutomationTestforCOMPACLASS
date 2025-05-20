using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace TestCompa.Production.Learn.SignUp
{
    [TestFixture]
    [Category("Register")]
    public class SignUpTests
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string productionUrl = "https://auth.compaclass.com/Auth/SignUp";

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

        /*  // Test 1: Kiểm tra đăng ký thành công
          [Test]
          public void TestSignUp_Success()
          {
              driver.Navigate().GoToUrl(productionUrl);

              driver.FindElement(By.Id("username")).SendKeys("new_user_123");
              driver.FindElement(By.Id("email")).SendKeys("newuser123@example.com");
              driver.FindElement(By.Id("password")).SendKeys("NewPass123@");
              driver.FindElement(By.Id("confirm_password")).SendKeys("NewPass123@");

              driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

              wait.Until(d => d.Url.Contains("/Auth/SignIn"));
              Assert.That(driver.Url.Contains("/Auth/SignIn"), Is.True, "User không được chuyển đến trang đăng nhập sau khi đăng ký thành công!");
          }
  */
        //Test 2: Đăng ký với email đã tồn tại
        [Test]
        public void TestSignUp_EmailAlreadyExists()
        {
            driver.Navigate().GoToUrl("https://auth.compaclass.com/Auth/SignIn");

            IWebElement signUpClick = wait.Until(d => d.FindElement(By.LinkText("Sign up")));
            signUpClick.Click();

            driver.FindElement(By.Id("username")).SendKeys("testuser");
            driver.FindElement(By.Id("email")).SendKeys("lozy564@gmail.com");
            driver.FindElement(By.Id("password")).SendKeys("Test@1234");
            driver.FindElement(By.Id("confirm_password")).SendKeys("Test@1234");
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.That(errorMessage.Text.Contains("Email 'lozy564@gmail.com' is already taken"), Is.True, "Không tìm thấy thông báo lỗi email đã tồn tại!");
        }

        // Test 3: Đăng ký với mật khẩu không hợp lệ
        [Test]
        public void TestSignUp_InvalidPassword()
        {
            driver.Navigate().GoToUrl(productionUrl);

            driver.FindElement(By.Id("username")).SendKeys("invalid_pass_user");
            driver.FindElement(By.Id("email")).SendKeys("invalidpass@example.com");
            driver.FindElement(By.Id("password")).SendKeys("123");
            driver.FindElement(By.Id("confirm_password")).SendKeys("123");
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.That(errorMessage.Text.Contains("The Password must be at least 6 and at max 100 characters long."), Is.True, "Không hiển thị thông báo lỗi mật khẩu không hợp lệ!");
        }

        //Test 4: Đăng ký với mật khẩu không khớp
        [Test]
        public void TestSignUp_PasswordMismatch()
        {
            driver.Navigate().GoToUrl(productionUrl);

            driver.FindElement(By.Id("username")).SendKeys("mismatch_user");
            driver.FindElement(By.Id("email")).SendKeys("mismatch@example.com");
            driver.FindElement(By.Id("password")).SendKeys("ValidPass123@");
            driver.FindElement(By.Id("confirm_password")).SendKeys("Pass123@");
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.That(errorMessage.Text.Contains("The password and confirmation password do not match."), Is.True, "Không hiển thị thông báo lỗi mật khẩu không khớp!");
        }

        //Test 5: Kiểm tra nút SIGN UP hoạt động
        [Test]
        public void TestSignUpButtonExists()
        {
            driver.Navigate().GoToUrl(productionUrl);

            IWebElement signUpButton = wait.Until(d => d.FindElement(By.XPath("//button[text()='SIGN UP']")));
            Assert.That(signUpButton.Displayed && signUpButton.Enabled, Is.True, "Nút SIGN UP không hiển thị hoặc không thể bấm!");
        }
        // Test 6: Đăng ký với username quá ngắn
        [Test]
        public void TestSignUp_UsernameTooShort()
        {
            driver.Navigate().GoToUrl(productionUrl);

            driver.FindElement(By.Id("username")).SendKeys("usr");
            driver.FindElement(By.Id("email")).SendKeys("shortuser@example.com");
            driver.FindElement(By.Id("password")).SendKeys("ValidPass123@");
            driver.FindElement(By.Id("confirm_password")).SendKeys("ValidPass123@");
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.That(errorMessage.Text.Contains("The Username must be at least 6 and at max 100 characters long."), Is.True,
                "Không hiển thị thông báo lỗi username quá ngắn!");
        }

        // Test 7: Đăng ký với username quá dài
        [Test]
        public void TestSignUp_UsernameTooLong()
        {
            driver.Navigate().GoToUrl(productionUrl);

            string longUsername = new('a', 101); // Username dài 101 ký tự
            driver.FindElement(By.Id("username")).SendKeys(longUsername);
            driver.FindElement(By.Id("email")).SendKeys("longuser@example.com");
            driver.FindElement(By.Id("password")).SendKeys("ValidPass123@");
            driver.FindElement(By.Id("confirm_password")).SendKeys("ValidPass123@");
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.That(errorMessage.Text.Contains("The Username must be at least 6 and at max 100 characters long."), Is.True,
                "Không hiển thị thông báo lỗi username quá dài!");

        }
        [Test]
        //Test 8: Đăng ký với password quá ngắn
        public void TestSignUp_PasswordTooShort()
        {
            driver.Navigate().GoToUrl(productionUrl);

            string pswShort = "abc";
            driver.FindElement(By.Id("username")).SendKeys("correctUsername");
            driver.FindElement(By.Id("email")).SendKeys("longuser@example.com");
            driver.FindElement(By.Id("password")).SendKeys(pswShort);
            driver.FindElement(By.Id("confirm_password")).SendKeys(pswShort);
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.That(errorMessage.Text.Contains("The Password must be at least 6 and at max 100 characters long."), Is.True,
                "Không hiển thị thông báo lỗi psw quá ngắn!");
        }

        [Test]
        //Test 9: Đăng ký với password quá dài
        public void TestSignUp_PasswordTooLong()
        {
            driver.Navigate().GoToUrl(productionUrl);

            string pswLong = new('a', 101);
            driver.FindElement(By.Id("username")).SendKeys("correctUsername");
            driver.FindElement(By.Id("email")).SendKeys("longuser@example.com");
            driver.FindElement(By.Id("password")).SendKeys(pswLong);
            driver.FindElement(By.Id("confirm_password")).SendKeys(pswLong);
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.That(errorMessage.Text.Contains("The password and confirmation password do not match."), Is.True,
                "Không hiển thị thông báo lỗi psw quá dài!");
        }


        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
