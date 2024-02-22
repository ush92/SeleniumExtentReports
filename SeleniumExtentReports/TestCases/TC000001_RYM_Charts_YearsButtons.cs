using AventStack.ExtentReports;
using FluentAssertions;
using NUnit.Framework.Internal;
using Selenium.PageObjects;

namespace Selenium.TestCases
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class TC000001_RYM_Charts_YearsButtons : TestBase
    {
        private ChartPageObject chartPage;

        string actualString = "";
        string expectedString = "";

        List<string> ads = new() { "ad-close-button", "close-button" };

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            chartPage = new ChartPageObject(driver);
        }

        [Test]
        public void TC000001_RYM_CheckChartPageYearsButtons_Test()
        {
            Step_1();
            Step_2();
            Step_3();
        }

        private void Step_1()
        {
            test.Log(Status.Info, "Step 1: Navigate to Charts tab. Accept cookies if present and close ads. Verify 'Top albums of {current year}' header." +
                "Next click 'Top albums of all time' button and confirm header and url changes to match.");

            driver.Navigate().GoToUrl("https://rateyourmusic.com/charts/");
            chartPage.AcceptCookiesIfPresent();

            actualString = chartPage.GetChartHeader();
            expectedString = $"Top albums of {DateTime.Now.Year}";

            ExecuteAssertion(
                () => actualString.Should().Be(expectedString),
                $"Initial header verification passed: {actualString}",
                $"Initial header verification failed. It's {actualString} instead of {expectedString}."
            );

            chartPage.CloseAdsByClass(ads);

            chartPage.ClickButtonByText("Top albums of all time");

            actualString = driver.Url;
            expectedString = "https://rateyourmusic.com/charts/top/album/all-time/deweight:live,archival,soundtrack/";

            ExecuteAssertion(
                () => actualString.Should().Be(expectedString),
                $"'Top albums of all time' button click - URL is proper: {actualString}",
                $"'Top albums of all time' button click - URL verification failed. It's {actualString} instead of {expectedString}."
            );

            actualString = chartPage.GetChartHeader();
            expectedString = "Top albums of all time";

            ExecuteAssertion(
                () => actualString.Should().Be(expectedString),
                $"'Top albums of all time' header verification passed: {actualString}",
                $"'Top albums of all time' button click verification failed. It's {actualString} instead of {expectedString}."
            );
        }

        private void Step_2()
        {
            test.Log(Status.Info, "Step 2: Navigate back to the current year and verify the header and url");

            chartPage.ClickButtonByText($"{DateTime.Now.Year}");
            chartPage.CloseAdsByClass(ads);

            actualString = driver.Url;
            expectedString = $"https://rateyourmusic.com/charts/top/album/{DateTime.Now.Year}/";

            ExecuteAssertion(
                () => actualString.Should().Be(expectedString),
                $"'{DateTime.Now.Year}' button click - URL is proper: {actualString}",
                $"'{DateTime.Now.Year}' button click - URL verification failed. It's {actualString} instead of {expectedString}."
            );

            actualString = chartPage.GetChartHeader();
            expectedString = $"Top albums of {DateTime.Now.Year}";

            ExecuteAssertion(
                () => actualString.Should().Be(expectedString),
                $"Header verification for '{DateTime.Now.Year}' passed: {actualString}",
                $"Header verification for '{DateTime.Now.Year}' failed. It's {actualString} instead of {expectedString}."
            );
        }

        private void Step_3()
        {
            test.Log(Status.Info, "Step 3: Navigate to all visible decade buttons and verify the header and url");

            var decades = new[] { "2020s", "2010s", "2000s", "1990s", "1980s", "1970s", "1960s" };

            foreach (var decade in decades)
            {
                chartPage.ClickButtonByText(decade);
                chartPage.CloseAdsByClass(ads);

                actualString = driver.Url;
                expectedString = $"https://rateyourmusic.com/charts/top/album/{decade}/";

                ExecuteAssertion(
                    () => actualString.Should().Be(expectedString),
                    $"'{decade}' button click - URL is proper: {actualString}",
                    $"'{decade}' button click - URL verification failed. It's {actualString} instead of {expectedString}."
                );

                actualString = chartPage.GetChartHeader();
                expectedString = $"Top albums of the {decade}";

                ExecuteAssertion(
                    () => actualString.Should().Be(expectedString),
                    $"Header verification for '{decade}' passed: {actualString}",
                    $"Header verification for '{decade}' failed. It's {actualString} instead of {expectedString}."
                );
            }
        }
    }
}
