using Common.Enums;


namespace Data.Entities
{
    public class Assignment
    {
        public Guid AssignmentId { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedDate { get; set; }
        public string AssignedBy { get; set; }
        public string AcceptedBy { get; set; }
        public string ReturnDate { get; set; }
        public string Note { get; set; }
        public AssignmentStateEnum AssignmentState { get; set; }
        public string RequestBy { get; set; }
        public string AssetCode { get; set; }
        public string AssetName {get; set;}
        public string Specification { get; set; }
    }
}