using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace LiteCart
{
    internal class ProductPage : Page
    {
        public ProductPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Name, Using = "quantity")]
        internal IWebElement QuantityInput;

        [FindsBy(How = How.CssSelector, Using = "div#cart span.quantity")]
        internal IWebElement CartQuantitySpan;

        [FindsBy(How = How.Name, Using = "add_cart_product")]
        internal IWebElement AddCartButton;

        internal bool IsOnThisPage()
        {
            return driver.FindElements(By.Name("buy_now_form")).Count > 0;
        }

        internal int CartQuantity()
        {
            return Int32.Parse(CartQuantitySpan.Text);
        }

        internal ProductPage AddToCart(int productCount, string size = null)
        {
            var quantity = CartQuantity();
            QuantityInput.Clear();
            QuantityInput.SendKeys(productCount.ToString());
            SelectSize(size);
            AddCartButton.Click();
            
            wait.Until(ExpectedConditions.ElementExists(By.XPath(
                string.Format("//div[@id='cart']//span[@class='quantity' and .='{0}']", quantity + 1))));

            return this;
        }

        internal ProductPage SelectSize(string size = null)
        {
            var elements = driver.FindElements(By.CssSelector("select[name='options[Size]']"));
            if (elements.Count == 0)
                return this;

            var select = new SelectElement(elements[0]);

            var selText = "select[name='options[Size]']";
            if (size != null)
                selText = " option[value=" + size + "]";
            wait.Until(d => d.FindElement(By.CssSelector(selText)));

            if (size != null)
                select.SelectByValue(size);
            else if (select.Options.Count > 0)
                select.SelectByIndex(1);

            return this;
        }

    }
}