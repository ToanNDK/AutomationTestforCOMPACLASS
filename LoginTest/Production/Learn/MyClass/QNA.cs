using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;

namespace TestCompa.Production.Learn.QNA
{
    public class ClassTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "https://compaclass.com/learn/home";
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
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(5000);
            IWebElement testclass = wait.Until(d => d.FindElement(By.XPath("//a[contains(text(),'Class Test')]")));
            ScrollToElement(testclass);
            Thread.Sleep(4000);
            testclass.Click();
            Thread.Sleep(2000);
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
            IWebElement Title = driver.FindElement(By.XPath("//span[@class='block truncate text-start text-darkGray ']"));
            Title.Click();
            Thread.Sleep(5000);
            Thread.Sleep(3000);


            IWebElement chooseContent = driver.FindElement(By.XPath("//span[contains(text(),'Giới thiệu về hệ sinh thái PowerBI')]"));
            chooseContent.Click();
            IWebElement content = driver.FindElement(By.CssSelector("textarea[placeholder='Mô tả vấn đề']"));

            content.SendKeys("New Title!");
            Thread.Sleep(2000);

            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(2000);

        }
        //3. Sửa Post
        [Test, Order(2)]
        public void editQNA()
        {
            addQuestion();
            IWebElement svg = driver.FindElement(By.XPath("//button[contains(@class, 'outline-none') and contains(@class, 'cursor-pointer') and contains(@class, 'group')]"));
            svg.Click();
            Thread.Sleep(1000);
            IWebElement edit = driver.FindElement(By.XPath("//span[@class='whitespace-nowrap text-sm group-hover:text-primary transition-all' and (normalize-space(.)='Edit question' or normalize-space(.)='Sửa câu hỏi')]"));
            edit.Click();
            Thread.Sleep(4000);

            IWebElement content = driver.FindElement(By.CssSelector("textarea[placeholder='Mô tả vấn đề']"));
            content.Click();
            content.Clear();
            Thread.Sleep(3000);
            content.SendKeys("New Title Update!");
            Thread.Sleep(2000);


        }
        //4. Xóa bài đăng
        [Test, Order(3)]
        public void deleteQNA()
        {
            addQuestion();
            Thread.Sleep(3000);
            IWebElement svg = driver.FindElement(By.XPath("//button[contains(@class, 'outline-none') and contains(@class, 'cursor-pointer') and contains(@class, 'group')]"));
            svg.Click();
            Thread.Sleep(2000);
            IWebElement delete = driver.FindElement(By.XPath("//span[@class='whitespace-nowrap text-sm group-hover:text-primary transition-all' and (normalize-space(.)='Delete question' or normalize-space(.)='Xóa câu hỏi')]"));
            delete.Click();
            Thread.Sleep(4000);
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Xóa']"));
            confirm.Click();

            Thread.Sleep(2000);

        }
        //5.React bài đăng
        [Test, Order(4)]
        public void reactQNA()
        {
            addQuestion();
            Thread.Sleep(2000);
            IWebElement react = driver.FindElement(By.XPath("//button[contains(@class, 'group') and contains(@class, 'flex') and contains(@class, 'items-center')]"));
            react.Click();
            Thread.Sleep(5000);
        }

        //6.Trả lời 
        [Test,Order(5)]
        public void replyQNA()
        {
            addQuestion();
            IWebElement rep = driver.FindElement(By.XPath("//button[.//span[text()='Trả lời'] or .//span[text()='Answer']]"));
            rep.Click();
            Thread.Sleep(4000);
            IWebElement txtRep = driver.FindElement(By.CssSelector("textarea[placeholder='Viết câu trả lời...']"));
            txtRep.SendKeys("Reply ! ");
            Thread.Sleep(2000);

            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(2000);
        }
        public void Login()
        {
            Thread.Sleep(5000);
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("lozy564@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Toanking2k3*");

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();

        }
        public void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});", element);
        }


        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
