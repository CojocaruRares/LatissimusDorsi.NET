using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.Pages
{
    public class HomePage
    {
        private IWebDriver _driver;
        private IWebElement _getStarted => _driver.FindElement(By.XPath("//button[contains(text(), 'Get Started')]"));
        private IWebElement _role;

        public HomePage(IWebDriver driver)
        {
            this._driver = driver;
        }

        public SignInPage GoToSignInPage()
        {
            _getStarted.Click();
            return new SignInPage(_driver);
        }

        public void setRole()
        {
            this.WaitForPageToLoad();
            _role = _driver.FindElement(By.CssSelector("p.role"));
        }

        private void WaitForPageToLoad()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            wait.Until(driver => driver.FindElement(By.CssSelector("p.role")).Displayed);
        }

        public string GetRoleText()
        {
            try
            {
                return _role.Text;
            }
            catch (NoSuchElementException)
            {
                return "Role not found";
            }
        }
    }
}
