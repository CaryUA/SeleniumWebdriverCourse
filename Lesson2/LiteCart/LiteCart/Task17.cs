using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Diagnostics;

namespace LiteCart
{
    [TestFixture]
    public class Task17
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void CheckBrowserLogs()
        {
            driver.Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1";
            IWebElement element = wait.Until(d => d.FindElement(By.Name("username")));
            element.SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            element = wait.Until(d => d.FindElement(By.CssSelector("table.dataTable")));

            var rows = element.FindElements(By.XPath(".//tr[.//a[contains(@href, 'product_id')]]"));

            List<string> links = new List<string>();

            for (int i = 0; i < rows.Count; i++)
                links.Add(rows[i].FindElement(By.CssSelector("a[href*=product_id]")).GetAttribute("href"));

            for (int i = 0; i < links.Count; i++)
            {
                driver.Url = links[i];
                foreach (LogEntry l in driver.Manage().Logs.GetLog("browser"))
                {
                   Debug.WriteLine(l);
                }
            }
        }


        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
