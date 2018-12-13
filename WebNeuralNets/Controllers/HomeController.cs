using Microsoft.AspNetCore.Mvc;
using WebNeuralNets.Models.DB;
namespace WebNeuralNets.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebNeuralNetDbContext _context;

        public HomeController(WebNeuralNetDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
