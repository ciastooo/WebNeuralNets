using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Dto;

namespace GroupProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(LoginModelDto model)
        {
            try
            {
                if (_signInManager.IsSignedIn(User))
                {
                    return BadRequest("Already logged in");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = new ApplicationUser
                {
                    UserName = model.Username
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok("Registered");
                }
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModelDto model)
        {
            try
            {
                if(_signInManager.IsSignedIn(User))
                {
                    return BadRequest("Already logged in");
                }
                var loginResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
                if (loginResult.Succeeded)
                {
                    var user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var result = new LoginModelDto
                    {
                        Id = user.Id,
                        Username = user.UserName
                    };
                    return Ok(result);
                }
                return BadRequest("Couldn't log in");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("IsAuthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            var user = await _userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = new LoginModelDto
            {
                Id = user.Id,
                Username = user.UserName
            };
            return Ok(result);
        }
    }
}
