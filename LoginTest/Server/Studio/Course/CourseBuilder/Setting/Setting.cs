﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Studio.Course.CourseBuilder.Setting
{
    [TestFixture]
    [Category("CourseSetting")]
    public class Setting
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
            InitDriver(true);
        }
        //Test 0: Truy cập trang 
        [Test]
        public void StudioTest()
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
        public void CourseBuilder()
        {
            StudioTest();

            Thread.Sleep(5000);

            string expectedText = "CourseTest";


            IWebElement h2Element = driver.FindElement(By.XPath($"//h3[contains(text(),'{expectedText}')]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", h2Element);
            h2Element.Click();
            /*IWebElement parentDiv = h2Element.FindElement(By.XPath("./ancestor::div[contains(@class, 'flex justify-between')]"));


            IWebElement svgButton = parentDiv.FindElement(By.XPath(".//button[contains(@class, 'group/btn')]"));


            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", svgButton);
            Thread.Sleep(1000);


            svgButton.Click();
            Thread.Sleep(2000);
            IWebElement showVersions = driver.FindElement(By.CssSelector("div[role='menuitem']"));
            showVersions.Click();
            Thread.Sleep(5000);
            IWebElement toBuilder = driver.FindElement(By.XPath("//button[contains(@class, 'group/btn') and contains(@class, 'h-6') and contains(@class, 'w-6')]"));
            toBuilder.Click();
            Thread.Sleep(3000);
            IWebElement Builder = driver.FindElement(By.XPath("//div[normalize-space()='Builder page']"));
            Builder.Click();*/
            Thread.Sleep(3000);
        }
        //Test 2:Bấm tab Setting
        [Test]
        public void SettingBtn()
        {
            CourseBuilder();

            IWebElement setting = driver.FindElement(By.XPath("//span[normalize-space()='Setting']"));
            setting.Click();
            Thread.Sleep(5000);
        }

        //Test 3: Đổi banner, thumbnail, mobilethumbnail
        [Test]
        public void changeImg()
        {

            SettingBtn();
            string imgPath = @"C:\Users\Hello\Pictures\TestImage\pexels-anjana-c-169994-674010.jpg";

            IWebElement thumbnail = driver.FindElement(By.Id("thumbnail"));
            thumbnail.SendKeys(imgPath);
            IWebElement mThumbnail = driver.FindElement(By.Id("mThumbnail"));
            mThumbnail.SendKeys(imgPath);
            Thread.Sleep(5000);

        }
        //Test 4: Course Level

        [Test]
        public void CourseLevel()
        {
            InitDriver(false);
            SettingBtn();
            IWebElement CourseLevel = driver.FindElement(By.XPath("(//button[@role='combobox' and @aria-expanded='false'])[1]"));

            CourseLevel.Click();
            Thread.Sleep(2000);
            IReadOnlyCollection<IWebElement> options = driver.FindElements(By.XPath("//select/option"));
            if (options.Count > 0)
            {
                Random rand = new();
                int randomIndex = rand.Next(options.Count);


                IWebElement randomOption = options.ElementAt(randomIndex);
                string optionValue = randomOption.GetAttribute("value");

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].selected = true;", randomOption);

                Console.WriteLine("Selected option: " + optionValue);
            }
            else
            {
                Console.WriteLine("Không có tùy chọn nào trong combobox!");
            }

        }
        //Test 5: Sửa topic
        [Test]
        public void Topic()
        {
            SettingBtn();

            IWebElement Topic = driver.FindElement(By.XPath("(//button[contains(@role,'combobox')])[3]"));
            Topic.Click();
            Random rand = new();
            int randomValue = rand.Next(1, 4);

            string optionSelector = $"option[value='{randomValue}']";
            Console.WriteLine(optionSelector);
            IWebElement option = driver.FindElement(By.CssSelector(optionSelector));
            option.Click();
            Thread.Sleep(5000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(5000);
        }
        //Test 6: Nhập Tag
        [Test]
        public void Tag()
        {
            SettingBtn();
            IWebElement fieldTag = driver.FindElement(By.XPath("//input[@placeholder='Search tag...']"));

            fieldTag.Click();
            Random tagRandom = new();

            fieldTag.SendKeys(tagRandom.ToString());
            fieldTag.SendKeys(Keys.Enter);
            Thread.Sleep(5000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(3000);
        }

        //Test 7 : Thay đổi Slug
        [Test]
        public void Slug()
        {
            SettingBtn();
            IWebElement fieldSlug = driver.FindElement(By.Name("courseSlug"));

            fieldSlug.Click();
            Thread.Sleep(1000);
            fieldSlug.SendKeys(Keys.Control + 'a');
            fieldSlug.SendKeys(Keys.Backspace);

            Thread.Sleep(3000);
            Random rd = new();

            fieldSlug.SendKeys($"course-test-slug {rd.Next(1000, 9999)}");
            Thread.Sleep(3000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(3000);
        }
        //

        /* //Test 6: Đổi tên Chapter
         [Test]
         public void createNewCoursewithoutName()
         {
             CourseBuilder();
             IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'Chapter 1:')]"));
             title.Click();
             Thread.Sleep(1000);
             IWebElement titleField = driver.FindElement(By.Id("rowTitleControl"));
             titleField.SendKeys(Keys.Control + 'a');
             titleField.SendKeys(Keys.Backspace);
             titleField.SendKeys("TextBlock");
             titleField.SendKeys(Keys.Enter);
             Thread.Sleep(5000);
         }*/
        //Test 7: Thêm Unit


        private void Login()
        {
            Thread.Sleep(2000);
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
