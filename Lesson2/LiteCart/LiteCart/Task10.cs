using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Globalization;

namespace LiteCart
{
    [TestFixture]
    public class Task10
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
        public void CheckProduct()
        {
            driver.Url = "http://localhost/litecart";
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("div#box-campaigns li:nth-child(1)")));

            string productTitle = element.FindElement(By.CssSelector("div.name")).Text;
            string productPrice = element.FindElement(By.CssSelector("s.regular-price")).Text;
            string productSalePrice = element.FindElement(By.CssSelector("strong.campaign-price")).Text;

            IWebElement price = driver.FindElement(By.CssSelector("div#box-campaigns div.price-wrapper"));
            char[] px = { 'p', 'x' };

            Assert.True(price.FindElement(By.CssSelector("s.regular-price")).GetCssValue("color") == "rgba(119, 119, 119, 1)");
            Assert.True(price.FindElement(By.CssSelector("s.regular-price")).GetCssValue("text-decoration") == "line-through");
            var regPrice = double.Parse(price.FindElement(By.CssSelector("s.regular-price")).GetCssValue("font-size").TrimEnd(px),
                CultureInfo.InvariantCulture);

            Assert.True(price.FindElement(By.CssSelector("strong.campaign-price")).GetCssValue("color") == "rgba(204, 0, 0, 1)");
            Assert.True(price.FindElement(By.CssSelector("strong.campaign-price")).GetCssValue("font-weight") == "bold");
            var comPrice = double.Parse(price.FindElement(By.CssSelector("strong.campaign-price")).GetCssValue("font-size").TrimEnd(px),
                CultureInfo.InvariantCulture);

            Assert.Greater(comPrice, regPrice);

            element.Click();

            Assert.True(driver.FindElement(By.CssSelector("h1.title")).Text.CompareTo(productTitle) == 0);
            Assert.True(driver.FindElement(By.CssSelector("div.price-wrapper s.regular-price")).Text.CompareTo(productPrice) == 0);
            Assert.True(driver.FindElement(By.CssSelector("div.price-wrapper strong.campaign-price")).Text.CompareTo(productSalePrice) == 0);

            price = driver.FindElement(By.CssSelector("div.price-wrapper"));

            Assert.True(price.FindElement(By.CssSelector("s.regular-price")).GetCssValue("color") == "rgba(102, 102, 102, 1)");
            Assert.True(price.FindElement(By.CssSelector("s.regular-price")).GetCssValue("text-decoration") == "line-through");
            regPrice = double.Parse(price.FindElement(By.CssSelector("s.regular-price")).GetCssValue("font-size").TrimEnd(px),
                CultureInfo.InvariantCulture);


            Assert.True(price.FindElement(By.CssSelector("strong.campaign-price")).GetCssValue("color") == "rgba(204, 0, 0, 1)");
            Assert.True(price.FindElement(By.CssSelector("strong.campaign-price")).GetCssValue("font-weight") == "bold");
            comPrice = double.Parse(price.FindElement(By.CssSelector("strong.campaign-price")).GetCssValue("font-size").TrimEnd(px),
                CultureInfo.InvariantCulture);

            Assert.Greater(comPrice, regPrice);
        }


        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}