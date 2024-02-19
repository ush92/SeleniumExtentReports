using OpenQA.Selenium;

namespace Selenium.PageObjects
{
    public class ChartPage
    {
        private IWebDriver driver;
        public ChartPage(IWebDriver driver) => this.driver = driver;

        public void ClickButtonByText(string buttonText)
        {
            IWebElement button = driver.FindElement(By.XPath($"//a[contains(@class, 'ui_button btn_page_charts_common_charts') and contains(text(), '{buttonText}')]"));
            button.Click();
        }

        public string GetChartHeader()
        {
            var chartHeader = driver.FindElement(By.Id("page_charts_section_charts_header_chart_name"));
            return chartHeader.Text;
        }

        public void AcceptCookiesIfPresent()
        {
            try
            {
                var cookieDialog = driver.FindElement(By.ClassName("fc-dialog"));
                if (cookieDialog.Displayed)
                {
                    var acceptButton = driver.FindElement(By.XPath("//button[contains(@class, 'fc-cta-consent')]"));
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
