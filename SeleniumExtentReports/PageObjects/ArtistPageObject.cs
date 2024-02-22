using OpenQA.Selenium;

namespace SeleniumExtentReports.PageObjects
{
    internal class ArtistPageObject : BasePageObject
    {
        public ArtistPageObject(IWebDriver driver) : base(driver)
        {

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

        public string GetArtistHeader()
        {
            WaitForElementVisible(By.ClassName("artist_name_hdr"));
            var artistHeader = driver.FindElement(By.ClassName("artist_name_hdr"));
            return artistHeader.Text;
        }

        public string GetArtistActivePageSection()
        {
            WaitForElementVisible(By.CssSelector(".artist_page_section_active_music h2.page_section_active"));
            var activeSectionHeader = driver.FindElement(By.CssSelector(".artist_page_section_active_music h2.page_section_active")).Text;
            return activeSectionHeader;
        }
    }
}
