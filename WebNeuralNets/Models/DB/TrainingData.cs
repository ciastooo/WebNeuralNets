using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebNeuralNets.Models.DB
{
    public class TrainingData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NeuralNetId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<TrainingSet> TrainingSet { get; set; }

        public NeuralNet NeuralNet { get; set; }
    }
}
