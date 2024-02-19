using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Selenium
{
    public class TestBase
    {
        protected IWebDriver driver;
        protected ExtentReports extent;
        protected ExtentTest test;

        private string baseDirectory;
        private string testName;

        public TestBase()
        {
            extent = new ExtentReports();
        }

        [SetUp]
        public virtual void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            testName = TestContext.CurrentContext.Test.ClassName.Split('.').Last();
            var testExecutionTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss").Replace(':', '-');
            baseDirectory = Path.Combine(@"D:\TestResults\", testName, testExecutionTime);
            Directory.CreateDirectory(baseDirectory);

            var reportPath = Path.Combine(baseDirectory, "ExtentReport.html");
            var htmlReporter = new ExtentSparkReporter(reportPath);
            extent.AttachReporter(htmlReporter);
        }

        protected string TakeScreenshot(string stepName)
        {
            var screenshotDirectory = Path.Combine(baseDirectory, "Screenshots");
            Directory.CreateDirectory(screenshotDirectory);

            var screenshotFileName = $"{stepName}_{Guid.NewGuid()}.png";
            var screenshotFilePath = Path.Combine(screenshotDirectory, screenshotFileName);
            var relativeScreenshotPath = screenshotFilePath.Substring(baseDirectory.Length + 1).Replace("\\", "/");

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(screenshotFilePath);

            return relativeScreenshotPath;
        }

        protected void ExecuteAssertion(Action assertionAction, string successMessage, string failureMessage)
        {
            try
            {
                assertionAction.Invoke();
                var screenshotPath = TakeScreenshot("Success_" + testName);
                test.Pass(successMessage).AddScreenCaptureFromPath(screenshotPath);
            }
            catch (Exception ex)
            {
                var screenshotPath = TakeScreenshot("Failure_" + testName);
                test.Fail($"{failureMessage}<br>{ex.Message}<br>").AddScreenCaptureFromPath(screenshotPath);
                throw;
            }
        }

        [TearDown]
        public void Teardown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = "<pre>" + TestContext.CurrentContext.Result.StackTrace + "</pre>";
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
            {
                test.Fail(stackTrace + errorMessage);
            }
            else if (status == TestStatus.Passed)
            {
                test.Pass("Test passed successfully.");
            }

            if (driver != null)
            {
                driver.Quit();
            }

            extent.Flush();
        }
    }
}