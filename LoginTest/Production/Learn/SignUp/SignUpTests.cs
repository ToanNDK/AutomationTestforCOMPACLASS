using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;


namespace TestCompa.Production.Learn.SignUp
{
    public class SignUpTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {

            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://auth.compaclass.com/Auth/SignUp");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // Test 1: Kiểm tra đăng ký thành công
        [Test]
        public void TestSignUp_Success()
        {
            driver.FindElement(By.Id("username")).SendKeys("new_user_123");
            driver.FindElement(By.Id("email")).SendKeys("newuser123@example.com");
            driver.FindElement(By.Id("password")).SendKeys("NewPass123@");
            driver.FindElement(By.Id("confirm_password")).SendKeys("NewPass123@");

            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            wait.Until(d => d.Url.Contains("/Auth/SignIn"));
            Assert.IsTrue(driver.Url.Contains("/Auth/SignIn"), "User không được chuyển đến trang đăng nhập sau khi đăng ký thành công!");
        }

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
            Assert.IsTrue(errorMessage.Text.Contains("Email 'lozy564@gmail.com' is already taken"), "Không tìm thấy thông báo lỗi email đã tồn tại!");
        }

        // Test 3: Đăng ký với mật khẩu không hợp lệ
        [Test]
        public void TestSignUp_InvalidPassword()
        {
            driver.FindElement(By.Id("username")).SendKeys("invalid_pass_user");
            driver.FindElement(By.Id("email")).SendKeys("invalidpass@example.com");
            driver.FindElement(By.Id("password")).SendKeys("123");
            driver.FindElement(By.Id("confirm_password")).SendKeys("123");
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.IsTrue(errorMessage.Text.Contains("The Password must be at least 6 and at max 100 characters long."), "Không hiển thị thông báo lỗi mật khẩu không hợp lệ!");
        }

        //Test 4: Đăng ký với mật khẩu không khớp
        [Test]
        public void TestSignUp_PasswordMismatch()
        {
            driver.FindElement(By.Id("username")).SendKeys("mismatch_user");
            driver.FindElement(By.Id("email")).SendKeys("mismatch@example.com");
            driver.FindElement(By.Id("password")).SendKeys("ValidPass123@");
            driver.FindElement(By.Id("confirm_password")).SendKeys("Pass123@");
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.IsTrue(errorMessage.Text.Contains("The password and confirmation password do not match."), "Không hiển thị thông báo lỗi mật khẩu không khớp!");
        }

        //Test 5: Kiểm tra nút SIGN UP hoạt động
        [Test]
        public void TestSignUpButtonExists()
        {
            IWebElement signUpButton = wait.Until(d => d.FindElement(By.XPath("//button[text()='SIGN UP']")));
            Assert.IsTrue(signUpButton.Displayed && signUpButton.Enabled, "Nút SIGN UP không hiển thị hoặc không thể bấm!");
        }
        // Test 6: Đăng ký với username quá ngắn
        [Test]
        public void TestSignUp_UsernameTooShort()
        {
            driver.FindElement(By.Id("username")).SendKeys("usr");
            driver.FindElement(By.Id("email")).SendKeys("shortuser@example.com");
            driver.FindElement(By.Id("password")).SendKeys("ValidPass123@");
            driver.FindElement(By.Id("confirm_password")).SendKeys("ValidPass123@");
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.IsTrue(errorMessage.Text.Contains("The Username must be at least 6 and at max 100 characters long."),
                "Không hiển thị thông báo lỗi username quá ngắn!");
        }

        // Test 7: Đăng ký với username quá dài
        [Test]
        public void TestSignUp_UsernameTooLong()
        {
            string longUsername = new string('a', 101); // Username dài 101 ký tự
            driver.FindElement(By.Id("username")).SendKeys(longUsername);
            driver.FindElement(By.Id("email")).SendKeys("longuser@example.com");
            driver.FindElement(By.Id("password")).SendKeys("ValidPass123@");
            driver.FindElement(By.Id("confirm_password")).SendKeys("ValidPass123@");
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.IsTrue(errorMessage.Text.Contains("The Username must be at least 6 and at max 100 characters long."),
                "Không hiển thị thông báo lỗi username quá dài!");
            Thread.Sleep(10000);
        }
        [Test]
        //Test 8: Đăng ký với password quá ngắn
        public void TestSignUp_PasswordTooShort()
        {
            string pswShort = "abc";
            driver.FindElement(By.Id("username")).SendKeys("correctUsername");
            driver.FindElement(By.Id("email")).SendKeys("longuser@example.com");
            driver.FindElement(By.Id("password")).SendKeys(pswShort);
            driver.FindElement(By.Id("confirm_password")).SendKeys(pswShort);
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.IsTrue(errorMessage.Text.Contains("The Password must be at least 6 and at max 100 characters long."),
                "Không hiển thị thông báo lỗi psw quá ngắn!");
        }

        [Test]
        //Test 9: Đăng ký với password quá dài
        public void TestSignUp_PasswordTooLong()
        {
            string pswLong = new string('a', 101);
            driver.FindElement(By.Id("username")).SendKeys("correctUsername");
            driver.FindElement(By.Id("email")).SendKeys("longuser@example.com");
            driver.FindElement(By.Id("password")).SendKeys(pswLong);
            driver.FindElement(By.Id("confirm_password")).SendKeys(pswLong);
            driver.FindElement(By.XPath("//button[text()='SIGN UP']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.CssSelector("p.text-xs.font-medium.text-red-500")));
            Assert.IsTrue(errorMessage.Text.Contains("The password and confirmation password do not match."),
                "Không hiển thị thông báo lỗi psw quá dài!");
        }


        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
