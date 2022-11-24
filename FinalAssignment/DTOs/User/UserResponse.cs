using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAssignment.DTOs.User
{
    public class UserResponse
    {
        public string FirstName {get; set;}
        public string LastName { get; set; }
        public string StaffCode {get; set;}
        public string FullName {get; set;}
        public string UserName { get; set; }
        public int? Gender { get; set; }
        public DateTime? JoinedDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Type { get; set; }
        public string Location { get; set; }
    }
}