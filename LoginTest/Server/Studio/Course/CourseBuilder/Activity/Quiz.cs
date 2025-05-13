using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.CourseBuilder.Activity.Quiz
{
    public class Quiz
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

        public void studioTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = driver.FindElement(By.XPath("//button[.//span[text()='Course']]"));
            course.Click();

            Thread.Sleep(2000);
        }

        public void courseBuilder()
        {
            studioTest();
            IWebElement tab = driver.FindElement(By.XPath("//button[normalize-space()='1']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView(true);", tab);
            Thread.Sleep(3000);
            tab.Click();
            Thread.Sleep(2000);


            string expectedText = "CourseTest";


            IWebElement h2Element = driver.FindElement(By.XPath($"//h3[contains(text(),'{expectedText}')]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", h2Element);
            Thread.Sleep(3000);
            h2Element.Click();

            Thread.Sleep(3000);
        }



        public void AddQuestion()
        {
            courseBuilder();
            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'QuizBlock')]"));
            title.Click();
            Thread.Sleep(1000);
            IWebElement quiz = driver.FindElement(By.XPath("//button[normalize-space()='Activity']"));
            quiz.Click();
            Thread.Sleep(2000);
            IWebElement quizOption = driver.FindElement(By.XPath("//span[@class='font-medium select-none'][normalize-space()='Quiz']"));
            quizOption.Click();
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Continue']"));
            submit.Click();
            Thread.Sleep(1000);
            IWebElement elementTab = driver.FindElement(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]"));
            elementTab.Click();
            Thread.Sleep(2000);
            IWebElement addquestion = driver.FindElement(By.XPath("//span[normalize-space()='Add question']"));
            addquestion.Click();
            Thread.Sleep(1000);

        }
        [Test]
        public void singleChoice()
        {
            AddQuestion();
            IWebElement single = driver.FindElement(By.XPath("//p[normalize-space()='Single Choice']"));
            single.Click();
            Thread.Sleep(1000);
        }
        //Test 1: Display Settings (UC-S229)
        [Test]
        public void displaySetting()
        {
            singleChoice();
            Thread.Sleep(3000);
            IWebElement dropdown = driver.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(4) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > button:nth-child(2)"));
            dropdown.Click();
            Thread.Sleep(4000);
            IWebElement displayAllQuestions = driver.FindElement(By.XPath("//span[normalize-space()='Display all questions']"));
            displayAllQuestions.Click();
            Thread.Sleep(2000);
        }
        //Test 2: Allow Attempt ( Số lần làm ) (UC-S230)
        [Test]
        public void allowAttempt()
        {
            singleChoice();
            Thread.Sleep(3000);
            IWebElement dropdown = driver.FindElement(By.XPath("//button[.//span[normalize-space()='Unlimited']]"));
            dropdown.Click();
            Thread.Sleep(4000);
            IWebElement attempt = driver.FindElement(By.XPath("//span[normalize-space()='1 attempt']"));
            attempt.Click();
            Thread.Sleep(4000);
            dropdown.Click();
            Thread.Sleep(2000);
            IWebElement attempts = driver.FindElement(By.XPath("//span[normalize-space()='3 attempts']"));
            attempts.Click();
            Thread.Sleep(2000);
        }

        public void Login()
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
