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
        private readonly FirebaseAuthService _firebaseAuthService;
        private readonly IWebHostEnvironment _environment;


        public TrainerController(TrainerService trainerService, FirebaseAuthService firebaseAuthService, IWebHostEnvironment env)
        {
            this._trainerService = trainerService;
            this._firebaseAuthService = firebaseAuthService;
            this._environment = env;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "trainer")
            {
                return Unauthorized();
            }
            var user = await _trainerService.GetAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] TrainerDTO authdto)
        {
            Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            string name = "";
            if (authdto.profileImage != null)
                name = await SaveImage(authdto.profileImage);

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
            await _trainerService.CreateAsync(user);
            await _firebaseAuthService.CreateUserWithClaim(authdto.Email, authdto.Password, user.id, "trainer");
            return CreatedAtAction(nameof(Get), new { id = user.id }, user);
        }

        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody] Trainer trainer)
        {
            var existingTrainer = await _trainerService.GetAsync(id);
            string token = Request.Headers.Authorization.ToString().Substring("Bearer ".Length).Trim();
            string role = await _firebaseAuthService.GetRoleForUser(token);
            if (role != "trainer")
            {
                return Unauthorized();
            }
           
            if (existingTrainer == null)
            {
                return NotFound($"User with id = {id} not found");
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
    }
}
