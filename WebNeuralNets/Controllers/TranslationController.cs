using Microsoft.AspNetCore.Mvc;
using WebNeuralNets.BusinessLogic;
using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly ITranslationHelper _translationHelper;

        public TranslationController(ITranslationHelper translationHelper)
        {
            _translationHelper = translationHelper;
        }

        [HttpGet]
        public IActionResult GetTranslation(string key, LanguageCode language = LanguageCode.ENG)
        {
            var result = _translationHelper.GetTranslation(language, key);

            return Ok(result);
        }
    }
}