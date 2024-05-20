using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.Pages
{
    public class Navbar
    {
        private IWebDriver _driver;
        private IWebElement _home;
        private IWebElement _profile;
        private IWebElement _workout;
        private IWebElement _sessions;

        public Navbar(IWebDriver driver) 
        {
            _driver = driver;
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            wait.Until(driver => driver.FindElement(By.TagName("nav")).Displayed);
            _home = _driver.FindElement(By.LinkText("Home"));
            _profile = _driver.FindElement(By.LinkText("Profile"));
            _workout = _driver.FindElement(By.LinkText("Workout Plan"));
            _sessions = _driver.FindElement(By.LinkText("Training Sessions"));
        }
    

        public HomePage ClickHome()
        {
            _home.Click();
            return new HomePage(_driver);
        }

        public ProfilePage ClickProfile()
        {
            _profile.Click();
            return new ProfilePage(_driver);
        }

    }
}
