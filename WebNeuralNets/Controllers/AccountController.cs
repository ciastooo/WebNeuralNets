﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Dto;
using WebNeuralNets.BusinessLogic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebNeuralNets.Controllers;

namespace GroupProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly WebNeuralNetDbContext _dbContext;

        public AccountController(WebNeuralNetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(LoginModelDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.Username,
                    PasswordHash = PasswordManager.GenerateSaltedHash(model.Password, model.Username.ToUpperInvariant())
                };

                await _dbContext.ApplicationUsers.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                HttpContext.Session.Clear();
                HttpContext.Response.Cookies.Append("id", user.Id);
                HttpContext.Session.SetString("_userName", model.Username);
                HttpContext.Session.SetString("_id", user.Id.ToString());
                return Ok("Registered");
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
                if (HttpContext.Session.TryGetValue("_id", out var userId))
                {
                    return BadRequest("Already logged in");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var passwordHash = PasswordManager.GenerateSaltedHash(model.Password, model.Username.ToUpperInvariant());

                var user = await _dbContext.ApplicationUsers.Where(u => u.UserName == model.Username && u.PasswordHash == passwordHash).FirstOrDefaultAsync();
                if (user == null)
                {
                    return BadRequest("Couldn't log in");
                }

                HttpContext.Session.Clear();
                HttpContext.Response.Cookies.Append("id", user.Id);
                HttpContext.Session.SetString("_userName", model.Username);
                HttpContext.Session.SetString("_id", user.Id.ToString());
                return Ok("Logged in");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }

        [HttpGet]
        [Route("IsAuthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            var userId = HttpContext.Session.GetString("_id");
            var userName = HttpContext.Session.GetString("_userName");
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userName))
            {
                var result = new LoginModelDto
                {
                    Id = userId,
                    Username = userName
                };
                return Ok(result);
            }
            return Unauthorized();
        }
    }
}
