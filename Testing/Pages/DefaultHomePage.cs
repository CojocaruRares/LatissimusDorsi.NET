using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.Pages
{
    public class DefaultHomePage
    {
        private IWebDriver _driver;
        private IWebElement GetStarted => _driver.FindElement(By.XPath("//button[contains(text(), 'Get Started')]"));

        public DefaultHomePage(IWebDriver driver)
        {
            this._driver = driver;
        }

        public SignInPage GoToSignInPage()
        {
            GetStarted.Click();
            return new SignInPage(_driver);
        }

      


    }
}
