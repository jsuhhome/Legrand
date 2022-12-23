using System;
using System.Threading;
using AppDriver.Selenium;
using OpenQA.Selenium;

namespace AppDriver.Pages
{
    public class EtsyHomePage
    {
        protected AllPages Pages;

        protected UiDriver UiDriver;

        public EtsyHomePage(AllPages allPages, IWebDriver webDriver)
        {
            UiDriver = new UiDriver(webDriver);
            Pages = allPages;
        }

        public void Goto()
        {
            try
            {
                UiDriver.GotoPage("https:\\www.etsy.com");
                UiDriver.WaitForPageToLoad();
            }
            catch (Exception ex)
            {
                // throw exception, removed to simplify this assessment
            }
        }

        public void SignInViaEmail(string email, string password)
        {
            // click sign in
        }

        public void Register(string email, string firstname, string password)
        {
            try
            {
                // invoke sign in pop up

                UiDriver.Click(BtnSignInNav);
                UiDriver.WaitForPageToLoad();

                // click register
                UiDriver.Click(LnkRegister);
                Thread.Sleep(5000); // normally, we would avoid sleeps. 
                UiDriver.WaitForPageToLoad();

                // enter in email address, first name, password
                UiDriver.WaitUntilClickable(TxtBxEmail);
                UiDriver.Click(TxtBxEmail);
                UiDriver.SendKeys(TxtBxEmail, email);
                UiDriver.WaitForPageToLoad();
                UiDriver.WaitUntilClickable(TxtBxFirstName);
                UiDriver.Click(TxtBxFirstName);
                UiDriver.SendKeys(TxtBxFirstName, firstname);
                UiDriver.WaitForPageToLoad();
                UiDriver.WaitUntilClickable(TxtBxPassword);
                UiDriver.Click(TxtBxPassword);
                UiDriver.SendKeys(TxtBxPassword, password);
                UiDriver.Click(BtnRegister);
            }
            catch (Exception ex)
            {
                // throw exception, log it, but removed for this take home
            }
        }

        public void Login(string email, string password)
        {
            try
            {
                // invoke sign in pop up

                UiDriver.Click(BtnSignInNav);
                UiDriver.WaitForPageToLoad();

                Thread.Sleep(1000);

                // enter in email address, first name, password
                UiDriver.WaitUntilClickable(TxtBxEmail);
                UiDriver.Click(TxtBxEmail);
                UiDriver.SendKeys(TxtBxEmail, email);

                UiDriver.WaitForPageToLoad();

                UiDriver.WaitUntilClickable(TxtBxPassword);
                UiDriver.Click(TxtBxPassword);
                UiDriver.SendKeys(TxtBxPassword, password);

                UiDriver.WaitUntilClickable(BtnSignInLogin);
                UiDriver.Click(BtnSignInLogin);

                Thread.Sleep(5000); // need to wait on login info to show. do later.
                UiDriver.WaitForPageToLoad();
            }
            catch (Exception ex)
            {
                // throw exception, log it, but removed for this take home
            }
        }

        public int GetCartCount()
        {
            return int.Parse(UiDriver.FindElement(TxtCartCount).Text);
        }


        /*
         *
         * Ideally, this should also be implemented as an ui action.
         */
        public void Logout()
        {
            try
            {
                UiDriver.GotoPage(@"https:\\www.etsy.com\logout.php");
                UiDriver.WaitForPageToLoad();
            }
            catch (Exception ex)
            {
                // throw exception, removed to simplify this assessment
            }
        }

        #region IWebElements

        // registration and sign in pop up
        private readonly By BtnSignInNav = By.CssSelector("nav[aria-label='Main'] button");
        private readonly By TxtBxFirstName = By.CssSelector("input[id='join_neu_first_name_field']");
        private readonly By TxtBxEmail = By.CssSelector("input[id='join_neu_email_field']");
        private readonly By TxtBxPassword = By.CssSelector("input[id='join_neu_password_field']");
        private readonly By LnkRegister = By.CssSelector("h1[id='join-neu-overlay-title'] + button");
        private readonly By BtnRegister = By.CssSelector("button[name='submit_attempt'][value='register']");
        private readonly By BtnSignInLogin = By.CssSelector("button[name='submit_attempt'][value='sign-in']");
        private readonly By TxtCartCount = By.CssSelector("span[data-selector='header-cart-count']");

        #endregion
    }
}