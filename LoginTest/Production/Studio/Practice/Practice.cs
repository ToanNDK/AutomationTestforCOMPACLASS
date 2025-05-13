using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.Studio.Practice
{
    [TestFixture]
    [Category("Practice")]
    public class Blog
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string blogUrl = "https://studio.compaclass.com/en/practice";
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

        //Truy cập Practice từ Studio 
        [Test]
        public void NavigateToPractice()
        {
            driver.Navigate().GoToUrl(blogUrl);
            Login();
            Thread.Sleep(1000);
        }

        [Test]
        public void PracticeTab()
        {
            NavigateToPractice();
            IWebElement CreateNewQuiz = driver.FindElement(By.XPath("//button[normalize-space()='New Practice Quiz']"));
            CreateNewQuiz.Click();
            // Navigate to Practice Page
            Assert.True(driver.Url.Contains("https://studio.compaclass.com/en/practice"));
            Thread.Sleep(1000);

        }

        [Test]
        public void AddQuestion()
        {

            NavigateToPractice();
            IWebElement quiz = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//h3[contains(text(), 'Untitled Practice Quiz')]")));
            quiz.Click();
            Thread.Sleep(500);
        }

        //Test SingleChoice 
        [Test]
        public void SingleChoice()
        {
            AddQuestion();
            //Bấm nút AddQuestion
            IWebElement addquestion = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[normalize-space()='Add question']")));
            addquestion.Click();
            Thread.Sleep(1000);
            //Chọn SingleChoice
            IWebElement single = driver.FindElement(By.XPath("//p[normalize-space()='Single Choice']"));
            single.Click();
            Thread.Sleep(1000);
        }
        [Test]
        public void AddContentSingleQuiz()
        {
            SingleChoice();
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

                IWebElement toastMessage = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//li[contains(@class, 'group toast') and contains(., 'Please select exactly one correct answer')]")));

                // Kiểm tra thông báo đã đúng
                Assert.That(toastMessage.Text.Trim(), Is.EqualTo("Please select exactly one correct answer for single choice question"), "Thông báo không chính xác");
            }
            catch (WebDriverTimeoutException)
            {
                // Nếu không tìm thấy thông báo trong thời gian chờ
                Assert.Fail("Không tìm thấy thông báo trong thời gian chờ.");
            }
        }

        [Test]
        public void ChooseCorrectAnswer()
        {
            AddContentSingleQuiz();
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

        //Test Multiple Choice
        [Test]
        public void MultipleChoice()
        {
            AddQuestion();
            //Bấm nút AddQuestion
            IWebElement addquestion = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[normalize-space()='Add question']")));
            addquestion.Click();
            Thread.Sleep(1000);
            //Chọn MulitpleChoice
            IWebElement multi = driver.FindElement(By.XPath("//p[normalize-space()='Multiple Choice']"));
            multi.Click();
            Thread.Sleep(1000);
        }

        [Test]
        public void AddContentMultipleQuiz()
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

        //Matching

        [Test]
        public void Matching()
        {
            AddQuestion();
            //Bấm nút AddQuestion
            IWebElement addquestion = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[normalize-space()='Add question']")));
            addquestion.Click();
            Thread.Sleep(1000);
            //Chọn SingleChoice
            IWebElement matching = driver.FindElement(By.XPath("//p[normalize-space()='Matching']"));
            matching.Click();
            Thread.Sleep(1000);
        }

        [Test]
        public void AddContentMatching()
        {
            Matching();
            Random rd = new();

            // Nhập câu hỏi ngẫu nhiên
            IWebElement question = driver.FindElement(By.XPath("//p[@class='ck-placeholder']"));
            question.Click();
            Thread.Sleep(300);
            question.SendKeys($"Câu hỏi {rd.Next(1000, 9999)}");
            Thread.Sleep(1000);

            // Nhập câu trả lời ngẫu nhiên bằng SendKeys vào các CKEditor
            var answerElements = driver.FindElements(By.XPath("//div[@contenteditable='true']"));

            if (answerElements.Count == 0)
            {
                Console.WriteLine("Không tìm thấy ô nhập câu trả lời.");
                return;
            }

            foreach (var element in answerElements)
            {
                string answerText = $"Câu trả lời {rd.Next(1000, 9999)}";

                // Click để focus và nhập dữ liệu
                element.Click();
                Thread.Sleep(300);
                element.SendKeys(answerText);
                Thread.Sleep(500);
            }

        }

        //Ordering
        [Test]
        public void Ordering()
        {
            AddQuestion();
            //Bấm nút AddQuestion
            IWebElement addquestion = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[normalize-space()='Add question']")));
            addquestion.Click();
            Thread.Sleep(1000);
            //Chọn SingleChoice
            IWebElement ordering = driver.FindElement(By.XPath("//p[normalize-space()='Ordering']"));
            ordering.Click();
            Thread.Sleep(1000);
        }



        [Test]
        public void AddContentOrdering()
        {

            Ordering();
            Random rd = new();

            // Nhập câu hỏi ngẫu nhiên
            IWebElement question = driver.FindElement(By.XPath("//p[@class='ck-placeholder']"));
            question.SendKeys($" Câu hỏi {rd.Next(1000, 9999)}");
            Thread.Sleep(1000);

            // Nhập câu trả lời ngẫu nhiên
            var answerElements = driver.FindElements(By.XPath("//p[@data-placeholder='Nhập nội dung item...']"));
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

            // Nhấn nút Save sau khi cuộn
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Save']"));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", submit);

            Thread.Sleep(1000);

            js.ExecuteScript("arguments[0].click();", submit);

            Thread.Sleep(1000);
        }

        //TrueFalse
        [Test]
        public void TrueFalse()
        {

            AddQuestion();
            //Bấm nút AddQuestion
            IWebElement addquestion = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[normalize-space()='Add question']")));
            addquestion.Click();
            Thread.Sleep(1000);
            //Chọn SingleChoice
            IWebElement tfquestion = driver.FindElement(By.XPath("//p[normalize-space()='True or False']"));
            tfquestion.Click();
            Thread.Sleep(1000);
        }

        [Test]
        public void AddContentTF()
        {
            TrueFalse();
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

        [Test]
        public void ChooseTF()
        {
            AddContentTF();
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

        //Typing
        [Test]
        public void Typing()
        {
            AddQuestion();
            //Bấm nút AddQuestion
            IWebElement addquestion = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[normalize-space()='Add question']")));
            addquestion.Click();
            Thread.Sleep(1000);
            //Chọn Typing
            IWebElement typing = driver.FindElement(By.XPath("//p[normalize-space()='Typing']"));
            typing.Click();
            Thread.Sleep(1000);
        }
        [Test]
        public void AddContentTyping()
        {
            InitDriver(false);
            Typing();
            Random rd = new();

            // Nhập câu hỏi ngẫu nhiên
            IWebElement question = driver.FindElement(By.XPath("//p[@class='ck-placeholder']"));
            question.SendKeys($" Câu hỏi {rd.Next(1000, 9999)}");
            Thread.Sleep(1000);

            // Nhập câu trả lời ngẫu nhiên
            var answerElements = driver.FindElements(By.XPath("//input[@placeholder='Add answer']"));
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

            // Nhấn nút Save sau khi cuộn
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Save']"));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", submit);

            Thread.Sleep(1000);

            js.ExecuteScript("arguments[0].click();", submit);

            Thread.Sleep(1000);
        }

        //Fill in the blanks

        [Test]
        public void Fillintheblank()
        {

            AddQuestion();
            //Bấm nút AddQuestion
            IWebElement addquestion = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[normalize-space()='Add question']")));
            addquestion.Click();
            Thread.Sleep(1000);
            //Chọn Fill in the blank
            IWebElement tfquestion = driver.FindElement(By.XPath("//p[normalize-space()='Fill in the blank']"));
            tfquestion.Click();
            Thread.Sleep(1000);
        }

        [Test]
        public void AddContentFITB()
        {
            InitDriver(false);
            Fillintheblank();

            // Tìm textarea
            IWebElement blank = driver.FindElement(By.XPath("//textarea[@placeholder='Paste question content here...']"));

            // Nhập nội dung
            blank.SendKeys("Paste");
            Thread.Sleep(1000);

            // Scroll tới element nếu cần (tránh bị che khuất)
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", blank);

            // Double click vào vị trí chính giữa textarea
            Actions action = new(driver);
            action.MoveToElement(blank).DoubleClick().Perform();

            Thread.Sleep(1000);

            IWebElement createblank = driver.FindElement(By.XPath("//button[normalize-space()='Create blank']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", createblank);
            Thread.Sleep(1000);
            blank.SendKeys("TESTING FILL IN THE BLANKS");
            Thread.Sleep(2000);

            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Save']"));
            submit.Click();
            Thread.Sleep(2000);
        }
        private void Login()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            //IWebElement emailInput = driver.FindElement(By.Id("email"));
            IWebElement emailInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("email")));
            emailInput.SendKeys("info@kpim.vn");

            //IWebElement passwordInput = driver.FindElement(By.Id("password"));
            IWebElement passwordInput = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("password")));
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
