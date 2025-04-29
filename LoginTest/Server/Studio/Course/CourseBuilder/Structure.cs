using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Studio.Course.CourseBuilder
{
    public class Structure
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
        //Test 2: Tạo Chapter
        [Test]
        public void CreateNewChapter()
        {
            CourseBuilder();
            IWebElement btnCreate = driver.FindElement(By.XPath("//button[normalize-space()='Chapter']"));
            for (int i = 0; i < 3; i++)
            {
                btnCreate.Click();
                Thread.Sleep(1000);
            }
            IWebElement chapterDelete = driver.FindElement(By.XPath("//p[normalize-space()='Chapter 2: Untitled']"));
            chapterDelete.Click();
            Thread.Sleep(2000);
            Actions acitons = new(driver);

            acitons.MoveToElement(chapterDelete);
            Thread.Sleep(5000);
            IWebElement svgDelete = driver.FindElement(By.CssSelector("button[aria-haspopup='dialog'] svg.lucide-ellipsis"));
            svgDelete.Click();
            Thread.Sleep(3000);
            IWebElement delete = driver.FindElement(By.XPath("//button[normalize-space()='Delete']"));
            delete.Click();
            btnCreate.Click();
            Thread.Sleep(2000);
        }

        //Test 3: Nhập title, des, cho overview
        [Test]
        public void Overview()
        {
            CourseBuilder();
            IList<IWebElement> inputFields = driver.FindElements(By.CssSelector("input[placeholder='Enter your text here...']"));
            inputFields[0].SendKeys("Title");
            inputFields[1].SendKeys("Description");
            Thread.Sleep(5000);
        }
        //Test 4: Thêm infomation 
        [Test]
        public void OverviewInfo()
        {
            Overview();
            IWebElement addInfomation = driver.FindElement(By.XPath("//button[@class='group/btn inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium ring-offset-white transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 border border-gray-l20 bg-white hover:bg-gray-l10 h-10 w-10']"));
            for (int i = 0; i < 3; i++)
            {
                addInfomation.Click();
                Thread.Sleep(1000);
            }
            IList<IWebElement> inputFields = driver.FindElements(By.CssSelector("input[placeholder='What will the learner learn from completing this course?']"));
            Random stringRd = new();
            for (int i = 0; i < 2; i++)
            {
                inputFields[i].SendKeys(stringRd.ToString());
                Thread.Sleep(2000);
            }

        }
        //Test 5: Thêm teacher
        [Test]
        public void AddTeacher()
        {
            Overview();
            IWebElement btnAddTeacher = driver.FindElement(By.XPath("//button[contains(@class, 'border-2 border-blue') and contains(@class, 'rounded-full')]"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnAddTeacher);
            Thread.Sleep(5000);
            btnAddTeacher.Click();
            IWebElement emailField = driver.FindElement(By.CssSelector("input[placeholder='Email address or name']"));
            emailField.SendKeys("lozy564@gmail.com");
            Thread.Sleep(2000);
            IWebElement confirm = driver.FindElement(By.CssSelector("p[class='text-darkGray']"));
            confirm.Click();
            IWebElement btnAdd = driver.FindElement(By.XPath("//button[normalize-space()='Add']"));
            btnAdd.Click();
            Thread.Sleep(2000);
        }

        /* //Test 6: Đổi tên Chapter
         [Test]
         public void createNewCoursewithoutName()
         {
             courseBuilder();
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

        [Test]
        public void UnitTest()
        {
            CourseBuilder();
            IWebElement editChapter = driver.FindElement(By.CssSelector("svg.lucide.lucide-chevron-right"));
            editChapter.Click();
            Thread.Sleep(3000);
            IWebElement addUnit = driver.FindElement(By.XPath("//button[normalize-space()='Unit']"));
            addUnit.Click();
            Thread.Sleep(2000);
        }

        //Test 8 : Thêm activity
        [Test]
        public void AddActivity()
        {
            CourseBuilder();
            IWebElement btnCreate = driver.FindElement(By.XPath("//button[normalize-space()='Chapter']"));
            for (int i = 0; i < 2; i++)
            {
                btnCreate.Click();
                Thread.Sleep(1000);
            }
            Thread.Sleep(3000);
            IWebElement addActivity = driver.FindElement(By.XPath("//button[normalize-space()='Activity']"));
            addActivity.Click();
            Thread.Sleep(3000);

        }
        //8.1 Thêm Lesson
        [Test]
        public void AddLesson()
        {
            AddActivity();
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Continue']"));
            confirm.Click();
            Thread.Sleep(5000);

        }

        //8.2 Video 
        [Test]

        public void AddVideo()
        {
            AddActivity();
            IWebElement video = driver.FindElement(By.XPath("//span[@class='font-medium select-none'][normalize-space()='Video']"));
            video.Click();
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Continue']"));
            confirm.Click();
            Thread.Sleep(5000);
        }
        //8.3 Quiz
        [Test]
        public void AddQuiz()
        {
            AddActivity();
            IWebElement quiz = driver.FindElement(By.XPath("//span[@class='font-medium select-none'][normalize-space()='Quiz']"));
            quiz.Click();
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Continue']"));
            confirm.Click();
            Thread.Sleep(5000);
        }
        //8.3.1 Quiz 
        [Test]
        public void AddQuizQuestion()
        {
            AddQuiz();

        }

        //8.4 Game
        [Test]
        public void AddGame()
        {
            AddActivity();
            IWebElement game = driver.FindElement(By.XPath("//span[@class='font-medium select-none'][normalize-space()='Game']"));
            game.Click();
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Continue']"));
            confirm.Click();
            Thread.Sleep(5000);
        }
        //8.5 Assignment

        [Test]
        public void AddAssignment()
        {
            AddActivity();
            IWebElement assignment = driver.FindElement(By.XPath("//span[@class='font-medium select-none'][normalize-space()='Assignment']"));
            assignment.Click();
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Continue']"));
            confirm.Click();
            Thread.Sleep(5000);
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
