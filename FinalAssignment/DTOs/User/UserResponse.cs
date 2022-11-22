using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAssignment.DTOs.User
{
    public class UserResponse
    {
        public string FirstName {get; set;}
        public string StaffCode {get; set;}
        public string FullName {get; set;}
        public string UserName { get; set; }
        public DateTime JoinDate { get; set; }
        public string? Type { get; set; }
    }
}