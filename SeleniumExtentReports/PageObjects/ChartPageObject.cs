using OpenQA.Selenium;
using SeleniumExtentReports.PageObjects; // Upewnij się, że ta przestrzeń nazw jest poprawna

namespace Selenium.PageObjects
{
    public class ChartPageObject : BasePageObject
    {
        public ChartPageObject(IWebDriver driver) : base(driver)
        {
        }

        public void ClickButtonByText(string buttonText)
        {
            WaitForElementClickable(By.XPath($"//a[contains(@class, 'ui_button btn_page_charts_common_charts') and contains(text(), '{buttonText}')]"));
            IWebElement button = driver.FindElement(By.XPath($"//a[contains(@class, 'ui_button btn_page_charts_common_charts') and contains(text(), '{buttonText}')]"));
            button.Click();
        }

        public string GetChartHeader()
        {
            WaitForElementVisible(By.Id("page_charts_section_charts_header_chart_name"));
            var chartHeader = driver.FindElement(By.Id("page_charts_section_charts_header_chart_name"));
            return chartHeader.Text;
        }

        public void AcceptCookiesIfPresent()
        {
            try
            {
                WaitForElementClickable(By.XPath("//button[contains(@class, 'fc-cta-consent')]"));
                var acceptButton = driver.FindElement(By.XPath("//button[contains(@class, 'fc-cta-consent')]"));
                if (acceptButton.Displayed)
                {
                    acceptButton.Click();
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Cookies window didn't appear");
            }
        }
    }
}
