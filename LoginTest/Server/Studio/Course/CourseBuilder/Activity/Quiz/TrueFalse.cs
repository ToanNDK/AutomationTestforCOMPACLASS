using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Studio.Course.Activity.Quiz.TF
{
    [TestFixture]
    [Category("Quiz")]
    public class TrueFalse
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
        public void StudioTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = driver.FindElement(By.XPath("//button[.//span[text()='Course']]"));
            course.Click();

            Thread.Sleep(5000);
        }

        public void CourseBuilder()
        {
            StudioTest();
            IWebElement tab = driver.FindElement(By.XPath("//button[normalize-space()='1']"));
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



        public void AddQuestion()
        {
            CourseBuilder();
            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'TrueFalse')]"));
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
        public void TFQuestion()
        {
            AddQuestion();
            IWebElement single = driver.FindElement(By.XPath("//p[normalize-space()='True or False']"));
            single.Click();
            Thread.Sleep(1000);
        }
        //Test 1: Display Settings (UC-S229)
        [Test]
        public void displaySetting()
        {
            TFQuestion();
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
        public void AllowAttempt()
        {
            TFQuestion();
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

        //Test 3: Change Time Limit (UC-S231)

        [Test]
        public void ChangeTimeLimit()
        {
            TFQuestion();
            IWebElement time = driver.FindElement(By.XPath("//input[@id='time-input']"));

            //Nhập giá trị hợp lệ 
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='';", time);  // Xóa nội dung hiện tại
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='00:11:22';", time);  // Nhập giá trị mới

            Thread.Sleep(2000);
            //nhập giá trị không hợp lệ
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='';", time);  // Xóa nội dung hiện tại
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='00:25:700';", time);  // Nhập giá trị mới
            time.SendKeys(Keys.Space);
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[@class='text-sm text-red-500']")));
            Assert.That(errorMessage.Displayed, Is.True, "Thông báo lỗi không hiển thị");
        }
        //Test 4: Nhập content cho câu hỏi:
        [Test]
        public void AddContentQuiz()
        {
            TFQuestion();
            Random rd = new();

            IWebElement question = driver.FindElement(By.XPath("//p[@class='ck-placeholder']"));
            question.SendKeys($" Câu hỏi {rd.Next(1000, 9999)}");
            Thread.Sleep(1000);



            // Nhấn nút Save
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Save']"));
            submit.Click();

            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            try
            {

                IWebElement toastMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//li[contains(@class, 'group toast') and contains(., 'Please select the correct answer')]")));

                // Kiểm tra thông báo đã đúng
                Assert.That(toastMessage.Text.Trim(), Is.EqualTo("Please select the correct answer"), "Thông báo không chính xác");
            }
            catch (WebDriverTimeoutException)
            {
                // Nếu không tìm thấy thông báo trong thời gian chờ
                Assert.Fail("Không tìm thấy thông báo trong thời gian chờ.");
            }
        }

        //Test 5: Chọn đáp án đúng 
        [Test]
        public void ChooseCorrectAnswer()
        {
            AddContentQuiz();
            var answer = driver.FindElements(By.XPath("//div[@class='w-12 h-12 rounded-full flex items-center justify-center border border-gray-200 flex-shrink-0 bg-white text-primary cursor-pointer']"));
            if (answer.Count > 0)
            {
                Random rd = new();
                int RdIndex = rd.Next(0, answer.Count);
                answer[RdIndex].Click();
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("Không tìm thấy đáp án");
            }
            Thread.Sleep(5000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Save']"));
            submit.Click();
            Thread.Sleep(2000);
        }
        //Test 6: Show correct answer (UC-S234)

        [Test]
        public void ShowCorrectAnswer()
        {
            ChooseCorrectAnswer();
            IWebElement toggle = driver.FindElement(By.Id("show-answers"));
            toggle.Click();
            Thread.Sleep(2000);
        }

        //Test 7: Add Guide (UC-S240)
        [Test]
        public void AddGuide()
        {
            AddQuestion();
            IWebElement Slide = driver.FindElement(By.XPath("//span[normalize-space()='Add slide']"));
            Slide.Click();
            Thread.Sleep(1000);

        }
        //Test 8: Modify Guide (UC-S241)
        [Test]
        public void ModifyGuide()
        {
            AddGuide();
            IWebElement Slide = driver.FindElement(By.XPath("//div[@role='button' and contains(@class, 'touch-manipulation')]"));
            Slide.Click();
            Thread.Sleep(2000);
            IWebElement ContentField = driver.FindElement(By.XPath("//p[@class='ck-placeholder']"));
            ContentField.SendKeys("Content Guide");
            Thread.Sleep(1000);
        }
        //Test 9: Change Foramt Text Guide (UC_S242) 
        [Test]
        public void ChangeFormat()
        {
            ModifyGuide();
            IWebElement SelectAll = driver.FindElement(By.XPath("//p[@data-placeholder='Enter your content here...']"));
            SelectAll.SendKeys(Keys.Control + 'a');
            var boldButton = driver.FindElement(By.XPath("//button[@data-cke-tooltip-text='Bold (Ctrl+B)']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", boldButton);
            Thread.Sleep(500);
            js.ExecuteScript("arguments[0].click();", boldButton);

            IWebElement italic = driver.FindElement(By.XPath("//button[@data-cke-tooltip-text='Italic (Ctrl+I)']"));
            js.ExecuteScript("arguments[0].click();", italic);
            Thread.Sleep(1000);
        }

        //Test 10: (UC-249)
        [Test]
        public void AddImage()
        {
            // Bước 1: Tạo câu hỏi (nếu cần thiết)
            AddContentQuiz();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            // Bước 2: Tìm khu vực chứa CKEditor và bấm vào đó để kích hoạt thanh công cụ
            var editorArea = wait.Until(d => d.FindElement(By.XPath("//div[contains(@class, 'ck-editor__editable')]")));
            editorArea.Click();  // Kích hoạt thanh công cụ
            Thread.Sleep(2000);
            // Bước 3: Tìm và bấm vào nút upload trên thanh công cụ CKEditor
            var uploadButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(@class, 'ck-file-dialog-button')]")));
            Thread.Sleep(2000);
            uploadButton.Click();

            Thread.Sleep(1000); // Đợi một chút để cửa sổ chọn file mở ra

            // Bước 4: Tìm phần tử input để gửi file
            var fileInput = wait.Until(d => d.FindElement(By.XPath("//input[@type='file' and contains(@class, 'ck-hidden')]")));

            // Bước 5: Gửi đường dẫn ảnh vào input
            fileInput.SendKeys(@"C:\Users\Hello\Pictures\TestImage\Test1.jpg");

            Thread.Sleep(3000); // Đợi một chút để ảnh được upload vào CKEditor

            // Bước 6: Kiểm tra xem ảnh đã được tải lên thành công chưa
            wait.Until(driver => driver.FindElement(By.XPath("//div[contains(@class, 'ck-content')]//img")));

            Console.WriteLine("Ảnh đã được upload thành công!");
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
