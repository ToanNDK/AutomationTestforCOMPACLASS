/*using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools.V131.FedCm;
using OpenQA.Selenium.Interactions;
using System.Xml.Linq;
using static OpenQA.Selenium.BiDi.Modules.Input.Pointer;

namespace TestCompa.Server.CourseBuilder
{
    public class addCourse
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "http://10.10.10.30:3000/";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(devUrl);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        //Test 0: Truy cập trang 
        [Test]
        public void studioTest()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            Thread.Sleep(3000);
            IWebElement course = driver.FindElement(By.XPath("//button[.//span[text()='Course']]"));
            course.Click();
            
            Thread.Sleep(5000);
        }
        //Test 1: bấm nút vào course builder
        [Test]
        public void courseBuilder()
        {
            studioTest();
            
            Thread.Sleep(5000);

            string expectedText = "CourseTest";


            IWebElement h2Element = driver.FindElement(By.XPath($"//h3[contains(text(),'{expectedText}')]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", h2Element);
            h2Element.Click();
            *//*IWebElement parentDiv = h2Element.FindElement(By.XPath("./ancestor::div[contains(@class, 'flex justify-between')]"));


            IWebElement svgButton = parentDiv.FindElement(By.XPath(".//button[contains(@class, 'group/btn')]"));


            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", svgButton);
            Thread.Sleep(1000);


            svgButton.Click();
            Thread.Sleep(2000);
            IWebElement showVersions = driver.FindElement(By.CssSelector("div[role='menuitem']"));
            showVersions.Click();
            Thread.Sleep(5000);
            IWebElement toBuilder = driver.FindElement(By.XPath("//button[contains(@class, 'group/btn') and contains(@class, 'h-6') and contains(@class, 'w-6')]"));
            toBuilder.Click();
            Thread.Sleep(3000);
            IWebElement Builder = driver.FindElement(By.XPath("//div[normalize-space()='Builder page']"));
            Builder.Click();*//*
            Thread.Sleep(3000);
        }
        
        //Test 2: Drag&Drop Text (UC-S154)
        [Test]
        public void textDragNDrop()
        {
            courseBuilder();
            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'TextBlock')]"));
            title.Click();
            Thread.Sleep(1000);
            IWebElement elementTab = driver.FindElement(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]"));
            elementTab.Click();
            Thread.Sleep(3000);
            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Text Block Icon']]"));

            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            Actions actions = new Actions(driver);
            actions.DragAndDrop(textElement, targetElement).Perform();

            Thread.Sleep(4000);

        }
        //Test 2.1 Nhập text sau khi kéo text vào (UC-S155)
        [Test]
        public void editText()
        {
            textDragNDrop();
            IWebElement textEditor = driver.FindElement(By.CssSelector("div.ck.ck-content.ck-editor__editable"));
            Actions DBclick = new Actions(driver);
            DBclick.DoubleClick(textEditor);
            Thread.Sleep(4000);
            textEditor.SendKeys("abcxyz");
            Thread.Sleep(8000);//chờ autosave
        }
        //2.2 Xóa 
        [Test]
        public void removeText()
        {
            editText();
            IWebElement remove = driver.FindElement(By.CssSelector("button.inline-flex svg.lucide-trash2"));
            remove.Click();
            Thread.Sleep(2000);

        }
        //2.3 Di chuyển các khối text với nhau
        [Test]
        public void ChangeLocation()
        {
            editText();

            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Text Block Icon']]"));
            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            Actions actions = new Actions(driver);

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
        //Test 3: Test Image 
        [Test]
        public void imgDragNDrop()
        {
            courseBuilder();
            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'ImageBlock')]"));
            title.Click();
            Thread.Sleep(1000);
            IWebElement elementTab = driver.FindElement(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]"));
            elementTab.Click();
            Thread.Sleep(3000);

            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Image Block Icon']]"));

            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            Actions actions = new Actions(driver);
            actions.DragAndDrop(textElement, targetElement).Perform();

            Thread.Sleep(4000);
        }
        //3.1 test upload ảnh
        [Test]
        public void imgUpload()
        {
            imgDragNDrop();

            IWebElement clickAddImg = driver.FindElement(By.XPath("//div[contains(@class,'bg-gray-100 flex items-center')]"));
            Actions action = new Actions(driver);
            action.DoubleClick(clickAddImg).Perform();
            Thread.Sleep(3000);

            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            //ảnh test
            string imgPath = @"C:\Users\Hello\Pictures\TestImage\pexels-anjana-c-169994-674010.jpg";

            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(imgPath);
            Thread.Sleep(3000);
            IWebElement remove = driver.FindElement(By.CssSelector("button.inline-flex svg.lucide-trash2"));
            remove.Click();
            Thread.Sleep(2000);
        }
        //3.2 Sử dụng embed (link)
        [Test]
        public void imgEmbed()
        {
            imgDragNDrop();
            IWebElement clickAddImg = driver.FindElement(By.XPath("//div[contains(@class,'bg-gray-100 flex items-center')]"));
            Actions action = new Actions(driver);
            action.DoubleClick(clickAddImg).Perform();
            Thread.Sleep(3000);
            IWebElement embed = driver.FindElement(By.XPath("//button[normalize-space()='Embed link']"));
            embed.Click();
            Thread.Sleep(4000);
            IWebElement embedImg = driver.FindElement(By.CssSelector("input[placeholder = 'Paste the image link']"));
            embedImg.SendKeys("https://media.istockphoto.com/id/814423752/photo/eye-of-model-with-colorful-art-make-up-close-up.jpg?s=612x612&w=0&k=20&c=l15OdMWjgCKycMMShP8UK94ELVlEGvt7GmB_esHWPYE=");
            Thread.Sleep(1000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Embed']"));
            submit.Click();
            Thread.Sleep(3000);
            IWebElement remove = driver.FindElement(By.CssSelector("button.inline-flex svg.lucide-trash2"));
            remove.Click();
            Thread.Sleep(2000);

        }
        //3.3 Thay đổi ảnh
        [Test]
        public void changeImg()
        {
            imgDragNDrop();

            IWebElement clickAddImg = driver.FindElement(By.XPath("//div[contains(@class,'bg-gray-100 flex items-center')]"));
            Actions action = new Actions(driver);
            action.DoubleClick(clickAddImg).Perform();
            Thread.Sleep(3000);

            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            //ảnh test
            string imgPath = @"C:\Users\Hello\Pictures\TestImage\pexels-anjana-c-169994-674010.jpg";

            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(imgPath);
            Thread.Sleep(3000);


            IWebElement changeButton = driver.FindElement(By.XPath("//button[normalize-space()='Change']"));
            changeButton.Click();
            Thread.Sleep(5000);

            IWebElement embed = driver.FindElement(By.XPath("//button[normalize-space()='Embed link']"));
            embed.Click();
            Thread.Sleep(4000);
            IWebElement embedImg = driver.FindElement(By.CssSelector("input[placeholder = 'Paste the image link']"));
            embedImg.SendKeys("https://media.istockphoto.com/id/814423752/photo/eye-of-model-with-colorful-art-make-up-close-up.jpg?s=612x612&w=0&k=20&c=l15OdMWjgCKycMMShP8UK94ELVlEGvt7GmB_esHWPYE=");
            Thread.Sleep(1000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Embed']"));
            submit.Click();
            Thread.Sleep(3000);


        }
        //Test 3.4 Đổi ảnh nhưng đường dẫn sai 
        [Test]
        public void invalidUrlImg()
        {
            imgDragNDrop();

            IWebElement clickAddImg = driver.FindElement(By.XPath("//div[contains(@class,'bg-gray-100 flex items-center')]"));
            Actions action = new Actions(driver);
            action.DoubleClick(clickAddImg).Perform();
            Thread.Sleep(3000);

            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            //ảnh test
            string imgPath = @"C:\Users\Hello\Pictures\TestImage\pexels-anjana-c-169994-674010.jpg";

            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(imgPath);
            Thread.Sleep(3000);


            IWebElement changeButton = driver.FindElement(By.XPath("//button[normalize-space()='Change']"));
            changeButton.Click();
            Thread.Sleep(5000);

            IWebElement embed = driver.FindElement(By.XPath("//button[normalize-space()='Embed link']"));
            embed.Click();
            Thread.Sleep(4000);
            IWebElement embedImg = driver.FindElement(By.CssSelector("input[placeholder = 'Paste the image link']"));
            embedImg.SendKeys("abcxyz");
            Thread.Sleep(1000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Embed']"));
            submit.Click();
            Thread.Sleep(3000);
            //thông báo lỗi
            IWebElement errorMessage = wait.Until(d => d.FindElement(By.XPath("//p[@class='text-red text-xs']")));
            Assert.IsTrue(errorMessage.Displayed, "Thông báo lỗi không hiển thị khi nhập sai thông tin!");
            Thread.Sleep(2000);

        }
        //Test 3.5 Điều chỉnh ảnh 
        [Test]
        public void editSizeImg()
        {
            imgDragNDrop();
            Thread.Sleep(2000);
            IWebElement clickAddImg = driver.FindElement(By.XPath("//div[contains(@class,'bg-gray-100 flex items-center')]"));
            Actions action = new Actions(driver);
            action.DoubleClick(clickAddImg).Perform();
            Thread.Sleep(3000);

            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            //ảnh test
            string imgPath = @"C:\Users\Hello\Pictures\TestImage\pexels-anjana-c-169994-674010.jpg";

            // Gửi đường dẫn ảnh vào input file
            inputFile.SendKeys(imgPath);
            Thread.Sleep(3000);
            IWebElement edit = driver.FindElement(By.Id("radix-:r5m:"));
            edit.Click();
            Thread.Sleep(5000);
            IWebElement setFillImg = driver.FindElement(By.CssSelector(".lucide.lucide - move - horizontal"));
            setFillImg.Click();
            Thread.Sleep(5000);

        }
        //Test 4 Code
        [Test]
        public void codeDragNDrop()
        {
            courseBuilder();


            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'CodeBlock')]"));
            title.Click();
            Thread.Sleep(1000);


            IWebElement elementTab = driver.FindElement(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]"));
            elementTab.Click();
            Thread.Sleep(3000);


            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Code Block Icon']]"));


            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", targetElement);
            Thread.Sleep(1000);


            Actions actions = new Actions(driver);
            actions.ClickAndHold(textElement)
                   .MoveToElement(targetElement, 0, -20)
                   .Release()
                   .Perform();

            Thread.Sleep(4000);
        }

        //4.1 Nhập code vào 
        [Test]
        public void editCode()
        {
            codeDragNDrop();  // Kéo Code Block vào vùng trắng

            // Tìm và cuộn đến phần tử
            IWebElement element = driver.FindElement(By.XPath("//div[contains(@class, 'pointer-events-auto') and contains(@class, 'group-hover/cell:border-blue')]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            Thread.Sleep(1000);

            // Kiểm tra nếu phần tử có thể nhấn
            Thread.Sleep(5000);

            // Dùng JavaScript click để tránh lỗi bị che khuất
            js.ExecuteScript("arguments[0].click();", element);
            Thread.Sleep(2000);

            // Lấy danh sách tất cả vùng nhập code
            IList<IWebElement> codeEditors = driver.FindElements(By.XPath("//div[contains(@class, 'cm-content')]"));

            if (codeEditors.Count > 0)
            {
                // Chọn vùng nhập cuối cùng
                IWebElement lastCodeEditor = codeEditors[codeEditors.Count - 1];

                // Cuộn tới vùng nhập cuối cùng
                js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", lastCodeEditor);
                Thread.Sleep(3000);

                // Nhấn vào vùng nhập trước khi nhập nội dung
                Actions actions = new Actions(driver);
                actions.MoveToElement(lastCodeEditor).Click().SendKeys("<h1>COMPACLASS</h1>").Perform();
                Thread.Sleep(5000);
            }
            else
            {
                Console.WriteLine("Không tìm thấy vùng nhập code nào.");
            }

            Thread.Sleep(2000);
        }

        //4.2 Test nhập code với Language bất kì 
        [Test]
        public void htmlCode()
        {
            codeDragNDrop();
            IWebElement element = driver.FindElement(By.XPath("//div[contains(@class, 'pointer-events-auto') and contains(@class, 'group-hover/cell:border-blue')]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            Thread.Sleep(1000);

            Thread.Sleep(5000);

            js.ExecuteScript("arguments[0].click();", element);
            Thread.Sleep(2000);

            IWebElement typeCode = driver.FindElement(By.CssSelector(".lucide.lucide-code-xml"));
            typeCode.Click();
            Thread.Sleep(4000);

            IWebElement menu = driver.FindElement(By.XPath("//div[@role='menu']"));

            IList<IWebElement> menuItems = menu.FindElements(By.XPath(".//div[@role='menuitem']"));

            if (menuItems.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(menuItems.Count);
                IWebElement selectedItem = menuItems[index];

                IWebElement spanElement = selectedItem.FindElement(By.XPath(".//span"));

                IJavaScriptExecutor js1 = (IJavaScriptExecutor)driver;
                js1.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", selectedItem);
                Thread.Sleep(500);

                selectedItem.Click();
                Thread.Sleep(5000);

            }
            else
            {
                Console.WriteLine("Không tìm thấy lựa chọn nào.");
            }

            Actions actions = new Actions(driver);
            actions.MoveToElement(element).Click().SendKeys("<h1>COMPACLASS</h1>").Perform();
            Thread.Sleep(2000);
        }
        //Test 4.3 Test các mode light dark cold
        [Test]
        public void mode()
        {
            codeDragNDrop();
            Thread.Sleep(5000);

            IWebElement mode = driver.FindElement(By.CssSelector("button.inline-flex svg.lucide-moon-star"));
            mode.Click();

            Thread.Sleep(2000);
            IWebElement light = driver.FindElement(By.XPath("//span[normalize-space()='Light']"));
            light.Click();
            Thread.Sleep(4000);
            IWebElement remove = driver.FindElement(By.CssSelector("button.inline-flex svg.lucide-trash2"));
            remove.Click();
            Thread.Sleep(2000);
        }

        //Test 5: Video
        [Test]
        public void videoDragNDrop()
        {
            courseBuilder();
            
            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'VideoBlock')]"));
            title.Click();
            Thread.Sleep(1000);
            IWebElement elementTab = driver.FindElement(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]"));
            elementTab.Click();
            Thread.Sleep(3000);

            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Video Block Icon']]"));

            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            Actions actions = new Actions(driver);
            actions
                    .MoveToElement(textElement)
                    .ClickAndHold()
                    .MoveToElement(targetElement)
                    .Release()
                    .Perform();

            Thread.Sleep(4000);
        }
        //5.1 Upload Video từ máy tính
        [Test]
        public void uploadVideo()
        {
            videoDragNDrop(); 
            IWebElement uploadField = driver.FindElement(By.XPath("//p[contains(@class,'text-xs truncate flex-1')]"));
            uploadField.Click();
            Thread.Sleep(3000);
           *//* IWebElement upload = driver.FindElement(By.XPath("//label[@for='video-audio']"));
            upload.Click();*//*

            IWebElement inputFile = driver.FindElement(By.XPath("//input[@type='file']"));
            //video test
            string vidPath = @"C:\Users\Hello\Videos\Captures\Home - Google Chrome 2024-12-13 10-32-40.mp4";

            inputFile.SendKeys(vidPath);

            Thread.Sleep(4000);


        }
        //5.2 Embed video từ URL

        [Test]
        public void embedVideo()
        {
            videoDragNDrop();
            IWebElement uploadbtn = driver.FindElement(By.CssSelector("div.text-gray-d10.cursor-pointer"));
            uploadbtn.Click();
            Thread.Sleep(3000);
            IWebElement embedLink = driver.FindElement(By.XPath("//button[normalize-space()='Embed link']"));
            embedLink.Click();

            IWebElement link = driver.FindElement(By.XPath("//input[@id='video-link']"));
            link.SendKeys("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerFun.mp4");
            Thread.Sleep(5000);
            IWebElement submit = driver.FindElement(By.XPath("//button[normalize-space()='Embed']"));
            submit.Click();
            Thread.Sleep(3000);
        }

        //5.3 Test toggle title và description 
        [Test]
        public void toggleTitleAndDescription()
        {
            embedVideo();
            Thread.Sleep(5000);
            IWebElement titleToggle = driver.FindElement(By.Id("video-title"));
            titleToggle.Click();
            Thread.Sleep(2000);
            IWebElement descToggle = driver.FindElement(By.Id("video-desciption"));
            descToggle.Click();
            Thread.Sleep(2000);
            IWebElement remove = driver.FindElement(By.CssSelector("button.inline-flex svg.lucide-trash2"));
            remove.Click();
            Thread.Sleep(2000);
        }
        //5.4 Thêm title, description
       
        [Test]
        public void addTitleAndDesc()
        {
            embedVideo();
            Thread.Sleep(4000);
            IWebElement titleField = driver.FindElement(By.XPath("//input[@placeholder='Add video title...']"));
            titleField.SendKeys("Title Video 123");
            Thread.Sleep(2000);
            IWebElement descField = driver.FindElement(By.XPath("//input[@placeholder='Write a description...']"));
            titleField.SendKeys("Description Video 123");
            Thread.Sleep(2000);
            IWebElement remove = driver.FindElement(By.CssSelector("button.inline-flex svg.lucide-trash2"));
            remove.Click();
            Thread.Sleep(2000);
        }

        //Test 6: Markdown
        [Test]
        public void markdownDragNDrop()
        {
             courseBuilder();
            
            IWebElement title = driver.FindElement(By.XPath("//p[starts-with(normalize-space(), 'MarkdownBlock')]"));
            title.Click();
            Thread.Sleep(1000);
            IWebElement elementTab = driver.FindElement(By.XPath("//button[contains(@class, 'flex flex-col gap-1 justify-center items-center') and .//span[contains(text(),'Elements')]]"));
            elementTab.Click();
            Thread.Sleep(3000);

            IWebElement textElement = driver.FindElement(By.XPath("//div[img[@alt='Markdown Block Icon']]"));

            IWebElement targetElement = driver.FindElement(By.XPath("//div[contains(@class, 'border-t-4 border-t-transparent')]"));

            Actions actions = new Actions(driver);
            actions
                    .MoveToElement(textElement)
                    .ClickAndHold()
                    .MoveToElement(targetElement)
                    .Release()
                    .Perform();

            Thread.Sleep(4000);
        }
        //6.1 Test các chức năng trên tab
        [Test]
        public void toolbarTest()
        {
            markdownDragNDrop();

            IList<IWebElement> toolbarButtons = driver.FindElements(By.CssSelector("div.w-md-editor-toolbar button[data-name]"));

            foreach (IWebElement button in toolbarButtons)
            {
                try
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", button);

                    if (button.Displayed && button.Enabled)
                    {
                        string buttonName = button.GetAttribute("data-name");

                        // Nhấn nút
                        button.Click();

                        Console.WriteLine($"Đã ấn nút: {buttonName}");
                        Thread.Sleep(1000); 
                    }
                    else
                    {
                        Console.WriteLine($"Nút không thể nhấn được: {button.GetAttribute("data-name")}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi nhấn nút: {ex.Message}");
                }
            }
        }
        

        public void Login()
        {
            Thread.Sleep(2000);
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("info@kpim.vn");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
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
*/