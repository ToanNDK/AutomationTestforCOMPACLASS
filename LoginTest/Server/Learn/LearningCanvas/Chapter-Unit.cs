using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCompa.Server.Learn.LearningCanvas
{
    public class CourseTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "http://10.10.10.30/Auth/SignIn?login_challenge=a635429bdb604fb08d123fff4bbb1add";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(devUrl);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        //Test 1: Truy cập, bấm tiếp tục học -> Mục đã học

        [Test,Order(1)]
        public void testContinueLearn()
        {
            driver.Navigate().GoToUrl("http://10.10.10.30/learn/home");
            Login();
            Assert.IsTrue(driver.Url.Contains("http://10.10.10.30/learn/home"));
            Thread.Sleep(5000);
            IWebElement continueLearn = driver.FindElement(By.XPath("//a[span[text()='Tiếp tục học']]"));
            continueLearn.Click();
            Thread.Sleep(10000);
            Assert.IsTrue(driver.Url.Contains("http://10.10.10.30/vn/learn/course"));
        }

        //Test 2: Bấm Tiếp tục học -> bấm tiếp theo trên thanh ( FE-L21) 
        [Test, Order(2)]
        public void testBtnCourse()
        {
            driver.Navigate().GoToUrl("http://10.10.10.30/learn/home");
            Login();
            /*IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
            course.Click();*/
            Thread.Sleep(5000);
            IWebElement continueLearn = driver.FindElement(By.XPath("//a[span[text()='Tiếp tục học']]"));
            continueLearn.Click();
            Thread.Sleep(5000);
            Assert.IsTrue(driver.Url.Contains("http://10.10.10.30/vn/learn/course"));
            //Bấm nút <- và ->
            /*IWebElement btnContinue = driver.FindElement(By.XPath("//button[contains(text(),'Đi đến học')]"));
            btnContinue.Click();*/
            Assert.IsTrue(driver.Url.Contains("http://10.10.10.30/vn/learn/course"));
            Thread.Sleep(2000);
            IWebElement previousButton = driver.FindElement(By.XPath("//button[span[text()='Trước đó']]"));
            IWebElement nextButton = driver.FindElement(By.XPath("//button[span[text()='Tiếp theo']]"));
            clickBtn(previousButton, 3);
            clickBtn(nextButton, 5);

        }
        //Test 3: Bấm hoàn thành sau khi học xong (UC L248)
        [Test, Order(3)]
        public void finishCourse()
        {
            driver.Navigate().GoToUrl("http://10.10.10.30/learn/home");
            Login();
            /*IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
            course.Click();*/
            Thread.Sleep(5000);
            IWebElement continueLearn = driver.FindElement(By.XPath("//a[span[text()='Tiếp tục học']]"));
            continueLearn.Click();
            Thread.Sleep(5000);
            Assert.IsTrue(driver.Url.Contains("http://10.10.10.30/vn/learn/course"));
            //Bấm nút <- và ->
            /*IWebElement btnContinue = driver.FindElement(By.XPath("//button[contains(text(),'Đi đến học')]"));
            btnContinue.Click();*/
            Assert.IsTrue(driver.Url.Contains("http://10.10.10.30/vn/learn/course"));
            Thread.Sleep(2000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(3000);
            IWebElement finish = driver.FindElement(By.XPath("//button[normalize-space()='Hoàn thành']"));
            finish.Click();
            Thread.Sleep(2000);
        }
        //Test 4: Bấm hoàn thành -> Bấm tiếp tục để điều hướng tới bài tiếp theo
        [Test, Order(4)]
        public void finishNContinue()
        {
            finishCourse();
            IWebElement continueLesson = driver.FindElement(By.XPath("//button[contains(text(),'Tiếp tục')]"));
            continueLesson.Click();
        }
        //Test 5: Bấm các tab trên scroll bar
        [Test, Order(5)]
        public void testScrollBar()
        {
            driver.Navigate().GoToUrl("http://10.10.10.30/learn/home");
            Login();
            Thread.Sleep(2000);
            IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
            course.Click();
            Thread.Sleep(5000);
            //Chuyển hướng đúng trang overview
            string startUrl = "http://10.10.10.30/learn/course/";
            string endUrl = "overview";
            string actualUrl = driver.Url;
            Assert.IsTrue(actualUrl.StartsWith(startUrl) && actualUrl.EndsWith(endUrl), $"URL không đúng . Url hiện tại là {actualUrl}");
            IWebElement member = driver.FindElement(By.XPath("//a[text()='Thành viên']"));
            member.Click();
            Thread.Sleep(1000);
            IWebElement post = driver.FindElement(By.XPath("//a[text()='Bài đăng']"));
            post.Click();
            Thread.Sleep(1000);
            IWebElement QA = driver.FindElement(By.XPath("//a[text()='Hỏi & Đáp']"));
            QA.Click();
            Thread.Sleep(1000);
            IWebElement mark = driver.FindElement(By.XPath("//a[text()='Điểm số']"));
            mark.Click();
            Thread.Sleep(1000);
            IWebElement feedback = driver.FindElement(By.XPath("//a[text()='Đánh giá']"));
            feedback.Click();
            Thread.Sleep(1000);
            IWebElement certificate = driver.FindElement(By.XPath("//a[text()='Chứng chỉ']"));
            certificate.Click();
            Thread.Sleep(1000);

        }
       
        //Test 6: Kiểm tra việc comment trong phần QA
        [Test, Order(6)]
        public void addcomnentQA()
        {
            driver.Navigate().GoToUrl("http://10.10.10.30/learn/home");
            Thread.Sleep(2000);
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
        //Test 7: Sửa bình luận ( Viết thêm vào bình luận cũ ) -> Lưu
        [Test, Order(7)]
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
        //Test 8: Sửa bình luận ( Xóa bình luận cũ -> Viết bình luận mới ) -> Lưu
        [Test, Order(8)]
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
        //Test 9: Xóa bình luận 
        [Test, Order(9)]
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
        public void clickBtn(IWebElement button, int times)
        {
            for (int i = 0; i < times; i++)
            {
                button.Click();
                Thread.Sleep(1000);
            }
        }
        /*public void clickScrollBar()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            string baseCourseUrl = driver.Url; // Lưu lại URL ban đầu

            // Lấy danh sách ban đầu
            IReadOnlyList<IWebElement> links = driver.FindElements(By.CssSelector("div.flex a"));

            if (links.Count == 0)
            {
                Assert.Fail("Không tìm thấy liên kết nào trong danh mục.");
            }

            // Click từng link từ trái qua phải
            for (int i = 0; i < links.Count; i++)
            {
                links = driver.FindElements(By.CssSelector("div.flex a")); // Lấy lại danh sách mới

                if (i >= links.Count) // Kiểm tra nếu index vượt quá danh sách
                    break;

                string href = links[i].GetAttribute("href");
                links[i].Click();
                wait.Until(d => d.Url.Contains(href));

                // Nếu bị redirect về trang home, thì quay lại trang course ban đầu
                if (driver.Url.Contains("/learn/home"))
                {
                    driver.Navigate().GoToUrl(baseCourseUrl);
                    Thread.Sleep(2000);
                }
            }

            // Click ngược lại từ phải qua trái
            links = driver.FindElements(By.CssSelector("div.flex a")); // Lấy lại danh sách mới
            for (int i = links.Count - 1; i >= 0; i--)
            {
                links = driver.FindElements(By.CssSelector("div.flex a")); // Lấy lại danh sách mới

                if (i >= links.Count) // Kiểm tra nếu index không hợp lệ
                    break;

                string href = links[i].GetAttribute("href");
                links[i].Click();
                wait.Until(d => d.Url.Contains(href));

                // Nếu bị redirect về trang home, thì quay lại trang course ban đầu
                if (driver.Url.Contains("/learn/home"))
                {
                    driver.Navigate().GoToUrl(baseCourseUrl);
                    Thread.Sleep(2000);
                }
            }
        }*/



        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
