using LatissimusDorsi.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.Pages;

namespace Testing
{
    [TestClass]
    public class WorkoutTest : BaseTest
    {

        static string GenerateRandomName()
        {
            string[] exerciseNames = { "Squat", "Push-up", "Deadlift", "Bench Press", "Pull-up", "Plank",
                    "Lunge", "Bicep Curl", "Tricep Extension" };
            Random rand = new Random();
            return exerciseNames[rand.Next(exerciseNames.Length)];
        }

        static string GenerateRandomDescription()
        {
            string[] descriptions = {
                "Focus on leg muscles.",
                "Core strength exercise.",
                "Upper body workout.",
                "Strength and conditioning.",
                "Cardiovascular exercise."
            };
            Random rand = new Random();
            return descriptions[rand.Next(descriptions.Length)];
        }

        [TestMethod]
        public void CreateWorkout()
        {
            HomePage homePage = new HomePage(_driver);
            string email = "trainer_test@gmail.com";
            string password = "trainer_test";
            SignInPage signInPage = PerformLogin(email, password, homePage);
            Navbar navbar = new Navbar(_driver);

            WorkoutPage workoutPage = navbar.ClickWorkout();
            workoutPage.GoToCreateWorkout();
            Random rand = new Random();
            workoutPage.SetWorkoutTitle("Test title");
            List<Exercise> exercises = new List<Exercise>();
            for (int i = 0; i < 4; i++)
            {
                Exercise exercise = new Exercise
                {
                    name = GenerateRandomName(),
                    sets = rand.Next(1, 10),
                    reps = rand.Next(1, 10),
                    rpe = rand.Next(1,10),
                    description = GenerateRandomDescription()
                };

                exercises.Add(exercise);
            }
            workoutPage.AddExercise("Monday", exercises);
            workoutPage.SaveWorkout();
            Thread.Sleep(1000);
            string text = workoutPage.GetLastWorkoutTitle();
            Assert.IsTrue(text == "Test title", "workout creation failes");
        }
    }
}
