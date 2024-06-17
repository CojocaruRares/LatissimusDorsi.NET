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
    public class EditProfileTest : BaseTest
    {

        [TestMethod]
        public void EditTrainerSpecialization()
        {
            HomePage homePage = new HomePage(_driver);
            string email = "trainer_test@gmail.com";
            string password = "trainer_test";
            SignInPage signInPage = PerformLogin(email, password, homePage);
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
            string email = "user_test@gmail.com";
            string password = "user_test";
            SignInPage signInPage = PerformLogin(email, password, homePage);
            Navbar navbar = new Navbar(_driver);

            ProfilePage profilePage = navbar.ClickProfile();
            profilePage.EditUserName("TestUser");

            Thread.Sleep(1000);
            string actualName = profilePage.GetProfileName();
            Assert.AreEqual("TestUser",actualName,"incorrect name");
        }
    }
}
