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
    public class Task9
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

    
        private void CheckTableSort(IList<IWebElement> rows, By elementSelector)
        {
            Assert.True(rows.Count > 0);
            string nextString, prevString = rows[0].FindElement(elementSelector).Text;
            for (int i = 1; i < rows.Count; i++)
            {
                nextString = rows[i].FindElement(elementSelector).Text;
                Assert.True(nextString.CompareTo(prevString) > 0);
                prevString = nextString;
            }
        }

        [Test]
        public void CheckCountries()
        {
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            IWebElement element = wait.Until(d => d.FindElement(By.Name("username")));
            element.SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            element = wait.Until(d => d.FindElement(By.CssSelector("table.dataTable")));

            var rows = element.FindElements(By.CssSelector("tr.row"));

            CheckTableSort(rows, By.CssSelector("td:nth-child(5)"));

            List<string> SubCountries = new List<string>();

            for (int i = 0; i < rows.Count; i++)
            {
                if (Int32.Parse(rows[i].FindElement(By.CssSelector("td:nth-child(6)")).Text) > 0)
                {
                    SubCountries.Add(rows[i].FindElement(By.CssSelector("td:nth-child(5) a")).GetAttribute("href"));
                }
            }

            for (int i = 0; i < SubCountries.Count; i++)
            {
                driver.Url = SubCountries[i];
                IWebElement table = wait.Until(d => d.FindElement(By.CssSelector("table#table-zones")));
                var zones = table.FindElements(By.XPath(".//tr[.//a[@id='remove-zone']]"));
                CheckTableSort(zones, By.CssSelector("td:nth-child(3)"));
            }

        }

        [Test]
        public void CheckZones()
        {
            driver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";
            IWebElement element = wait.Until(d => d.FindElement(By.Name("username")));
            element.SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            element = wait.Until(d => d.FindElement(By.CssSelector("table.dataTable")));

            var rows = element.FindElements(By.CssSelector("tr.row"));

            List<string> Geozones = new List<string>();

            for (int i = 0; i < rows.Count; i++)
            {
                Geozones.Add(rows[i].FindElement(By.CssSelector("td:nth-child(3) a")).GetAttribute("href"));
            }


            for (int i = 0; i < Geozones.Count; i++)
            {
                driver.Url = Geozones[i];
                IWebElement table = wait.Until(d => d.FindElement(By.CssSelector("table#table-zones")));
                var zones = table.FindElements(By.XPath(".//tr[.//a[@id='remove-zone']]"));

                CheckTableSort(zones, By.CssSelector("td:nth-child(3) > select > option[selected]"));
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