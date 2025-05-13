using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace TestCompa.Production.CourseBuilder.Image
{
    [TestFixture]
    [Category("Element")]
    public class Image
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "https://studio.compaclass.com/";
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
            InitDriver(true);
        }

        public void StudioTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = driver.FindElement(By.XPath("//button[.//span[text()='Course']]"));
            course.Click();

            Thread.Sleep(5000);
        }
        [Test]

        public void CourseBuilder()
        {
            StudioTest();
            IWebElement tab = driver.FindElement(By.XPath("//button[normalize-space()='1']"));
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
        //Test 2: Test Image (UC-S108)
        [Test]
        public void ImgDragNDrop()
        {
            CourseBuilder();
            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'ImageBlock')]"));
            title.Click();
            Thread.Sleep(1000);
            IWebElement elementTab = driver.FindElement(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]"));
            elementTab.Click();
            Thread.Sleep(3000);

            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Image Block Icon']]"));

            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            Actions actions = new(driver);
            actions.DragAndDrop(textElement, targetElement).Perform();

            Thread.Sleep(4000);
        }
        //3 test upload ảnh ( Single Mode )  (UC-S109)
        [Test]
        public void ImgUpload()
        {
            ImgDragNDrop();

            IWebElement clickAddImg = driver.FindElement(By.XPath("//div[contains(@class,'bg-gray-100 flex items-center')]"));
            Actions action = new(driver);
            action.DoubleClick(clickAddImg).Perform();
            Thread.Sleep(2000);

            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            //ảnh test
            string imgPath = @"C:\Users\Hello\Pictures\TestImage\Test1.jpg";

            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(imgPath);
            Thread.Sleep(2000);

        }
        //4. Test upload Ảnh ( Mutiple ) (UC-S110) (chưa có đang ở chế độ Disable) 
        //5. Xóa ảnh ( Single Mode ) (UC-S111)
        [Test]
        public void DeleteImgUpload()
        {
            ImgUpload();
            Thread.Sleep(2000);
            IWebElement removeButton = driver.FindElement(By.XPath("//button[normalize-space()='Remove']"));

            // Scroll nếu cần
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", removeButton);
            Thread.Sleep(500);

            // Click bằng JavaScript
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", removeButton);
            Thread.Sleep(3000);
        }
        //6. Xóa ảnh ( Multiple Mode ) (UC-S112) ( Disable ) 
        //7. Thêm, sửa caption (SM) (UC-S113) 
        [Test]
        public void CaptionImg()
        {
            ImgUpload();
            IWebElement toggleButton = driver.FindElement(By.XPath("//div[contains(@class, 'flex items-center justify-between')]//button[@role='switch']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", toggleButton);
            Thread.Sleep(500);
            toggleButton.Click();

            Thread.Sleep(2000);
            IWebElement captionInput = driver.FindElement(By.CssSelector("input[placeholder='Write a caption...']"));
            captionInput.SendKeys("Caption For IMG");
            Thread.Sleep(500);
            //update caption
            captionInput.SendKeys(Keys.Control + 'a');
            Thread.Sleep(1000);
            captionInput.SendKeys("UpdateCaption");
            Thread.Sleep(2000);
        }
        //8.THêm, sửa caption (MM) (UC-S114) (Disable)
        //9.Thay đổi ảnh ( SM) (UC-S115) 
        [Test]
        public void ReplaceImg()
        {
            ImgUpload();
            IWebElement change = driver.FindElement(By.XPath("//button[normalize-space()='Change']"));
            change.Click();
            Thread.Sleep(500);
            IWebElement embed = driver.FindElement(By.XPath("//button[normalize-space()='Embed link']"));
            embed.Click();
            Thread.Sleep(500);
            IWebElement link = driver.FindElement(By.CssSelector("input[placeholder='Paste the image link']"));
            link.SendKeys("https://picsum.photos/id/1/200/300");
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Embed']"));
            submit.Click();
            Thread.Sleep(2000);
        }
        //9.Thay đổi ảnh ( MM) (UC-S116) 
        //10. Chỉnh sửa ảnh (UC-S117)
        [Test]
        public void ChangeSizeImg()
        {
            ImgUpload();



            Thread.Sleep(2000);
        }


        //3.2 Sử dụng embed (link)
        [Test]
        public void ImgEmbed()
        {
            InitDriver(false);
            ImgDragNDrop();
            IWebElement clickAddImg = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'bg-gray-100 flex items-center')]")));
            Actions action = new(driver);
            action.DoubleClick(clickAddImg).Perform();
            Thread.Sleep(3000);
            IWebElement embed = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[normalize-space()='Embed link']")));
            embed.Click();
            IWebElement embedImg = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("input[placeholder = 'Paste the image link']")));
            embedImg.SendKeys("https://media.istockphoto.com/id/814423752/photo/eye-of-model-with-colorful-art-make-up-close-up.jpg?s=612x612&w=0&k=20&c=l15OdMWjgCKycMMShP8UK94ELVlEGvt7GmB_esHWPYE=");
            Thread.Sleep(1000);
            IWebElement submit = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[normalize-space()='Embed']")));
            submit.Click();
            IWebElement remove = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[normalize-space()='Remove']")));
            remove.Click();
            Thread.Sleep(2000);

        }
        //3.3 Thay đổi ảnh
        [Test]
        public void ChangeImg()
        {
            ImgDragNDrop();

            IWebElement clickAddImg = driver.FindElement(By.XPath("//div[contains(@class,'bg-gray-100 flex items-center')]"));
            Actions action = new(driver);
            action.DoubleClick(clickAddImg).Perform();
            Thread.Sleep(3000);

            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            //ảnh test
            string imgPath = @"C:\Users\Hello\Pictures\TestImage\pexels-anjana-c-169994-674010.jpg";

            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(imgPath);
            Thread.Sleep(5000);

            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(5));
            IWebElement changeButton = wait.Until(d => d.FindElement(By.XPath("//button[normalize-space()='Change']")));
            changeButton.Click();
            Thread.Sleep(5000);

            IWebElement embed = driver.FindElement(By.XPath("//button[normalize-space()='Embed link']"));
            embed.Click();
            Thread.Sleep(4000);
            IWebElement embedImg = driver.FindElement(By.CssSelector("input[placeholder = 'Paste the image link']"));
            embedImg.SendKeys("https://media.istockphoto.com/id/814423752/photo/eye-of-model-with-colorful-art-make-up-close-up.jpg?s=612x612&w=0&k=20&c=l15OdMWjgCKycMMShP8UK94ELVlEGvt7GmB_esHWPYE=");
            Thread.Sleep(1000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Embed']"));
            submit.Click();
            Thread.Sleep(3000);


        }
        //Test 3.4 Đổi ảnh nhưng đường dẫn sai 
        [Test]
        public void InvalidUrlImg()
        {
            ImgDragNDrop();

            IWebElement clickAddImg = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'bg-gray-100 flex items-center')]")));
            Actions action = new(driver);
            action.DoubleClick(clickAddImg).Perform();
            Thread.Sleep(3000);

            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            //ảnh test
            string imgPath = @"C:\Users\Hello\Pictures\TestImage\pexels-anjana-c-169994-674010.jpg";

            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(imgPath);
            Thread.Sleep(3000);


            IWebElement changeButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[normalize-space()='Change']")));
            changeButton.Click();
            Thread.Sleep(5000);

            IWebElement embed = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[normalize-space()='Embed link']")));
            embed.Click();
            Thread.Sleep(2000);
            IWebElement embedImg = driver.FindElement(By.CssSelector("input[placeholder = 'Paste the image link']"));
            embedImg.SendKeys("abcxyz");
            Thread.Sleep(1000);
            IWebElement submit = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[normalize-space()='Embed']")));
            submit.Click();
            Thread.Sleep(3000);
            //thông báo lỗi
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[@class='text-red text-xs']")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập sai thông tin!");
            Thread.Sleep(2000);

        }
        //Test 3.5 Điều chỉnh ảnh 
        [Test]
        public void EditSizeImg()
        {
            ImgDragNDrop();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            // Đợi phần tử "Click to add img" xuất hiện
            IWebElement clickAddImg = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//div[contains(@class,'bg-gray-100 flex items-center')]")));

            // Double click để mở file input
            Actions action = new Actions(driver);
            action.DoubleClick(clickAddImg).Perform();

            // Đợi input file xuất hiện
            IWebElement inputFile = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//button[normalize-space()='Embed link']")));
            inputFile.Click();
            Thread.Sleep(1000);
            inputFile.SendKeys("https://picsum.photos/200/300");
            Thread.Sleep(1000);

        }


        private void Login()
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
