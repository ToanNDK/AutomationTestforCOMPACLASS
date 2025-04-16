/*using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCompa.Server.Studio.Course.CourseBuilder.Element
{
    public class Structure
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
        //8.3 Quiz
        [Test]
        public void addQuiz()
        {
            addActivity();
            IWebElement quiz = driver.FindElement(By.XPath("//span[@class='font-medium select-none'][normalize-space()='Quiz']"));
            quiz.Click();
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Continue']"));
            confirm.Click();
            Thread.Sleep(5000);
        }


        public void addQuizQuestion()
        {
            addQuiz();
            IWebElement element = driver.FindElement(By.XPath("//span[normalize-space()='Elements']"));


            element.Click();
            Thread.Sleep(2000);
            IWebElement question = driver.FindElement(By.XPath("  //span[normalize-space()='Add question']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].selected = true;", question);
            question.Click();
            Thread.Sleep(2000);
        }
        public void addQuizSlide()
        {
            addQuiz();
            IWebElement element = driver.FindElement(By.XPath("//span[normalize-space()='Elements']"));
            element.Click();
            Thread.Sleep(2000);
            IWebElement slide = driver.FindElement(By.XPath("//span[normalize-space()='Add slide']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].selected = true;", slide);
            slide.Click();
            Thread.Sleep(2000);

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

        public void createNewChapter()
        {
            courseBuilder();
            IWebElement btnCreate = driver.FindElement(By.XPath("//button[normalize-space()='Chapter']"));
            for (int i = 0; i < 2; i++)
            {
                btnCreate.Click();
                Thread.Sleep(1000);
            }
            IWebElement chapterDelete = driver.FindElement(By.XPath("//p[normalize-space()='Chapter 2: Untitled']"));
            chapterDelete.Click();
            Thread.Sleep(2000);
            Actions acitons = new Actions(driver);

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

        public void addActivity()
        {
            courseBuilder();
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
        [Test]
        //test 1 : Multiple Choice
        public void multiChoice()
        {
            addQuizQuestion();
            IWebElement multiple = driver.FindElement(By.XPath("//p[normalize-space()='Multiple Choice']"));
            multiple.Click();
            Thread.Sleep(1000);

            IWebElement question = driver.FindElement(By.XPath("//p[@data-placeholder='Start typing your question']"));
            question.SendKeys("Câu hỏi 1");
            Thread.Sleep(2000);
            IList<IWebElement> answerFields = driver.FindElements(By.XPath("//p[@data-placeholder='Nhập nội dung đáp án...']"));
            Random random = new Random();

            foreach (var field in answerFields)
            {
                string randomText = "Đáp án " + random.Next(1, 100);
                field.SendKeys(randomText);
                Thread.Sleep(500);
            }
            Thread.Sleep(3000);
            IList<IWebElement> answerOptions = driver.FindElements(By.XPath("//div[contains(@class, 'rounded-full') and contains(@class, 'cursor-pointer')]"));

            if (answerOptions.Count > 0)
            {
                // Chọn ngẫu nhiên một đáp án và click
                int randomIndex = random.Next(answerOptions.Count);
                answerOptions[randomIndex].Click();
                Thread.Sleep(500);
            }
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Save']"));
            submit.Click();
            Thread.Sleep(2000);
        }
        //Test 2: Thêm đáp án


        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
*/