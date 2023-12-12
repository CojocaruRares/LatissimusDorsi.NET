using Licenta_V2.Server.Data;
using Licenta_V2.Server.Models;
using Licenta_V2.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace Licenta_V2.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly FirebaseAuthService _firebaseAuthService;

        public UserController(UserService userService, FirebaseAuthService firebaseAuthService)
        {
            this._userService = userService;
            this._firebaseAuthService = firebaseAuthService;
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _userService.GetAsync();
        }

        //metoda asta o folosesc pt test
        [HttpGet]
        [Route("GetWithID")]
        public async Task<string> Get(string id)
        {
            return await _firebaseAuthService.GetRoleForUser(id);
        }

        [HttpPost]
        [Route("PostUser")]
        public async Task<IActionResult> Post([FromBody] AuthDTO authdto)
        {
            User user = new User
            {
                name = authdto.name,
                address = authdto.address,
                age = authdto.age,
                height = authdto.height,
                weight = authdto.weight,
                Objective = authdto.Objective,
                Gender = authdto.Gender,
                BodyFatPercentage = authdto.BodyFatPercentage
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

    }
}
