using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Testing.Pages
{
    public class ProfilePage
    {
        private IWebDriver _driver;

        public ProfilePage(IWebDriver driver) 
        {
            _driver = driver;
        }
        private IWebElement _editProfile => _driver.FindElement(By.XPath("//button[contains(text(), 'Edit Profile')]"));

        public string GetTrainerSpec()
        {
            IWebElement spec = _driver.FindElement(By.XPath("//p[strong[contains(text(), 'Specialization:')]]"));
            return spec.Text;
        }

        public string GetProfileName()
        {
            IWebElement name = _driver.FindElement(By.CssSelector("p.name"));
            return name.Text;
        }

        //because, for some reason, Clear() method doesn't work :(
        public void clearWebField(IWebElement element)
        {
            while (!element.GetAttribute("value").Equals(""))
            {
                element.SendKeys(Keys.Backspace);
            }
        }

        public void EditTrainerSpec()
        {
            _editProfile.Click();

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            wait.Until(driver => driver.FindElement(By.CssSelector("div.MuiDialog-root")).Displayed);

            IWebElement weightLossElem = _driver.FindElement(By.XPath("//*[contains(@class, 'MuiTypography-root') and text()='Weightloss']"));
            weightLossElem.Click();
            IWebElement submit = _driver.FindElement(By.XPath("//button[contains(text(), 'Submit')]"));
            submit.Click();
        }

        public void EditUserName(string username)
        {
            _editProfile.Click();

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            wait.Until(driver => driver.FindElement(By.CssSelector("div.MuiDialog-root")).Displayed);

            IWebElement nameElem = _driver.FindElement(By.Name("name"));

            clearWebField(nameElem);
           
            nameElem.SendKeys(username);
            IWebElement submit = _driver.FindElement(By.XPath("//button[contains(text(), 'Submit')]"));
            submit.Click();
        }


    }
}
