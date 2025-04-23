using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCompa.Server.Learn.LearningCanvas.Lesson
{
    public class CourseTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        //UC-L258: Add new In-line Q&A

        //Test 1: Kiểm tra việc comment trong phần QA
        [Test, Order(1)]
        public void addcomnentQA()
        {
            driver.Navigate().GoToUrl("http://10.10.10.30/learn/home");
            Thread.Sleep(5000);
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("lozik480@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Toanking2k3*");

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();


            Thread.Sleep(5000);
            IWebElement continueLearn = driver.FindElement(By.XPath("//a[span[text()='Tiếp tục học']]"));
            continueLearn.Click();
            Thread.Sleep(10000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            Thread.Sleep(5000);
            IWebElement cmt = driver.FindElement(By.CssSelector("textarea[placeholder='Viết câu trả lời...']"));
            cmt.SendKeys("TestComment!");
            Thread.Sleep(5000);
            IWebElement sendCmt = driver.FindElement(By.XPath("//button[@type='submit']"));
            sendCmt.Click();
            Thread.Sleep(5000);
        }
        //Test 2: Sửa bình luận ( Viết thêm vào bình luận cũ ) -> Lưu
        [Test, Order(2)]
        public void editcommentQA()
        {
            addcomnentQA();
            IWebElement custom = driver.FindElement(By.CssSelector(".w-4[xmlns='http://www.w3.org/2000/svg'][width='26']"));
            custom.Click();
            Thread.Sleep(4000);
            IWebElement editbtnCmt = driver.FindElement(By.XPath("//span[contains(text(),'Sửa câu hỏi')]"));
            editbtnCmt.Click();
            Thread.Sleep(5000);
            IWebElement editNewCmt = driver.FindElement(By.XPath("//textarea[@placeholder='Mô tả vấn đề']"));
            editNewCmt.SendKeys("New Cmt");
            Thread.Sleep(5000);
            IWebElement btnSave = driver.FindElement(By.XPath("//button[contains(text(),'Lưu')]"));
            btnSave.Click();
            Thread.Sleep(5000);
            Console.WriteLine("Sửa thành công");
        }
        //Test 3: Sửa bình luận ( Xóa bình luận cũ -> Viết bình luận mới ) -> Lưu
        [Test, Order(3)]
        public void editNewComment()
        {
            addcomnentQA();
            IWebElement custom = driver.FindElement(By.CssSelector(".w-4[xmlns='http://www.w3.org/2000/svg'][width='26']"));
            custom.Click();
            Thread.Sleep(4000);
            IWebElement editbtnCmt = driver.FindElement(By.XPath("//span[contains(text(),'Sửa câu hỏi')]"));
            editbtnCmt.Click();
            Thread.Sleep(5000);
            IWebElement editNewCmt = driver.FindElement(By.XPath("//textarea[@placeholder='Mô tả vấn đề']"));
            editNewCmt.Clear();
            editNewCmt.SendKeys("New Comment");
            Thread.Sleep(5000);
            IWebElement btnSave = driver.FindElement(By.XPath("//button[contains(text(),'Lưu')]"));
            btnSave.Click();
            Thread.Sleep(5000);
            Console.WriteLine("Sửa thành công");
        }
        //Test 4: Xóa bình luận 
        [Test, Order(4)]
        public void deleteComment()
        {
            addcomnentQA();
            IWebElement custom = driver.FindElement(By.CssSelector(".w-4[xmlns='http://www.w3.org/2000/svg'][width='26']"));
            custom.Click();
            Thread.Sleep(4000);
            IWebElement deletebtnCmt = driver.FindElement(By.XPath("//span[contains(text(),'Xóa câu hỏi')]"));
            deletebtnCmt.Click();
            Thread.Sleep(5000);
            IWebElement delete = driver.FindElement(By.XPath("//button[normalize-space()='Xóa']"));
            delete.Click();
            Thread.Sleep(5000);
        }
        //Test 5: Trả lời bình luận 
        [Test, Order(5)]
        public void replyComment()
        {
            addcomnentQA(); // Gọi hàm thêm comment trước khi trả lời

            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));

            // Tìm và click nút "Trả lời"
            IWebElement rep = driver.FindElement(By.XPath("//button[.//span[text()='Trả lời']]"));
            rep.Click();

            // Chờ cho các textarea xuất hiện
            wait.Until(driver => driver.FindElements(By.CssSelector("textarea[placeholder='Viết câu trả lời...']")).Count >= 2);
            var textAreas = driver.FindElements(By.CssSelector("textarea[placeholder='Viết câu trả lời...']"));

            if (textAreas.Count >= 2)
            {
                IWebElement reply = textAreas[1]; // Lấy textarea thứ hai
                reply.SendKeys("Reply Comment!");

                // Chờ nút "Gửi" xuất hiện và được kích hoạt
                IWebElement sendButton = wait.Until(driver =>
                {
                    var btn = driver.FindElement(By.XPath("//button[.//span[text()='Gửi']]"));
                    return btn.Enabled ? btn : null;
                });

                // Cuộn đến nút "Gửi" để tránh bị che khuất
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", sendButton);

                // Chờ nút có thể click
                Thread.Sleep(5000);
                // Click vào nút "Gửi"
                sendButton.Click();
                Thread.Sleep(5000);
            }
            else
            {
                Assert.Fail("Không tìm thấy ô nhập câu trả lời thứ hai!");
            }
        }

        public void Login()
        {
            Thread.Sleep(5000);
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
