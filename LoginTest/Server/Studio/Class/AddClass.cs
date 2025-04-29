/*using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Studio
{
    public class addClass
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
        public void classTest()
        {
            driver.Navigate().GoToUrl(devUrl);

            Login();
            IWebElement classes = driver.FindElement(By.CssSelector("svg[width='20'][height='20'][viewBox='0 0 18 22']"));
            classes.Click();
            Thread.Sleep(5000);
        }
        //Test 1: bấm nút tạo lớp học mới
        [Test]
        public void btnNewClass()
        {
            classTest();
            IWebElement create = driver.FindElement(By.CssSelector("button[class='py-[10px] px-5 group w-fit flex items-center justify-start gap-2 bg-white rounded-xl hover:bg-primary transition-all']"));
            Actions action = new(driver);
            action.Click(create).Perform();
            Thread.Sleep(5000);
        }
        //Test 2: Tạo lớp mới thành công
        [Test]
        public void createNewCourse()
        {
            Create();
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();

        }
        //Test 3: Test nút Cancel
        [Test]
        public void btnCancelCreate()
        {
            Create();
            IWebElement cancel = driver.FindElement(By.CssSelector("button[class='px-4 py-3 min-w-[100px] lg:min-w-[120px] flex justify-center items-center gap-2 bg-gray-d10 rounded-lg transition-all text-center text-white text-base font-medium hover:bg-red']"));
            cancel.Click();
        }
        //Test 4: Không nhập giáo viên
        [Test]
        public void createNewClasswithoutTeacher()
        {
            Create();
            IWebElement clear = driver.FindElement(By.CssSelector("svg[width='20'][height='20'][viewBox='0 0 30 30']"));
            clear.Click();
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(5000);
            IWebElement errorMessage = driver.FindElement(By.XPath("//span[@class='text-red font-sm' and contains(text(), 'Please provide at least one email address')]"));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập thiếu thông tin!");

        }
        //Test 5: Không nhập tên class
        [Test]
        public void createNewClasswithoutName()
        {
            Create();
            IWebElement clear = driver.FindElement(By.XPath("//input[@name='name']"));
            clear.Click();
            Thread.Sleep(2000);
            clear.Clear();
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(5000);
            IWebElement errorMessage = driver.FindElement(By.XPath("//span[contains(@class, 'text-red') and contains(text(), 'You must not leave this field blank')]"));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập thiếu thông tin!");
        }
        //Test 6:Không nhập description
        [Test]
        public void createNewClasswithoutDescription()
        {
            Create();
            IWebElement clear = driver.FindElement(By.CssSelector("textarea[placeholder='Enter Description']"));
            clear.Click();
            Thread.Sleep(5000);
            clear.Clear();
            Thread.Sleep(10000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(8000);
            IWebElement errorMessage = driver.FindElement(By.XPath("div[class='flex space-x-1 items-center' and contains(text(), 'You must not leave this field blank')]"));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập thiếu thông tin!");
        }
        //Test 7: Không chọn course và version
        [Test]
        public void createNewClasswithoutCourse()
        {
            btnNewClass();

            IWebElement teacher = driver.FindElement(By.XPath("//div[@id='email-search-container-teacher']//input"));
            teacher.SendKeys("lozy564@gmail.com");
            Thread.Sleep(2000);


            IWebElement teacherSuggestion = driver.FindElement(By.XPath("//div[contains(@class, 'bg-white border rounded-lg shadow-lg')]//p[contains(text(), 'lozy564@gmail.com')]"));
            teacherSuggestion.Click();
            Thread.Sleep(5000);


            IWebElement teacherAssistant = driver.FindElement(By.XPath("//div[@id='email-search-container-ta']//input"));
            teacherAssistant.SendKeys("lozik480@gmail.com");
            Thread.Sleep(2000);


            IWebElement assistantSuggestion = driver.FindElement(By.XPath("//div[contains(@class, 'bg-white border rounded-lg shadow-lg')]//p[contains(text(), 'lozik480@gmail.com')]"));
            assistantSuggestion.Click();
            Thread.Sleep(2000);


            IWebElement name = driver.FindElement(By.CssSelector("input[placeholder='Enter Name']"));
            name.SendKeys("ClassTest");
            Thread.Sleep(2000);


            IWebElement description = driver.FindElement(By.CssSelector("textarea[placeholder='Enter Description']"));
            description.SendKeys("Description");

            Thread.Sleep(3000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(8000);
            IWebElement errorMessage = driver.FindElement(By.XPath("//span[contains(@class, 'text-red') and contains(text(), 'You must not leave this field blank')]"));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập thiếu thông tin!");

        }
        //Test 8: Nhập description quá dài
        [Test]
        public void createNewClassDescriptionTooLong()
        {
            Create();
            string longDes = new('a', 301);
            IWebElement clear = driver.FindElement(By.CssSelector("textarea[placeholder='Enter Description']"));
            clear.Click();
            Thread.Sleep(5000);
            clear.Clear();
            Thread.Sleep(3000);
            clear.Click();
            clear.SendKeys(longDes);
            Thread.Sleep(5000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(8000);
            IWebElement errorMessage = driver.FindElement(By.XPath("div[class='flex space-x-1 items-center' and contains(text(), 'You must not leave this field blank')]"));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị khi nhập thiếu thông tin!");
        }// chỉ cho phép nhập đúng 300 kí tự -> Passed



        public void Create()
        {
            btnNewClass();


            IWebElement courseButton = driver.FindElement(By.XPath("//span[normalize-space()='Select Course']"));
            courseButton.Click();
            Thread.Sleep(3000);


            IWebElement dropdownList = driver.FindElement(By.CssSelector("div[data-radix-popper-content-wrapper]"));
            Thread.Sleep(3000);

            IWebElement firstOption = dropdownList.FindElements(By.CssSelector("div[role='option']")).Last();
            firstOption.Click();
            Thread.Sleep(2000);


            IWebElement courseVersion = driver.FindElement(By.XPath("//button[@role='combobox'][span[contains(text(),'Select Course Version')]]"));
            courseVersion.Click();
            Thread.Sleep(3000);


            IWebElement dropdownVersionList = driver.FindElement(By.CssSelector("div[data-radix-popper-content-wrapper]"));
            Thread.Sleep(2000);


            IWebElement firstOptionVersion = dropdownVersionList.FindElements(By.CssSelector("div[role='option']")).First();
            firstOptionVersion.Click();
            Thread.Sleep(2000);


            IWebElement teacher = driver.FindElement(By.XPath("//div[@id='email-search-container-teacher']//input"));
            teacher.SendKeys("lozy564@gmail.com");
            Thread.Sleep(2000);


            IWebElement teacherSuggestion = driver.FindElement(By.XPath("//div[contains(@class, 'bg-white border rounded-lg shadow-lg')]//p[contains(text(), 'lozy564@gmail.com')]"));
            teacherSuggestion.Click();
            Thread.Sleep(5000);


            IWebElement teacherAssistant = driver.FindElement(By.XPath("//div[@id='email-search-container-ta']//input"));
            teacherAssistant.SendKeys("lozik480@gmail.com");
            Thread.Sleep(2000);


            IWebElement assistantSuggestion = driver.FindElement(By.XPath("//div[contains(@class, 'bg-white border rounded-lg shadow-lg')]//p[contains(text(), 'lozik480@gmail.com')]"));
            assistantSuggestion.Click();
            Thread.Sleep(2000);


            IWebElement name = driver.FindElement(By.CssSelector("input[placeholder='Enter Name']"));
            name.SendKeys("ClassTest");
            Thread.Sleep(2000);


            IWebElement description = driver.FindElement(By.CssSelector("textarea[placeholder='Enter Description']"));
            description.SendKeys("Description");
        }
        public void Login()
        {
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