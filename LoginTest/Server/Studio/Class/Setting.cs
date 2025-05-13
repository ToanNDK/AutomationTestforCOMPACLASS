/*using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Studio
{
    public class SettingClass
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "http://10.10.10.30:3000/";
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
            InitDriver(false);
        }
        //Test 0: Truy cập trang 
        [Test]
        public void ClassTest()
        {
            driver.Navigate().GoToUrl(devUrl);

            Login();
            Thread.Sleep(5000);
            IWebElement classes = driver.FindElement(By.CssSelector("svg[width='20'][height='20'][viewBox='0 0 18 22']"));
            classes.Click();
            Thread.Sleep(5000);
        }
        //Test 1: bấm nút 3 chấm -> Setting
        [Test]
        public void BtnSettingClass()
        {
            ClassTest();
            Thread.Sleep(3000);
            IWebElement edit = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > div:nth-child(2) > article:nth-child(1) > div:nth-child(2) > section:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > button:nth-child(2) > svg:nth-child(1)"));
            Thread.Sleep(3000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", edit);
            Thread.Sleep(2000);
            edit.Click();
            IWebElement setting = driver.FindElement(By.XPath("//span[@class='whitespace-nowrap text-sm transition-all']"));
            setting.Click();
            Thread.Sleep(3000);
        }

        //Test 2: Thay đổi className
        //2.1 Nhập className dài    
        [Test]
        public void ChangeClassName()
        {
            string longname = new('a', 150);
            BtnSettingClass();
            IWebElement classNametxt = driver.FindElement(By.CssSelector("input[placeholder='Enter class name']"));
            classNametxt.Click();
            Thread.Sleep(1000);
            classNametxt.SendKeys(longname);
            Thread.Sleep(3000);
            IWebElement btnSave = driver.FindElement(By.CssSelector("div[class='hidden lg:flex items-center justify-between gap-4'] button[type='submit']"));
            btnSave.Click();
            Thread.Sleep(3000);

        }
        //2.2 Xóa tên lớp, không nhập -> Hiển thị lỗi phải nhập ít nhất 1 kí tự
        [Test]
        public void DeleteClassName()
        {

            BtnSettingClass();
            IWebElement classNametxt = driver.FindElement(By.CssSelector("input[placeholder='Enter class name']"));
            classNametxt.Click();
            Thread.Sleep(1000);
            classNametxt.SendKeys("abc");
            classNametxt.Clear();
            Thread.Sleep(3000);
            IWebElement btnSave = driver.FindElement(By.CssSelector("div[class='hidden lg:flex items-center justify-between gap-4'] button[type='submit']"));
            btnSave.Click();
            Thread.Sleep(5000);
            try
            {
                IWebElement errorMessage = driver.FindElement(By.CssSelector("div.flex.space-x-1.items-center.pt-2 span.text-red-d10"));
                if (errorMessage.Displayed)
                {
                    Console.WriteLine("PASS: Thông báo lỗi hiển thị.");
                }
                else
                {
                    Console.WriteLine("FAILED: Thông báo lỗi không hiển thị.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("FAILED: Thông báo lỗi không tìm thấy.");
            }
        }


        //Test 3: Description
        //3.1 Nhập hơn 300 ký tự 
        [Test]
        public void ChangeDescription()
        {
            string longname = new('a', 301);
            BtnSettingClass();
            IWebElement classDesctxt = driver.FindElement(By.CssSelector("textarea[placeholder='Class description']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", classDesctxt);
            Thread.Sleep(2000);
            classDesctxt.Click();
            Thread.Sleep(1000);
            classDesctxt.Clear();
            classDesctxt.SendKeys(longname);
            Thread.Sleep(3000);
            IWebElement btnSave = driver.FindElement(By.CssSelector("div[class='hidden lg:flex items-center justify-between gap-4'] button[type='submit']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView(true);", btnSave);
            Thread.Sleep(2000);
            btnSave.Click();
            Thread.Sleep(3000);

        }

        //3.2 Không nhập description
        [Test]
        public void DeleteDescription()
        {

            BtnSettingClass();
            IWebElement classDesctxt = driver.FindElement(By.CssSelector("textarea[placeholder='Class description']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", classDesctxt);
            Thread.Sleep(2000);
            classDesctxt.Click();
            Thread.Sleep(1000);
            classDesctxt.SendKeys(Keys.Backspace);
            Thread.Sleep(3000);
            classDesctxt.SendKeys("f");
            Thread.Sleep(3000);
            classDesctxt.SendKeys(Keys.Control + "a");
            classDesctxt.SendKeys(Keys.Backspace);
            Thread.Sleep(2000);
            IWebElement btnSave = driver.FindElement(By.CssSelector("div[class='hidden lg:flex items-center justify-between gap-4'] button[type='submit']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView(true);", btnSave);
            Thread.Sleep(2000);
            btnSave.Click();
            Thread.Sleep(3000);
            try
            {
                IWebElement errorMessage = driver.FindElement(By.CssSelector("div.flex.space-x-1.items-center.pt-2 span.text-red-d10"));
                IJavaScriptExecutor jsDesc = (IJavaScriptExecutor)driver;
                jsDesc.ExecuteScript("arguments[0].scrollIntoView(true);", errorMessage);
                Thread.Sleep(5000);
                if (errorMessage.Displayed)
                {
                    Console.WriteLine("PASS: Thông báo lỗi hiển thị.");
                }
                else
                {
                    Console.WriteLine("FAILED: Thông báo lỗi không hiển thị.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("FAILED: Thông báo lỗi không tìm thấy.");
            }

        }
        //Test 4: Bấm ngẫu nhiên và các toggle và lưu 
        [Test]
        public void ToggleTest()
        {
            BtnSettingClass();
            Random random = new();
            IReadOnlyList<IWebElement> toggles = driver.FindElements(By.CssSelector("button.switch"));

            if (toggles.Count > 0)
            {
                int randomIndex = random.Next(0, toggles.Count); // Chọn một toggle ngẫu nhiên
                toggles[randomIndex].Click();
                Thread.Sleep(2000);
                Console.WriteLine($"Đã nhấn vào toggle thứ {randomIndex + 1}");
            }
            else
            {
                Console.WriteLine("Không tìm thấy toggle nào trên trang.");
            }
            IWebElement btnSave = driver.FindElement(By.CssSelector("div[class='hidden lg:flex items-center justify-between gap-4'] button[type='submit']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView(true);", btnSave);
            Thread.Sleep(2000);
            btnSave.Click();
            Thread.Sleep(2000);
        }
        //Test 5: Chọn copyURL
        [Test]
        public void CheckCopyUrl()
        {
            BtnSettingClass();
            Thread.Sleep(2000);


            IWebElement svg = driver.FindElement(By.XPath("//button[contains(@id, 'headlessui-listbox-button')]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", svg);
            Thread.Sleep(2000);


            IWebElement copyUrl = driver.FindElement(By.XPath("//span[contains(text(),'Lấy liên kết đến lớp học') or contains(text(),'Get link to the class')]"));
            copyUrl.Click();
            Thread.Sleep(2000);
            IWebElement link = driver.FindElement(By.CssSelector("svg[width='21'][height='21'][viewBox='0 0 24 24']"));
            link.Click();
            Thread.Sleep(3000);

        }
        //Test 6: Nhập Minimum Registrations lớn hơn Maximum Registrations
        [Test]
        public void TestRegistrations()
        {
            BtnSettingClass();
            Thread.Sleep(2000);
            IWebElement input = driver.FindElement(By.XPath("//input[@type='number' and @min='1']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", input);
            Thread.Sleep(2000);
            input.Clear();
            input.SendKeys("200");
            Thread.Sleep(2000);
            IWebElement btnSave = driver.FindElement(By.CssSelector("div[class='hidden lg:flex items-center justify-between gap-4'] button[type='submit']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView(true);", btnSave);
            Thread.Sleep(2000);
            btnSave.Click();
            Thread.Sleep(3000);
            try
            {
                IWebElement errorMessage = driver.FindElement(By.CssSelector("span.text-red-d10.text-sm.text-red"));
                IJavaScriptExecutor jsDesc = (IJavaScriptExecutor)driver;
                jsDesc.ExecuteScript("arguments[0].scrollIntoView(true);", errorMessage);
                Thread.Sleep(5000);
                if (errorMessage.Displayed)
                {
                    Console.WriteLine("PASS: Thông báo lỗi hiển thị.");
                }
                *//*else
                {
                    Console.WriteLine("FAILED: Thông báo lỗi không hiển thị.");
                }*//*
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("FAILED: Thông báo lỗi không tìm thấy.");
            }
        }

        //Test 7

        public void Login()
        {
            Thread.Sleep(5000);
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
*/