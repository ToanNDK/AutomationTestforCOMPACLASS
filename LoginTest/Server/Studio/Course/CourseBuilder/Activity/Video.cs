using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using TestCompa.Utilities;
using static OpenQA.Selenium.BiDi.Modules.Input.Pointer;

namespace TestCompa.Server.CourseBuilder.Activity.Video
{
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

        public void studioTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = driver.FindElement(By.XPath("//button[.//span[text()='Course']]"));
            course.Click();

            Thread.Sleep(5000);
        }

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
        
        
        [Test]
        public void videoActivity()
        {
            courseBuilder();
            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'VideoBlock')]"));
            title.Click();
            Thread.Sleep(1000);
            IWebElement video = driver.FindElement(By.XPath("//button[normalize-space()='Activity']"));
            video.Click();
            Thread.Sleep(2000);
            IWebElement videoOption = driver.FindElement(By.XPath("//span[@class='font-medium select-none'][normalize-space()='Video']"));
            videoOption.Click();
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Continue']"));
            submit.Click();
            Thread.Sleep(1000);
            IWebElement elementTab = driver.FindElement(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]"));
            elementTab.Click();
            Thread.Sleep(2000);
            IWebElement uploadField = driver.FindElement(By.XPath("//p[@class='text-xs truncate flex-1 ']"));
            uploadField.Click();
            Thread.Sleep(1000);

        }
        //Test 1: Test upload video từ local (UC-S191)
        [Test]
        public void UploadVideo()
        {
            videoActivity();
            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));

            string vidPath = @"C:\Users\Hello\Videos\Screen Recordings\15-2-2025.mp4";
            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(vidPath);
            Thread.Sleep(2000);
        }
        //2. Test upload video từ URL (UC-S192)
        [Test]
        public void UploadVideoURL()
        {
            videoActivity();
            IWebElement embed = driver.FindElement(By.XPath("//button[normalize-space()='Embed link']"));
            embed.Click();
            IWebElement link = driver.FindElement(By.XPath("//input[@id='video-link']"));
            link.SendKeys("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4");
            // Gửi đường dẫn ảnh vào input file
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Embed']"));
            submit.Click();
            Thread.Sleep(2000);
        }
        //3. Preview Video (UC-S193)
        [Test]
        public void previewVideo()
        {
            UploadVideoURL();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement video = wait.Until(driver => driver.FindElement(By.XPath("//video[@preload='metadata']")));

            // Dùng JavaScript để click, bỏ qua overlay
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", video);

            Thread.Sleep(2000); // Hoặc xác nhận video đang phát
        }
        //4. Replace video (UC-S198)
        [Test]
        public void replaceVideo()
        {
            UploadVideoURL();
            IWebElement upload = driver.FindElement(By.XPath("//div[normalize-space(text())='Upload']"));
            upload.Click();
            Thread.Sleep(3000);
            
            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));

            string vidPath = @"C:\Users\Hello\Videos\Screen Recordings\15-2-2025.mp4";
            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(vidPath);
            Thread.Sleep(3000);
        }
        //5. Replace video (Embed) (UC-S199)
        [Test]
        public void replaceVideoURL()
        {
            UploadVideoURL();
            

            IWebElement remove = driver.FindElement(By.XPath("//div[@title='Remove']"));
            remove.Click();
            
            Thread.Sleep(3000);
            IWebElement upload = driver.FindElement(By.XPath("//div[normalize-space(text())='Upload']"));
            upload.Click();
            Thread.Sleep(3000);
            IWebElement embed = driver.FindElement(By.XPath("//button[normalize-space()='Embed link']"));
            embed.Click();
            IWebElement link = driver.FindElement(By.XPath("//input[@id='video-link']"));
            link.SendKeys("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4");
            // Gửi đường dẫn ảnh vào input file
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Embed']"));
            submit.Click();
            Thread.Sleep(3000);
        }
        //6. Remove video (UC-S200)
        [Test]
        public void removeVid()
        {
            UploadVideoURL();


            IWebElement remove = driver.FindElement(By.XPath("//div[@title='Remove']"));
            remove.Click();

            Thread.Sleep(3000);
        }
        //7. Add custom Thumbnail (UC-S202)
        [Test]
        public void addThumbnail()
        {
            UploadVideoURL();
            IWebElement uploadButton = driver.FindElement(By.XPath("//div[div[text()='Custom thumbnail']]//div[text()='Upload']"));
            uploadButton.Click();
            Thread.Sleep(2000);
            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));

            string thumbnail = @"C:\Users\Hello\Pictures\TestImage\loginBG.jpg";
            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(thumbnail);
            Thread.Sleep(2000);
        }
        //8.Replace thumbnail (UC-S203)
        [Test]
        public void editThumbnail()
        {
            UploadVideoURL();
            IWebElement uploadButton = driver.FindElement(By.XPath("//div[div[text()='Custom thumbnail']]//div[text()='Upload']"));
            uploadButton.Click();
            Thread.Sleep(2000);
            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));

            string thumbnail = @"C:\Users\Hello\Pictures\TestImage\loginBG.jpg";
            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(thumbnail);
            Thread.Sleep(5000);
            uploadButton.Click();
            IWebElement embed = driver.FindElement(By.XPath("//button[normalize-space()='Embed link']"));
            embed.Click();

            Thread.Sleep(2000);
            IWebElement link = driver.FindElement(By.XPath("//input[@id='thumbnail-link']"));
            link.SendKeys("https://picsum.photos/200/300");
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Embed']"));
            submit.Click();
            Thread.Sleep(3000);
        }//9. Custom Description (UC-S205)
        [Test]
        public void customDescription()
        {
            UploadVideoURL();
            IWebElement txtDes = driver.FindElement(By.XPath("//textarea[@id='message-Description']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", txtDes);
            Thread.Sleep(500);
            txtDes.Click();
            Thread.Sleep(2000);
            txtDes.SendKeys("00:30");
            txtDes.SendKeys(Keys.Enter);
            Thread.Sleep(1000);
            txtDes.SendKeys("00:55");
            txtDes.SendKeys(Keys.Enter);
            Thread.Sleep(1000);
            txtDes.SendKeys("00:40");
            txtDes.SendKeys(Keys.Enter);
            Thread.Sleep(1000); 
            txtDes.SendKeys("00:40");
            txtDes.SendKeys(Keys.Enter);
            Thread.Sleep(3000);
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
