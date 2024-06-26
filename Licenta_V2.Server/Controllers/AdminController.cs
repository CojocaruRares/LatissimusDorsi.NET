﻿using LatissimusDorsi.Server.Data;
using LatissimusDorsi.Server.Models;
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

        [HttpGet("users")]
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

            var resourceWrapper = new ResourceWrapper<IEnumerable<UserProjection>>(users);
            foreach (var user in users)
            {
                resourceWrapper.Links.Add(new Link(Url.Action(nameof(GetAllUsers)), "self", "GET"));
                resourceWrapper.Links.Add(new Link(Url.Action(nameof(DeleteUser), new { id = user.Id }), "delete_user", "DELETE"));
            }

            return Ok(resourceWrapper);
        }

        [HttpGet("trainers")]
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
            var resourceWrapper = new ResourceWrapper<IEnumerable<Trainer>>(trainers);

            foreach (var trainer in trainers)
            {
                resourceWrapper.Links.Add(new Link(Url.Action(nameof(GetAllTrainers)), "self", "GET"));
                resourceWrapper.Links.Add(new Link(Url.Action(nameof(DeleteTrainer), new { id = trainer.id }), "delete_trainer", "DELETE"));
            }

            return Ok(resourceWrapper);
        }

        [HttpDelete("user/id")]
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

        [HttpDelete("trainer/id")]
        public async Task<IActionResult> DeleteTrainer(string id)
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

            await _trainingSessionService.DeleteSessionByTrainerAsync(id);
            await _trainerService.DeleteAsync(id);
            await _firebaseAuthService.DeleteUser(id);
          
            return NoContent();
        }

    }
}
