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
using static OpenQA.Selenium.BiDi.Modules.Input.Pointer;

namespace TestCompa.Server.CourseBuilder.Video
{
    [TestFixture]
    [Category("Element")]
    public class addCourse
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "http://10.10.10.30:3000/";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(devUrl);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        //Test 0: Truy cập trang 
        [Test]
        public void studioTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = driver.FindElement(By.XPath("//button[.//span[text()='Course']]"));
            course.Click();

            Thread.Sleep(5000);
        }
        //Test 1: bấm nút vào course builder
        [Test]
        public void courseBuilder()
        {
            studioTest();
            IWebElement tab = driver.FindElement(By.XPath("//button[normalize-space()='5']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView(true);", tab);
            Thread.Sleep(3000);
            tab.Click();
            Thread.Sleep(5000);
            

            string expectedText = "CourseTest";


            IWebElement h2Element = driver.FindElement(By.XPath($"//h3[contains(text(),'{expectedText}')]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", h2Element);
            Thread.Sleep(3000);
            h2Element.Click();
            
            Thread.Sleep(3000);
        }
        //Test 2: Add New Element (UC-S118)
        [Test]
        public void videoDragNDrop()
        {
            courseBuilder();

            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'VideoBlock')]"));
            title.Click();
            Thread.Sleep(1000);
            IWebElement elementTab = driver.FindElement(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]"));
            elementTab.Click();
            Thread.Sleep(3000);

            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Video Block Icon']]"));

            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            Actions actions = new Actions(driver);
            actions
                    .MoveToElement(textElement)
                    .ClickAndHold()
                    .MoveToElement(targetElement)
                    .Release()
                    .Perform();

            Thread.Sleep(4000);
        }
        //5.1 Upload Video từ máy tính
        [Test]
        public void uploadVideo()
        {
            videoDragNDrop();
            IWebElement uploadField = driver.FindElement(By.XPath("//p[contains(@class,'text-xs truncate flex-1')]"));
            uploadField.Click();
            Thread.Sleep(3000);
            *//* IWebElement upload = driver.FindElement(By.XPath("//label[@for='video-audio']"));
             upload.Click();*//*

            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            //video test
            string vidPath = @"C:\Users\Hello\Videos\Captures\Home - Google Chrome 2024-12-13 10-32-40.mp4";

            inputFile.SendKeys(vidPath);

            Thread.Sleep(4000);


        }
        //5.2 Embed video từ URL

        [Test]
        public void embedVideo()
        {
            videoDragNDrop();
            IWebElement uploadbtn = driver.FindElement(By.CssSelector("div.text-gray-d10.cursor-pointer"));
            uploadbtn.Click();
            Thread.Sleep(3000);
            IWebElement embedLink = driver.FindElement(By.XPath("//button[normalize-space()='Embed link']"));
            embedLink.Click();

            IWebElement link = driver.FindElement(By.XPath("//input[@id='video-link']"));
            link.SendKeys("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerFun.mp4");
            Thread.Sleep(5000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Embed']"));
            submit.Click();
            Thread.Sleep(3000);
        }

        //5.3 Test toggle title và description 
        [Test]
        public void toggleTitleAndDescription()
        {
            embedVideo();
            Thread.Sleep(5000);
            IWebElement titleToggle = driver.FindElement(By.Id("video-title"));
            titleToggle.Click();
            Thread.Sleep(2000);
            IWebElement descToggle = driver.FindElement(By.Id("video-desciption"));
            descToggle.Click();
            Thread.Sleep(2000);
            IWebElement remove = driver.FindElement(By.CssSelector("button.inline-flex svg.lucide-trash2"));
            remove.Click();
            Thread.Sleep(2000);
        }
        //5.4 Thêm title, description
        [Test]
        public void addTitleAndDesc()
        {
            embedVideo();
            Thread.Sleep(4000);
            IWebElement titleField = driver.FindElement(By.XPath("//input[@placeholder='Add video title...']"));
            titleField.SendKeys("Title Video 123");
            Thread.Sleep(2000);
            IWebElement descField = driver.FindElement(By.XPath("//input[@placeholder='Write a description...']"));
            titleField.SendKeys("Description Video 123");
            Thread.Sleep(2000);
            IWebElement remove = driver.FindElement(By.CssSelector("button.inline-flex svg.lucide-trash2"));
            remove.Click();
            Thread.Sleep(2000);
        }

        public void Login()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            //IWebElement emailInput = driver.FindElement(By.Id("email"));
            IWebElement emailInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("email")));
            emailInput.SendKeys("info@kpim.vn");

            //IWebElement passwordInput = driver.FindElement(By.Id("password"));
            IWebElement passwordInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("password")));
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