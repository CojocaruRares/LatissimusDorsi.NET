using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.Pages
{
    public class SessionPage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        public SessionPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(4));
        }

        public void GoToCreateSession()
        {
            var goToSessionButton = _driver.FindElement(By.XPath("//button[contains(text(), 'Create Training Session')]"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(500);
            goToSessionButton.Click();
        }

        public bool IsErrorDisplayed()
        {
            try
            {
                IWebElement element = _driver.FindElement(By.CssSelector(".MuiPaper-root"));
                return element.Displayed;
            } catch(Exception)
            {
                return false;
            }
            
        }

        public void CreateTrainingSession(string title, string date,string time, string city, string gym, int slots)
        {
            var titleInput = _wait.Until(d => d.FindElement(By.Id("title")));
            titleInput.Clear();
            titleInput.SendKeys(title);

            var startDateInput = _wait.Until(d => d.FindElement(By.Id("startDate")));
            startDateInput.Clear();
            startDateInput.SendKeys(date);
            startDateInput.SendKeys(Keys.Right);
            startDateInput.SendKeys(time);

            var cityInput = _wait.Until(d => d.FindElement(By.Id("city")));
            cityInput.Clear();
            cityInput.SendKeys(city);

            var gymInput = _wait.Until(d => d.FindElement(By.Id("gym")));
            gymInput.Clear();
            gymInput.SendKeys(gym);

            var slotsInput = _wait.Until(d => d.FindElement(By.Id("slots")));
            slotsInput.Clear();
            slotsInput.SendKeys(slots.ToString());

            var submitButton = _driver.FindElement(By.XPath("//button[contains(text(), 'Create Session')]"));
            submitButton.Click();
        }

    }
}
