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
    public class Task13
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        private void AddToCart(string productName)
        {

            driver.Url = "http://localhost/litecart";
            IWebElement element = wait.Until(d => d.FindElement(By.CssSelector("div#box-most-popular")));

            element.FindElement(By.CssSelector("a.link[title='" + productName + "']")).Click();

            var form = wait.Until(ExpectedConditions.ElementExists(By.Name("buy_now_form")));
            var quantity = Int32.Parse(driver.FindElement(By.CssSelector("div#cart span.quantity")).Text);

            var size = form.FindElements(By.Name("options[Size]"));
            if (size.Count > 0)
                new SelectElement(size[0]).SelectByIndex(1);

            form.FindElement(By.CssSelector("button[name=add_cart_product]")).Click();
            wait.Until(ExpectedConditions.ElementExists(By.XPath(
                string.Format("//div[@id='cart']//span[@class='quantity' and .='{0}']", quantity + 1))));
        }

        private void RemoveFromCart()
        {
            var table = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table[class^=dataTable]")));
            var rowsCount = table.FindElements(By.CssSelector("tr")).Count;
            var productsCount = table.FindElements(By.CssSelector("ul.shortcuts li")).Count;

            driver.FindElement(By.Name("remove_cart_item")).Click();

            wait.Until(ExpectedConditions.StalenessOf(table));
            if (productsCount > 1)
            {
                table = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table[class^=dataTable]")));
                Assert.True(table.FindElements(By.CssSelector("tr")).Count == rowsCount - 1);
            }
        }


        [Test]
        public void ShoppingCartTesting()
        {
            AddToCart("Green Duck");
            AddToCart("Red Duck");
            AddToCart("Yellow Duck");

            driver.FindElement(By.LinkText("Checkout »")).Click();

            RemoveFromCart();
            RemoveFromCart();
            RemoveFromCart();
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
