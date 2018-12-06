using System.Collections.Generic;

namespace WebNeuralNets.Models.Dto
{
    public class NeuronDto
    {
        public int Id { get; set; }

        public double Bias { get; set; }

        public List<DendriteDto> PreviousDendrites { get; set; }

        public List<DendriteDto> NextDendrites { get; set; }
    }
}
