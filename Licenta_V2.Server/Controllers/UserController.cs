using Licenta_V2.Server.Data;
using Licenta_V2.Server.Models;
using Licenta_V2.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Licenta_V2.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly FirebaseAuthService _firebaseAuthService;
        private readonly IWebHostEnvironment _environment;


        public UserController(UserService userService, FirebaseAuthService firebaseAuthService, IWebHostEnvironment env)
        {
            this._userService = userService;
            this._firebaseAuthService = firebaseAuthService;
            this._environment = env;
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<User> Get(string id)
        {
            return await _userService.GetAsync(id);
        }


        [HttpPost]
        [Route("PostUser")]
        public async Task<IActionResult> Post([FromForm] AuthDTO authdto)
        {
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
            await _userService.CreateAsync(user);
            await _firebaseAuthService.CreateUserWithClaim(authdto.Email, authdto.Password, user.id, "user");
            return CreatedAtAction(nameof(Get), new { id = user.id }, user);
        }

        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody] User user)
        {
            var existingUser = await _userService.GetAsync(id);
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
