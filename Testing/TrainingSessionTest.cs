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
    public class TrainingSessionTest : BaseTest
    {
        [TestMethod]
        public void CreateOverlappingSessions()
        {
            DateTime date = DateTime.UtcNow.AddDays(2);
            string dateString = date.ToString("MMddyyyy");
            string timeString = date.ToString("HH:mm");
            
            HomePage homePage = new HomePage(_driver);
            string email = "trainer_test@gmail.com";
            string password = "trainer_test";
            SignInPage signInPage = PerformLogin(email, password, homePage);
            Navbar navbar = new Navbar(_driver);

            SessionPage sessionPage = navbar.ClickSessions();
            sessionPage.GoToCreateSession();
            sessionPage.CreateTrainingSession("sessionTest", dateString,timeString, "cityTest", "gymTest", 5);
            Thread.Sleep(1000);

            sessionPage.GoToCreateSession();
            sessionPage.CreateTrainingSession("sessionTest", dateString,timeString, "cityTest", "gymTest", 10);
            Thread.Sleep(1000);
            Assert.IsTrue(sessionPage.IsErrorDisplayed());
        }
    }
}
