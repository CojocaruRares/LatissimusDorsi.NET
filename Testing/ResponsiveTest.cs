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
    [TestClass]
    public class ResponsiveTest : BaseTest
    {
       
        [TestMethod]
        public void TestHomePageResponsiveness()
        {
            HomePage homePage = new HomePage(_driver);
            SignInPage signInPage = PerformLogin("trainer_test@gmail.com",
                "trainer_test", homePage);
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            _driver.Manage().Window.Size = new System.Drawing.Size(700, 865);
            Thread.Sleep(1000);
            IWebElement navMenu = _driver.FindElement(By.CssSelector("div.menu"));
            IWebElement planWorkout = _driver.FindElement(By.CssSelector("div.plan-workout"));
            IWebElement planSession = _driver.FindElement(By.CssSelector("div.plan-session"));
            IWebElement userImage = _driver.FindElement(By.CssSelector("img.user-image"));
            string flexDirectionWorkout = planWorkout.GetCssValue("flex-direction");
            string flexDirectionSession = planSession.GetCssValue("flex-direction");
            string userImageWidth = userImage.GetCssValue("height");

            Assert.IsTrue(navMenu.Displayed, "Menu is not displayed");
            Assert.AreEqual("column", flexDirectionWorkout, "Flex direction is not column for plan-workout element.");
            Assert.AreEqual("column", flexDirectionSession, "Flex direction is not column for plan-session element.");
            Assert.AreEqual("100px", userImageWidth, "User image height is not as expected.");
        }
    }
}
