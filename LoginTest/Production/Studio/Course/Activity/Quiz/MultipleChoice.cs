using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using TestCompa.Utilities;
using static OpenQA.Selenium.BiDi.Modules.Input.Pointer;
using OpenQA.Selenium.Interactions;

namespace TestCompa.Production.Studio.Course.Activity.Quiz.MC
{
    public class MulitpleChoice
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "https://studio.compaclass.com";
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
            InitDriver(false);
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
            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'MultipleChoice')]"));
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
        public void MultipleChoice()
        {
            AddQuestion();
            IWebElement single = driver.FindElement(By.XPath("//p[normalize-space()='Multiple Choice']"));
            single.Click();
            Thread.Sleep(1000);
        }
        //Test 1: Display Settings (UC-S229)
        [Test]
        public void displaySetting()
        {
            MultipleChoice();
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
            MultipleChoice();
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
            MultipleChoice();
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
            MultipleChoice();
            Random rd = new();

            IWebElement question = driver.FindElement(By.XPath("//p[@data-placeholder='Start typing your question']"));
            question.SendKeys($" Câu hỏi {rd.Next(1000, 9999)}");
            Thread.Sleep(1000);

            // Nhập câu trả lời ngẫu nhiên
            var answerElements = driver.FindElements(By.XPath("//p[@data-placeholder='Nhập nội dung đáp án...']"));
            if (answerElements.Count == 4)
            {
                for (int i = 0; i < answerElements.Count; i++)
                {
                    answerElements[i].SendKeys($" Câu trả lời {rd.Next(1000, 9999)}");
                    Thread.Sleep(500);
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy đúng 4 ô trả lời.");
            }

            // Nhấn nút Save
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Save']"));
            submit.Click();

            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            try
            {

                IWebElement toastMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//li[contains(@class, 'group toast') and contains(., 'Please select at least one correct answer')]")));

                // Kiểm tra thông báo đã đúng
                Assert.That(toastMessage.Text.Trim(), Is.EqualTo("Please select at least one correct answer"), "Thông báo không chính xác");
            }
            catch (WebDriverTimeoutException)
            {
                // Nếu không tìm thấy thông báo trong thời gian chờ
                Assert.Fail("Không tìm thấy thông báo trong thời gian chờ.");
            }
        }

        //Test 5: Chọn đáp án đúng 
        [Test]
        public void ChooseCorrectAnswers()
        {
            AddContentQuiz();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            List<int> selectedIndexes = new();
            Random rd = new();

            // Lấy answers lần đầu
            var answers = wait.Until(drv => drv.FindElements(By.XPath("//div[@class='w-12 h-12 rounded-full flex items-center justify-center border border-gray-200 flex-shrink-0 bg-white text-primary cursor-pointer']")));

            if (answers.Count < 2)
            {
                Console.WriteLine("Không đủ đáp án để chọn!");
                return;
            }

            while (selectedIndexes.Count < 2)
            {
                // Mỗi lần loop cần reload lại danh sách answers
                answers = driver.FindElements(By.XPath("//div[@class='w-12 h-12 rounded-full flex items-center justify-center border border-gray-200 flex-shrink-0 bg-white text-primary cursor-pointer']"));

                int index = rd.Next(0, answers.Count);
                if (!selectedIndexes.Contains(index))
                {
                    selectedIndexes.Add(index);
                    answers[index].Click();
                    Thread.Sleep(500); 
                }
            }

            Thread.Sleep(2000); 
            IWebElement submitButton = driver.FindElement(By.XPath("//button[normalize-space()='Save']"));
            submitButton.Click();

            Thread.Sleep(2000);

            // Kiểm tra lỗi có hiển thị hay không
            bool errorDisplayed = false;
            try
            {
                IWebElement errorMessage = driver.FindElement(By.XPath("//*[contains(text(),'Please select at least 2 correct answers for multiple choice questions')]"));
                if (errorMessage.Displayed)
                {
                    errorDisplayed = true;
                    Console.WriteLine("Thông báo lỗi xuất hiện.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Không có lỗi, bài làm hợp lệ.");
            }
        }


        //Test 6: Show correct answer (UC-S234)

        [Test]
        public void ShowCorrectAnswer()
        {
            ChooseCorrectAnswers();
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


            var ItalicButton = driver.FindElement(By.XPath("//button[@data-cke-tooltip-text='Italic (Ctrl+I)']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView(true);", ItalicButton);
            Thread.Sleep(500);
            js.ExecuteScript("arguments[0].click();", ItalicButton);
            Thread.Sleep(1000);
        }

        //Test UC-S248
        //Chọn câu hỏi muốn thay đổi, nhập thông tin câu hỏi 
        [Test]
        public void ChangeQuestion()
        {
            ChooseCorrectAnswers();
            Thread.Sleep(1000);
            IWebElement question = driver.FindElement(By.XPath("//p[@data-placeholder='Start typing your question']"));
            question.Click();
            question.SendKeys(Keys.Control + 'a');
            question.SendKeys(Keys.Backspace);
            Thread.Sleep(3000);
            question.SendKeys("New Question");
            Thread.Sleep(1000);
        }
        //Upload hình ảnh
        [Test]
        public void UploadImage()
        {
            ChangeQuestion();
            Thread.Sleep(1000);
            Actions actions = new(driver);

            // Lấy tất cả các ô (question và answer)
            var allBoxes = driver.FindElements(By.XPath("//p[@data-placeholder='Start typing your question' or contains(@data-placeholder, 'Answer')]"));

            foreach (var box in allBoxes)
            {
                actions.MoveToElement(box).Click().Perform();
                Thread.Sleep(500);
                box.Clear();
                Thread.Sleep(500);

                // Sau khi click vào ô đó rồi, tìm nút upload toàn trang
                var uploadButton = driver.FindElement(By.XPath("//button[contains(@class, 'ck-file-dialog-button')]"));
                if (uploadButton.Displayed && uploadButton.Enabled)
                {
                    uploadButton.Click();
                    Thread.Sleep(500);

                    var fileInput = driver.FindElement(By.XPath("//input[@type='file' and contains(@class, 'ck-hidden')]"));
                    fileInput.SendKeys(@"C:\Users\Hello\Pictures\TestImage\Test1.jpg");

                    Thread.Sleep(2000);
                }
            }
        }
        //Preview
        [Test]
        public void Preview()
        {
            ChooseCorrectAnswers(); 

            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(30));

            // Đợi nút Preview sẵn sàng
            IWebElement preview = wait.Until(driver => driver.FindElement(By.XPath("//button[normalize-space()='Preview']")));

            // Dùng JavaScript click xuyên Toastify
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", preview);

            Thread.Sleep(2000);

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
