using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;

namespace ParallelLTSelenium
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ParallelLTTests
    {
        ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
        private String browser;
        private String version;
        private String os;

        private static IEnumerable<TestCaseData> AddBrowserConfs()
        {
            yield return new TestCaseData("chrome", "89.0", "Windows 10");
            yield return new TestCaseData("internet explorer", "11.0", "Windows 10");
            yield return new TestCaseData("Safari", "11.0", "macOS High Sierra");
            yield return new TestCaseData("MicrosoftEdge", "18.0", "Windows 10");
        }

        [Test,TestCaseSource("AddBrowserConfs")]
        public void DuckDuckGo_TestCase_Demo(String browser, String version, String os)
        {
            String username = "user-name";
            String accesskey = "access-key";
            String gridURL = "@hub.lambdatest.com/wd/hub";

            DesiredCapabilities capabilities = new DesiredCapabilities();

            capabilities.SetCapability("user", ${{ secrets.LT_USER_NAME }});
            capabilities.SetCapability("accessKey", ${{ secrets.LT_ACCESS_KEY }});
            capabilities.SetCapability("browserName", browser);
            capabilities.SetCapability("version", version);
            capabilities.SetCapability("platform", os);

            driver.Value = new RemoteWebDriver(new Uri("https://" + username + ":" + accesskey + gridURL), capabilities, TimeSpan.FromSeconds(600));

            System.Threading.Thread.Sleep(2000);

            driver.Value.Url = "https://www.duckduckgo.com";

            IWebElement element = driver.Value.FindElement(By.XPath("//*[@id='search_form_input_homepage']"));

            element.SendKeys("LambdaTest");

            /* Submit the Search */
            element.Submit();

            /* Perform wait to check the output */
            System.Threading.Thread.Sleep(2000);
        }

        [TearDown]
        public void Cleanup()
        {
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed;
            try
            {
                // Logs the result to Lambdatest
                ((IJavaScriptExecutor)driver.Value).ExecuteScript("lambda-status=" + (passed ? "passed" : "failed"));
            }
            finally
            {
                // Terminates the remote webdriver session
                driver.Value.Quit();
            }
        }
    }
}
