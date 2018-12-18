using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebNeuralNets.Models.DB
{
    public class NeuralNet
    {
        public NeuralNet()
        {
            Layers = new HashSet<Layer>();
            TrainingData = new HashSet<TrainingData>();
        }

        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public double TrainingRate { get; set; }

        public bool Training { get; set; }

        public int TrainingIterations { get; set; }

        public virtual ApplicationUser User { get; set; }

        public ICollection<Layer> Layers { get; set; }

        public ICollection<TrainingData> TrainingData { get; set; }
    }

    public class QueryNeuralNet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double TrainingRate { get; set; }

        public bool Training { get; set; }

        public int TrainingIterations { get; set; }

        public int LayersCount { get; set; }

        public int NeuronsCount { get; set; }
    }
}
