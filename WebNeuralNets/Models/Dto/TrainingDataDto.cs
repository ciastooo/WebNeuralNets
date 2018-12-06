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

        public IList<TrainingSetDto> TrainingSet { get; set; }
    }

    public class TrainingSetDto
    {
        public IList<double> Input { get; set; }

        public IList<double> Output { get; set; }
    }
}
