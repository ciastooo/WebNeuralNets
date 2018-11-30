using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.Models.DB
{
    public class TranslationValue
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public LanguageCode LanguageCode { get; set; }
    }
}
