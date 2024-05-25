using LatissimusDorsi.Server.Models;

namespace LatissimusDorsi.Server.Data
{
    public class EmailWorkoutDTO
    {
        public string Email { get; set; }
        public Workout Workout { get; set; }

        public EmailWorkoutDTO(string email, Workout workout) 
        {
            Email = email;
            Workout = workout;
        }
    }
}
