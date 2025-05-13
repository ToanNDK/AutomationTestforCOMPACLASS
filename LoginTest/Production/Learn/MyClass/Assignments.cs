using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.Learn.Assignments
{
    [TestFixture]
    [Category("Class")]
    public class ClassTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private readonly string devUrl = "https://compaclass.com/learn/home";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(devUrl);

            wait = new(driver, TimeSpan.FromSeconds(10));
        }
        // 1. Test chức năng điều hướng tới Assignment

        [Test, Order(1)]
        public void Assignments()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();


            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(3000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            Thread.Sleep(5000);
            IWebElement testclass = driver.FindElement(By.XPath("//a[contains(text(),'Class Test')]"));
            testclass.Click();
            Thread.Sleep(5000);
            IWebElement assign = driver.FindElement(By.XPath("//a[text()='Bài tập']"));
            assign.Click();
            Thread.Sleep(2000);




        }
        //2. test điều hướng trang chấm điểm
        /*[Test, Order(2)]
        public void AssignmentsMark()
        {
            Assignments();
            IWebElement mark = driver.FindElement(By.XPath("//button[text()='Chấm điểm']"));
            mark.Click();
            Thread.Sleep(5000);

        }
        //3. test ghi điểm bài làm
        [Test, Order(3)]
        public void MarkAssignments()
        {
            AssignmentsMark();
            IWebElement student = driver.FindElement(By.XPath("//tbody/tr[1]/td[1]"));
            student.Click();
            Thread.Sleep(5000);
            IWebElement mark = driver.FindElement(By.CssSelector("input[type='number'][min='0'][max='100']"));
            mark.Click();
            mark.Clear();

            Random random = new Random();
            int randomNumber = random.Next(0, 101);

            mark.SendKeys(randomNumber.ToString());

            IWebElement submit = driver.FindElement(By.XPath("//button[contains(text(),'Lưu')]"));
            submit.Click();
            Thread.Sleep(5000);
        }
        //4. test feedback
        [Test, Order(4)]
        public void feedbackAssignments()
        {
            MarkAssignments();
            Thread.Sleep(5000);
            IWebElement feedback = driver.FindElement(By.XPath("//textarea[@name='feedbackComment' and @maxlength='1000']"));
            feedback.Click();
            feedback.Clear();
            feedback.SendKeys("Excellent!");
            Thread.Sleep(5000);
            IWebElement submit = driver.FindElement(By.XPath("//button[contains(text(),'Lưu')]"));
            submit.Click();
            Thread.Sleep(5000);
        }*/
        //5. 
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
