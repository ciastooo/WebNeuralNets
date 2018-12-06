using System.Collections.Generic;

namespace WebNeuralNets.Models.DB
{
    public class TrainingSet
    {
        public ICollection<double> Input { get; set; }

        public ICollection<double> Output { get; set; }
    }
}
