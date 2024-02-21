using AventStack.ExtentReports;
using FluentAssertions;
using NUnit.Framework.Internal;
using Selenium.PageObjects;

namespace Selenium.TestCases
{
    public class TC000001_RYM_CheckChartPageYearsButtons : TestBase
    {
        private ChartPageObject chartPage;

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
        }

        private void Step_1()
        {
            test.Log(Status.Info, "Starting Step 1: Navigate and verify the header for 'Top albums of all time'");

            driver.Navigate().GoToUrl("https://rateyourmusic.com/charts/");
            chartPage.AcceptCookiesIfPresent();

            string actualString = chartPage.GetChartHeader();
            string expectedString = $"Top albums of {DateTime.Now.Year}";

            ExecuteAssertion(
                () => actualString.Should().Be(expectedString),
                $"Initial header verification passed: {actualString}",
                $"Initial header verification failed. It's {actualString} instead of {expectedString}."
            );

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
            test.Log(Status.Info, "Starting Step 2: Navigate back to the current year and verify the header and URL");

            chartPage.ClickButtonByText($"{DateTime.Now.Year}");

            string actualString = driver.Url;
            string expectedString = $"https://rateyourmusic.com/charts/top/album/{DateTime.Now.Year}/";

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
    }
}
