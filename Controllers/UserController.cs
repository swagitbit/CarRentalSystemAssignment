using CarRentalSystemAssignment.Models;
using CarRentalSystemAssignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystemAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // User registration
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserModel user)
        {
            // Input validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid input.", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var result = await _userService.RegisterUser(user);
            if (result == "User registered successfully.")
                return Ok(new { Message = result });

            return BadRequest(new { Message = result });
        }

        // User login (returns JWT)
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid input.", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var token = await _userService.AuthenticateUser(loginModel.Email, loginModel.Password);

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { Message = "Invalid email or password." });

            return Ok(new { Token = token });
        }
    }

    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
