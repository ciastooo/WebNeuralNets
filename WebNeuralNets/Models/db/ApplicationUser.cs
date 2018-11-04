using Microsoft.AspNetCore.Identity;
using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.Models.db
{
    public class ApplicationUser : IdentityUser
    {
        public LanguageCode LanguageCode { get; set; }
    }
}
