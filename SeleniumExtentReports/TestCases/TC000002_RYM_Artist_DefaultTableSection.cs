using AventStack.ExtentReports;
using FluentAssertions;
using Selenium;
using Selenium.PageObjects;
using SeleniumExtentReports.PageObjects;

namespace SeleniumExtentReports.TestCases
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class TC000002_RYM_Artist_DefaultTableSection : TestBase
    {
        private ChartPageObject chartPage;
        private ArtistPageObject artistPage;

        string actualString = "";
        string expectedString = "";

        List<string> ads = new () { "ad-close-button", "close-button" };

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            chartPage = new ChartPageObject(driver);
            artistPage = new ArtistPageObject(driver);
        }

        [Test]
        public void TC000002_RYM_Artist_DefaultTableSection_Test()
        {
            Step_1();
            Step_2();
        }

        private void Step_1()
        {
            test.Log(Status.Info, "Step 1: Navigate to Charts tab. Accept cookies if present. Next click 'Top albums of all time'." + 
                "Close ad if appears and click on any Artist name, e.g. Radiohead");

            driver.Navigate().GoToUrl("https://rateyourmusic.com/charts/");
            artistPage.AcceptCookiesIfPresent();

            chartPage.ClickButtonByText("Top albums of all time");

            actualString = driver.Url;
            expectedString = "https://rateyourmusic.com/charts/top/album/all-time/deweight:live,archival,soundtrack/";

            ExecuteAssertion(
                () => actualString.Should().Be(expectedString),
                $"'Top albums of all time' button click - URL is proper: {actualString}",
                $"'Top albums of all time' button click - URL verification failed. It's {actualString} instead of {expectedString}."
            );

            chartPage.CloseAdsByClass(ads);

            chartPage.ClickLinkByClassAndHref("artist", "/artist/radiohead");

            actualString = artistPage.GetArtistHeader();
            expectedString = "Radiohead";

            ExecuteAssertion(
                () => actualString.Should().Be(expectedString),
                $"'Artist header is proper: {actualString}",
                $"'Artist header - verification failed. It's {actualString} instead of {expectedString}."
            );
        }

        private void Step_2()
        {
            test.Log(Status.Info, "Step 2: Close ad if visible. Check if Discography table is selected by default");

            artistPage.CloseAdsByClass(ads);

            actualString = artistPage.GetArtistActivePageSection();
            expectedString = "Discography";

            ExecuteAssertion(
                () => actualString.Should().Be(expectedString),
                $"'Default table tab is proper: {actualString}",
                $"'Default table tab - verification failed. It's {actualString} instead of {expectedString}."
            );         
        }
    }
}
