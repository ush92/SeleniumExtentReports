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
    }
}
