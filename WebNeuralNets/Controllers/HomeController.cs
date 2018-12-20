using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNeuralNets.Models.DB;
namespace WebNeuralNets.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
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

        [Route("Home/NeuralNetDetails/{id:int}")]
        public IActionResult NeuralNetDetails(int id)
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

        public IActionResult TrainingDataList()
        {
            if (!IsLoggedIn)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}
