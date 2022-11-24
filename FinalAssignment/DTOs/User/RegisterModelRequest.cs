using System.ComponentModel.DataAnnotations;
using Common.Enums;
namespace FinalAssignment.DTOs.User
{
    public class RegisterModelRequest
    {
        [Required]
        public string Email { get; set; }

        [MinLength(1)]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        // [RegularExpression(@"[REGEX HERE]", ErrorMessage = "The Password field can not  have white space.")]
        public string Password { get; set; }

        [MinLength(1)]
        [MaxLength(20)]
        public string? FirstName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string? LastName { get; set; }
        public GenderEnum Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateOnly JoinedDate { get; set; }
        public string? TypeStaff { get; set; }
        public string? UserRole { get; set; }
        public string? Location { get; set; }
    }
}