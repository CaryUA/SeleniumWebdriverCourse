using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Diagnostics;
using AutomatedTester.BrowserMob.HAR;
using AutomatedTester.BrowserMob;

namespace LiteCart
{
    [TestFixture]
    public class Task19
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private Client client;
        private Server server;


        [SetUp]
        public void start()
        {
            server = new Server(@"C:\Tools\browsermob-proxy-2.0-beta-6\bin\browsermob-proxy.bat");
            server.Start();

            client = server.CreateProxy();
            client.NewHar("google");

            var seleniumProxy = new Proxy { HttpProxy = client.SeleniumProxy };
            var options = new ChromeOptions();
            options.Proxy = seleniumProxy;
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }


        [Test]
        public void ProxyTest()
        {
            driver.Url = "http://google.com";
            HarResult harData = client.GetHar();
            foreach (var en in harData.Log.Entries)
            {
                Debug.WriteLine(en.Response.Status.ToString() + " " + en.Response.StatusText + " " + en.Request.Url);
            }
        }


        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
            client.Close();
            server.Stop();
        }
    }
}