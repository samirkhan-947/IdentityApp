using API.Dtos.Account;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtServices _jwtServices;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AccountController(JwtServices jwtServices, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _jwtServices = jwtServices;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [Authorize]
        [HttpGet("RefreshUserToken")]
        public async Task<ActionResult<UserDto>> RefreshUserToken()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return CreateApplicationDto(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if(user == null) return Unauthorized("Invalid username or password");

            if (user.EmailConfirmed == false) return Unauthorized("Please confirm you email");
            var result = await _signInManager.CheckPasswordSignInAsync(user,login.Password,false);

            if (!result.Succeeded) return Unauthorized("Invalid username or password");
            return CreateApplicationDto(user);
        }
        private UserDto CreateApplicationDto(User user)
        {
            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Jwt = _jwtServices.CreateJWT(user),
            };
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if(await CheckEmailExistsAsync(register.Email))
            {
                return BadRequest($"An existing account is using {register.Email}, email address. Please try with another email address");
            }
            var userToAdd = new User
                {
                    FirstName = register.FirstName.ToLower(),
                    LastName = register.LastName.ToLower(),
                    UserName = register.Email.ToLower(),
                    Email = register.Email.ToLower(),
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(userToAdd,register.Password);
           
            return Ok("Your account has been created,you can login");
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
