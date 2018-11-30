using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebNeuralNets.Models.DB
{
    public class Layer
    {
        public Layer()
        {
            Neurons = new HashSet<Neuron>();
        }

        [Key]
        public int Id { get; set; }

        public int NeuralNetId { get; set; }

        public int Order { get; set; }

        public int Iteration { get; set; }

        public NeuralNet NeuralNet { get; set; }
        public IEnumerable<Neuron> Neurons { get; set; }
    }
}
