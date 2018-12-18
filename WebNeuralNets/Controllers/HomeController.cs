﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNeuralNets.Models.DB;
namespace WebNeuralNets.Controllers
{
    public class HomeController : Controller
    {
        private bool IsLoggedIn
        {
            get
            {
                var userId = HttpContext.Session.GetString("_id");
                return !string.IsNullOrEmpty(userId);
            }
        }

        private readonly WebNeuralNetDbContext _context;

        public HomeController(WebNeuralNetDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (IsLoggedIn)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Register()
        {
            if (IsLoggedIn)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult NeuralNetDetails()
        {
            if (!IsLoggedIn)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult NeuralNetsList()
        {
            if (!IsLoggedIn)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}
