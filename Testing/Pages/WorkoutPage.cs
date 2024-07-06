using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatissimusDorsi.Server.Models;

namespace Testing.Pages
{
    public class WorkoutPage
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        public WorkoutPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(4));
        }

        public void GoToCreateWorkout()
        {
            var createTrainingButton = _wait.Until(d => d.FindElement(By.XPath("//button[contains(text(), 'Create Training Program')]")));
            createTrainingButton.Click();
        }

        public void SetWorkoutTitle(string title)
        {
            var titleInput = _wait.Until(d => d.FindElement(By.CssSelector(".work-title input.form-control")));
            titleInput.Clear();
            titleInput.SendKeys(title);
        }

        public void AddExercise(string day, List<Exercise> exercises)
        {
            var addExerciseButton = _wait.Until(d =>
            {
                var h3Element = d.FindElement(By.XPath($"//div[h3[contains(text(), '{day}')]]/h3"));
                var addButton = h3Element.FindElement(By.XPath("following-sibling::button[contains(@class, 'btn-add-work')]"));
                return addButton;
            });
            int counter = 0;
            foreach (var exercise in exercises)
            {
                addExerciseButton.Click();
                SetExerciseDetails(counter, exercise, day);
                counter++;
            }
        }

        private void SetExerciseDetails(int exerciseIndex, Exercise ex, string day)
        {
           
            string exerciseRowSelector = $".table-workout tbody tr:nth-child({exerciseIndex + 1})";

            var nameInput = _wait.Until(d => d.FindElement(By.CssSelector($"{exerciseRowSelector} input[name='name']")));
            nameInput.Clear();
            nameInput.SendKeys(ex.name);

            var setsInput = _wait.Until(d => d.FindElement(By.CssSelector($"{exerciseRowSelector} input[name='sets']")));
            setsInput.Clear();
            setsInput.SendKeys(ex.sets.ToString());

            var repsInput = _wait.Until(d => d.FindElement(By.CssSelector($"{exerciseRowSelector} input[name='reps']")));
            repsInput.Clear();
            repsInput.SendKeys(ex.reps.ToString());

            var rpeInput = _wait.Until(d => d.FindElement(By.CssSelector($"{exerciseRowSelector} input[name='rpe']")));
            rpeInput.Clear();
            rpeInput.SendKeys(ex.rpe.ToString());

            var descriptionInput = _wait.Until(d => d.FindElement(By.CssSelector($"{exerciseRowSelector} textarea[name='description']")));
            descriptionInput.Clear();
            descriptionInput.SendKeys(ex.description);
        }

        public void SaveWorkout()
        {
            var saveButton = _driver.FindElement(By.CssSelector(".btn-save button"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(1000);
            saveButton.Click();
        }

        public string GetLastWorkoutTitle()
        {        
            var listItems = _driver.FindElements(By.CssSelector(".list-group.workout-group li"));
            var lastListItem = listItems.LastOrDefault();

            if (lastListItem != null)
            {
                try
                {
                    var titleElement = lastListItem.FindElement(By.CssSelector("h3"));

                    return titleElement.Text;
                }
                catch (NoSuchElementException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return null; 
        }

    }
}
