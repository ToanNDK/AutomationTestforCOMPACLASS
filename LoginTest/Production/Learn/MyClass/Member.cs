﻿using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;

namespace TestCompa.Production.Learn.MyClassMember
{
    public class ClassTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string devUrl = "https://compaclass.com/learn/home";
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl(devUrl);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        // 1. Test chức năng điều hướng tới Member


        [Test, Order(1)]
        public void memberClass()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(3000);
            IWebElement overview = wait.Until(d => d.FindElement(By.XPath("//div[contains(@class,'flex flex-col gap-2 lg:grid sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-3 lg:gap-4 xl:grid-cols-4 2xl:grid-cols-5')]//div[1]//div[1]//a[1]")));
            overview.Click();
            Thread.Sleep(5000);

            IWebElement member = driver.FindElement(By.XPath("//a[text()='Thành viên']"));
            member.Click();
            Thread.Sleep(2000);

        }
        [Test, Order(2)]
        public void memberList()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(3000);
           
          
            IWebElement testclass = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Cơ Bản')]"));
            ScrollToElement(testclass);
            Thread.Sleep(4000);
            testclass.Click();
            Thread.Sleep(5000);
            IWebElement member = driver.FindElement(By.XPath("//a[text()='Thành viên']"));
            member.Click();
            Thread.Sleep(5000);
            IWebElement hocVienButton = driver.FindElement(By.CssSelector("button.flex.items-center.gap-2"));
            hocVienButton.Click();
            Thread.Sleep(3000);
        }
        [Test, Order(3)]
        public void hoverCard()
        {
            driver.Navigate().GoToUrl(devUrl);
            Login();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("a[href='/learn/class']")));
            element.Click();
            Thread.Sleep(3000);
            IWebElement testclass = driver.FindElement(By.XPath("//a[contains(text(),'Power BI Cơ Bản')]"));
            ScrollToElement(testclass);
            testclass.Click();
            Thread.Sleep(5000);
            IWebElement member = driver.FindElement(By.XPath("//a[text()='Thành viên']"));
            member.Click();
            Thread.Sleep(5000);
            //Hover
            Actions action = new Actions(driver);
            IList<IWebElement> profileElements = driver.FindElements(By.CssSelector("div.col-span-2.flex.gap-2.items-center a[href*='/learn/public-profile']"));
            foreach (var profile in profileElements)
            {
                action.MoveToElement(profile).Perform();
                Thread.Sleep(500);
            }


            Thread.Sleep(5000);
        }

        public void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});", element);
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
