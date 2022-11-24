using System.ComponentModel.DataAnnotations;
using Common.Enums;
namespace FinalAssignment.DTOs.User
{
    public class EditUserRequest
    {
        [Required]
        public string UserName {get; set;}
        public GenderEnum Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateOnly JoinedDate { get; set; }
        public string UserRole {get; set;}
    }
}