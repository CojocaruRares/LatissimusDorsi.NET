using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.Pages;
using OpenQA.Selenium.Support.UI;

namespace Testing
{
    public class BaseTest
    {
        protected IWebDriver _driver;

        [TestInitialize]
        public void SetupTest()
        {
            _driver = new EdgeDriver();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl("https://localhost:5173/");
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _driver.Close();
        }

        protected SignInPage PerformLogin(string email, string password, HomePage homePage)
        {
            SignInPage signInPage = homePage.GoToSignInPage();

            string expectedTitle = "Sign in";
            string actualTitle = signInPage.GetPageTitle();
            Assert.AreEqual(expectedTitle, actualTitle, "Page title is not the expected one.");

            signInPage.FillUserCred(email, password);

            return signInPage;
        }

    

    }
}
