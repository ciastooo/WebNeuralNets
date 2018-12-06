using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebNeuralNets.Models.Dto
{
    public class TrainingDataDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<TrainingSetDto> TrainingSet { get; set; }
    }

    public class TrainingSetDto
    {
        public List<double> Input { get; set; }

        public List<double> Output { get; set; }
    }
}
