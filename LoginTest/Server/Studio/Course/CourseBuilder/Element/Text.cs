using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Server.CourseBuilder.Text
{
    public class Text
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
            Thread.Sleep(1000);
            IWebElement courseContent = driver.FindElement(By.XPath("//button[normalize-space()='Course Content']"));
            courseContent.Click();
            Thread.Sleep(2000);

        }

        //Test 2: Drag&Drop Text (UC-S154)
        [Test]
        public void TextDragNDrop()
        {
            CourseBuilder();

            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'TextBlock')]"));
            title.Click();
            Thread.Sleep(1000);
            IWebElement elementTab = driver.FindElement(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]"));
            elementTab.Click();
            Thread.Sleep(3000);
            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Text Block Icon']]"));

            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            Actions actions = new(driver);
            actions.DragAndDrop(textElement, targetElement).Perform();

            Thread.Sleep(4000);

        }
        //Test 3 Nhập text sau khi kéo text vào (UC-S155)
        [Test]
        public void EditText()
        {
            TextDragNDrop();
            IWebElement textEditor = driver.FindElement(By.CssSelector("div.ck.ck-content.ck-editor__editable"));
            Actions DBclick = new(driver);
            DBclick.DoubleClick(textEditor);
            Thread.Sleep(4000);
            textEditor.SendKeys("abcxyz");
            Thread.Sleep(5000);//chờ autosave
        }
        //Test 4 Thay đổi cài đặt liên quan đến font (typeface) của chữ (UC-S156) (Chưa có)
        [Test]
        public void ChangeFont()
        {


        }
        //Test 5:Thay đổi FontSize ( Chưa có ) (UC-S157)
        /*[Test]
        public void changeFontSize()
        {

        }*/
        //Test 6: Thay đổi Format ( in đậm, nghiêng, ...) (UC-S158)
        [Test]
        public void ChangeFormat()
        {
            EditText();
            IWebElement textEditor = driver.FindElement(By.CssSelector("div.ck.ck-content.ck-editor__editable"));
            textEditor.Click();
            Thread.Sleep(2000);
            textEditor.SendKeys(Keys.Control + 'a');
            Thread.Sleep(3000);
            textEditor.SendKeys(Keys.Control + 'b');

            Thread.Sleep(1000);

            textEditor.SendKeys(Keys.Control + 'i');
            Thread.Sleep(1000);
            textEditor.SendKeys(Keys.Control + 'u');
            Thread.Sleep(1000);

        }
        //Test 7: Thay đổi alignment (UC-S159)
        [Test]
        public void ChangeAlignment()
        {
            ChangeFormat();
            IWebElement textEditor = driver.FindElement(By.CssSelector("div.ck.ck-content.ck-editor__editable"));
            Thread.Sleep(2000);
            textEditor.SendKeys(Keys.Control + 'a');
            IWebElement alignRight = driver.FindElement(By.CssSelector(".lucide.lucide-align-horizontal-justify-end"));
            alignRight.Click();
            Thread.Sleep(2000);
            IWebElement alignCenter = driver.FindElement(By.CssSelector(".lucide.lucide-align-horizontal-space-around"));
            alignCenter.Click();
            Thread.Sleep(2000);
            IWebElement alignLeft = driver.FindElement(By.CssSelector(".lucide.lucide-align-horizontal-justify-start"));
            alignLeft.Click();
            Thread.Sleep(2000);
        }
        //Test 8: Đổi màu chữ ( Chưa có ) (UC-S160)
        //Test 9: ĐỔi độ bão hòa màu chữ ( Chưa có ) (UC-S161)
        //Test 10: Đổi heading (UC-S162)
        [Test]
        public void ChangeHeading()
        {
            ChangeFormat();

            // Chọn toàn bộ nội dung trong editor
            IWebElement textEditor = driver.FindElement(By.CssSelector("div.ck.ck-content.ck-editor__editable"));
            textEditor.Click();
            Thread.Sleep(500);
            textEditor.SendKeys(Keys.Control + "a");

            // Mở dropdown "Paragraph"
            IWebElement headingDropdown = driver.FindElement(By.CssSelector("svg.lucide-type"));
            headingDropdown.Click();
            Thread.Sleep(3000); // Đợi dropdown hiển thị

            // Random một heading từ 1 đến 6
            Random random = new();
            int headingLevel = random.Next(1, 7); // Tạo số ngẫu nhiên từ 1 đến 6

            // Tạo XPath dùng text (normalize-space) của heading
            string headingText = $"Heading {headingLevel}";
            string headingXPath = $"//span[normalize-space()='{headingText}']/ancestor::div[@role='menuitem']";
            Thread.Sleep(2000);
            // Tìm và click vào heading tương ứng
            IWebElement heading = driver.FindElement(By.XPath(headingXPath));
            heading.Click();

            Thread.Sleep(2000);
        }



        //2.2 Xóa  
        [Test]
        public void RemoveText()
        {
            EditText();
            IWebElement remove = driver.FindElement(By.CssSelector("button.inline-flex svg.lucide-trash2"));
            remove.Click();
            Thread.Sleep(2000);

        }
        //2.3 Di chuyển các khối text với nhau
        [Test]
        public void ChangeLocation()
        {
            EditText();

            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Text Block Icon']]"));
            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            Actions actions = new(driver);

            // Kéo phần tử "Text Block Icon" vào vị trí "targetElement"
            actions.ClickAndHold(textElement)
                   .MoveToElement(targetElement)
                   .Release()
                   .Perform();

            actions.Click(textElement)
           .Click(textElement)
           .SendKeys("Text Block")
           .Perform();
            Thread.Sleep(4000);

            // Tìm icon move để tiếp tục kéo lên trên cùng
            IWebElement move = driver.FindElement(By.CssSelector(".lucide.lucide-move"));

            // Kéo icon move lên đầu danh sách
            actions.ClickAndHold(move)
                   .MoveByOffset(0, -300)  // Di chuyển lên trên (giá trị Y có thể chỉnh sửa)
                   .Release()
                   .Perform();

            Thread.Sleep(8000);
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
        public void createChapter()
        {

        }
        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }


}


