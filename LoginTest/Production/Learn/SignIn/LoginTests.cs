using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.Learn.Login
{
    public class LoginTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string productionUrl = "https://auth.compaclass.com/Auth/SignIn";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(productionUrl);
            driver.Navigate().GoToUrl(productionUrl);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }


        //Test 0: Bấm login khi chưa nhập thông tin 
        [Test]
        public void TestLogin_EmptyFields()
        {
            driver.Navigate().GoToUrl(productionUrl);


            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();


            IWebElement emailInput = driver.FindElement(By.Id("email"));
            bool isEmailInvalid = emailInput.GetAttribute("validationMessage").Length > 0;


            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            bool isPasswordInvalid = passwordInput.GetAttribute("validationMessage").Length > 0;

            Assert.IsTrue(isEmailInvalid, "Trường email không hiển thị cảnh báo required!");
            Assert.IsTrue(isPasswordInvalid, "Trường password không hiển thị cảnh báo required!");
        }
        //Test 1 : Đăng nhập thành công -> chuyển về trang chủ

        [Test]
        public void TestLoginSuccess()
        {
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("abc@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("abcyxz");

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();
            Thread.Sleep(5000);
            
            string currentUrl = driver.Url;

            Thread.Sleep(2000);
            Assert.IsTrue(currentUrl.Contains("https://compaclass.com/vn/learn/home"));
        }

        //Test 2: Lỗi 
        [Test]
        public void TestLogin_InvalidCredentials()
        {

            driver.Navigate().GoToUrl(productionUrl);
            driver.FindElement(By.Id("email")).SendKeys("invalid_email@example.com");
            driver.FindElement(By.Id("password")).SendKeys("invalid_password");
            driver.FindElement(By.XPath("//button[text()='SIGN IN']")).Click();

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'There was a problem logging in')]")));
            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không hiển thị khi nhập sai thông tin đăng nhập!");
        }
        // Test 3 : Nhập đúng định dạng email nhưng password có độ dài quá ngắn
        [Test]
        public void TestLogin_PasswordTooShort()
        {

            driver.Navigate().GoToUrl(productionUrl);
            driver.FindElement(By.Id("email")).SendKeys("ValidEmail@example.com");
            driver.FindElement(By.Id("password")).SendKeys("123");
            driver.FindElement(By.XPath("//button[text()='SIGN IN']")).Click();
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'There was a problem logging in. Check your email and password or create an account.')]")));
            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không hiển thị khi nhập sai thông tin đăng nhập!");
        }

        //Test 4: Nhập đúng định dạng email nhưng password có độ dài quá dài
        [Test]
        public void TestLogin_PasswordTooLong()
        {

            string longPsw = new string('a', 100);
            driver.Navigate().GoToUrl(productionUrl);
            driver.FindElement(By.Id("email")).SendKeys("ValidEmail@example.com");
            driver.FindElement(By.Id("password")).SendKeys(longPsw);
            driver.FindElement(By.XPath("//button[text()='SIGN IN']")).Click();
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'There was a problem logging in. Check your email and password or create an account.')]")));
            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không hiển thị khi nhập sai thông tin đăng nhập!");
        }
        // Test 5  : Quên mk
        [Test]
        public void TestForgotPassword()
        {

            driver.Navigate().GoToUrl(productionUrl);
            IWebElement forgotPasswordLink = wait.Until(d => d.FindElement(By.XPath("//a[contains(text(), 'Forgot password?')]")));
            forgotPasswordLink.Click();

            Assert.AreEqual("https://auth.compaclass.com/Auth/ForgotPassword", driver.Url, "Không điều hướng đến trang quên mật khẩu!");


        }
        //Test 6: Đăng nhập bằng Google
        [Test]
        public void loginWithGoogle()
        {
            driver.Navigate().GoToUrl(productionUrl);
            IWebElement btnGoogle = driver.FindElement(By.XPath("//a[contains(@href, 'ExternalLogin') and contains(@href, 'Google')]"));
            btnGoogle.Click();
            Thread.Sleep(5000);
            Assert.IsTrue(driver.Url.Contains("https://accounts.google.com/"));
            Thread.Sleep(5000);
        }
        //Test 7: Đăng nhập bằng Microsoft
        [Test]
        public void loginWithMicrosoft()
        {
            driver.Navigate().GoToUrl(productionUrl);
            IWebElement btnMicrosoft = driver.FindElement(By.XPath("//a[contains(@href, 'ExternalLogin') and contains(@href, 'Microsoft')]"));
            btnMicrosoft.Click();
            Thread.Sleep(5000);
            Assert.IsTrue(driver.Url.Contains("https://login.microsoftonline.com/"));
            Thread.Sleep(5000);
        }
        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
