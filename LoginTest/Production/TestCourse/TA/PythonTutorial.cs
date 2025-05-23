﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestCompa.Production.TestCourse.TA.PythonTutorial
{
    public class CourseTests
    {
        private IWebDriver driver = null!;
        private WebDriverWait wait = null!;

        private void InitDriver(bool headless)
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
            driver.Navigate().GoToUrl("https://auth.compaclass.com/Auth/SignIn");
        }

        //Xem nội dung trang overview
        [Test]
        public void runOverviewContent()
        {
            try
            {
                InitDriver(headless: true);  // chạy headless trước
                content();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                content();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }

        //Tìm kiếm & Sort Đánh giá
        //[Test]
        public void runSearchComment()
        {
            try
            {
                InitDriver(headless: true);  // chạy headless trước
                searchComment();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                searchComment();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }


        //Thêm câu hỏi
        //[Test]
        public void addQuestion()
        {
            try
            {
                InitDriver(headless: true);  // chạy headless trước
                RunAddQuestionTest();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                RunAddQuestionTest();

                throw; // vẫn throw lỗi để Jenkins/log biết test fail
            }
            finally
            {
                driver.Quit();
            }
        }

        //Check trang điểm của học viên ( chỉ hiển thị điểm của mình ) 
        [Test]
        public void checkGrade()
        {
            try
            {
                InitDriver(headless: true);  // chạy headless trước
                RunCheckGradeTest();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                RunCheckGradeTest();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }
        //Check theo tên chương
        [Test]
        public void CheckGradeChapter()
        {
            try
            {
                InitDriver(headless: true);  // chạy headless trước
                runCheckGradeChapter();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                RunCheckGradeTest();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }


        //Bài đăng
        //[Test]
        public void RunAddPost()
        {
            try
            {
                InitDriver(headless: true);
                AddPost();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                AddPost();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }

        //[Test]
        public void RunEditPost()
        {
            try
            {
                InitDriver(headless: true);
                editPost();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                editPost();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }
        /*
                [Test]
                public void runDeletePost()
                {
                    try
                    {
                        InitDriver(headless: true);
                        deletePost();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                        Console.WriteLine("🔧 Lỗi: " + ex.Message);

                        driver.Quit();
                        InitDriver(headless: false); // chạy lại với giao diện
                        deletePost();

                        throw; // vẫn throw để log biết test bị fail
                    }
                    finally
                    {
                        driver.Quit();
                    }
                }*/

        //[Test]
        public void runSearchPost()
        {
            try
            {
                InitDriver(headless: true);
                searchPost();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                searchPost();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }
        //[Test]
        public void runSortPost()
        {
            try
            {
                InitDriver(headless: true);
                sortPost();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                sortPost();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }

        //Đi đến học
        [Test]
        public void runGotoLearn()
        {
            try
            {
                InitDriver(headless: true);  // chạy headless trước
                GotoLearn();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                GotoLearn();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }


        //Thêm comment
        //[Test]
        public void runCommentLearningCanvas()
        {
            try
            {
                InitDriver(headless: true);  // chạy headless trước
                AddComment();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                AddComment();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }

        //Xóa comment
        [Test]
        public void runDeleteCommentLearningCanvas()
        {
            try
            {
                InitDriver(headless: true);  // chạy headless trước
                DeleteComment();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                DeleteComment();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }

        //Tìm kiếm comment LearningCanvas
        //[Test]
        public void runSerachCommentLC()
        {
            try
            {
                InitDriver(headless: true);
                SearchCommentLC();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                SearchCommentLC();

                throw; // vẫn throw để log biết test bị fail
            }
            finally
            {
                driver.Quit();
            }
        }

        [Test]
        public void runFinishLesson()
        {
            try
            {
                InitDriver(headless: true);
                finishLesson();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false);
                finishLesson();
            }
            finally
            {
                driver.Quit();
            }
        }

        /*[Test]
        public void runVideoTest()
        {
            try
            {
                InitDriver(headless: true);  // chạy headless trước
                videoTest();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Test lỗi ở chế độ headless. Đang chạy lại với giao diện UI...");
                Console.WriteLine("🔧 Lỗi: " + ex.Message);

                driver.Quit();
                InitDriver(headless: false); // chạy lại với giao diện
                videoTest();

                throw; // vẫn throw để log biết test bị fail
            }
        }*/
        // 
        private void RunAddQuestionTest()
        {
            Login();
            Navigate();
            Member();

            //  Chờ lớp phủ (overlay) biến mất
            wait.Until(driver =>
            {
                try
                {
                    var overlay = driver.FindElement(By.CssSelector("div.bg-black\\/25"));
                    return !overlay.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
            });

            IWebElement addQuestion = wait.Until(d =>
            {
                var el = d.FindElement(By.XPath("//button[contains(@class,'relative bg-primary rounded-2xl flex items-center text-white')]"));
                return el.Displayed ? el : null;
            });

            // 👇 Scroll đến phần tử trước khi click
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addQuestion);
            addQuestion.Click();

            IWebElement Title = wait.Until(d =>
            {
                var el = d.FindElement(By.XPath("//span[@class='block truncate text-start text-darkGray ']"));
                return el.Displayed ? el : null;
            });

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", Title);
            Title.Click();
            Thread.Sleep(1000); // Có thể giảm xuống nếu chờ đủ

            IReadOnlyCollection<IWebElement> options = wait.Until(d =>
            {
                var elements = d.FindElements(By.CssSelector("div.flex.items-center.gap-2"));
                return elements.Count > 0 ? elements : null;
            });

            Random rnd = new();
            int index = rnd.Next(options.Count);
            var option = options.ElementAt(index);

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", option);
            option.Click();
            Thread.Sleep(1000);

            IWebElement des = driver.FindElement(By.XPath("//textarea[@placeholder='Mô tả vấn đề']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", des);
            des.SendKeys("Description !");
            Thread.Sleep(1000);

            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", submit);
            Thread.Sleep(1000);
            submit.Click();
            Thread.Sleep(2000);
        }

        private void RunCheckGradeTest()
        {
            Login();
            Navigate();
            Grade();

            Thread.Sleep(3000);

            var elementLabels = new List<string> {
        "Quốc gia", "Tổng tiến độ", "Điểm tổng thể", "Khóa học đã đăng ký"
    };

            foreach (var label in elementLabels)
            {
                try
                {
                    var element = wait.Until(d => d.FindElement(By.XPath($"//span[text()='{label}']")));
                    if (!element.Displayed)
                        throw new Exception($"❗ Element với label '{label}' không hiển thị.");
                }
                catch (WebDriverTimeoutException)
                {
                    throw new Exception($"❗ Không tìm thấy element với label '{label}'");
                }
            }
        }

        private void runCheckGradeChapter()
        {
            RunCheckGradeTest();
            IWebElement chapter = driver.FindElement(By.XPath("//span[@class='flex-1 text-start ']"));
            chapter.Click();
            Thread.Sleep(3000);
        }

        //Login
        private void Login()
        {
            Thread.Sleep(2000);
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("lozik480@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("Toanking2k3*");

            IWebElement loginButton = driver.FindElement(By.XPath("//button[text()='SIGN IN']"));
            loginButton.Click();
        }
        //home->discovery->Python Tutorial
        private void Navigate()
        {
            Thread.Sleep(4000);
            IWebElement discovery = wait.Until(d => d.FindElement(By.CssSelector("a[href='/discovery']")));
            discovery.Click();
            Thread.Sleep(2000);

            IWebElement course = driver.FindElement(By.XPath("//a[contains(text(),'Python Tutorial')]"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", course);
            Thread.Sleep(1000);
            course.Click();
            Thread.Sleep(2000);
        }

        //Test Dropdown Overview ( Nội dung học tập ) 
        private void content()
        {
            Login();
            Navigate();
            var buttons = wait.Until(d =>
            d.FindElements(By.CssSelector("button.flex.justify-center.items-center.w-6.h-6")));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            foreach (var button in buttons)
            {
                try
                {
                    // Scroll đến phần tử để đảm bảo nó trong tầm nhìn
                    js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", button);
                    Thread.Sleep(500); // đợi scroll xong

                    // Kiểm tra nếu button có thể click
                    if (button.Displayed && button.Enabled)
                    {
                        js.ExecuteScript("arguments[0].click();", button);
                        Console.WriteLine("✅ Đã click thành công!");
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Button không sẵn sàng để click.");
                    }

                    Thread.Sleep(1000); // đợi sau mỗi lần click
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Lỗi khi click button: {ex.Message}");
                }
            }

        }

        //Tìm kiếm & Sort đánh giá ()
        private void searchComment()
        {
            Login();
            Navigate();
            Thread.Sleep(2000);
            IWebElement dropdown = driver.FindElement(By.XPath("//span[@class='shrink-0']"));
            IJavaScriptExecutor jss = (IJavaScriptExecutor)driver;
            jss.ExecuteScript("arguments[0].scrollIntoView(true);", dropdown);
            Thread.Sleep(1000);

            dropdown.Click();
            Thread.Sleep(2000);
            // Lấy danh sách tất cả các <span> nằm trong <button> có class 
            var spans = driver.FindElements(By.XPath("//button[contains(@class, 'cursor-default')]//span[normalize-space()]"));

            // Kiểm tra số lượng và chọn ngẫu nhiên
            if (spans.Count > 0)
            {
                Random rnd = new();
                int index = rnd.Next(spans.Count);

                var randomSpan = spans[index];

                // Scroll đến phần tử và click
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", randomSpan);
                Thread.Sleep(500); // đợi cuộn xuống hoàn tất
                randomSpan.Click();
            }
            else
            {
                Console.WriteLine("❗Không tìm thấy span nào để click.");
            }

            //tìm thanh tìm kiếm
            var searchInputs = driver.FindElements(By.CssSelector("input[placeholder='Tìm kiếm'][role='combobox']"));
            if (searchInputs.Count >= 2)
            {
                var secondSearchInput = searchInputs[1]; // Chỉ mục bắt đầu từ 0
                Thread.Sleep(2000);
                secondSearchInput.SendKeys("Tuyệt vời");
                Thread.Sleep(2000);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("❗Không tìm thấy đủ 2 thanh tìm kiếm.");
            }

        }


        //QNA
        private void Member()
        {
            WebDriverWait localWait = new(driver, TimeSpan.FromSeconds(10));
            IWebElement QNA = localWait.Until(d =>
            {
                var el = d.FindElement(By.XPath("//a[text()='Hỏi & Đáp']"));
                return el.Displayed ? el : null;
            });
            QNA.Click();
            Thread.Sleep(3000);
        }

        //Grade
        private void Grade()
        {

            WebDriverWait localWait = new(driver, TimeSpan.FromSeconds(10));
            IWebElement Grade = localWait.Until(d =>
            {
                var el = d.FindElement(By.XPath("//a[text()='Điểm số']"));
                return el.Displayed ? el : null;
            });
            Grade.Click();
            Thread.Sleep(3000);
        }

        //Đi đến học
        private void GotoLearn()
        {
            Login();
            Navigate();
            IWebElement gotoLearn = driver.FindElement(By.XPath("//a[@href='/learn/course/python-tutorial-v1?action=continue_learning']"));
            gotoLearn.Click();
            Thread.Sleep(2000);
        }

        //comment learning canvas 
        private void AddComment()
        {
            GotoLearn();
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            Thread.Sleep(3000);

            // Nhập comment
            IWebElement cmt = driver.FindElement(By.CssSelector("textarea[placeholder='Viết câu trả lời...']"));
            cmt.SendKeys("TestComment!");
            Thread.Sleep(5000);

            // Tìm nút gửi comment
            IWebElement sendCmt = driver.FindElement(By.XPath("//button[@type='submit']"));

            // Kiểm tra nút có hiển thị không trước khi click
            if (sendCmt.Displayed && sendCmt.Enabled)
            {
                sendCmt.Click();
            }
            else
            {
                throw new Exception("Nút gửi bình luận không khả dụng!");
            }

            Thread.Sleep(5000); // Chờ gửi comment
        }

        //delete comment 
        private void DeleteComment()
        {
            AddComment();
            IWebElement custom = driver.FindElement(By.CssSelector(".w-4[xmlns='http://www.w3.org/2000/svg'][width='26']"));
            custom.Click();
            Thread.Sleep(2000);
            IWebElement deletebtnCmt = driver.FindElement(By.XPath("//span[contains(text(),'Xóa câu hỏi')]"));
            deletebtnCmt.Click();
            Thread.Sleep(2000);
            IWebElement delete = driver.FindElement(By.XPath("//button[normalize-space()='Xóa']"));
            delete.Click();
            Thread.Sleep(2000);
        }
        //Serach comment LearningCanvas
        private void SearchCommentLC()
        {
            AddComment();
            var searchInputs = driver.FindElements(By.CssSelector("input[placeholder='Tìm kiếm'][role='combobox']"));
            if (searchInputs.Count >= 2)
            {
                var secondSearchInput = searchInputs[1]; // Chỉ mục bắt đầu từ 0
                Thread.Sleep(2000);
                secondSearchInput.SendKeys("TestComment");
                Thread.Sleep(2000);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("❗Không tìm thấy đủ 2 thanh tìm kiếm.");
            }
            Thread.Sleep(2000);
            IWebElement custom = driver.FindElement(By.CssSelector(".w-4[xmlns='http://www.w3.org/2000/svg'][width='26']"));
            custom.Click();
            Thread.Sleep(2000);
            IWebElement deletebtnCmt = driver.FindElement(By.XPath("//span[contains(text(),'Xóa câu hỏi')]"));
            deletebtnCmt.Click();
            Thread.Sleep(2000);
            IWebElement delete = driver.FindElement(By.XPath("//button[normalize-space()='Xóa']"));
            delete.Click();
            Thread.Sleep(2000);
        }

        //Post
        private void AddPost()
        {
            Login();
            Navigate();
            WebDriverWait localWait = new(driver, TimeSpan.FromSeconds(10));
            IWebElement posts = localWait.Until(d =>
            {
                var el = d.FindElement(By.XPath("//a[text()='Bài đăng']"));
                return el.Displayed ? el : null;
            });
            posts.Click();
            Thread.Sleep(1000);
            IWebElement addPost = driver.FindElement(By.CssSelector("button.bg-primary.rounded-2xl.flex.items-center.text-white"));
            addPost.Click();
            Thread.Sleep(1000);
            IWebElement Title = driver.FindElement(By.CssSelector("textarea[placeholder='Chủ đề của bài đăng']"));
            Title.SendKeys("Welcome");
            Thread.Sleep(1000);
            IWebElement Content = driver.FindElement(By.CssSelector("textarea[placeholder='Nội dung của bài đăng']"));
            Content.SendKeys("Xin chào mọi người");
            Thread.Sleep(1000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(1000);
            IWebElement svg = driver.FindElement(By.CssSelector("button.outline-none.cursor-pointer.group"));
            svg.Click();
            Thread.Sleep(1000);
            IWebElement delete = driver.FindElement(By.XPath("//button[span[text()='Xóa bài đăng']]"));
            delete.Click();
            Thread.Sleep(1000);
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Xóa']"));
            confirm.Click();

            Thread.Sleep(1000);
        }

        private void editPost()
        {
            AddPost();
            Thread.Sleep(3000);
            IWebElement svg = driver.FindElement(By.CssSelector("button.outline-none.cursor-pointer.group"));
            svg.Click();
            Thread.Sleep(2000);
            IWebElement edit = driver.FindElement(By.XPath("//button[span[text()='Chỉnh sửa bài đăng']]"));
            edit.Click();
            IWebElement Title = driver.FindElement(By.CssSelector("textarea[placeholder='Chủ đề của bài đăng']"));
            Title.Click();
            Title.Clear();
            Title.SendKeys("Xin chào");
            Thread.Sleep(2000);
            IWebElement Content = driver.FindElement(By.CssSelector("textarea[placeholder='Nội dung của bài đăng']"));
            Content.Click();
            Content.Clear();
            Content.SendKeys("Hello everybody!");
            Thread.Sleep(2000);
            IWebElement submit = driver.FindElement(By.CssSelector("button[type='submit']"));
            submit.Click();
            Thread.Sleep(2000);
            IWebElement delete = driver.FindElement(By.XPath("//button[span[text()='Xóa bài đăng']]"));
            delete.Click();
            Thread.Sleep(1000);
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Xóa']"));
            confirm.Click();

            Thread.Sleep(1000);
        }

        private void deletePost()
        {
            AddPost();
            Thread.Sleep(1000);
            IWebElement svg = driver.FindElement(By.CssSelector("button.outline-none.cursor-pointer.group"));
            svg.Click();
            Thread.Sleep(1000);
            IWebElement delete = driver.FindElement(By.XPath("//button[span[text()='Xóa bài đăng']]"));
            delete.Click();
            Thread.Sleep(1000);
            IWebElement confirm = driver.FindElement(By.XPath("//button[normalize-space()='Xóa']"));
            confirm.Click();

            Thread.Sleep(1000);
        }

        private void searchPost()
        {
            AddPost();
            Thread.Sleep(1000);
            IWebElement search = driver.FindElement(By.XPath("(//input[@placeholder='Tìm kiếm'])[2]"));
            search.SendKeys("POWERBI");
            Thread.Sleep(2000);
        }

        private void sortPost()
        {
            AddPost();
            Thread.Sleep(1000);
            IWebElement sort = driver.FindElement(By.CssSelector("button.max-w-\\[200px\\] span.text-darkGray"));
            sort.Click();
            Thread.Sleep(1000);
            IWebElement oldest = driver.FindElement(By.XPath("//span[@class='text-left whitespace-nowrap'][contains(text(),'Cũ nhất')]"));
            oldest.Click();
            Thread.Sleep(2000);
            sort.Click();
            IWebElement latest = driver.FindElement(By.XPath("//span[@class='text-left whitespace-nowrap'][contains(text(),'Mới nhất')]"));
            latest.Click();
            Thread.Sleep(2000);
        }
        //Finish lesson 
        private void finishLesson()
        {
            GotoLearn();

            // Tìm nút "Hoàn thành"
            IWebElement finish = driver.FindElement(By.CssSelector("button.bg-primary.text-white"));

            // Cuộn đến phần tử
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", finish);
            Thread.Sleep(2000); // đợi animation scroll

            // Click nút sau khi đã cuộn
            finish.Click();
            Thread.Sleep(2000);
        }
        //Xem video
        private void videoTest()
        {
            GotoLearn();
            IWebElement playVideo = driver.FindElement(By.XPath("//div[@class='vds-blocker']"));
            playVideo.Click();
            Thread.Sleep(3000);
            //Tạm dừng
            playVideo.Click();
            IWebElement settingVideo = driver.FindElement(By.XPath("//button[@id='media-menu-button-1']"));
            settingVideo.Click();
            Thread.Sleep(2000);
            //chất lượng
            IWebElement quality = driver.FindElement(By.XPath("//span[@class='ml-auto text-sm text-white/50'][normalize-space()='Auto']"));
            quality.Click();
            Thread.Sleep(2000);
            //tốc độ
            IWebElement speed = driver.FindElement(By.XPath("//span[contains(text(),'Tốc độ')]"));
            speed.Click();
            Thread.Sleep(2000);
            IWebElement slowestSpeed = driver.FindElement(By.XPath("//span[normalize-space()='0.25x']"));
            slowestSpeed.Click();
            Thread.Sleep(1000);
            IWebElement Speedslow = driver.FindElement(By.XPath("//span[normalize-space()='0.5x']"));
            Speedslow.Click();
            Thread.Sleep(1000);
            IWebElement lower = driver.FindElement(By.XPath("//span[normalize-space()='0.75x']"));
            lower.Click();
            Thread.Sleep(1000);
            IWebElement mute = driver.FindElement(By.XPath("//button[contains(@aria-label,'Mute')]"));
            mute.Click();
            Thread.Sleep(1000);
        }
    }
}
