using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.BusinessLogic
{
    public interface ITranslationHelper
    {
        void FlushTranslations();

        string GetTranslation(LanguageCode language, string key);

        string[] GetKeys();
    }
}