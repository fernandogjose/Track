using System;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Track.XUnit {
        public class SeleniumTest {
            [Fact]
            public void ChromeDriverTest () {
                using (var driver = new ChromeDriver (Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location))) {
                    driver.Navigate ().GoToUrl (@"http://localhost:4200/#/);
                    var link = driver.FindElement (By.PartialLinkText ("TFS Test API"));
                    var jsToBeExecuted = $"window.scroll(0, {link.Location.Y});";
                    ((IJavaScriptExecutor) driver).ExecuteScript (jsToBeExecuted);
                    var wait = new WebDriverWait (driver, TimeSpan.FromMinutes (1));
                    
                }
            }
        }
}