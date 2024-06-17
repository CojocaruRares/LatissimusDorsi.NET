using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using Testing.Pages;


namespace Testing
{
    [TestClass]
    public class LoginTest : BaseTest
    {
        
        [TestMethod]
        public void ValidLogInUser()
        {
            HomePage homePage = new HomePage(_driver);
            string email = "user_test@gmail.com";
            string password = "user_test";

            SignInPage signInPage = PerformLogin(email, password, homePage);
            homePage.setRole();
            var role = homePage.GetRoleText();
            Assert.IsTrue(role == "Role: user", "Log in failes");
        }

        [TestMethod]
        public void ValidLogInTrainer()
        {
            HomePage homePage = new HomePage(_driver);
            string email = "trainer_test@gmail.com";
            string password = "trainer_test";

            SignInPage signInPage = PerformLogin(email, password, homePage);
            homePage.setRole();
            var role = homePage.GetRoleText();
            Assert.IsTrue(role == "Role: trainer", "Log in failes");
        }

        [TestMethod]
        public void InvalidLogInUser()
        {
            HomePage homePage = new HomePage(_driver);

            string email = "randomabcd";
            string password = "12345";
            SignInPage signInPage = PerformLogin(email, password, homePage);
            signInPage.FillUserCred(email, password);
            Assert.IsTrue(signInPage.IsErrorDisplayed(), "Error message is not displayed.");
        }
    }
}