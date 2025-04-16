/*using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools.V131.FedCm;
using OpenQA.Selenium.Interactions;
using System.Xml.Linq;

namespace TestCompa.Server.Studio.CourseSetting
{
    public class CourseSetting
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "http://10.10.10.30:3000/"; 
        private string newName = "UpdateCourse";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
        }

        //0. Điều hướng tới Course
        public void CourseTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > aside:nth-child(1) > div:nth-child(1) > a:nth-child(3) > button:nth-child(1)"));
            course.Click();
            Thread.Sleep(5000);
        }
        public void Save()
        {
            IWebElement save = driver.FindElement(By.CssSelector("button[type='submit']"));
            save.Click();
            Thread.Sleep(2000);
        }
        //Test 1: chọn khóa học
        [Test]
        public void navigatetoSetting()
        {
            CourseTest();
            IWebElement create = driver.FindElement(By.XPath($"//h3[normalize-space()='{newName}']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true)", create);
            Thread.Sleep(1000);
            create.Click();
            Thread.Sleep(3000);
        }

        //Test 2: điều hướng tới courseSetting
        [Test]
        public void courseSetting()
        {
            navigatetoSetting();
            Thread.Sleep(3000);

            IWebElement svg = driver.FindElement(By.CssSelector("button.group.cursor-pointer"));
            svg.Click();
            Thread.Sleep(1000);
            IWebElement Setting = driver.FindElement(By.XPath("//span[normalize-space()='Course Setting']"));
            Setting.Click();
            Thread.Sleep(1000);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("tag=setting"));
            Assert.IsTrue(driver.Url.Contains("tag=setting"), "Chuyển hướng không đúng, thiếu 'tag=setting' trong URL.");


        }
        //Test 3:  test thay đổi tên và des => Save                       
        [Test]
        public void changeNameField()
        {
            courseSetting();

            
            IWebElement name = driver.FindElement(By.Name("courseName"));
            name.Click();
            name.Clear();
            name.SendKeys($"{newName}");
            Thread.Sleep(2000);


            IWebElement desc = driver.FindElement(By.CssSelector("textarea[placeholder='Course description...']"));
            desc.Click();
            desc.Clear();
            Thread.Sleep(2000);
            desc.SendKeys("Description for Test Course");
            Thread.Sleep(3000);
            Save();
        }
        //Test 4: Thay đổi banner và thumbnail 
        [Test]
        public void changeBanner()
        {
            courseSetting();
           
           
            IWebElement inputBanner = driver.FindElement(By.Id("class-banner"));
            //img test
            string imgBanner = @"C:\Users\Hello\Pictures\TestImage\pexels-anjana-c-169994-674010.jpg";

            inputBanner.SendKeys(imgBanner);
            Thread.Sleep(4000);
            IWebElement inputThumbnail = driver.FindElement(By.Id("class-thumbnail"));

            string imgThumbnail = @"C:\Users\Hello\Pictures\TestImage\pexels-anjana-c-169994-674010.jpg";

            inputThumbnail.SendKeys(imgThumbnail);
            Thread.Sleep(4000);
            Save();
        }
        //Test 5: Thay đổi course Level
        [Test]
        public void changeCourseLevel()
        {
            courseSetting();

            // Tìm button combobox và cuộn tới nó
            IWebElement comboBoxButton = driver.FindElement(By.XPath("//button[@role='combobox' and @aria-expanded='false']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true)", comboBoxButton);
            Thread.Sleep(2000); 
            comboBoxButton.Click();
            Thread.Sleep(4000); 

            // Bước 2: Lấy tất cả các phần tử trong comboBox (danh sách các lựa chọn)
            IReadOnlyCollection<IWebElement> options = driver.FindElements(By.XPath("//ul[@role='listbox']//li"));

            if (options.Count > 0)
            {
                // Bước 3: Chọn ngẫu nhiên một phần tử từ danh sách các tùy chọn
                Random rand = new Random();
                int randomIndex = rand.Next(options.Count);
                IWebElement randomOption = options.ElementAt(randomIndex);

                // Bước 4: Cuộn tới phần tử đã chọn và nhấp vào
                js.ExecuteScript("arguments[0].scrollIntoView(true);", randomOption); 
                Thread.Sleep(500); 
                randomOption.Click();
                Thread.Sleep(1000); 
            }
            else
            {
                // Nếu không có tùy chọn nào, xử lý trường hợp này
                Console.WriteLine("Không có lựa chọn nào trong combobox.");
            }
            Save();
        }



        // Test 6 Accessbility
        [Test]
        public void accessibility()
        {
            courseSetting();

            IWebElement accessibiltityFree = driver.FindElement(By.CssSelector("#accessibility-free"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("arguments[0].scrollIntoView(true);", accessibiltityFree);
            Thread.Sleep(500);
            accessibiltityFree.Click();
            Thread.Sleep(3000);


            IWebElement accessibiltityBuy = driver.FindElement(By.CssSelector("#accessibility-buy"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;

            jss.ExecuteScript("arguments[0].scrollIntoView(true);", accessibiltityBuy);
            Thread.Sleep(500);
            accessibiltityBuy.Click();
            Thread.Sleep(3000);
            IWebElement classPrice = driver.FindElement(By.XPath("//input[@type='number' and @min='1']"));
            classPrice.Clear();
            classPrice.SendKeys("12345678");

            Thread.Sleep(3000);

        }
        // Test 5.3 Bỏ trống hoàn toàn 
        [Test]
        public void emptyField()
        {
            courseSetting();

            IWebElement submit = driver.FindElement(By.XPath("//button[@type='submit' and contains(text(), 'Create')]"));
            submit.Click();
            Thread.Sleep(5000);

            IWebElement descriptionError = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'Description must be at least 10 characters.')]")));
            IWebElement nameError = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'Name must be at least 2 characters.')]")));

            Assert.IsTrue(descriptionError.Displayed, "Thông báo lỗi về Description không hiển thị!");
            Assert.IsTrue(nameError.Displayed, "Thông báo lỗi về Name không hiển thị!");


        }
        //Test 6: Hủy tạo mới
        [Test]
        public void cancelCreate()
        {
            courseSetting();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            IWebElement closeButton = driver.FindElement(By.XPath("//button[contains(@class, 'absolute right-4 top-4')]"));
            js.ExecuteScript("arguments[0].click();", closeButton);
            Thread.Sleep(2000);
        }

        public void Login()
        {
            Thread.Sleep(2000);
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
*/