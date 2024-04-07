using LatissimusDorsi.Server.Data;
using LatissimusDorsi.Server.Models;
using LatissimusDorsi.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace LatissimusDorsi.NET.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainerController : Controller
    {
        private readonly TrainerService _trainerService;
        private readonly WorkoutService _workoutService;
        private readonly FirebaseAuthService _firebaseAuthService;
        private readonly TrainingSessionService _trainingSessionService;
        private readonly IWebHostEnvironment _environment;


        public TrainerController(TrainerService trainerService, WorkoutService workoutService, FirebaseAuthService firebaseAuthService,
           TrainingSessionService trainingSession, IWebHostEnvironment env)
        {
            this._workoutService = workoutService;
            this._trainerService = trainerService;
            this._firebaseAuthService = firebaseAuthService;
            this._trainingSessionService = trainingSession;
            this._environment = env;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _trainerService.GetAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] TrainerDTO authdto)
        {
            Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            string name = "";
            Trainer user = new Trainer
            {
                name = authdto.name,
                address = authdto.address,
                age = authdto.age,
                description = authdto.description,
                motto = authdto.motto,
                gym = authdto.gym,
                specialization = authdto.specialization,
                profileImage = name
            };
            if (emailRegex.IsMatch(authdto.Email) == false)
            {
                return BadRequest();
            }
            if (authdto.profileImage != null)
                name = await SaveImage(authdto.profileImage);
            await _trainerService.CreateAsync(user);
            await _firebaseAuthService.CreateUserWithClaim(authdto.Email, authdto.Password, user.id, "trainer");
            return CreatedAtAction(nameof(Get), new { id = user.id }, user);
        }

        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody] Trainer trainer)
        {
            var existingTrainer = await _trainerService.GetAsync(id);
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "trainer")
            {
                return Forbid();
            }

            if (existingTrainer == null)
            {
                return NotFound($"Trainer with id = {id} not found");
            }
            await _trainerService.UpdateAsync(id, trainer);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _trainerService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("Workout")]
        public async Task<IActionResult> PostWorkout(string id, [FromBody] Workout workout)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "trainer")
            {
                return Forbid();
            }

            var existingTrainer = await _trainerService.GetAsync(id);
            if (existingTrainer == null)
            {
                return NotFound($"Trainer with id = {id} not found");
            }

            if(WorkoutValidator(workout)==false)
            {
                return BadRequest();
            }

            await _workoutService.AddWorkoutAsync(id, workout);
            return CreatedAtAction(nameof(Get),workout);
        }

        [HttpGet("Workout")]
        public async Task<IActionResult> GetWorkouts(string id)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if(token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "trainer")
            {
                return Forbid();
            }

            var workouts = await _workoutService.GetWorkoutAsync(id);

            return Ok(workouts);
        }

        [HttpDelete("Workout")]
        public async Task<IActionResult> DeleteWorkout(string id,int index)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "trainer")
            {
                return Forbid();
            }

            await _workoutService.DeleteWorkoutAsync(id, index);
            return NoContent();
        }

        [HttpPost("TrainingSession")]
        public async Task<IActionResult> CreateSession(string id, [FromBody] TrainingSession session)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "trainer")
            {
                return Forbid();
            }

            var existingTrainer = await _trainerService.GetAsync(id);
            if (existingTrainer == null)
            {
                return NotFound($"Trainer with id = {id} not found");
            }

            if(TrainingSessionValidator(session) == false)
            {
                return BadRequest();
            }
            
            session.trainerId = id;
            await _trainingSessionService.CreateAsync(session);
            return CreatedAtAction(nameof(Get),session);

        }

        [HttpGet("TrainingSession")]
        public async Task<IActionResult> GetTrainingSessions(string id)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "trainer")
            {
                return Unauthorized();
            }

            var sessions = await _trainingSessionService.GetByTrainer(id);
            return Ok(sessions);

        }

        [HttpGet("AvailableTrainingSessions")]
        public async Task<IActionResult> GetAvailableSessions(string userId, DateTime datetime)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "trainer")
            {
                return Forbid();
            }
            var date = datetime.Date;

            var sessions = await _trainingSessionService.GetSessionsByDateAndTraineridAsync(userId, date);
            return Ok(sessions);
        }



        [NonAction]
        public async Task<string> SaveImage(IFormFile file)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(file.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(file.FileName);
            var imagePath = Path.Combine(_environment.ContentRootPath, "Images", imageName);
            using (Stream fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return imageName;
        }

        [NonAction]
        public bool TrainingSessionValidator(TrainingSession session)
        {
            DateTime currentDate = DateTime.UtcNow;
            if (session == null)
            {
                return false;
            }

            if (session.title.Length < 3)
            {
                return false;
            }

            if (session.slots <= 0)
            {
                return false;
            }

            if (session.city.Length < 3)
            {
                return false;
            }

            if (!Regex.IsMatch(session.city, "^[a-zA-Z]+$"))
            {
                return false;
            }
            
            if(session.startDate < currentDate.Date.AddDays(1))
            {
                return false;
            }

            return true;
        }

        [NonAction]
        public bool WorkoutValidator(Workout workout)
        {
            if(workout == null)
            {
                return false;
            }
            if(workout.title.Length < 3)
            {
                return false;
            }
            if(workout.intensity != "moderate" &&  workout.intensity != "low" && workout.intensity != "high")
            {
                return false;
            }
            foreach (var pair in workout.exercises)
            {
                string day = pair.Key;
                List<Exercise> exercisesList = pair.Value;

                foreach (Exercise exercise in exercisesList)
                {
                    if (exercise.name == null || exercise.name.Length < 3)
                    {
                        return false; 
                    }

                    if (exercise.rpe != null && (exercise.rpe < 1 || exercise.rpe > 10))
                    {
                        return false; 
                    }
                }
            }
            return true;
        }
    }
}
