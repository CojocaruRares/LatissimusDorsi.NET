using LatissimusDorsi.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace LatissimusDorsi.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly TrainerService _trainerService;
        private readonly UserService _userService;
        private readonly TrainingSessionService _trainingSessionService;
        private readonly FirebaseAuthService _firebaseAuthService;

        public AdminController(UserService userService, FirebaseAuthService firebaseAuthService, TrainerService trainerService,
            TrainingSessionService trainingSessionService)
        {      
            this._userService = userService;
            this._firebaseAuthService = firebaseAuthService;          
            this._trainingSessionService = trainingSessionService;
            this._trainerService = trainerService;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "admin")
            {
                return Forbid();
            }

            var users = await _userService.GetPartialAsync();
            return Ok(users);
        }

        [HttpGet("Trainers")]
        public async Task<IActionResult> GetAllTrainers()
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "admin")
            {
                return Forbid();
            }

            var trainers = await _trainerService.GetAsync();
            return Ok(trainers);
        }

        [HttpDelete("Users")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "admin")
            {
                return Forbid();
            }
            await _userService.DeleteAsync(id);
            await _firebaseAuthService.DeleteUser(id);
            return NoContent();

        }

    }
}
