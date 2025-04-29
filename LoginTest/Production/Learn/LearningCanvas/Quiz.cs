using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.Learn.LearningCanvas
{
    public class Quiz
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;


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
        //Test 1: Truy cập learning canvas
        [Test]
        public void testContinueLearn()
        {
            driver.Navigate().GoToUrl("http://compaclass.com/learn/class");
            Thread.Sleep(5000);
            Login();
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(2000);

            Thread.Sleep(5000);
            IWebElement testclass = wait.Until(d => d.FindElement(By.XPath("//a[contains(text(),'Power BI Cơ Bản - BSC')]")));
            ScrollToElement(testclass);
            Thread.Sleep(2000);
            testclass.Click();
            Thread.Sleep(5000);

            IWebElement gotoLearn = driver.FindElement(By.XPath("//button[@class='group/btn inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium ring-offset-white transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 bg-primary text-white hover:bg-primary/90 h-10 px-4 py-2 w-full relative']//a[@class='absolute inset-0']"));
            gotoLearn.Click();
            Thread.Sleep(5000);

        }

        //Test 2: Truy cập tới CourseContent
        [Test]
        public void gotoCourseContent()
        {
            testContinueLearn();
            IWebElement chapterLink = driver.FindElement(By.XPath("//a[text()='Chapter 5: Tạo biểu đồ và thiết kế báo cáo']"));
            chapterLink.Click();
            Thread.Sleep(3000);
        }
        //Test 3:Chọn Quiz
        [Test]
        public void selectQuiz()
        {
            gotoCourseContent();
            IWebElement quizTest = driver.FindElement(By.XPath("//a[contains(text(),'Bài kiểm tra cuối khoá')]"));
            quizTest.Click();
            Thread.Sleep(2000);

        }
        //Test 4: Bấm Start
        [Test]
        public void startQuiz()
        {
            selectQuiz();
            IWebElement start = driver.FindElement(By.CssSelector("button[class=' min-w-[140px] bg-primary rounded-[50px] px-12 py-4 text-white font-medium']"));
            start.Click();
            Thread.Sleep(3000);
        }
        //Test 5: Thực hiện làm bài
        [Test]
        public void testScrollBar()
        {
            startQuiz();
            int questionCount = 0;

            while (questionCount < 12)
            {

                IList<IWebElement> answerOptions = driver.FindElements(By.XPath("//button[contains(@class, 'p-2 h-full rounded-md')]"));

                if (answerOptions.Count > 0)
                {
                    Random random = new();


                    int numberOfChoices = random.Next(1, answerOptions.Count + 1);
                    // Console.WriteLine($"Câu {questionCount + 1}: Chọn {numberOfChoices} đáp án");


                    List<IWebElement> selectedOptions = answerOptions.OrderBy(x => random.Next()).Take(numberOfChoices).ToList();


                    foreach (var option in selectedOptions)
                    {
                        option.Click();
                        //  Console.WriteLine($"Chọn đáp án: {option.Text}");
                    }
                }
                else
                {
                    Console.WriteLine($"Câu {questionCount + 1}: Không tìm thấy đáp án nào!");
                }

                Thread.Sleep(2000);

                try
                {

                    IWebElement submit = driver.FindElement(By.XPath("//button[contains(text(),'Nộp bài')]"));

                    if (!submit.Enabled || submit.GetAttribute("disabled") != null)
                    {
                        // Console.WriteLine("Nút 'Nộp bài' đã bị vô hiệu hóa. Kết thúc bài kiểm tra.");
                        break;
                    }

                    submit.Click();
                    Thread.Sleep(2000);
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Không tìm thấy nút 'Nộp bài'. Kết thúc bài kiểm tra.");
                    break;
                }
                catch (ElementClickInterceptedException)
                {
                    // Console.WriteLine("Nút 'Nộp bài' bị che khuất hoặc không thể click. Kết thúc bài kiểm tra.");
                    break;
                }

                questionCount++;

                if (questionCount == 12)
                {
                    // Console.WriteLine("Hoàn thành 12 câu hỏi. Tìm nút 'Hoàn thành'...");

                    try
                    {
                        IWebElement completeButton = driver.FindElement(By.XPath("//button[contains(text(),'Hoàn thành')]"));
                        completeButton.Click();
                        //  Console.WriteLine("Đã nhấn nút 'Hoàn thành'.");
                        Thread.Sleep(3000);
                    }
                    catch (NoSuchElementException)
                    {
                        // Console.WriteLine("Không tìm thấy nút 'Hoàn thành'.");
                    }
                    catch (ElementClickInterceptedException)
                    {
                        // Console.WriteLine("Nút 'Hoàn thành' bị che khuất hoặc không thể click.");
                    }

                    break; // Dừng vòng lặp sau khi hoàn thành
                }

                // Click vào "Câu hỏi tiếp theo"
                try
                {
                    IWebElement next = driver.FindElement(By.XPath("//button[contains(text(),'Câu hỏi tiếp theo')]"));

                    if (!next.Enabled || next.GetAttribute("disabled") != null)
                    {
                        Console.WriteLine("Nút 'Câu hỏi tiếp theo' đã bị vô hiệu hóa. Kết thúc bài kiểm tra.");
                        break;
                    }

                    next.Click();
                    Thread.Sleep(3000);
                }
                catch (NoSuchElementException)
                {
                    //Console.WriteLine("Không tìm thấy nút 'Câu hỏi tiếp theo'. Kết thúc bài kiểm tra.");
                    break;
                }
                catch (ElementClickInterceptedException)
                {
                    //Console.WriteLine("Nút 'Câu hỏi tiếp theo' bị che khuất hoặc không thể click. Kết thúc bài kiểm tra.");
                    break;
                }
            }

            Console.WriteLine("Hoàn tất bài kiểm tra.");
        }
        //Test 6: Nhấn hoàn thành khi chưa làm bài -> Hiển thị popup

        [Test]
        public void finishWithoutTest()
        {
            startQuiz();
            IWebElement completeButton = driver.FindElement(By.XPath("//button[contains(text(),'Hoàn thành')]"));
            completeButton.Click();
            Thread.Sleep(4000);
            IWebElement popup = driver.FindElement(By.XPath("//div[@role='alertdialog']"));

            Assert.That(popup.Displayed, Is.True, "Không hiển thị thông báo");
            Thread.Sleep(5000);
        }




        public void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});", element);
        }
        public void Login()
        {
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
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
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
