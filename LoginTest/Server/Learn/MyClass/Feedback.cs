﻿using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;

namespace TestCompa.Server.Learn.Feedback
{
    public class ClassTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "http://10.10.10.30/learn/home";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(devUrl);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        // 1. Test chức năng điều hướng tới Feedback

        [Test, Order(1)]
        public void Feedback()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(3000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            Thread.Sleep(5000);
            IWebElement tab2 = driver.FindElement(By.XPath("//button[normalize-space()='2']"));
            tab2.Click();
            Thread.Sleep(2000);
            IWebElement testclass = driver.FindElement(By.XPath("//a[contains(text(),'Test feedback')]"));
            testclass.Click();
            Thread.Sleep(5000);
            IWebElement assign = driver.FindElement(By.XPath("//a[text()='Đánh giá']"));
            assign.Click();
            Thread.Sleep(2000);




        }
        //2. Sắp xếp
        
        [Test, Order(2)]
        public void FeedbackSort()
        {
            Feedback();
            IWebElement sort = driver.FindElement(By.XPath("//span[@class='shrink-0']//*[name()='svg']"));
            sort.Click();
            Thread.Sleep(5000);

        }
        //2.1 Mới nhất 
        [Test,Order(3)]
        public void latestFeedback()
        {
            FeedbackSort();
            IWebElement latest = driver.FindElement(By.XPath("//span[contains(text(),'Đánh giá mới nhất')]"));
            latest.Click();
            Thread.Sleep(5000);
        }
        //2.2 Cũ nhất
        [Test,Order(4)]
        public void oldestFeedback()
        {
            FeedbackSort();
            IWebElement latest = driver.FindElement(By.XPath("//span[contains(text(),'Đánh giá cũ nhất')]"));
            latest.Click();
            Thread.Sleep(5000);
        }
        //2.3 Cao nhất ( Theo sao ) 
        [Test, Order(5)]
        public void highestFeedback()
        {
            FeedbackSort();
            IWebElement highest = driver.FindElement(By.XPath("//span[contains(text(),'Đánh giá cao nhất')]"));
            highest.Click();
            Thread.Sleep(5000);
        }
        //2.4 Thấp  nhất ( Theo sao ) 
        [Test, Order(5)]
        public void lowestFeedback()
        {
            FeedbackSort();
            IWebElement lowest = driver.FindElement(By.XPath("//span[contains(text(),'Đánh giá thấp nhất')]"));
            lowest.Click();
            Thread.Sleep(5000);
        }
        //3. Tìm kiếm
        [Test]
        public void searchFeedback()
        {
            Feedback();
            Thread.Sleep(3000);
            IWebElement search = driver.FindElement(By.XPath("(//input[@placeholder='Tìm kiếm'])[2]"));
            search.SendKeys("very goodd");
            Thread.Sleep(5000);
        }
        //4. Chỉnh sửa feedback
        [Test]
        public void editFeedback()
        {
            searchFeedback();
            IWebElement svg = driver.FindElement(By.XPath("//button[@class='outline-none cursor-pointer group ring-transparent']"));
            svg.Click();
            Thread.Sleep(3000);
            IWebElement editFeedback = driver.FindElement(By.XPath("//span[@class='whitespace-nowrap text-sm group-hover:text-primary transition-all']"));
            editFeedback.Click();
            Thread.Sleep(3000);
        }
        public void Login()
        {
            Thread.Sleep(5000);
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
