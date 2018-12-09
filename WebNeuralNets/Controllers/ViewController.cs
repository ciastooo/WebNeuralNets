using Microsoft.AspNetCore.Mvc;
using WebNeuralNets.Models.DB;
namespace WebNeuralNets.Controllers
{
    public class ViewController : Controller
    {
        private readonly WebNeuralNetDbContext _context;

        public ViewController(WebNeuralNetDbContext context)
        {
            _context = context;
        }

        // GET: LoginModelDtoes
        public IActionResult Index()
        {
            return View("~/Views/Index.cshtml");
        }

    }
}
