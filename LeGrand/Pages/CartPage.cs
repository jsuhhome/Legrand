using System;
using AppDriver.Selenium;
using OpenQA.Selenium;

namespace AppDriver.Pages
{
    public class CartPage
    {
        protected AllPages Pages;
        protected UiDriver UiDriver;

        public CartPage(AllPages allPages, IWebDriver webDriver)
        {
            UiDriver = new UiDriver(webDriver);
            Pages = allPages;
        }


        public void Goto()
        {
            try
            {
                UiDriver.GotoPage("https://www.etsy.com/cart");
                UiDriver.WaitForPageToLoad();
            }
            catch (Exception ex)
            {
                // throw exception, removed to simplify this assessment
            }
        }

        public void CheckOut()
        {
            try
            {
                UiDriver.WaitForPageToLoad();
                // click on "Proceed to checkout"
                UiDriver.WaitUntilClickable(BtnProceedToCheckout);
                UiDriver.Click(BtnProceedToCheckout);
            }
            catch (Exception ex)
            {
                // throw exception, removed to simplify this assessment
            }
        }

        #region IWebElements

        private readonly By BtnProceedToCheckout =
            By.CssSelector("form[class='enter-checkout-form'] button[type='submit']");

        private readonly By H1EnterAnAddress = By.CssSelector("h1[id='-overlay-title']");
        private readonly By FirstTimeEnterAnAddress = By.CssSelector("div[id='shipping-address-form'] h1");

        #endregion
    }
}