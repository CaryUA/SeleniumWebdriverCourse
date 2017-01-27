using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace LiteCart
{
    internal class MainPage : Page
    {
        public MainPage(IWebDriver driver) : base(driver) { }

        internal MainPage Open()
        {
            driver.Url = "http://localhost/litecart";
            return this;
        }

        internal bool IsOnThisPage()
        {
            return ExpectedConditions.TitleContains("Online Store |").Invoke(driver);
        }

        internal IWebElement MostPopularProduct(string productName)
        {
            return driver.FindElement(By.CssSelector("div#box-most-popular a.link[title='" + productName + "']"));
        }

        internal IList<string> GetMostPopularProductNames()
        {
            var elements = driver.FindElements(By.CssSelector("div#box-most-popular div.name"));
            var names = new List<string>();
            foreach (var element in elements)
                names.Add(element.Text);
            return names;
        }

    }
}