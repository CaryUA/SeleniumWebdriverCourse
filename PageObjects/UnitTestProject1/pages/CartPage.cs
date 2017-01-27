using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using OpenQA.Selenium.Support.PageObjects;

namespace LiteCart
{
    internal class CartPage : Page
    {
        public CartPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        internal CartPage Open()
        {
            driver.Url = "http://localhost/litecart/checkout";
            return this;
        }

        [FindsBy(How = How.CssSelector, Using = "table[class^=dataTable] tr")]
        internal IList<IWebElement> CartTableRows;

        [FindsBy(How = How.CssSelector, Using = "ul.shortcuts li")]
        internal IList<IWebElement> Shortcuts;

        internal bool IsOnThisPage()
        {
            return ExpectedConditions.TitleContains("Checkout |").Invoke(driver);
        }

        internal IWebElement CartTable()
        {
            return driver.FindElement(By.CssSelector("table[class^=dataTable]"));
        }
        
        internal IWebElement RemoveFromCartButton()
        {
            return driver.FindElement(By.Name("remove_cart_item"));
        }


        internal CartPage RemoveFromCart()
        {
            if (Shortcuts.Count > 0)
                Shortcuts[0].Click();
            var table = CartTable();
            RemoveFromCartButton().Click();
            wait.Until(ExpectedConditions.StalenessOf(table));
            return this;
        }
    }
}