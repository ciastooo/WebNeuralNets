using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebNeuralNets.BusinessLogic;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly ITranslationHelper _translationHelper;
        private readonly WebNeuralNetDbContext _dbContext;

        public TranslationController(ITranslationHelper translationHelper, WebNeuralNetDbContext dbContext)
        {
            _translationHelper = translationHelper;
            _dbContext = dbContext;
        }

        [HttpGet("{key}")]
        public IActionResult GetTranslation(string key, LanguageCode language = LanguageCode.ENG)
        {
            if (string.IsNullOrEmpty(key) || !Enum.IsDefined(language.GetType(), language))
            {
                return BadRequest();
            }

            var result = _translationHelper.GetTranslation(language, key.ToUpper());

            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpGet]
        public IActionResult GetTranslation()
        {
            var result = _translationHelper.GetKeys();

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Add(LanguageCode languageCode, string key, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value) || !Enum.IsDefined(languageCode.GetType(), languageCode))
                {
                    return BadRequest();
                }

                if (!string.IsNullOrEmpty(_translationHelper.GetTranslation(languageCode, key)))
                {
                    return BadRequest("VALIDATION_TRANSLATIONALREADYEXISTS");
                }

                var translation = new TranslationValue
                {
                    Key = key.ToUpperInvariant(),
                    Value = value,
                    LanguageCode = languageCode
                };

                await _dbContext.TranslationValues.AddAsync(translation);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(LanguageCode languageCode, string key, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value) || !Enum.IsDefined(languageCode.GetType(), languageCode))
                {
                    return BadRequest();
                }

                if (!string.IsNullOrEmpty(_translationHelper.GetTranslation(languageCode, key)))
                {
                    return BadRequest("VALIDATION_TRANSLATIONDOESNTEXISTS");
                }

                var translation = await _dbContext.TranslationValues.Where(t => t.LanguageCode == languageCode && t.Key == key.ToUpperInvariant()).FirstOrDefaultAsync();
                translation.Value = value;
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return BadRequest();
                }

                var translations = await _dbContext.TranslationValues.Where(t => t.Key == key.ToUpperInvariant()).ToListAsync();

                _dbContext.TranslationValues.RemoveRange(translations);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Flush")]
        public IActionResult FlushTranslations()
        {
            _translationHelper.FlushTranslations();

            return Ok();
        }
    }
}