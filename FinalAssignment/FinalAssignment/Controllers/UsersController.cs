using Data.Auth;
using FinalAssignment.DTOs.User;
using FinalAssignment.Services.Implements;
using FinalAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var data = await _userService.Login(model);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("something error" + ex);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterModelRequest model)
        {
            try
            {
                var data = await _userService.Register(model);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("something error");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            try
            {
                var data = await _userService.ResetPassword(model);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("something error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditUser([FromBody] EditUserRequest model)
        {
            try
            {
                var data = await _userService.EditUser(model);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("something error");
            }
        }

        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            try
            {
                var data = await _userService.DeleteUser(userName);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("something error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserDependLocation(string userName)
        {
            try
            {
                var data = await _userService.GetAllUserDependLocation(userName);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("something error" + ex);
            }
        }

    }
}