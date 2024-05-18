using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using Testing.Pages;


namespace Testing
{
    [TestClass]
    public class LoginTest
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
        public void ValidLogInUser()
        {
            HomePage homePage = new HomePage(_driver);
            SignInPage signInPage = homePage.GoToSignInPage();

            string expectedTitle = "Sign in";
            string actualTitle = signInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle, "Page title is not the expected one.");

            string email = "pentaadoge@gmail.com";
            string password = "pentaadoge";

            signInPage.FillUserCred(email, password);
            homePage.setRole();
            var role = homePage.GetRoleText();
            Assert.IsTrue(role == "Role: user", "Log in failes");
        }

        [TestMethod]
        public void ValidLogInTrainer()
        {
            HomePage homePage = new HomePage(_driver);
            SignInPage signInPage = homePage.GoToSignInPage();

            string expectedTitle = "Sign in";
            string actualTitle = signInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle, "Page title is not the expected one.");

            string email = "trainer_test@gmail.com";
            string password = "trainer_test";

            signInPage.FillUserCred(email, password);
            homePage.setRole();
            var role = homePage.GetRoleText();
            Assert.IsTrue(role == "Role: trainer", "Log in failes");
        }

        [TestMethod]
        public void InvalidLogInUser()
        {
            HomePage homePage = new HomePage(_driver);
            SignInPage signInPage = homePage.GoToSignInPage();

            string expectedTitle = "Sign in";
            string actualTitle = signInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle, "Page title is not the expected one.");

            string email = "randomabcd";
            string password = "12345";

            signInPage.FillUserCred(email, password);
            Assert.IsTrue(signInPage.IsErrorDisplayed(), "Error message is not displayed.");
        }
    }
}