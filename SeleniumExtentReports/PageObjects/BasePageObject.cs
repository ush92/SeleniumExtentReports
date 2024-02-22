using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumExtentReports.PageObjects
{
    public abstract class BasePageObject
    {
        protected IWebDriver driver;
        private TimeSpan timeout = TimeSpan.FromSeconds(10);

        public BasePageObject(IWebDriver driver)
        {
            this.driver = driver;
        }

        protected IWebElement WaitForElement(Func<IWebDriver, IWebElement> condition, TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(driver, timeout ?? this.timeout)
            {
                PollingInterval = TimeSpan.FromMilliseconds(500),
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            return wait.Until(condition);
        }

        protected void WaitForElementVisible(By locator, TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(driver, timeout ?? this.timeout)
            {
                PollingInterval = TimeSpan.FromMilliseconds(500),
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));

            wait.Until(driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed;
            });
        }

        protected void WaitForElementClickable(By locator, TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(driver, timeout ?? this.timeout)
            {
                PollingInterval = TimeSpan.FromMilliseconds(500),
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));

            wait.Until(driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed && element.Enabled;
            });
        }

        public void NavigateTo(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public void ClickLinkByClassAndHref(string className, string hrefValue)
        {
            var link = driver.FindElement(By.CssSelector($"a.{className}[href='{hrefValue}']"));
            link.Click();
        }

        public void CloseAdsByClass(List<string> classNames)
        {
            foreach (var className in classNames)
            {
                var adsToClose = driver.FindElements(By.XPath($"//div[contains(@class, '{className}')]"));

                foreach (var ad in adsToClose)
                {
                    try
                    {
                        var adLocator = By.XPath($"//div[contains(@class, '{className}') and not(contains(@class, 'hidden'))]");

                        WaitForElementClickable(adLocator);

                        if (ad.Displayed && ad.Enabled)
                        {
                            ad.Click();
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine($"Ad with class '{className}' didn't appear.");
                    }
                    catch (ElementClickInterceptedException)
                    {
                        Console.WriteLine($"Ad with class '{className}' was not clickable.");
                    }
                    catch (WebDriverTimeoutException)
                    {
                        Console.WriteLine($"Timeout waiting for ad with class '{className}' to become clickable.");
                    }
                }
            }
        }
    }
}
