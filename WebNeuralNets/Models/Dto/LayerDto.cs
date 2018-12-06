using System.Collections.Generic;

namespace WebNeuralNets.Models.Dto
{
    public class LayerDto
    {
        public int Id { get; set; }

        public int Iteration { get; set; }

        public List<NeuronDto> Neurons { get; set; }
    }
}
