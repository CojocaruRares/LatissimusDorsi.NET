﻿using LatissimusDorsi.Server.Data;
using LatissimusDorsi.Server.Models;
using LatissimusDorsi.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.RegularExpressions;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace LatissimusDorsi.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly WorkoutService _workoutService;
        private readonly FirebaseAuthService _firebaseAuthService;
        private readonly PdfService _pdfService;
        private readonly EmailService _emailService;
        private readonly TrainingSessionService _trainingSessionService;
        private readonly IWebHostEnvironment _environment;

        public UsersController(UserService userService, FirebaseAuthService firebaseAuthService, WorkoutService workoutService,
            TrainingSessionService trainingSession, IWebHostEnvironment env, PdfService pdfService, EmailService emailService)
        {
            this._workoutService = workoutService;
            this._userService = userService;
            this._firebaseAuthService = firebaseAuthService;
            this._environment = env;
            this._pdfService = pdfService;
            this._emailService = emailService;
            this._trainingSessionService = trainingSession;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "user")
            {
                return Forbid();
            }

            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound($"User with id = {id} not found");
            }

            var resourceWrapper = new ResourceWrapper<User>(user);
            resourceWrapper.Links.Add(new Link(Url.Action(nameof(Get), new { id = user.id }), "self", "GET"));
            resourceWrapper.Links.Add(new Link(Url.Action(nameof(Put), new { id = user.id }), "update", "PUT"));
            resourceWrapper.Links.Add(new Link(Url.Action(nameof(GetWorkout), new { id = user.id }), "get_workout", "GET"));
            resourceWrapper.Links.Add(new Link(Url.Action(nameof(EmailWorkout), new {id = user.id}), "email_workout", "POST"));
            resourceWrapper.Links.Add(new Link(Url.Action(nameof(GetTrainingSession)), "get_all_sessions", "GET"));
            resourceWrapper.Links.Add(new Link(Url.Action(nameof(GetAvailableSessions), new {id = user.id}), "get_my_sessions", "GET"));

            return Ok(resourceWrapper);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromForm] UserDTO authdto)
        {
            Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            string name = "";
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
            if (authdto.profileImage != null)
                name = await SaveImage(authdto.profileImage);
            user.profileImage = name;
            await _userService.CreateAsync(user);
            await _firebaseAuthService.CreateUserWithClaim(authdto.Email, authdto.Password, user.id, "user");

            return CreatedAtAction(nameof(Get), new { id = user.id }, user);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] User user)
        {
            var existingUser = await _userService.GetAsync(id);
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "user")
            {
                return Forbid();
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

      
        [HttpGet("{id}/workout")]
        public async Task<IActionResult> GetWorkout(string id)
        {
            var user = await _userService.GetAsync(id);
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "user")
            {
                return Forbid();
            }
            if (user == null)
            {
                return NotFound($"User with id = {id} not found");
            }

            var workout = await _workoutService.GetPerfectWorkoutAsync(user.Objective, user.weight, user.height, user.age, user.Gender);
            if (workout == null)         
            {
                return NotFound();
            }

            var resourceWrapper = new ResourceWrapper<Workout>(workout);
            resourceWrapper.Links.Add(new Link(Url.Action(nameof(GetWorkout), new { id = user.id }), "self", "GET"));
            resourceWrapper.Links.Add(new Link(Url.Action(nameof(EmailWorkout), new { id = user.id }), "email_workout", "POST"));

            return Ok(resourceWrapper);

        }

        [HttpPost("{id}/workout/email")]
        public async Task<IActionResult> EmailWorkout(string id, [FromBody] EmailWorkoutDTO data)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "user")
            {
                return Forbid();
            }
          
            string path = Path.Combine(_environment.ContentRootPath,"Workout.pdf");
            this._pdfService.GenerateWorkoutPDF(data.Workout,path);
            this._emailService.SendPdf(data.Email,path);

            return Ok("Email has been sent!");
        }

        [HttpGet("training-sessions")]
        public async Task<IActionResult> GetTrainingSession()
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "user")
            {
                return Forbid();
            }

            var sessions = await _trainingSessionService.GetAvailableAsync();

            var resourceWrapper = new ResourceWrapper<IEnumerable<TrainingSession>>(sessions);
            foreach (var session in sessions)
            {
                resourceWrapper.Links.Add(new Link(Url.Action(nameof(JoinTrainingSession), new { sessionId = session.id }), "join_session", "PATCH"));
            }

            return Ok(sessions);
        }

        [HttpPatch("training-sessions/{sessionId}")]
        public async Task<IActionResult> JoinTrainingSession(string sessionId, [FromBody] UseridDTO userId)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "user")
            {
                return Forbid();
            }
            var isJoin = await _trainingSessionService.JoinSessionAsync(sessionId, userId.UserId);
            if (isJoin == true)
            {
                var response = new { Message = "success: User joined session" };
                var resourceWrapper = new ResourceWrapper<object>(response);
                resourceWrapper.Links.Add(new Link(Url.Action(nameof(JoinTrainingSession), new { sessionId = sessionId }), "self", "PATCH"));
                resourceWrapper.Links.Add(new Link(Url.Action(nameof(GetTrainingSession)), "available_sessions", "GET"));

                return Ok(resourceWrapper);
            }
            else
            {
                return BadRequest("fail: There are no available slots");
            }

        }

        [HttpGet("training-sessions/{id}/enrolled-users")]
        public async Task<IActionResult> GetEnrolledUsers(string id)
        {      
            var dataList = new List<SessionUsersDTO>();
            var userList = await _trainingSessionService.GetUsersAsync(id);
            if(userList == null)
                return NotFound();
            foreach (var userId in userList)
            {
                string gender;
                var user = await _userService.GetAsync(userId);
                if (user.Gender == 0)
                    gender = "male";
                else gender = "female";
                var userDTO = new SessionUsersDTO(user.profileImage, user.name, user.Objective, user.age, gender);
                dataList.Add(userDTO); 
            }
            var resourceWrapper = new ResourceWrapper<IEnumerable<SessionUsersDTO>>(dataList);
            resourceWrapper.Links.Add(new Link(Url.Action(nameof(GetEnrolledUsers), new { id = id }), "self", "GET"));

            return Ok(resourceWrapper);
        }

        [HttpGet("{id}/my-sessions")]
        public async Task<IActionResult> GetAvailableSessions(string id, [FromQuery] DateTime datetime)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            if (token == null)
            {
                return Unauthorized();
            }

            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "user")
            {
                return Forbid();
            }
            var date = datetime.Date;

            var sessions = await _trainingSessionService.GetSessionsByDateAndUidAsync(id, date);

            var resourceWrapper = new ResourceWrapper<IEnumerable<TrainingSession>>(sessions);
            resourceWrapper.Links.Add(new Link(Url.Action(nameof(GetAvailableSessions), new { id = id, datetime = datetime }), "self", "GET"));

            return Ok(resourceWrapper);
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
