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

        public double TrainingRate { get; set; }

        public int Iterations { get; set; }

        public IList<LayerDto> Layers { get; set; }
    }
}
