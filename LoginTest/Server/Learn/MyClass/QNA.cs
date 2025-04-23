using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;

namespace TestCompa.Server.Learn.QNA
{
    public class ClassTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private readonly string devUrl = "http://10.10.10.30/learn/home";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(devUrl);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        // 1. Test chức năng điều hướng tới Q&A
        [Test, Order(1)]
        public void QNA()
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
            IWebElement testclass = driver.FindElement(By.XPath("//div[6]//div[1]//a[1]"));
            testclass.Click();
            Thread.Sleep(5000);
            IWebElement assign = driver.FindElement(By.XPath("//a[text()='Hỏi & Đáp']"));
            assign.Click();
            Thread.Sleep(2000);




        }
        //2. Thêm Post
        [Test, Order(2)]
        public void addQuestion()
        {
            QNA();
            IWebElement addQuestion = driver.FindElement(By.XPath("//button[contains(@class,'relative bg-primary rounded-2xl flex items-center text-white')]"));
            addQuestion.Click();
            Thread.Sleep(5000);
            IWebElement Title = driver.FindElement(By.CssSelector("button[id='headlessui-listbox-button-:r7d:']"));
            Title.Click();

           
        }
        //3. Sửa Post
        [Test, Order(2)]
        public void editPosts()
        {
            addQuestion();
            Thread.Sleep(3000);
            IWebElement svg = driver.FindElement(By.CssSelector("button.outline-none.cursor-pointer.group"));
            svg.Click();
            Thread.Sleep(2000);
            IWebElement edit = driver.FindElement(By.XPath("//button[span[text()='Chỉnh sửa bài đăng']]"));
            edit.Click();
            IWebElement Title = driver.FindElement(By.CssSelector("textarea[placeholder='Chủ đề của bài đăng']"));
            Title.Click();
            Title.Clear();
            Title.SendKeys("New Title!");
            Thread.Sleep(2000);
            IWebElement Content = driver.FindElement(By.CssSelector("textarea[placeholder='Nội dung của bài đăng']"));
            Content.Click();
            Content.Clear();
            Content.SendKeys("New Content!");
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(2000);



        }
        //4. Xóa bài đăng
        [Test, Order(3)]
        public void deletePosts()
        {
            addQuestion();
            Thread.Sleep(3000);
            IWebElement svg = driver.FindElement(By.CssSelector("button.outline-none.cursor-pointer.group"));
            svg.Click();
            Thread.Sleep(2000);
            IWebElement delete = driver.FindElement(By.XPath("//button[span[text()='Xóa bài đăng']]"));
            delete.Click();
            Thread.Sleep(4000);
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Xóa']"));
            confirm.Click();

            Thread.Sleep(2000);

        }
        //5.React bài đăng
        [Test, Order(4)]
        public void reactPosts()
        {
            addQuestion();
            IWebElement react = driver.FindElement(By.CssSelector("button.flex.items-center.gap-2.group"));
            react.Click();
            Thread.Sleep(5000);
        }
        public void Login()
        {
            Thread.Sleep(5000);
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("lozik480@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Toanking2k3*");

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
