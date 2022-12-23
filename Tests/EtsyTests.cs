using System;
using System.IO;
using System.Threading;
using AppDriver.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using Xunit.Abstractions;

namespace Etsy.Tests
{
    public class EtsyTests
    {
        protected IWebDriver driver;
        protected ITestOutputHelper output;

        protected AllPages Pages;


        /*
         * Normally, this set up would be in the constructor 
         */
        public EtsyTests(ITestOutputHelper output)
        {
            this.output = output;
            driver = LoadDriver();
            Pages = new AllPages(driver);
        }


        public string ChromeDriverDirectory
        {
            get
            {
                var workingDirectory = Environment.CurrentDirectory;
                var browserDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;
                return browserDirectory + "\\Browsers";
            }
        }

        protected IWebDriver LoadDriver()
        {
            IWebDriver driver;

            //chrome
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--start-maximized");
            chromeOptions.AddArguments("--disable-web-security");
            chromeOptions.AddArguments("--allow-cross-origin-auth-prompt");
            chromeOptions.AddArguments("--disable-blink-features=AutomationControlled");
            driver = new ChromeDriver(ChromeDriverDirectory, chromeOptions, TimeSpan.FromMinutes(2));

            return driver;
        }


        ~EtsyTests()
        {
            driver.Dispose();
        }


        [Fact]
        public void EtsyRegisterAndAddTwoItemsToCart()
        {
            // this is sign up with etsy, then log in, add items to cart, enter payment info, and log out and log back in
            var emailId = "fake13344556@gmail.com";
            var firstName = "John";
            var passwd = "Test123!";

            // goto etsy home page
            Pages.EtsyHome.Goto();

            // complete registration process
            Pages.EtsyHome.Register(emailId, firstName, passwd);

            Pages.EtsyHome.Logout();

            Thread.Sleep(5000);

            // log in add 2 items to the cart
            Pages.EtsyHome.Goto();
            Pages.EtsyHome.Login(emailId, passwd);

            // go to amethyst-pale-earrings-purple-stud listing page (474793820), add it
            Pages.Listing.Goto("474793820");
            Pages.Listing.AddToCart();

            // go to "Was That a Fart or a Poop?" listing page (532991074), add it
            Pages.Listing.Goto("532991074");
            Pages.Listing.AddToCart();

            // view the cart
            Pages.Cart.Goto();

            // log out
            Pages.EtsyHome.Logout();

            // log in
            Pages.EtsyHome.Login(emailId, passwd);

            Thread.Sleep(5000);
            // verify cart count is 2
            Assert.Equal(2, Pages.EtsyHome.GetCartCount());
        }


        [Fact]
        public void EtsyLogin()
        {
            // this is sign up with etsy, then log in, add items to cart, enter payment info, and log out and log back in
            var emailId = "jsuhwor.k@gmail.com";
            var firstName = "John";
            var passwd = "Test123!";

            // goto etsy home page
            Pages.EtsyHome.Goto();

            // log in add 2 items to the cart
            Pages.EtsyHome.Login(emailId, passwd);
        }
    }
}