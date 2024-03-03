using LatissimusDorsi.Server.Data;
using LatissimusDorsi.Server.Models;
using LatissimusDorsi.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.RegularExpressions;

namespace LatissimusDorsi.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly WorkoutService _workoutService;
        private readonly FirebaseAuthService _firebaseAuthService;
        private readonly PdfService _pdfService;
        private readonly EmailService _emailService;
        private readonly IWebHostEnvironment _environment;


        public UserController(UserService userService, FirebaseAuthService firebaseAuthService, WorkoutService workoutService,
            IWebHostEnvironment env, PdfService pdfService, EmailService emailService)
        {
            this._workoutService = workoutService;
            this._userService = userService;
            this._firebaseAuthService = firebaseAuthService;
            this._environment = env;
            this._pdfService = pdfService;
            this._emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "user")
            {
                return Unauthorized();
            }
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromForm] UserDTO authdto)
        {
            Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            string name = "";
            if (authdto.profileImage != null)
                name = await SaveImage(authdto.profileImage);

            User user = new User
            {
                name = authdto.name,
                address = authdto.address,
                age = authdto.age,
                height = authdto.height,
                weight = authdto.weight,
                Objective = authdto.Objective,
                Gender = authdto.Gender,
                BodyFatPercentage = authdto.BodyFatPercentage,
                profileImage = name
            };
            if (emailRegex.IsMatch(authdto.Email) == false || UserValidator(user) == false)
            {
                return BadRequest();
            }
            await _userService.CreateAsync(user);
            await _firebaseAuthService.CreateUserWithClaim(authdto.Email, authdto.Password, user.id, "user");
            return CreatedAtAction(nameof(Get), new { id = user.id }, user);
        }

        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody] User user)
        {
            var existingUser = await _userService.GetAsync(id);
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "user")
            {
                return Unauthorized();
            }
            if (UserValidator(user) == false)
            {
                return BadRequest();
            }

            if (existingUser == null)
            {
                return NotFound($"User with id = {id} not found");
            }
            await _userService.UpdateAsync(id, user);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("Workout")]
        public async Task<IActionResult> GetWorkout(string id)
        {
            var user = await _userService.GetAsync(id);
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "user")
            {
                return Unauthorized();
            }
            if (user == null)
            {
                return NotFound($"User with id = {id} not found");
            }

            var workout = await _workoutService.GetPerfectWorkoutAsync(user.Objective, user.weight, user.height, user.age, user.Gender);
            if (workout != null)
            {
                return Ok(workout);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost("Workout")]
        public async Task<IActionResult> EmailWorkout(string email, [FromBody] Workout workout)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "user")
            {
                return Unauthorized();
            }
          
            string path = Path.Combine(_environment.ContentRootPath,"Workout.pdf");
            this._pdfService.GenerateWorkoutPDF(workout,path);
            this._emailService.SendPdf(email,path);

            return Ok();
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
        public bool UserValidator(User user)
        {
            Regex nameRegex = new Regex("^[a-zA-Z]+$");

            if (!nameRegex.IsMatch(user.name))
            {
                return false;
            }
            if (user.age < 3 || user.age > 130)
            {
                return false;
            }
            if (user.height < 50 || user.height > 250)
            {
                return false;
            }
            if (user.weight < 20 || user.weight > 300)
            {
                return false;
            }
            if (user.Gender != 0 && user.Gender != 1)
            {
                return false;
            }
            if (user.Objective != "Bodybuilding" && user.Objective != "Powerlifting" && user.Objective != "Weightloss")
            {
                return false;
            }
            if (user.BodyFatPercentage < 3 || user.BodyFatPercentage > 90)
            {
                return false;
            }
            return true;

        }

    }
}
