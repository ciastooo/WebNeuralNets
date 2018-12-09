using System.ComponentModel.DataAnnotations;

namespace WebNeuralNets.Models.Dto
{
    public class LoginModelDto
    {
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
