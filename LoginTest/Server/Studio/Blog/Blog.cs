﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
namespace TestCompa.Server.Studio.Blog
{
    [TestFixture]
    [Category("Blog")]
    public class Blog
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string blogUrl = "http://10.10.10.30:3000/en/blog";
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


        [Test]
        public void CreateNew()
        {

            driver.Navigate().GoToUrl(blogUrl);
            Login();
            Thread.Sleep(2000);
            IWebElement Create = driver.FindElement(By.XPath("//button[normalize-space()='New blog']"));
            Create.Click();
            Thread.Sleep(1000);
        }

        [Test]
        public void SelectThumbnail()
        {
            driver.Navigate().GoToUrl(blogUrl);
            Login();
            Thread.Sleep(2000);
            IWebElement blog = driver.FindElement(By.XPath("//a[text()='Test']"));
            blog.Click();
            Thread.Sleep(1000);

            //Add thumbnail
            /*
               IWebElement upload = driver.FindElement(By.XPath("//button[normalize-space()='Upload from computer']"));
               upload.Click();
               Thread.Sleep(1000);*/
            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            //ảnh test
            string imgPath = @"C:\Users\Hello\Pictures\TestImage\Test1.jpg";

            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(imgPath);
            Thread.Sleep(5000);

        }
        //Edit Thumbnail 
        [Test]
        public void EditThumbnail()
        {

            SelectThumbnail();
            Thread.Sleep(2000);
            IWebElement edit = driver.FindElement(By.XPath("//button[text()='Edit image']"));
            edit.Click();
            Thread.Sleep(2000);

            IWebElement changeImg = driver.FindElement(By.XPath("//div[text()='Change image']"));
            changeImg.Click();
            Thread.Sleep(1000);

            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            //ảnh test
            string imgPath = @"C:\Users\Hello\Pictures\TestImage\loginBG.jpg";

            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(imgPath);
            Thread.Sleep(2000);
        }
        //Delete Thumbnail

        [Test]
        public void DeleteThumbnail()
        {
            SelectThumbnail();
            IWebElement edit = driver.FindElement(By.XPath("//button[text()='Edit image']"));
            edit.Click();
            Thread.Sleep(2000);

            IWebElement changeImg = driver.FindElement(By.XPath("//div[text()='Delete']"));
            changeImg.Click();

        }
        //Setting Blog
        [Test]
        public void EditSetting()
        {
            SelectThumbnail();
            //Mở Setting
            IWebElement btnSetting = driver.FindElement(By.CssSelector("button.group\\/btn.absolute.left-0.top-2"));
            btnSetting.Click();
            Thread.Sleep(500);
        }

        [Test]
        public void AddDescription()
        {
            EditSetting();
            IWebElement des = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//textarea[@id='description']")));
            des.SendKeys("Description");
            Thread.Sleep(500);
        }
        [Test]
        public void AddCategory()
        {
            SelectThumbnail();
            IWebElement btnSetting = driver.FindElement(By.CssSelector("button.group\\/btn.absolute.left-0.top-2"));
            btnSetting.Click();
            Thread.Sleep(2000);
            IWebElement dropdown = driver.FindElement(By.CssSelector(".lucide.lucide-chevrons-up-down.opacity-50"));
            dropdown.Click();
            Thread.Sleep(2000);

            //Chọn bất kì 1 category
            var items = driver.FindElements(By.XPath("//div[@role='option' and @cmdk-item]"));
            if (items.Count > 0)
            {
                var random = new Random();
                var randomItem = items[random.Next(items.Count)];
                randomItem.Click();

                string text = randomItem.Text;
                Console.WriteLine("Vừa bấm vào ->" + text);
            }
            Thread.Sleep(500);
        }

        [Test]
        public void EstimateReadTime()
        {
            SelectThumbnail();
            IWebElement btnSetting = driver.FindElement(By.CssSelector("button.group\\/btn.absolute.left-0.top-2"));
            btnSetting.Click();
            Thread.Sleep(2000);
            IWebElement ReadTime = driver.FindElement(By.XPath("//input[@id='estimatedReadTime']"));
            ReadTime.SendKeys("20");
            Thread.Sleep(500);
        }

        [Test]
        public void Tag()
        {
            SelectThumbnail();
            IWebElement btnSetting = driver.FindElement(By.CssSelector("button.group\\/btn.absolute.left-0.top-2"));
            btnSetting.Click();
            Thread.Sleep(2000);

            IWebElement tag = driver.FindElement(By.XPath("//span[@class='text-muted-foreground']"));
            tag.Click();
            Thread.Sleep(1000);

            IWebElement tagField = driver.FindElement(By.XPath("//input[@placeholder='Search tags...']"));
            tagField.SendKeys("Tag");
            Thread.Sleep(1000);
            tagField.SendKeys(Keys.Enter);


        }
        [Test]
        public void Preview()
        {
            driver.Navigate().GoToUrl(blogUrl);
            Login();
            Thread.Sleep(2000);
            IWebElement blog = driver.FindElement(By.XPath("//a[text()='Test']"));
            blog.Click();
            Thread.Sleep(1000);
            IWebElement previewBtn = driver.FindElement(By.CssSelector(".lucide.lucide-play"));
            previewBtn.Click();
            Thread.Sleep(2000);

            IWebElement back = driver.FindElement(By.CssSelector(".lucide.lucide-pause"));
            back.Click();
            Thread.Sleep(2000);
        }

        [Test]
        public void Text()
        {
            SelectThumbnail();
            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Text Block Icon']]"));

            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            Actions actions = new(driver);
            actions.DragAndDrop(textElement, targetElement).Perform();

            Thread.Sleep(3000);

            IWebElement edit = driver.FindElement(By.XPath("//p[@class='ck-placeholder']"));
            edit.Click();
            Thread.Sleep(2000);

            edit.SendKeys("CONTENT");//text
            Thread.Sleep(2000);
            edit.SendKeys(Keys.Control + 'a');

            edit.SendKeys(Keys.Control + 'b');//bold
            edit.SendKeys(Keys.Control + 'i');//italic
            edit.SendKeys(Keys.Control + 'u');//underline
        }
        private void Login()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            //IWebElement emailInput = driver.FindElement(By.Id("email"));
            IWebElement emailInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("email")));
            emailInput.SendKeys("info@kpim.vn");

            //IWebElement passwordInput = driver.FindElement(By.Id("password"));
            IWebElement passwordInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("password")));
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
