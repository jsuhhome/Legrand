using System;
using AppDriver.Selenium;
using OpenQA.Selenium;

namespace AppDriver.Pages
{
    public class ListingPage
    {
        #region IWebElements

        private readonly By BtnAddToCart =
            By.CssSelector("div[data-selector='add-to-cart-button'] button[type='submit']");

        #endregion

        protected AllPages Pages;


        protected UiDriver UiDriver;

        public ListingPage(AllPages allPages, IWebDriver webDriver)
        {
            UiDriver = new UiDriver(webDriver);
            Pages = allPages;
        }


        public void Goto(string listingNumber)
        {
            try
            {
                UiDriver.GotoPage("https://www.etsy.com/listing/" + listingNumber);
                UiDriver.WaitForPageToLoad();
            }
            catch (Exception ex)
            {
                // throw exception, removed to simplify this assessment
            }
        }

        public void AddToCart()
        {
            try
            {
                UiDriver.WaitForPageToLoad();
                UiDriver.WaitUntilClickable(BtnAddToCart);
                UiDriver.Click(BtnAddToCart);
                UiDriver.WaitForPageToLoad();
            }
            catch (Exception ex)
            {
                // throw exception, removed to simplify this assessment
            }
        }
    }
}