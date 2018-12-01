namespace WebNeuralNets.Models.Dto
{
    public class DendriteDto
    {
        public int Id { get; set; }

        public double Weight { get; set; }

        public double Delta { get; set; }

        public int NextNeuronId { get; set; }

        public int PreviousNeuronId { get; set; }
    }
}
