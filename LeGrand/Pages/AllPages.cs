using OpenQA.Selenium;

namespace AppDriver.Pages
{
    public class AllPages
    {
        public CartPage Cart;

        public EtsyHomePage EtsyHome;
        public ListingPage Listing;


        public AllPages(IWebDriver webDriver)
        {
            EtsyHome = new EtsyHomePage(this, webDriver);
            Listing = new ListingPage(this, webDriver);
            Cart = new CartPage(this, webDriver);
        }
    }
}