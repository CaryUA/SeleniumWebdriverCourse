using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.IO;

namespace LiteCart
{
    [TestFixture]
    public class Task12
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
        public void AddNewProduct()
        {
            driver.Url = "http://localhost/litecart/admin";
            IWebElement element = wait.Until(d => d.FindElement(By.Name("username")));
            element.SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.LinkText("Catalog"))).Click();
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("td#content a.button[href*=edit_product]"))).Click();

            var tabs = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("form div.tabs")));
            tabs.FindElement(By.CssSelector("input[name=status][value='1']")).Click();
            tabs.FindElement(By.Name("name[en]")).SendKeys("Inflatable Ball");
            tabs.FindElement(By.Name("code")).SendKeys("IB001");
            tabs.FindElement(By.CssSelector("input[name='product_groups[]'][value='1-3']")).Click();
            tabs.FindElement(By.Name("quantity")).Clear();
            tabs.FindElement(By.Name("quantity")).SendKeys("6");
            var imagename = "C:\\Ball_image.jpg"; //Картинку залила в репозиторий
            if (File.Exists(imagename))
                tabs.FindElement(By.Name("new_images[]")).SendKeys(imagename); 
            tabs.FindElement(By.Name("date_valid_from")).SendKeys(Keys.Home + "06.01.2017");
            tabs.FindElement(By.Name("date_valid_to")).SendKeys(Keys.Home + "06.01.2018");

            tabs.FindElement(By.CssSelector("a[href*=information]")).Click();

            var manufacturer = tabs.FindElement(By.Name("manufacturer_id"));
            new SelectElement(manufacturer).SelectByIndex(1);

            tabs.FindElement(By.Name("keywords")).SendKeys("ball, beach, game, colorful");
            tabs.FindElement(By.Name("short_description[en]")).SendKeys("Inflatable beach ball");
            tabs.FindElement(By.Name("description[en]")).SendKeys("This inflatable ball can be used for games at beach or at home, etc.");
            tabs.FindElement(By.Name("head_title[en]")).SendKeys("Inflatable ball");
            tabs.FindElement(By.Name("meta_description[en]")).SendKeys("Inflatable colorful ball");

            tabs.FindElement(By.CssSelector("a[href*=prices]")).Click();

            tabs.FindElement(By.Name("purchase_price")).Clear();
            tabs.FindElement(By.Name("purchase_price")).SendKeys("10");

            var currency = tabs.FindElement(By.Name("purchase_price_currency_code"));
            new SelectElement(currency).SelectByIndex(1);

            tabs.FindElement(By.Name("gross_prices[USD]")).Clear();
            tabs.FindElement(By.Name("gross_prices[USD]")).SendKeys("13");
            tabs.FindElement(By.Name("gross_prices[EUR]")).Clear();
            tabs.FindElement(By.Name("gross_prices[EUR]")).SendKeys("11");

            driver.FindElement(By.Name("save")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//form[@name='catalog_form']//a[.='Inflatable Ball']")));

        }


        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
