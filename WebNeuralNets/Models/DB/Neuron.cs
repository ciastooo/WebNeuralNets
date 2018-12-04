using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebNeuralNets.Models.DB
{
    public class Neuron
    {
        public Neuron()
        {
            PreviousDendrites = new HashSet<Dendrite>();
            NextDendrites = new HashSet<Dendrite>();
        }

        [Key]
        public int Id { get; set; }

        public int LayerId { get; set; }

        public double Bias { get; set; }

        public double Delta { get; set; }

        public ICollection<Dendrite> PreviousDendrites { get; set; }
        public ICollection<Dendrite> NextDendrites { get; set; }
        public Layer Layer { get; set; }
    }
}
