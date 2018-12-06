using System.Collections.Generic;
using System.Linq;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.BusinessLogic
{
    public class TranslationHelper : ITranslationHelper
    {
        private readonly Dictionary<LanguageCode, Dictionary<string, string>> _translations; 

        public TranslationHelper(WebNeuralNetDbContext dbContext)
        {
            var translations = dbContext.TranslationValues.GroupBy(t => t.LanguageCode).ToDictionary(gt => gt.Key, gt => gt.ToDictionary(tv => tv.Key, tv => tv.Value));
            _translations = translations;
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

        public string[] GetKeys()
        {
            return _translations.SelectMany(t => t.Value.Select(tt => tt.Key)).Distinct().ToArray();
        }
    }
}
