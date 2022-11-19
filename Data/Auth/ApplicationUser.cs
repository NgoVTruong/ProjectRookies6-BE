using Microsoft.AspNetCore.Identity;

using Common.Enums;

namespace Data.Auth
{
    public class ApplicationUser : IdentityUser
    {
        // public string? RefreshToken { get; set; }
        // public DateTime RefreshTokenExpiryTime { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName {get; set;}
        // {
        //     get { return string.Format(FirstName + " " + LastName); }
        // }
        public string? TypeStaff {get; set;}
        public GenderEnum Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? JoinedDate { get; set; }
        public string? StaffCode { get; set; }
        public string? Location { get; set; }
        public int LoginState { get; set; }

    }
}