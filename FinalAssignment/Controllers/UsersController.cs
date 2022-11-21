using FinalAssignment.DTOs.User;
using FinalAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FinalAssignment.Controllers
{
    [ApiController]
    // [Authorize(Roles = UserRoles.Admin)]
    [EnableCors("MyCors")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IUserService _userService;
        public UsersController(IUserService userService, ILoggerManager logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var data = await _userService.Login(model);

            return Ok(data);
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterModelRequest model)
        {
            var data = await _userService.Register(model);

            return Ok(data);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var data = await _userService.ResetPassword(model);

            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> EditUser([FromBody] EditUserRequest model)
        {
            var data = await _userService.EditUser(model);

            return Ok(data);
        }

        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var data = await _userService.DeleteUser(userName);

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserDependLocation(string userName)
        {
            var data = await _userService.GetAllUserDependLocation(userName);

            return Ok(data);
        }

    }
}