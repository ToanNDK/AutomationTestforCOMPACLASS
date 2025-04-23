using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Learn.Login
{
    public class LoginTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private readonly string devUrl = "http://10.10.10.30/Auth/SignIn?login_challenge=a635429bdb604fb08d123fff4bbb1add";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(devUrl);

            driver.Navigate().GoToUrl(devUrl);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        //Test 1 : Đăng nhập thành công -> chuyển về trang chủ



        [Test]
        public void TestLoginSuccess()
        {
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("lozik480@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Toanking2k3*");

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();

            // lần 2 nếu khác login_challenge
            IWebElement emailInput1 = driver.FindElement(By.Id("email"));
            emailInput1.SendKeys("lozik480@gmail.com");

            IWebElement passwordInput1 = driver.FindElement(By.Id("password"));
            passwordInput1.SendKeys("Toanking2k3*");

            IWebElement loginButton1 = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton1.Click();
            // Kiểm tra URL chuyển hướng đúng không
            string currentUrl = driver.Url;

            Assert.That(currentUrl.Contains("10.10.10.30"), Is.True);
        }

        //Test 2: Lỗi 
        [Test]
        public void TestLogin_InvalidCredentials()
        {

            driver.Navigate().GoToUrl(devUrl);
            driver.FindElement(By.Id("email")).SendKeys("invalid_email@example.com");
            driver.FindElement(By.Id("password")).SendKeys("invalid_password");
            driver.FindElement(By.XPath("//button[text()='SIGN IN']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'There was a problem logging in')]")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập sai thông tin đăng nhập!");
        }
        // Test 3 : Nhập đúng định dạng email nhưng password có độ dài quá ngắn
        [Test]
        public void TestLogin_PasswordTooShort()
        {

            driver.Navigate().GoToUrl(devUrl);
            driver.FindElement(By.Id("email")).SendKeys("ValidEmail@example.com");
            driver.FindElement(By.Id("password")).SendKeys("123");
            driver.FindElement(By.XPath("//button[text()='SIGN IN']")).Click();
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'There was a problem logging in. Check your email and password or create an account.')]")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập sai thông tin đăng nhập!");
        }

        //Test 4: Nhập đúng định dạng email nhưng password có độ dài quá dài
        [Test]
        public void TestLogin_PasswordTooLong()
        {

            string longPsw = new('a', 100);
            driver.Navigate().GoToUrl(devUrl);
            driver.FindElement(By.Id("email")).SendKeys("ValidEmail@example.com");
            driver.FindElement(By.Id("password")).SendKeys(longPsw);
            driver.FindElement(By.XPath("//button[text()='SIGN IN']")).Click();
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'There was a problem logging in. Check your email and password or create an account.')]")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập sai thông tin đăng nhập!");
        }
        // Test 5  : Quên mk
        [Test]
        public void TestForgotPassword()
        {

            driver.Navigate().GoToUrl(devUrl);
            IWebElement forgotPasswordLink = wait.Until(d => d.FindElement(By.XPath("//a[contains(text(), 'Forgot password?')]")));
            forgotPasswordLink.Click();

            Assert.That(driver.Url, Is.EqualTo("http://10.10.10.30/Auth/ForgotPassword"), "Không điều hướng đến trang quên mật khẩu!");
        }
        //Test 6 : Đăng nhập bằng google
        [Test]
        public void loginWithGoogle()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(3000);
            IWebElement btnGoogle = driver.FindElement(By.XPath("//img[@alt='google']"));
            btnGoogle.Click();
            Thread.Sleep(5000);
            Assert.That(driver.Url.Contains("https://accounts.google.com"), Is.True);
            Thread.Sleep(1000);
        }
        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
