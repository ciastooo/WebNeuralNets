using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.Models.DB
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Roles = new HashSet <IdentityUserRole<string>>();
            Claims = new List<IdentityUserClaim<string>>();
            Logins = new List<IdentityUserLogin<string>>();
            NeuralNets = new List<NeuralNet>();
        }

        public LanguageCode LanguageCode { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; }
        public virtual ICollection<NeuralNet> NeuralNets { get; }
    }
}
