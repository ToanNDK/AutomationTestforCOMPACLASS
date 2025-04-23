using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Learn.Cart
{
    public class CourseTest
    {

        private IWebDriver driver;
        private WebDriverWait wait;

        private readonly string devUrl = "http://10.10.10.30/academy/kpim";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));
        }


        //Test 1 : Truy cập Overview khi chưa login
        [Test, Order(1)]
        public void TestOverviewCourseWithoutLogin()
        {

            driver.Navigate().GoToUrl(devUrl);

            IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
            course.Click();

            wait.Until(d => d.Url.Contains("http://10.10.10.30/vn/learn/course/"));
            Assert.That(driver.Url.Contains("http://10.10.10.30/vn/learn/course/"), Is.True);
        }

        //Test 2: Truy cập OverView khi đã đăng nhập
        [Test, Order(2)]

        public void TestOverviewCourseLogin()
        {

            driver.Navigate().GoToUrl(devUrl);

            Thread.Sleep(2000);

            IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
            course.Click();

            Thread.Sleep(3000);

            IWebElement loginButton = driver.FindElement(By.XPath("//button[contains(text(), 'Đăng nhập')]"));
            loginButton.Click();

            Thread.Sleep(2000);

            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("lozik480@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Toanking2k3*");

            IWebElement signInButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            signInButton.Click();

            // Chờ trang chuyển hướng sau khi đăng nhập
            Thread.Sleep(5000);

            // Kiểm tra kết quả
            Assert.That(driver.Url.Contains("http://10.10.10.30/vn/learn/course/"), Is.True);
        }

        //Test 3: Thêm vào giỏ hàng khi chưa đăng nhập

        [Test, Order(3)]
        public void addCartWithoutLogin()
        {
            driver.Navigate().GoToUrl(devUrl);
            IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
            course.Click();

            Thread.Sleep(5000);
            IWebElement cart = driver.FindElement(By.XPath("//button[span[text()='Thêm vào giỏ hàng']]"));
            cart.Click();
            Thread.Sleep(5000);

            // Bắt buộc chuyển hướng sang trang login
            Assert.That(driver.Url.Contains("http://10.10.10.30/Auth/SignIn"), Is.True);

        }
        //Test 4 : Thêm vào giỏ hàng khi đã đăng nhập
        [Test, Order(4)]
        public void addCartLogin()
        {
            driver.Navigate().GoToUrl(devUrl);
            IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
            course.Click();
            Thread.Sleep(3000);

            IWebElement cart = driver.FindElement(By.XPath("//button[span[text()='Thêm vào giỏ hàng']]"));
            cart.Click();
            Thread.Sleep(3000);

            Assert.That(driver.Url.Contains("http://10.10.10.30/Auth/SignIn"), Is.True);
            Thread.Sleep(3000);

            //Đăng nhập
            Login();
            Thread.Sleep(1000);

            Assert.That(driver.Url.Contains("http://10.10.10.30/vn/learn/course/"), Is.True);
            //Bấm lại nút giỏ hàng sau khi login
            IWebElement cartAfterLogin = driver.FindElement(By.XPath("//button[span[text()='Thêm vào giỏ hàng']]"));
            cartAfterLogin.Click();
            Thread.Sleep(1000);

            IWebElement viewCart = driver.FindElement(By.XPath("//button[span[text()='Xem giỏ hàng']]"));
            cartAfterLogin.Click();
            Thread.Sleep(1000);

            Assert.That(driver.Url.Contains("http://10.10.10.30/learn/cart"), Is.True);
        }
        //Test 5: Xóa khóa học khỏi giỏ hàng
        [Test, Order(5)]
        public void removeCourseCart()
        {
            driver.Navigate().GoToUrl(devUrl);

            IWebElement login = driver.FindElement(By.XPath("//button[text()='Đăng nhập']"));
            login.Click();
            Thread.Sleep(3000);

            Login();
            Thread.Sleep(2000);

            IWebElement cart = driver.FindElement(By.XPath("//a[@href='/learn/cart']"));
            cart.Click();
            Thread.Sleep(1000);

            Assert.That(driver.Url.Contains("http://10.10.10.30/learn/cart"), Is.True);


            IWebElement editCart = driver.FindElement(By.XPath("//button[span[text()='Chỉnh sửa giỏ hàng']]"));
            editCart.Click();
            Thread.Sleep(1000);

            IWebElement checkbox = driver.FindElement(By.XPath("//input[@type='checkbox']"));
            checkbox.Click();

            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(2));
            wait.Until(d => checkbox.Selected);
            Assert.That(checkbox.Selected, Is.True, "Checkbox chưa được chọn");


            IWebElement deleteCourse = driver.FindElement(By.XPath("//button[text()='Xóa khỏi giỏ hàng']"));
            deleteCourse.Click();


            IWebElement confirmPopup = wait.Until(d => d.FindElement(By.XPath("//div[@role='alertdialog']")));
            Assert.That(confirmPopup.Displayed, Is.True, "Popup xác nhận không xuất hiện!");

            IWebElement closePopup = wait.Until(d => d.FindElement(By.XPath("//button[text()='Quay lại']")));
            closePopup.Click();
            Thread.Sleep(500);


            IWebElement deleteAll = wait.Until(d => d.FindElement(By.XPath("//button[text()='Xóa']")));
            deleteAll.Click();


            wait.Until(d => d.FindElements(By.XPath("//div[@role='alertdialog']")).Count == 0);
            Thread.Sleep(1000);
            Console.WriteLine(" Sản phẩm đã được xóa khỏi giỏ hàng.");
        }
        [Test]
        // Test 6: Kiểm tra checkbox trong giỏ hàng -> Sau khi click thì nút Thanh toán A
        public void cartCheckboxTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            IWebElement login = driver.FindElement(By.XPath("//button[text()='Đăng nhập']"));
            login.Click();
            Thread.Sleep(3000);

            Login();
            Thread.Sleep(2000);

            IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
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

        //Test 6: Test mua khóa học
        public void pucharseCourse()
        {

        }
        [Test]
        //Test 7: Test mua Pass -> Chuyển sang trang checkout

        public void purcharseSubscription()
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
            IWebElement confirm = driver.FindElement(By.XPath("//a[@class='group/btn inline-flex items-center justify-center gap-2 whitespace-nowrap font-medium ring-offset-white transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 bg-primary text-white hover:bg-primary/90 h-9 rounded-md px-3 text-xs']"));
            confirm.Click();
            Thread.Sleep(3000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,300)");
            Thread.Sleep(5000);
            IWebElement monthly = driver.FindElement(By.XPath("//button[@class='group/btn inline-flex items-center justify-center gap-2 whitespace-nowrap text-sm font-medium ring-offset-white transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 border bg-white hover:bg-gray-l10 h-11 rounded-md px-8 w-full border-darkGray']"));
            monthly.Click();
            Thread.Sleep(3000);
        }
        //Test 8: 
        public void Login()
        {
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

