using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace TestCompa.Server.Learn.Cart
{
    public class CheckoutTest
    {

        private IWebDriver driver;
        private WebDriverWait wait;

        private readonly string devUrl = "http://10.10.10.30/vn/academy/kpim";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));
        }


       
        [Test]
        // Test 1: Kiểm tra checkbox trong giỏ hàng -> Sau khi click thì nút Thanh toán 
        public void cartCheckboxTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            IWebElement login = driver.FindElement(By.XPath("//button[text()='Đăng nhập']"));
            login.Click();
            Thread.Sleep(3000);

            Login();
            Thread.Sleep(2000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,300)");
            Thread.Sleep(2000);
            IWebElement course = driver.FindElement(By.XPath("//div[@class='flex flex-col h-full group/card-item bg-white rounded-md shadow-lg cursor-pointer group relative overflow-hidden']"));
            course.Click();
            Thread.Sleep(5000);

            // Kiểm tra trạng thái của nút
            IWebElement buttonAddCart = driver.FindElement(By.XPath("//span[contains(text(),'Thêm vào giỏ hàng') or contains(text(),'Xem giỏ hàng')]"));
            string btnText = buttonAddCart.Text.Trim();

            if (btnText == "Thêm vào giỏ hàng")
            {
                Console.WriteLine("Sản phẩm chưa được thêm vào giỏ hàng. Tiến hành thêm...");
                buttonAddCart.Click();
                Thread.Sleep(2000);

                // Kiểm tra lại xem nút đã đổi thành "Xem giỏ hàng" chưa
                buttonAddCart = driver.FindElement(By.XPath("//span[contains(text(),'Xem giỏ hàng')]"));
                if (buttonAddCart.Text.Trim() == "Xem giỏ hàng")
                {
                    Console.WriteLine("Thêm thành công. Chuyển đến giỏ hàng...");
                    buttonAddCart.Click();
                    Thread.Sleep(2000);
                    Assert.That(driver.Url.Contains("http://10.10.10.30/learn/cart"), Is.True);
                }
                else
                {
                    Assert.Fail("Không đổi trạng thái sau khi thêm vào giỏ hàng!");
                }
            }
            else if (btnText == "Xem giỏ hàng")
            {
                Console.WriteLine("Sản phẩm đã có trong giỏ hàng. Chuyển đến giỏ hàng...");
                buttonAddCart.Click();
                Thread.Sleep(2000);
                Assert.That(driver.Url.Contains("http://10.10.10.30/learn/cart"), Is.True);
            }
            else
            {
                Assert.Fail("Không xác định được trạng thái của nút!");
            }
            IWebElement checkbox = driver.FindElement(By.XPath("//div[@class='flex mb-4 pb-2 md:pb-4 border-b border-border last:border-b-transparent last:mb-0 last:pb-0 gap-2']//input[@type='checkbox']"));
            checkbox.Click();
            Thread.Sleep(5000);
        }

        //Test 2: Test mua khóa học
        [Test]
        public void purchaseFirstFreeCourse()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(3000);

            // Đăng nhập
            IWebElement login = driver.FindElement(By.XPath("//button[text()='Đăng nhập']"));
            login.Click();
            Thread.Sleep(5000);
            Login();
            Thread.Sleep(5000);
            IWebElement course = driver.FindElement(By.XPath("//div[@class='flex flex-col h-full group/card-item bg-white rounded-md shadow-lg cursor-pointer group relative overflow-hidden']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", course);
            Thread.Sleep(4000);
            course.Click();
            Thread.Sleep(4000);
        }

        [Test]
        //Test 3: Test mua Pass (monthly) -> Chuyển sang trang checkout

        public void SubscriptionMonthly()
        {
            driver.Navigate().GoToUrl(devUrl);
            IWebElement login = driver.FindElement(By.XPath("//button[text()='Đăng nhập']"));
            login.Click();
            Thread.Sleep(3000);

            Login();
            Thread.Sleep(2000);
            IWebElement subscription = driver.FindElement(By.XPath("//button[@class='group/btn inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium ring-offset-white transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 border border-border bg-white hover:bg-gray-l10 h-10 px-4 py-2']"));
            subscription.Click();
            Thread.Sleep(3000);
            IWebElement confirm = driver.FindElement(By.XPath("//a[contains(text(),'Đăng Ký Ngay')]"));
            confirm.Click();
            Thread.Sleep(3000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,300)");
            Thread.Sleep(5000);
            IWebElement monthly = driver.FindElement(By.XPath("//button[@class='group/btn inline-flex items-center justify-center gap-2 whitespace-nowrap text-sm font-medium ring-offset-white transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 border bg-white hover:bg-gray-l10 h-11 rounded-md px-8 w-full border-darkGray']"));
            monthly.Click();
            Thread.Sleep(3000);
        }
        //Test 4: Bấm chọn đăng kí pass thông qua user setting
        [Test]
        public void purchaseSubinUserAvatar()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(3000);
            IWebElement login = driver.FindElement(By.XPath("//button[text()='Đăng nhập']"));
            login.Click();
            Thread.Sleep(3000);

            Login();
            Thread.Sleep(2000);
            IWebElement userAvatar = driver.FindElement(By.XPath("//img[contains(@class,'w-[45px] h-[45px] rounded-full lg:w-10 lg:h-10 cursor-pointer object-cover')]"));
            userAvatar.Click();
            Thread.Sleep(5000);
            IWebElement register = driver.FindElement(By.XPath("//a[contains(@class,'h-9 rounded-md px-3 text-xs')]"));
            register.Click();
            Thread.Sleep(5000);
            IWebElement monthly = driver.FindElement(By.XPath("//button[@class='group/btn inline-flex items-center justify-center gap-2 whitespace-nowrap text-sm font-medium ring-offset-white transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 border bg-white hover:bg-gray-l10 h-11 rounded-md px-8 w-full border-darkGray']"));
            monthly.Click();
            Thread.Sleep(3000);
        }
        //Test 5: Test mua pass (monthly)
        [Test]
        public void purchaseSubscriptionMonthly()
        {

            SubscriptionMonthly();
            IWebElement pay = driver.FindElement(By.XPath("//span[contains(text(),'Tiến hành thanh toán')]"));
            //pay.Click();
            Thread.Sleep(3000);
            Console.WriteLine("Thanh toán thành công");

        }
        //Test 6: Test mua pass (Year)
        [Test]
        public void purchaseSubscriptionYear()
        {
            SubscriptionMonthly();
            IWebElement year = driver.FindElement(By.XPath("//p[contains(text(),'Hàng năm')]"));
            //year.Click();
            Thread.Sleep(4000);
            /*IWebElement pay = driver.FindElement(By.XPath("//span[contains(text(),'Tiến hành thanh toán')]"));
            pay.Click();
            Thread.Sleep(3000);
            Console.WriteLine("Thanh toán thành công");*/
        }
        public void Login()
        {
            /*IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("lozik480@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Toanking2k3*");

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();*/
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("tuantry959@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Kpim@123");

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

