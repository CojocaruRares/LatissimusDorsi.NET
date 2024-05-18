using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Testing.Pages
{
    public class AuthHomePage
    {
        private IWebDriver _driver;
        private IWebElement _role;

        public AuthHomePage(IWebDriver driver)
        {
            _driver = driver;
            WaitForPageToLoad();
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
