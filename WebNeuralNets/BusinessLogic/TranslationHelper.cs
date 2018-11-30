using System.Collections.Generic;
using System.Linq;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.BusinessLogic
{
    public class TranslationHelper : ITranslationHelper
    {
        private readonly Dictionary<LanguageCode, Dictionary<string, string>> _translations;
        private readonly WebNeuralNetDbContext _c;
 

        public TranslationHelper(WebNeuralNetDbContext dbContext)
        {
            _c = dbContext;
            var translationsQuery = dbContext.TranslationValues.GroupBy(t => t.LanguageCode).ToDictionary(gt => gt.Key, gt => gt.ToDictionary(tv => tv.Key, tv => tv.Value));
            _translations = translationsQuery;
        }

        public string GetTranslation(LanguageCode language, string key)
        {
            if (!string.IsNullOrEmpty(key) && _translations.TryGetValue(language, out var translations))
            {
                if (translations.TryGetValue(key.ToUpperInvariant(), out var result))
                {
                    return result;
                }
            }
            return $"TRANSLATION_{key}_NOTFOUND";
        }
    }
}
