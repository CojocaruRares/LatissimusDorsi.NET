using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.Pages
{
    public class SignInPage
    {
        private IWebDriver _driver;

        public SignInPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement _pageTitle => _driver.FindElement(By.XPath("/html//div/div/div/h2"));
        private IWebElement _email => _driver.FindElement(By.Id("email"));
        private IWebElement _password => _driver.FindElement(By.Id("password"));
        private IWebElement _submit => _driver.FindElement(By.CssSelector("button.btn.btn-primary"));

        public string GetPageTitle()
        {
            return _pageTitle.Text;
        }

        public AuthHomePage FillUserCred(string email, string password,bool isDataValid)
        {
            _email.SendKeys(email);
            _password.SendKeys(password);
            _submit.Click();
            if(!isDataValid) { return null; }
            return new AuthHomePage(_driver);
        }

        public bool IsErrorDisplayed()
        {
            Thread.Sleep(1000); 
            try
            {
                _driver.FindElement(By.CssSelector("div.MuiPaper-root"));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false; 
            }
        }


    }
}
