using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;

namespace LiteCart
{
    public class Application
    {
        private IWebDriver driver;

        private MainPage mainPage;
        private ProductPage productPage;
        private CartPage cartPage;

        public Application()
        {
            driver = new ChromeDriver();
            mainPage = new MainPage(driver);
            productPage = new ProductPage(driver);
            cartPage = new CartPage(driver);
        }

        public void Quit()
        {
            driver.Quit();
        }

        internal void AddToCart(int productCount)
        {
            if (!mainPage.IsOnThisPage())
                mainPage.Open();

            var products = mainPage.GetMostPopularProductNames();
            Assert.IsTrue(products.Count > 0);
            for (int i = 0; i < productCount; i++)
            {
                mainPage.Open().MostPopularProduct(i < products.Count ? products[i] : products[products.Count - 1]).Click();
                productPage.AddToCart(1);
            }
        }

        internal void RemoveFromCart(int productCount)
        {
            if (!cartPage.IsOnThisPage())
                cartPage.Open();
            for (int i = 0; i < productCount; i++)
                cartPage.RemoveFromCart();
        }

    }
}