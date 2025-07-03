using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using school_major_project.GlobalServices;
using school_major_project.Models;

namespace school_major_project.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly UserManager<User> _userManager;

        public AuthController(JwtTokenService jwtTokenService, UserManager<User> userManager)
        {
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtTokenService.GenerateToken(user, roles);
                return Ok(new { token });
            }

            return Unauthorized("Sai thông tin đăng nhập");
        }
        [Authorize]
        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            return Ok(new { username = User.Identity.Name });
        }


    }
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
