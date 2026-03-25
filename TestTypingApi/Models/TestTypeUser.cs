using System.ComponentModel.DataAnnotations;

namespace TestTypingApi.Models
{
    public class TestTypeSpeedUser
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;


        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
