using System.ComponentModel.DataAnnotations;
using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.Models.db
{
    public class TranslationValue
    {
        [Key]
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public LanguageCode LanguageCode { get; set; }
    }
}
