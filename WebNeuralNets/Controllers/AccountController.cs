using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Web.Http;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Dto;
using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.Controllers
{
    [Route("Account")]
    public class AccountController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<object> Login([FromBody]LoginModelDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            
            if (result.Succeeded)
            {
                //var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                //return await GenerateJwtToken(model.Email, appUser);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Register([FromBody]LoginModelDto model)
        {
            var user = new ApplicationUser
            {
                Email = model.Email,
                LanguageCode = LanguageCode.ENG
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                //return await GenerateJwtToken(model.Email, user);
            }

            return Ok();
        }
    }
}