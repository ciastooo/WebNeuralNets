using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNeuralNets.Models.DB
{
    public class Dendrite
    {
        [Key]
        public int Id { get; set; }

        public int NextNeuronId { get; set; }

        public int PreviousNeuronId { get; set; }

        public double Weight { get; set; }

        [NotMapped]
        public double Delta { get; set; }

        public Neuron NextNeuron { get; set; }
        public Neuron PreviousNeuron { get; set; }
    }
}
