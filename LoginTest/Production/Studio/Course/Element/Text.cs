using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace TestCompa.Production.CourseBuilder.Text
{
    [TestFixture]
    [Category("Element")]
    public class Text
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;
        private readonly string devUrl = "https://studio.compaclass.com/";
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

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement textEditor = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector("div.ck.ck-content.ck-editor__editable")));

            textEditor.Click();

            // Đợi nội dung editor khả dụng
            wait.Until(driver =>
            {
                var content = textEditor.Text;
                return !string.IsNullOrEmpty(content);
            });

            textEditor.SendKeys(Keys.Control + "a");
            textEditor.SendKeys(Keys.Control + "b");
            textEditor.SendKeys(Keys.Control + "i");
            textEditor.SendKeys(Keys.Control + "u");
        }
        //Test 7: Thay đổi alignment (UC-S159)
        [Test]
        public void ChangeAlignment()
        {
            ChangeFormat();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement textEditor = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector("div.ck.ck-content.ck-editor__editable")));
            textEditor.SendKeys(Keys.Control + "a");

            IWebElement alignRight = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector(".lucide.lucide-align-horizontal-justify-end")));
            alignRight.Click();

            IWebElement alignCenter = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector(".lucide.lucide-align-horizontal-space-around")));
            alignCenter.Click();

            IWebElement alignLeft = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector(".lucide.lucide-align-horizontal-justify-start")));
            alignLeft.Click();
        }
        //Test 8: Đổi màu chữ ( Chưa có ) (UC-S160)
        //Test 9: ĐỔi độ bão hòa màu chữ ( Chưa có ) (UC-S161)
        //Test 10: Đổi heading (UC-S162)
        /*[Test]
        public void ChangeHeading()
        {
            InitDriver(false);
            ChangeFormat();

            // Mở dropdown "Paragraph"
            IWebElement headingDropdown = driver.FindElement(By.XPath("//span[normalize-space(text())='Paragraph']"));
            headingDropdown.Click();
            Thread.Sleep(3000); // Đợi dropdown hiển thị

            // Lấy tất cả các mục Heading trong dropdown (chứa "Heading" trong text)
            IReadOnlyCollection<IWebElement> headingOptions = driver.FindElements(By.XPath("//div[@role='menuitem']//span[contains(normalize-space(.), 'Heading')]"));

            if (headingOptions.Count == 0)
            {
                Assert.Fail("Không tìm thấy heading nào trong dropdown.");
            }

            // Random chọn 1 heading trong danh sách
            Random random = new();
            int randomIndex = random.Next(headingOptions.Count);

            IWebElement[] headingArray = headingOptions.ToArray();
            IWebElement selectedHeading = headingArray[randomIndex];
            selectedHeading.Click();

            Thread.Sleep(2000);
        }
*/



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
            InitDriver(false);
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
            // Tìm icon move và đợi đến khi nó khả dụng
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement move = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector(".lucide.lucide-move")));

            // Scroll icon vào vùng hiển thị
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", move);

            // Thực hiện kéo lên trên

            actions.ClickAndHold(move)
                   .MoveToElement(move, 0, -100)  // Di chuyển lên trên
                   .Release()
                   .Perform();
            Thread.Sleep(3000);
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


