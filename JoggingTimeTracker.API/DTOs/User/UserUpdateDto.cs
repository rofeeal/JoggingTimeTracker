using System.ComponentModel.DataAnnotations;

namespace JoggingTimeTracker.API.DTOs.User
{
    public class UserUpdateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public string Role { get; set; }
    }
}
