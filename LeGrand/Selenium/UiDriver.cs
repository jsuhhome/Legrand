using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace AppDriver.Selenium
{
    public class UiDriver
    {
        public IWebDriver driver;
        private readonly TimeSpan timeout = new TimeSpan(0, 3, 00);

        public UiDriver(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public void DefaultTimeout()
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(60);
        }

        public void Click(By by)
        {
            var element = FindElement(by);
            Click(element);
        }

        public void Click(IWebElement element)
        {
            try
            {
                WaitUntilClickable(element);
                element.Click();
            }
            catch (Exception ex)
            {
                throw new Exception("rethrown from Click(IWebElement element)", ex);
            }
        }

        public void ClickJsLink(By by)
        {
            try
            {
                var element = FindElement(by);
                WaitUntilClickable(element);
                var js = (IJavaScriptExecutor) driver;
                js.ExecuteScript("arguments[0].click();", element);
                //WebElement element = driver.findElement(By.xpath("//[@id='adHocAddDocDiv']/a"));
                //JavascriptExecutor executor = (JavascriptExecutor)driver;
                //executor.executeScript("arguments[0].click();", element);
            }
            catch (Exception ex)
            {
                throw new Exception("rethrown from ClickJsLink(By by)", ex);
            }
        }

        public void ClickJsLink(string linkText)
        {
            var by = By.LinkText(linkText);
            ClickJsLink(by);
        }

        public void ClickJsLink(IWebElement element)
        {
            try
            {
                WaitUntilClickable(element);
                var js = (IJavaScriptExecutor) driver;
                js.ExecuteScript("arguments[0].click();", element);
            }
            catch (Exception ex)
            {
                throw new Exception("rethrown from ClickJsLink(IWebElement element)", ex);
                ;
            }
        }

        public void ClickJsLinkUnder(By parentElement, string linkText)
        {
            var parent = driver.FindElement(parentElement);
            var byLinkText = By.LinkText(linkText);
            var childLinkTextElement = parent.FindElement(byLinkText);
            ClickJsLink(childLinkTextElement);
        }

        public void ClickJsLinkUnder(IWebElement parentElement, string linkText)
        {
            var byLinkText = By.LinkText(linkText);
            var childLinkTextElement = parentElement.FindElement(byLinkText);
            ClickJsLink(childLinkTextElement);
        }

        public void ClickByIndex(By by, int index)
        {
            try
            {
                var elements = FindElements(by);
                Click(elements[index]);
            }
            catch (Exception ex)
            {
            }
        }

        public void SendKeys(By by, string text)
        {
            var element = FindElement(by);
            SendKeys(element, text);
        }

        public void SendKeys(IWebElement element, string text)
        {
            WaitUntilClickable(element);
            element.SendKeys(text);
        }

        public Dictionary<string, IWebElement> GetDicOfElementsBySubElementText(By subElementInElement,
            By elementsForDic)
        {
            var RootElement = By.CssSelector(@"body");
            return GetDicOfElementsBySubElementText(subElementInElement, elementsForDic, FindElement(RootElement));
        }

        public Dictionary<string, IWebElement> GetDicOfElementsBySubElementText(By subElementInElement,
            By elementsForDic, IWebElement parentDiv)
        {
            var dicOfElementsByLabel = new Dictionary<string, IWebElement>();
            var childWebElements = parentDiv.FindElements(elementsForDic);
            foreach (var childWebElement in childWebElements)
            {
                var labelElement = childWebElement.FindElement(subElementInElement).Text;
                dicOfElementsByLabel.Add(labelElement, childWebElement);
            }

            return dicOfElementsByLabel;
        }

        public void NavigateToURL(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public void ClearAndSendKeys(By by, string text)
        {
            var element = FindElement(by);
            WaitUntilClickable(element);
            element.Clear();
            element.SendKeys(text);
        }

        public void ClearAndSendKeys(By by, string text, By waitElement)
        {
            ClearAndSendKeys(by, text);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(
                d =>
                {
                    try
                    {
                        return !driver.FindElement(waitElement).Size.IsEmpty;
                    }
                    catch
                    {
                        return false;
                    }
                });
            DefaultTimeout();
        }

        public void Clear(By by)
        {
            var element = FindElement(by);
            WaitUntilClickable(element);
            element.Clear();
        }

        public string Text(By by)
        {
            var element = FindElement(by);
            WaitUntilDisplayed(element);
            return element.Text;
        }

        public string TagName(By by)
        {
            var element = FindElement(by);
            WaitUntilDisplayed(element);
            return element.TagName;
        }

        public bool Status(By by)
        {
            var element = FindElement(by);
            return element.Enabled;
        }

        public bool Selected(By by)
        {
            var element = FindElement(by);
            return element.Selected;
        }

        public string GetAttribute(By by, string attribute)
        {
            try
            {
                var element = FindElement(by);
                return element.GetAttribute(attribute);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string GetAttribute(IWebElement element, string attribute)
        {
            try
            {
                return element.GetAttribute(attribute);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public void MoveToElement(By by)
        {
            try
            {
                var element = FindElement(by);
                WaitUntilClickable(element);
                var action = new Actions(driver);
                action.MoveToElement(element).Perform();
            }
            catch (Exception ex)
            {
            }
        }

        public void SwitchFrame(By by)
        {
            try
            {
                var element = FindElement(by);
                WaitUntilClickable(element);
                driver.SwitchTo().Frame(element);
            }
            catch (Exception ex)
            {
            }
        }

        public void SwitchDefaultFrame()
        {
            try
            {
                driver.SwitchTo().DefaultContent();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
            }
        }

        public IWebElement FindElement(By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (NotFoundException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IWebElement FindElement(By by, By byChild)
        {
            try
            {
                var parent = driver.FindElement(by);
                return parent.FindElement(byChild);
            }
            catch (NotFoundException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<IWebElement> FindElements(By by)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                return driver.FindElements(by);
            }
            catch (NotFoundException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                DefaultTimeout();
            }
        }

        public IList<IWebElement> FindElements(By by, By byChild)
        {
            try
            {
                var parent = driver.FindElement(by);
                return parent.FindElements(byChild);
            }
            catch (NotFoundException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void WaitUntilHide(By by)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            wait.Until(
                d =>
                {
                    try
                    {
                        var element = driver.FindElement(by);
                        DefaultTimeout();
                        return !element.Displayed;
                    }
                    catch (NoSuchElementException)
                    {
                        DefaultTimeout();
                        return true;
                    }
                    catch (StaleElementReferenceException)
                    {
                        DefaultTimeout();
                        return true;
                    }
                });
        }

        protected void WaitUntilDisplayed(By by)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(
                d =>
                {
                    try
                    {
                        var element = driver.FindElement(by);
                        DefaultTimeout();
                        return element.Displayed;
                    }
                    catch (NoSuchElementException)
                    {
                        DefaultTimeout();
                        return false;
                    }
                    catch (StaleElementReferenceException)
                    {
                        DefaultTimeout();
                        return false;
                    }
                });
        }

        protected void WaitUntilDisplayed(IWebElement element)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(
                d =>
                {
                    try
                    {
                        return element.Displayed;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                });
        }

        protected void WaitUntilNotClickable(By locator)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(
                d =>
                {
                    try
                    {
                        driver.FindElement(locator);
                        return false;
                    }
                    catch
                    {
                        return true;
                    }
                });
            DefaultTimeout();
        }

        public void WaitUntilClickable(By by)
        {
            var element = FindElement(by);
            WaitUntilClickable(element);
        }

        public void WaitUntilClickable(IWebElement element)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(
                d =>
                {
                    try
                    {
                        if (element != null && element.Displayed && element.Enabled)
                            return true;
                        return false;
                    }
                    catch
                    {
                        return false;
                    }
                });
            DefaultTimeout();
        }

        protected void WaitTextOnAttribute(By by, string attribute, string text)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(
                d =>
                {
                    try
                    {
                        var attribText = GetAttribute(by, attribute);
                        if (attribText.Contains(text)) return true;
                        return false;
                    }
                    catch
                    {
                        return false;
                    }
                });
            DefaultTimeout();
        }


        public bool IsDisplayed(By by)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(100);
            try
            {
                var element = driver.FindElement(by);
                DefaultTimeout();
                return element != null && element.Displayed;
            }
            catch (NoSuchElementException)
            {
                DefaultTimeout();
                return false;
            }
        }

        public bool IsElementClickable(By by)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(100);
            try
            {
                var element = driver.FindElement(by);
                DefaultTimeout();
                return element != null && element.Displayed && element.Enabled;
            }
            catch (NoSuchElementException)
            {
                DefaultTimeout();
                return false;
            }
        }

        public bool IsElementClickable(By by, int time)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(time);
            try
            {
                var element = driver.FindElement(by);
                DefaultTimeout();
                return element != null && element.Displayed && element.Enabled;
            }
            catch (NoSuchElementException)
            {
                DefaultTimeout();
                return false;
            }
        }

        public bool IsElementExist(By by)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(100);
            try
            {
                var element = driver.FindElement(by);
                DefaultTimeout();
                return element != null;
            }
            catch (NoSuchElementException)
            {
                DefaultTimeout();
                return false;
            }
        }

        public bool IsElementExist(IWebElement parent, By by)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(100);
            try
            {
                var element = parent.FindElement(by);
                DefaultTimeout();
                return element != null;
            }
            catch (NoSuchElementException)
            {
                DefaultTimeout();
                return false;
            }
        }

        public IWebElement GetParent(IWebElement element)
        {
            return element.FindElement(By.XPath(".."));
        }

        public IWebElement GetParent(By by)
        {
            var element = FindElement(by);
            return element.FindElement(By.XPath(".."));
        }

        protected bool ValidateElements(List<By> byList)
        {
            foreach (var by in byList)
            {
                var element = FindElement(by);
                WaitUntilClickable(element);
                if (!element.Displayed) return false;
            }

            return true;
        }

        public void WaitforModal(string ModalName)
        {
            var modal = By.CssSelector("iframe.cboxIframe[src*='" + ModalName + "']");
            WaitUntilClickable(modal);
            WaitUntilNotClickable(By.CssSelector("div#cboxLoadingGraphic[style*='display: block']"));
            SwitchFrame(modal);
        }

        public void GotoPage(string url)
        {
            driver.Navigate().GoToUrl(url);
            WaitForPageToLoad();
        }


        public void RefreshPage()
        {
            driver.Navigate().Refresh();
            WaitForPageToLoad();
        }

        public void ScrollIntoView(By by)
        {
            var element = driver.FindElement(by);
            var javascript = driver as IJavaScriptExecutor;
            javascript.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            WaitForPageToLoad();
        }

        public void ExecuteJavascript(string script)
        {
            var javascript = driver as IJavaScriptExecutor;
            javascript.ExecuteScript(script);
        }

        public void WaitForPageToLoad()
        {
            var wait = new WebDriverWait(driver, timeout);

            var javascript = driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("driver", "Driver must support javascript execution");

            WaitUntilJSReady(wait, javascript);
            AjaxComplete(javascript);
            WaitUntilJQueryReady(wait, javascript);
        }

        private void AjaxComplete(IJavaScriptExecutor javascript)
        {
            javascript.ExecuteScript("var callback = arguments[arguments.length - 1];"
                                     + "var xhr = new XMLHttpRequest();" + "xhr.open('GET', '/Ajax_call', true);"
                                     + "xhr.onreadystatechange = function() {" + "  if (xhr.readyState == 4) {"
                                     + "    callback(xhr.responseText);" + "  }" + "};" + "xhr.send();");
        }

        private void WaitUntilJSReady(WebDriverWait wait, IJavaScriptExecutor javascript)
        {
            try
            {
                wait.Until(d =>
                {
                    try
                    {
                        var readyState = javascript
                            .ExecuteScript("if (document.readyState) return document.readyState;").ToString();
                        return readyState.ToLower() == "complete";
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });
            }
            catch
            {
            }
        }

        private void WaitUntilJQueryReady(WebDriverWait wait, IJavaScriptExecutor javascript)
        {
            var jQueryDefined = (bool) javascript.ExecuteScript("return typeof jQuery != 'undefined'");
            if (jQueryDefined) WaitForJQueryLoad(wait, javascript);
        }

        private void WaitForJQueryLoad(WebDriverWait wait, IJavaScriptExecutor javascript)
        {
            try
            {
                var jqueryReady = (bool) javascript.ExecuteScript("return jQuery.active==0");

                if (jqueryReady)
                    wait.Until(d =>
                    {
                        try
                        {
                            return Convert.ToInt32(javascript.ExecuteScript("return jQuery.active")) == 0;
                        }
                        catch
                        {
                            return false;
                        }
                    });
            }
            catch
            {
            }
        }
    }
}