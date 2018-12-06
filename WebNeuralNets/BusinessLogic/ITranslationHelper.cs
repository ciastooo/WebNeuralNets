using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.BusinessLogic
{
    public interface ITranslationHelper
    {
        string GetTranslation(LanguageCode language, string key);

        string[] GetKeys();
    }
}