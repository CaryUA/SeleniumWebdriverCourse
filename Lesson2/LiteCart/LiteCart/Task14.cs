using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace LiteCart
{
        [TestFixture]
        public class Task14
        {
            private IWebDriver driver;
            private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private Func<IWebDriver, string> ThereIsWindowOtherThan(ICollection<string> oldWindows)
        {
            return d =>
            {
                List<string> handles = new List<string>(d.WindowHandles);
                foreach (string window in oldWindows)
                    handles.Remove(window);
                return handles.Count > 0 ? handles[0] : null;
            };
        }

        private void OpenWindow(IWebElement link)
        {
            string mainWindow = driver.CurrentWindowHandle;
            ICollection<string> oldWindows = driver.WindowHandles;
            link.Click();
            string newWindow = wait.Until(ThereIsWindowOtherThan(oldWindows));
            driver.SwitchTo().Window(newWindow);
            driver.Close();
            driver.SwitchTo().Window(mainWindow);
        }

        [Test]
        public void CheckExternalLinks()
        {
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            IWebElement element = wait.Until(d => d.FindElement(By.Name("username")));
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.TitleContains("Countries |"));

            driver.FindElement(By.CssSelector("a.button[href$=edit_country]")).Click();
            wait.Until(ExpectedConditions.TitleContains("Add New Country"));
            IList<IWebElement> links = driver.FindElements(By.CssSelector("i[class$='external-link'"));

            foreach (var link in links)
            OpenWindow(link);
        }


        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
