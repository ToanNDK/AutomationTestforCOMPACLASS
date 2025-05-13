using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.Learn.CartTest
{
    [TestFixture]
    [Category("Cart")]
    public class CartTest
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "https://compaclass.com/vn/academy/kpim";
        private readonly string email = "lozik480@gmail.com";
        private readonly string password = "Toanking2k3*";

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
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));
        }

        [SetUp]
        public void Setup()
        {
            // Gọi InitDriver với tham số headless = false (mặc định)
            // Thay đổi thành true nếu muốn chạy ở chế độ headless
            InitDriver(true);
        }

        //Test 1 : Truy cập Overview khi chưa login
        [Test, Order(1)]
        public void TestOverviewCourseWithoutLogin()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
            course.Click();

            wait.Until(d => d.Url.Contains("https://compaclass.com/vn/learn/course/"));
            Assert.That(driver.Url, Does.Contain("https://compaclass.com/vn/learn/course/"));
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

            Thread.Sleep(4000);

            // Đăng nhập
            Login();

            Thread.Sleep(6000);

            // Kiểm tra kết quả
            Assert.That(driver.Url, Does.Contain("https://compaclass.com/vn/learn/course"));
        }

        //Test 3: Thêm vào giỏ hàng khi chưa đăng nhập
        [Test, Order(3)]
        public void AddCartWithoutLogin()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
            course.Click();

            Thread.Sleep(5000);
            IWebElement cart = driver.FindElement(By.XPath("//div[contains(@class,'flex gap-1')]//button[1]"));
            cart.Click();
            Thread.Sleep(5000);

            // Bắt buộc chuyển hướng sang trang login
            Assert.That(driver.Url, Does.Contain("https://auth.compaclass.com/Auth/SignIn"));
        }

        //Test 4 : Thêm vào giỏ hàng khi đã đăng nhập
        [Test, Order(4)]
        public void AddCartLogin()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            // Chọn khóa học
            IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(100,300)");
            course.Click();
            Thread.Sleep(3000); // Chờ trang load

            // Bấm nút thêm vào giỏ hàng
            IWebElement cart = driver.FindElement(By.XPath("//div[contains(@class,'flex gap-1')]//button[1]"));
            cart.Click();
            Thread.Sleep(3000);

            // Kiểm tra đã chuyển hướng đến trang đăng nhập
            Assert.That(driver.Url, Does.Contain("https://auth.compaclass.com/Auth/SignIn"));
            Thread.Sleep(3000);

            // Đăng nhập
            Login();
            Thread.Sleep(3000); // Chờ trang chuyển hướng sau khi đăng nhập

            // Tìm lại phần tử "Thêm vào giỏ hàng" sau khi đăng nhập
            IWebElement cartAfterLogin = driver.FindElement(By.XPath("//span[contains(text(),'Thêm vào giỏ hàng')]"));
            cartAfterLogin.Click();
            Thread.Sleep(5000); // Chờ cập nhật giao diện

            // Tìm lại nút "Xem giỏ hàng" vì nút cũ đã bị thay đổi
            IWebElement viewCart = driver.FindElement(By.XPath("//button[span[text()='Xem giỏ hàng']]"));
            viewCart.Click();
            Thread.Sleep(5000);

            // Kiểm tra đã vào trang giỏ hàng
            Assert.That(driver.Url, Does.Contain("https://compaclass.com/learn/cart"));
        }

        //Test 5: Xóa khóa học khỏi giỏ hàng
        [Test, Order(5)]
        public void RemoveCourseCart()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            IWebElement login = driver.FindElement(By.XPath("//button[text()='Đăng nhập']"));
            login.Click();
            Thread.Sleep(3000);

            Login();
            Thread.Sleep(2000);

            IWebElement cart = driver.FindElement(By.XPath("//a[@href='/learn/cart']"));
            cart.Click();
            Thread.Sleep(1000);

            Assert.That(driver.Url, Does.Contain("https://compaclass.com/learn/cart"));

            IWebElement editCart = driver.FindElement(By.XPath("//button[span[text()='Chỉnh sửa giỏ hàng']]"));
            editCart.Click();
            Thread.Sleep(1000);

            IWebElement checkbox = driver.FindElement(By.XPath("//input[@type='checkbox']"));
            checkbox.Click();

            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(2));
            wait.Until(d => checkbox.Selected);
            Assert.That(checkbox.Selected, "Checkbox chưa được chọn");

            IWebElement deleteCourse = driver.FindElement(By.XPath("//button[text()='Xóa khỏi giỏ hàng']"));
            deleteCourse.Click();

            IWebElement confirmPopup = wait.Until(d => d.FindElement(By.XPath("//div[@role='alertdialog']")));
            Assert.That(confirmPopup.Displayed, "Popup xác nhận không xuất hiện!");

            IWebElement closePopup = wait.Until(d => d.FindElement(By.XPath("//button[text()='Quay lại']")));
            closePopup.Click();
            Thread.Sleep(500);

            IWebElement deleteAll = wait.Until(d => d.FindElement(By.XPath("//button[text()='Xóa']")));
            deleteAll.Click();

            wait.Until(d => d.FindElements(By.XPath("//div[@role='alertdialog']")).Count == 0);
            Thread.Sleep(1000);
            Console.WriteLine(" Sản phẩm đã được xóa khỏi giỏ hàng.");
        }

        // Test 6: Kiểm tra checkbox trong giỏ hàng -> Sau khi click thì nút Thanh toán sang
        [Test, Order(6)]
        public void CartCheckboxTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Thread.Sleep(2000);
            IWebElement login = driver.FindElement(By.XPath("//button[text()='Đăng nhập']"));
            login.Click();
            Thread.Sleep(3000);

            Login();
            Thread.Sleep(2000);

            IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
            course.Click();
            Thread.Sleep(5000);

            // Kiểm tra trạng thái của nút
            IWebElement buttonAddCart = driver.FindElement(By.XPath("//div[contains(@class,'flex gap-1')]//button[1]"));
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
                    buttonAddCart.Click();
                    Thread.Sleep(2000);
                    Assert.That(driver.Url, Does.Contain("https://compaclass.com/learn/cart"));
                }
                else
                {
                    Assert.Fail("Không đổi trạng thái sau khi thêm vào giỏ hàng!");
                }
            }
            else if (btnText == "Xem giỏ hàng")
            {
                buttonAddCart.Click();
                Thread.Sleep(2000);
                Assert.That(driver.Url, Does.Contain("https://compaclass.com/learn/cart"));
            }
            else
            {
                Assert.Fail("Không xác định được trạng thái của nút!");
            }
            IWebElement checkbox = driver.FindElement(By.XPath("//div[@class='flex mb-4 pb-2 md:pb-4 border-b border-border last:border-b-transparent last:mb-0 last:pb-0 gap-2']//input[@type='checkbox']"));
            checkbox.Click();
            Thread.Sleep(5000);
        }

        private void Login()
        {
            Thread.Sleep(1000);
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys(email);

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys(password);

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