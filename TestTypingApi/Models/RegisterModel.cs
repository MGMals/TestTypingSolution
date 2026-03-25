using System.ComponentModel.DataAnnotations;

namespace TestTypingApi.Models
{
    public class RegisterModel
    {
        [Required] public string FirstName { get; set; } = "";
        [Required] public string LastName { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required] public string Country { get; set; } = "";
        [Required, MinLength(10)] public string Password { get; set; } = "";

    }
}
