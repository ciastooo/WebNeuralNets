using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebNeuralNets.Models.DB
{
    public class NeuralNet
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public double TrainingRate { get; set; }

        public virtual ApplicationUser User { get; set; }

        public ICollection<Layer> Layers { get; set; }
    }
}
