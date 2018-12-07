using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebNeuralNets.Models.Dto
{
    public class NeuralNetDto
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Validation_FieldRequired")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Validation_FieldRequired")]
        public double TrainingRate { get; set; }

        public int Iterations { get; set; }

        public bool Training { get; set; }

        public int TrainingIterations { get; set; }

        public double AverageError { get; set; }

        public List<LayerDto> Layers { get; set; }

        public List<TrainingDataDto> TrainingData { get; set; }
    }
}
