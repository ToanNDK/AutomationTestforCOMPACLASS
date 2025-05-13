using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.Studio.Course.Activity.Quiz.Ordering
{
    [TestFixture]
    [Category("Quiz")]
    public class Ordering
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
            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'Ordering')]"));
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
        public void OrderingQuestion()
        {
            AddQuestion();
            IWebElement single = driver.FindElement(By.XPath("//p[normalize-space()='Ordering']"));
            single.Click();
            Thread.Sleep(1000);
        }
        //Test 1: Display Settings (UC-S229)
        [Test]
        public void displaySetting()
        {
            OrderingQuestion();
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
            OrderingQuestion();
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
            OrderingQuestion();
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
            OrderingQuestion();
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

        //Test 5: Kéo thả 
        [Test]
        public void DragNDrop()
        {
            AddContentQuiz();
            // Lấy tất cả các ô mục tiêu có class p-1 border-dashed
            var targetElements = driver.FindElements(By.XPath("//div[@class='p-1 border-dashed transition-colors flex justify-center items-center w-10 h-10 rounded border']"));

            // Kiểm tra nếu có ít nhất 4 ô mục tiêu
            if (targetElements.Count > 0)
            {
                Random random = new();

                // Danh sách lưu các ô mục tiêu đã được sử dụng
                List<IWebElement> usedTargets = new List<IWebElement>();

                // Lặp qua các đáp án và kéo từng phần tử vào ô mục tiêu ngẫu nhiên
                for (int i = 0; i < 4; i++) // Giả sử bạn có 4 đáp án
                {
                    // Kiểm tra nếu còn ô mục tiêu để kéo vào
                    if (usedTargets.Count < targetElements.Count)
                    {
                        // Chọn một ô mục tiêu ngẫu nhiên chưa được sử dụng
                        IWebElement targetElement = null;
                        do
                        {
                            int randomIndex = random.Next(0, targetElements.Count);
                            targetElement = targetElements[randomIndex];
                        } while (usedTargets.Contains(targetElement));

                        // Thêm ô mục tiêu vào danh sách đã sử dụng
                        usedTargets.Add(targetElement);

                        // Tìm phần tử đáp án cần kéo (ví dụ: A, B, C, D)
                        IWebElement answerTab = driver.FindElement(By.XPath($"//div[contains(text(),'{(char)('A' + i)}')]"));

                        // Cuộn tới phần tử tab để đảm bảo phần tử có thể nhìn thấy
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("arguments[0].scrollIntoView(true);", answerTab);

                        // Sử dụng Actions để kéo và thả
                        Actions actions = new Actions(driver);
                        actions.DragAndDrop(answerTab, targetElement).Perform();
                        Thread.Sleep(1000); // Cho thời gian để hành động diễn ra

                    }
                    else
                    {
                        Console.WriteLine("Không đủ ô mục tiêu.");
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy ô mục tiêu.");
            }
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Save']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].click();", submit);
            Thread.Sleep(1000);
        }


        [Test]
        public void PreviewCheckAnswerMode()
        {
            DragNDrop();


            PreviewQuestion();

            Thread.Sleep(1000);

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

        /*//Test 10: (UC-249)
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
        }*/


        public void PreviewQuestion()
        {
            IWebElement btnPreview = driver.FindElement(By.XPath("//button[normalize-space()='Preview']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].click();", btnPreview);

            IWebElement mode = driver.FindElement(By.XPath("//button[@role='combobox' and .//div[text()='Basic answer mode']]"));
            mode.Click();

            Thread.Sleep(2000);

            // Lấy danh sách tất cả các item trong dropdown
            var answerModes = driver.FindElements(By.XPath("//div[contains(@class,'font-medium')]"));

            // Chọn ngẫu nhiên một chế độ
            Random random = new Random();
            int index = random.Next(answerModes.Count);

            // Click vào chế độ được chọn
            answerModes[index].Click();

            Thread.Sleep(1000); // Chờ dropdown xử lý

            var answers = driver.FindElements(By.XPath("//div[@role='button']"));

            Actions actions = new(driver);

            // Ví dụ: kéo A -> D
            actions.ClickAndHold(answers[1])    // A
                   .MoveToElement(answers[4])   // D
                   .Release()
                   .Build()
                   .Perform();

            Thread.Sleep(1000); // Chờ animation

            // Ví dụ: kéo B -> C
            actions.ClickAndHold(answers[2])    // B
                   .MoveToElement(answers[3])   // C
                   .Release()
                   .Build()
                   .Perform();

            Thread.Sleep(1000);
            IWebElement check = driver.FindElement(By.XPath("//button[normalize-space()='Check']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", check);
            Thread.Sleep(1000);
        }

        public void Login()
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
