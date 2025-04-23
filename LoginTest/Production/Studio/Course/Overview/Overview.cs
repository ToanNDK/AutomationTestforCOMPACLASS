using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using TestCompa.Utilities;
using static OpenQA.Selenium.BiDi.Modules.Input.Pointer;
using OpenQA.Selenium.Interactions;

namespace TestCompa.Production.CourseBuilder.Overview
{
    public class Overview
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string StudioUrl = "https://studio.compaclass.com";


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
            driver.Navigate().GoToUrl(StudioUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = driver.FindElement(By.XPath("//button[.//span[text()='Course']]"));
            course.Click();

            Thread.Sleep(2000);
        }

        public void CourseBuilder()
        {
            StudioTest();
            IWebElement tab = driver.FindElement(By.XPath("//button[normalize-space()='1']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView(true);", tab);
            Thread.Sleep(3000);
            tab.Click();
            Thread.Sleep(3000);


            string expectedText = "CourseTest";


            IWebElement h2Element = driver.FindElement(By.XPath($"//h3[contains(text(),'{expectedText}')]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", h2Element);
            Thread.Sleep(3000);
            h2Element.Click();

            Thread.Sleep(3000);
        }

        //Test 1: Add Description
        [Test]
        public void AddDescription()
        {
            CourseBuilder();
            IWebElement description = driver.FindElement(By.XPath("//textarea[@id='description']"));
            description.Clear();
            description.SendKeys("Description Test");
            Thread.Sleep(2000);
        }
        //Test 2 : Add Infomation (What wil you learn ) ( UC-S088 )  
        [Test]
        public void AddInfomation()
        {
            CourseBuilder();
            IWebElement info = driver.FindElement(By.XPath("//input[@class='flex w-full bg-white rounded-md border ring-offset-white border-gray-l20 file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-darkGray focus-visible:outline-none focus-visible:ring-0 disabled:cursor-not-allowed disabled:opacity-50 focus-visible:border-primary-l40 h-10 px-3 py-2 text-sm flex-1 opacity-70']"));
            info.SendKeys("INFOMATION Test");
            Thread.Sleep(1000);
        }
        //Test 2.1 Delete Infomation (UC-S089)
        [Test]
        public void DeleteInfomation()
        {
            AddInfomation();
            IWebElement delete = driver.FindElement(By.CssSelector("button:has(svg.lucide.lucide-minus.text-red)"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView(true);", delete);
            Thread.Sleep(2000);
            delete.Click();
            Thread.Sleep(2000);
        }
        //Test 3: About the course 
        [Test]
        public void AboutTheCourse()
        {
            CourseBuilder();
            //nhập content vào ckeditor
            IWebElement ckeditor = driver.FindElement(By.CssSelector("div.ck-editor__editable"));
            ckeditor.Clear();
            ckeditor.SendKeys("INFOMATION Test");
            Thread.Sleep(1000);
        }
        //Test 3.1 About the course (Heading) 
        [Test]
        public void HeadingAboutTheCourse()
        {
            AboutTheCourse();
            Thread.Sleep(2000);
            IWebElement heading = driver.FindElement(By.XPath("//button[@role='combobox' and @type='button']"));
            heading.Click();
            Thread.Sleep(2000);
            IList<IWebElement> headingOptions = driver.FindElements(By.CssSelector("span.flex-1"));

            Random random = new();
            int randomIndex = random.Next(headingOptions.Count); 

            IWebElement randomHeading = headingOptions[randomIndex];
            randomHeading.Click();

        }
        //Test 3.2 Bold, Italic, Underline, Strikethrough 
        [Test]
        public void TextFormat()
        {
            AboutTheCourse();
            IWebElement ckeditor = driver.FindElement(By.CssSelector("div.ck-editor__editable"));
            ckeditor.SendKeys(Keys.Control+'a');
            Thread.Sleep(1000);
            ckeditor.SendKeys(Keys.Control+'b');
            Thread.Sleep(1000);
            ckeditor.SendKeys(Keys.Control + 'u');
            Thread.Sleep(1000);
            ckeditor.SendKeys(Keys.Control + 'i');
            Thread.Sleep(1000);
            IWebElement strikethrough = driver.FindElement(By.CssSelector(".lucide.lucide-strikethrough"));
            strikethrough.Click();
            Thread.Sleep(2000);
        }
        //Test 4: Quote 
        [Test]
        public void Quote()
        {
            AboutTheCourse();
            IWebElement Quote = driver.FindElement(By.CssSelector(".lucide.lucide-quote"));
            Quote.Click();
            Thread.Sleep(2000);
            IWebElement List = driver.FindElement(By.CssSelector(".lucide.lucide-list"));
            List.Click();
            Thread.Sleep(2000);
        }
        //Test 5: Add Teacher (UC-S090)
        [Test]
        public void AddTeacher()
        {
            CourseBuilder();
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(2000);
            IWebElement add = driver.FindElement(By.CssSelector("svg.lucide.lucide-plus.text-primary"));
            add.Click();
            Thread.Sleep(2000);

            IWebElement teacherEmail = driver.FindElement(By.XPath("//input[@placeholder='Email address or name']"));
            teacherEmail.SendKeys("lozy564");
            Thread.Sleep(3000);
            IWebElement confirm = driver.FindElement(By.XPath("//div[contains(@class,'flex flex-col text-sm')]"));
            confirm.Click();
            Thread.Sleep(2000);

        }
        //Test 5.1 Xem Profile Teacher (UC-S091)
        [Test]
        public void ProfileTeacher()
        {
            AddTeacher();

            // 1. Lưu lại tab hiện tại
            string originalTab = driver.CurrentWindowHandle;

            // 2. Click vào "TeacherAssistant" để mở profile
            IWebElement profile = driver.FindElement(By.XPath("//a[normalize-space()='TeacherAssistant']"));
            profile.Click();

            // 3. Đợi tab mới mở ra
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.WindowHandles.Count > 1);

            // 4. Switch sang tab mới
            var allTabs = driver.WindowHandles;
            foreach (var tab in allTabs)
            {
                if (tab != originalTab)
                {
                    driver.SwitchTo().Window(tab);
                    break;
                }
            }

            // 5. Đợi URL load xong
            wait.Until(driver => driver.Url.Contains("compaclass.com/learn/public-profile"));

            Assert.That(driver.Url.StartsWith("https://compaclass.com/learn/public-profile"), "URL is not correct after switching to profile tab.");

            driver.Close();
            driver.SwitchTo().Window(originalTab);
        }
        /*//Test 5.2 Delete teacher (UC-S092)
        [Test]
        public void DeleteTeacher()
        {
            AddTeacher();

            // Click vào nút Add
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Add']"));
            submit.Click();
            Thread.Sleep(2000);  // Đợi để giao diện ổn định

            // Hover vào avatar để thùng rác hiện ra
            Actions action = new Actions(driver);
            IWebElement avatar = driver.FindElement(By.XPath("//a[normalize-space()='toanndk']"));
            action.MoveToElement(avatar).Perform();
            Thread.Sleep(2000);  // Đợi để thùng rác xuất hiện

            // Kiểm tra xem thùng rác có xuất hiện chưa, nếu có thì click vào
            IWebElement trashIcon = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(
                By.XPath("//button[.//svg[contains(@class,'lucide-trash2')]]")));

            // Click vào thùng rác
            trashIcon.Click();

            Thread.Sleep(2000);  // Đợi sau khi click
        }
*/


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
