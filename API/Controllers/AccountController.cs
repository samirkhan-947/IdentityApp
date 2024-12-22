using API.Dtos.Account;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if(user == null) return Unauthorized("Invalid usernameor password");

            if (user.EmailConfirmed == false) return Unauthorized("Please confirm you email");
            var result = await _signInManager.CheckPasswordSignInAsync(user,login.Password,false);

            if (!result.Succeeded) return Unauthorized("Invalid usernameor password");
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
    }
}
