using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebNeuralNets.Models.Enums;

namespace WebNeuralNets.Models.DB
{
    public class ApplicationUser
    {
        public ApplicationUser()
        {
            NeuralNets = new List<NeuralNet>();
        }

        [Required]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public LanguageCode LanguageCode { get; set; }
        public virtual ICollection<NeuralNet> NeuralNets { get; }
    }
}
