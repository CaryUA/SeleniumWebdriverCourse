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
    public class AdminLogin
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
      
        private IList<IWebElement> GetItems()
        {
            return driver.FindElements(By.CssSelector("ul#box-apps-menu li#app-"));
        }

        private IList<IWebElement> GetSubItems(int index)
        {
            return GetItems()[index].FindElements(By.CssSelector("ul.docs li[id^=doc-]"));
        }


        [Test]
        public void CheckListItems()
        {
            driver.Url = "http://localhost/litecart/admin/";
            IWebElement element = wait.Until(d => d.FindElement(By.Name("username")));
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("a[title=Logout]")));

            int n = GetItems().Count;

            for (int i = 0; i < n; i++)
            {
                GetItems()[i].Click();
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#content > h1")));

                int m = GetSubItems(i).Count;
                for (int j = 0; j < m; j++)
                {
                    GetSubItems(i)[j].Click();
                    wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#content > h1")));
                }
            }
        }

        [Test]
        public void CheckStickers()
        {
            driver.Url = "http://localhost/litecart";
            wait.Until(ExpectedConditions.TitleContains("Online Store |"));
            
            IList<IWebElement> products = driver.FindElements(By.CssSelector("ul[class ^= listing-wrapper] li[class ^= product]"));
            int n = products.Count;
            
            for (int i = 0; i < n; i++)
            {
                Assert.True(products[i].FindElements(By.CssSelector("div[class ^= sticker]")).Count == 1);
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