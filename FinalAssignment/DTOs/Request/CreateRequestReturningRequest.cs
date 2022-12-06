using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Enums;
using Data.Auth;
using Data.Entities;

namespace FinalAssignment.DTOs.Request
{
    public class CreateRequestReturningRequest
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public Guid? AssignmentId { get; set; }

        public string? ReturnDate { get; set; }
        public RequestStateEnum RequestStatus { get; set; }

        // public virtual ApplicationUser? ApplicationUser { get; set; }

        // public virtual Assignment? Assignment { get; set; }
    }
}