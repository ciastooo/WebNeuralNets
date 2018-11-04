using Microsoft.AspNetCore.Mvc;

namespace WebNeuralNets.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}