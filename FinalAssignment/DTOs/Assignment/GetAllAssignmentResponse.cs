using Common.Enums;
using FinalAssignment.DTOs.Base;

namespace FinalAssignment.DTOs.Assignment
{
    public class GetAllAssignmentResponse 
    {
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedDate { get; set; }
        public AssignmentStateEnum AssignmentState { get; set; }
    }
}
