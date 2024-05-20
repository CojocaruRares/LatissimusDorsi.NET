using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.Pages;

namespace Testing
{
    [TestClass]
    public class EditProfileTest
    {
        private IWebDriver _driver;

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

        [TestMethod]
        public void EditTrainerSpecialization()
        {
            HomePage homePage = new HomePage(_driver);
            SignInPage signInPage = homePage.GoToSignInPage();     

            string expectedTitle = "Sign in";
            string actualTitle = signInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle, "Page title is not the expected one.");

            string email = "trainer_test@gmail.com";
            string password = "trainer_test";
            signInPage.FillUserCred(email, password);
            Navbar navbar = new Navbar(_driver);

            ProfilePage profilePage = navbar.ClickProfile();
            profilePage.EditTrainerSpec();

            Thread.Sleep(1000);
            string specialization = profilePage.GetTrainerSpec();
            Assert.AreEqual("Specialization: Weightloss", specialization,  "Wrong specialization. " +
                "spec must be weightloss after test");
        }

        [TestMethod]
        public void EditUserName()
        {
            HomePage homePage = new HomePage(_driver);
            SignInPage signInPage = homePage.GoToSignInPage();

            string expectedTitle = "Sign in";
            string actualTitle = signInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle, "Page title is not the expected one.");

            string email = "user_test@gmail.com";
            string password = "user_test";
            signInPage.FillUserCred(email, password);
            Navbar navbar = new Navbar(_driver);

            ProfilePage profilePage = navbar.ClickProfile();
            profilePage.EditUserName("TestUser");

            Thread.Sleep(1000);
            string actualName = profilePage.GetProfileName();
            Assert.AreEqual("TestUser",actualName,"incorrect name");
        }
    }
}
