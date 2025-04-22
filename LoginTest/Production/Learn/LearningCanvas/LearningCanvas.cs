using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TestCompa.Production.Learn.LearningCanvas
{
    public class CourseTests
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string homeUrl = "https://compaclass.com/learn/home";
        private readonly string courseUrl = "https://compaclass.com/learn/course";
        private readonly string email = "info@kpim.vn";
        private readonly string password = "KPIM@123";

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
        public void Setup()
        {
            // Gọi InitDriver với tham số headless = false (mặc định)
            // Thay đổi thành true nếu muốn chạy ở chế độ headless
            InitDriver(true);
            driver.Navigate().GoToUrl("https://auth.compaclass.com/Auth/SignIn");
        }

        // Helper method for login
        private void Login(string userEmail = null, string userPassword = null)
        {
            Thread.Sleep(2000);
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys(userEmail ?? email);

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys(userPassword ?? password);

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();

            // Wait for login to complete
            Thread.Sleep(5000);
        }

        // Helper method to click a button multiple times
        private void ClickButton(IWebElement button, int times)
        {
            for (int i = 0; i < times; i++)
            {
                button.Click();
                Thread.Sleep(1000);
            }
        }

        // Helper method to navigate to the learning course
        private void NavigateToLearningCourse()
        {
            driver.Navigate().GoToUrl(homeUrl);
            Thread.Sleep(5000);

            Login();
            Thread.Sleep(2000);

            // Find and click the "Continue Learning" button
            IWebElement continueLearn = wait.Until(d => d.FindElement(By.XPath("//a[span[text()='Tiếp tục học']]")));

            // Scroll to the button to make sure it's visible
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", continueLearn);
            Thread.Sleep(2000);

            continueLearn.Click();
            Thread.Sleep(5000);

            // Verify we're on the course page
            Assert.That(driver.Url.Contains("https://compaclass.com/vn/learn/course"), Is.True);
        }

        // Test 1: Access, click continue learning -> Course section
        [Test]
        public void TestContinueLearn()
        {
            try
            {
                NavigateToLearningCourse();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                throw;
            }
        }

        // Test 2: Click Continue learning -> click next on the bar
        [Test]
        public void TestBtnCourse()
        {
            try
            {
                NavigateToLearningCourse();

                // Find and test the previous and next buttons
                IWebElement previousButton = driver.FindElement(By.XPath("//button[span[text()='Trước đó']]"));
                IWebElement nextButton = driver.FindElement(By.XPath("//button[span[text()='Tiếp theo']]"));

                ClickButton(previousButton, 3);
                ClickButton(nextButton, 5);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                throw;
            }
        }

        // Test 3: Click finish after completing a lesson
        [Test]
        public void FinishCourse()
        {
            try
            {
                NavigateToLearningCourse();

                // Scroll to bottom of page
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Thread.Sleep(3000);

                // Find and click the finish button
                IWebElement finish = driver.FindElement(By.XPath("//button[contains(text(),'Hoàn thành') and contains(@class, 'bg-primary')]"));
                js.ExecuteScript("arguments[0].click();", finish);
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                throw;
            }
        }

        // Test 4: Click finish -> Click continue to navigate to next lesson
        [Test]
        public void FinishAndContinue()
        {
            try
            {
                FinishCourse();

                // Click continue button
                IWebElement continueLesson = driver.FindElement(By.XPath("//button[contains(text(),'Tiếp tục')]"));
                continueLesson.Click();
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                throw;
            }
        }

        // Test 5: Click tabs on the scroll bar
        [Test]
        public void TestScrollBar()
        {
            try
            {
                driver.Navigate().GoToUrl(homeUrl);
                Thread.Sleep(5000);

                Login();
                Thread.Sleep(2000);

                // Navigate to a course
                IWebElement course = driver.FindElement(By.XPath("//a[contains(@class, 'absolute') and contains(@class, 'bg-dark')]"));
                course.Click();
                Thread.Sleep(5000);

                // Verify we're on the course overview page
                string startUrl = "https://compaclass.com/learn/course/";
                string endUrl = "overview";
                string actualUrl = driver.Url;
                Assert.That(actualUrl.StartsWith(startUrl) && actualUrl.EndsWith(endUrl), Is.True,
                    $"URL doesn't match. Current URL is {actualUrl}");

                // Navigate through various tabs
                string[] tabNames = { "Thành viên", "Bài đăng", "Hỏi & Đáp", "Điểm số", "Đánh giá", "Chứng chỉ" };

                foreach (string tabName in tabNames)
                {
                    IWebElement tab = driver.FindElement(By.XPath($"//a[text()='{tabName}']"));
                    tab.Click();
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                throw;
            }
        }

        // Test 6: Test commenting in QA section
        [Test]
        public void AddCommentQA()
        {
            try
            {
                driver.Navigate().GoToUrl(courseUrl);
                Thread.Sleep(2000);

                // Use a different login for this test
                Login("lozy564@gmail.com", "Toanking2k3*");

                // Find and click the "Continue Learning" button
                IWebElement continueLearn = wait.Until(d => d.FindElement(By.XPath("//a[span[text()='Tiếp tục học']]")));

                // Scroll to make the button visible
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", continueLearn);
                Thread.Sleep(2000);

                if (continueLearn.Displayed && continueLearn.Enabled)
                {
                    continueLearn.Click();
                }
                else
                {
                    throw new Exception("Continue learning button is not available!");
                }

                Thread.Sleep(10000);

                // Scroll to bottom of page
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                Thread.Sleep(5000);

                // Add a comment
                IWebElement commentTextarea = driver.FindElement(By.CssSelector("textarea[placeholder='Viết câu trả lời...']"));
                commentTextarea.SendKeys("TestComment!");
                Thread.Sleep(5000);

                // Submit the comment
                IWebElement submitButton = driver.FindElement(By.XPath("//button[@type='submit']"));

                if (submitButton.Displayed && submitButton.Enabled)
                {
                    submitButton.Click();
                }
                else
                {
                    throw new Exception("Submit comment button is not available!");
                }

                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                throw;
            }
        }

        // Test 7: Edit comment (Add to existing comment) -> Save
        [Test]
        public void EditCommentQA()
        {
            try
            {
                AddCommentQA();

                // Open comment menu
                IWebElement customMenu = driver.FindElement(By.CssSelector(".w-4[xmlns='http://www.w3.org/2000/svg'][width='26']"));
                customMenu.Click();
                Thread.Sleep(4000);

                // Click edit button
                IWebElement editButton = driver.FindElement(By.XPath("//span[contains(text(),'Sửa câu hỏi')]"));
                editButton.Click();
                Thread.Sleep(5000);

                // Add to existing comment
                IWebElement editTextarea = driver.FindElement(By.XPath("//textarea[@placeholder='Mô tả vấn đề']"));
                editTextarea.SendKeys("New Cmt");
                Thread.Sleep(5000);

                // Save comment
                IWebElement saveButton = driver.FindElement(By.XPath("//button[contains(text(),'Lưu')]"));
                saveButton.Click();
                Thread.Sleep(5000);

                Console.WriteLine("Edit successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                throw;
            }
        }

        // Test 8: Edit comment (Delete old comment -> Write new comment) -> Save
        [Test]
        public void EditNewComment()
        {
            try
            {
                AddCommentQA();

                // Open comment menu
                IWebElement customMenu = driver.FindElement(By.CssSelector(".w-4[xmlns='http://www.w3.org/2000/svg'][width='26']"));
                customMenu.Click();
                Thread.Sleep(4000);

                // Click edit button
                IWebElement editButton = driver.FindElement(By.XPath("//span[contains(text(),'Sửa câu hỏi')]"));
                editButton.Click();
                Thread.Sleep(5000);

                // Clear and rewrite comment
                IWebElement editTextarea = driver.FindElement(By.XPath("//textarea[@placeholder='Mô tả vấn đề']"));
                editTextarea.Clear();
                editTextarea.SendKeys("New Comment");
                Thread.Sleep(5000);

                // Save comment
                IWebElement saveButton = driver.FindElement(By.XPath("//button[contains(text(),'Lưu')]"));
                saveButton.Click();
                Thread.Sleep(5000);

                Console.WriteLine("Edit successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                throw;
            }
        }

        // Test 9: Delete comment
        [Test]
        public void DeleteComment()
        {
            try
            {
                AddCommentQA();

                // Open comment menu
                IWebElement customMenu = driver.FindElement(By.CssSelector(".w-4[xmlns='http://www.w3.org/2000/svg'][width='26']"));
                customMenu.Click();
                Thread.Sleep(4000);

                // Click delete button
                IWebElement deleteButton = driver.FindElement(By.XPath("//span[contains(text(),'Xóa câu hỏi')]"));
                deleteButton.Click();
                Thread.Sleep(5000);

                // Confirm deletion
                IWebElement confirmDelete = driver.FindElement(By.XPath("//button[normalize-space()='Xóa']"));
                confirmDelete.Click();
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                throw;
            }
        }

        [TearDown]
        public void Teardown()
        {
            driver?.Quit();
        }
    }
}