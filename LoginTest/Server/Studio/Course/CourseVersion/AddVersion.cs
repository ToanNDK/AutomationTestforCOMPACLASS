using OpenQA.Selenium.Chrome;
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

namespace TestCompa.Server.Studio
{
    public class addCourseVersion
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "http://10.10.10.30:3000/";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        
        //0. Điều hướng tới CourseVersion

        [Test]

        public void studioTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > aside:nth-child(1) > div:nth-child(1) > a:nth-child(3) > button:nth-child(1)"));
            course.Click();
            Thread.Sleep(5000);
        }
        //Test 1: bấm nút tạo version khóa học mới
        [Test]
        public void btnNewCourse()
        {
            studioTest();
            IWebElement create = driver.FindElement(By.CssSelector("body > div:nth-child(1) > article:nth-child(2) > article:nth-child(2) > div:nth-child(2) > article:nth-child(1) > div:nth-child(1) > button:nth-child(2)"));
            Actions action = new Actions(driver);
            action.DoubleClick(create).Perform();
            Thread.Sleep(5000);
        }

        //Test 2: test hiển thị khung pop - up tạo mới version
        [Test]
        public void popupCreateVersion()
        {
            btnNewCourse();
            Thread.Sleep(5000);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(5000);


            IWebElement courseTest = driver.FindElement(By.XPath("//h2[text()='123123132']/ancestor::div[@class='bg-white rounded-lg shadow-md overflow-hidden']"));
            Thread.Sleep(2000);

            IWebElement edit = courseTest.FindElement(By.XPath(".//button[contains(@class, 'group/btn')]"));
            edit.Click();
            Thread.Sleep(3000);


            IWebElement showVersions = driver.FindElement(By.CssSelector("div[role='menuitem']"));
            showVersions.Click();
            Thread.Sleep(3000);

            IWebElement createNewVer = driver.FindElement(By.XPath("//button[normalize-space()='Create new version']"));
            createNewVer.Click();
            Thread.Sleep(5000);
            
        }
        //Test 3:  test nhập đầy đủ thông tin và nhấn Create                       
        [Test]
        public void createVersion()
        {
            popupCreateVersion();

            IWebElement nameVer = driver.FindElement(By.XPath("//input[@placeholder='Phiên bản 1']"));
            nameVer.SendKeys("Version Test 1");
            Thread.Sleep(3000);


            IWebElement desVer = driver.FindElement(By.XPath("//textarea[@placeholder='Phiên bản 1']"));
            desVer.SendKeys("Description for Test Course");
            Thread.Sleep(3000);

            IWebElement submit = driver.FindElement(By.XPath("//button[@type='submit' and contains(text(), 'Create')]"));
            submit.Click();
            Thread.Sleep(5000);
        }
        //Test 4: Sau khi tạo Course -> Chuyển sang giao diện Course Builder
        [Test]
        public void btnCreateVersion()
        {
            createVersion();
            string currentUrl = driver.Url;

            Assert.IsTrue(currentUrl.Contains("http://10.10.10.30:3000/teach/builder"), "Chưa chuyển sang trang Course Builder!");
            Thread.Sleep(2000);
        }
        //Test 5: Không nhập thông tin ở các trường bắt buộc
        // Test 5.1 Trường Name
        [Test]
        public void emptyName()
        {
            popupCreateVersion();

            IWebElement desVer = driver.FindElement(By.XPath("//textarea[@placeholder='Phiên bản 1']"));
            desVer.SendKeys("Description for Test Course");
            Thread.Sleep(3000);

            IWebElement submit = driver.FindElement(By.XPath("//button[@type='submit' and contains(text(), 'Create')]"));
            submit.Click();
            Thread.Sleep(5000);
           

            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'Name must be at least 2 characters.')]")));
            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không hiển thị khi không nhập thông tin !");
        }

        // Test 5.2 Trường Description
        [Test]
        public void emptyDescription()
        {
            popupCreateVersion();

            IWebElement nameVer = driver.FindElement(By.XPath("//input[@placeholder='Phiên bản 1']"));
            nameVer.SendKeys("Version Test 1");
            Thread.Sleep(3000); ;

            IWebElement submit = driver.FindElement(By.XPath("//button[@type='submit' and contains(text(), 'Create')]"));
            submit.Click();
            Thread.Sleep(5000);


            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[contains(text(), 'Description must be at least 10 characters.')]")));
            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không hiển thị khi không nhập thông tin !");
        }
        // Test 5.3 Bỏ trống hoàn toàn 
        [Test]
        public void emptyField()
        {
            popupCreateVersion();

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
            popupCreateVersion();
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
