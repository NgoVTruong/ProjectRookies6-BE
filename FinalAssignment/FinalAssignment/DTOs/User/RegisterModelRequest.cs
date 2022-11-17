using System.ComponentModel.DataAnnotations;
using Common.Enums;
namespace FinalAssignment.DTOs.User
{
    public class RegisterModelRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"[REGEX HERE]", ErrorMessage = "The Password field can not  have white space.")]
        public string Password { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string? FirstName { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string? LastName { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime JoinedDate { get; set; }
        public string TypeStaff { get; set; }
        public string UserRole { get; set; }
        public string Location { get; set; }
    }
}