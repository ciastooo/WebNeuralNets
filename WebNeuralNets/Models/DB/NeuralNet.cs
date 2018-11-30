using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebNeuralNets.Models.DB
{
    public class NeuralNet
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; } 

        public virtual ApplicationUser User { get; set; }

        public ICollection<Layer> Layers { get; set; }
    }
}
