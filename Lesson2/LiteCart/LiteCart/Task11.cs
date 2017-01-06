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
    public class Task11
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
        public void UserRegistration()
        {
            driver.Url = "http://localhost/litecart";
            wait.Until(ExpectedConditions.TitleContains("Online Store |"));

            driver.FindElement(By.CssSelector("div#box-account-login a[href*=create_account]")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.Name("firstname"))).SendKeys("John");
            driver.FindElement(By.Name("lastname")).SendKeys("Smith");
            driver.FindElement(By.Name("address1")).SendKeys("351 Hale St.");
            driver.FindElement(By.Name("postcode")).SendKeys("N5W 1G6");
            driver.FindElement(By.Name("city")).SendKeys("London");

            var country = driver.FindElement(By.Name("country_code"));
            new SelectElement(country).SelectByText("Canada");

            var province = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("select[name=zone_code]")));
            new SelectElement(province).SelectByText("Ontario");

            driver.FindElement(By.Name("email")).SendKeys("john.smith.canada@gmail.com");
            driver.FindElement(By.Name("phone")).SendKeys("+15198000808");
            driver.FindElement(By.Name("password")).SendKeys("11111");
            driver.FindElement(By.Name("confirmed_password")).SendKeys("11111");

            driver.FindElement(By.Name("create_account")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#box-account li:last-child > a[href*=logout]"))).Click();

            IWebElement loginform = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#box-account-login")));
            loginform.FindElement(By.Name("email")).SendKeys("john.smith.canada@gmail.com");
            loginform.FindElement(By.Name("password")).SendKeys("11111");
            loginform.FindElement(By.Name("login")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div#box-account li:last-child > a[href*=logout]"))).Click();
        }

        
        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
