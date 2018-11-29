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
            Roles = new HashSet <IdentityUserRole<int>>();
            Claims = new List<IdentityUserClaim<int>>();
            Logins = new List<IdentityUserLogin<int>>();
            NeuralNets = new List<NeuralNet>();
        }

        [Key]
        public int Id { get; set; }

        public LanguageCode LanguageCode { get; set; }

        public virtual ICollection<IdentityUserRole<int>> Roles { get; }
        public virtual ICollection<IdentityUserClaim<int>> Claims { get; }
        public virtual ICollection<IdentityUserLogin<int>> Logins { get; }
        public virtual ICollection<NeuralNet> NeuralNets { get; }
    }
}
