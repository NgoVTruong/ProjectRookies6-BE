using Common.Enums;


namespace Data.Entities
{
    public class RequestReturning
    {
        public Guid RequestReturningId { get; set; }
        public string AssignTo { get; set; }
        public string AssetCode { get; set; }
        public DateTime AssignedDate { get; set; }
        public AssignmentStateEnum AssignmentStateEnum { get; set; }
        public string Note { get; set; }
        public string? ReturnDate { get; set; }
        public string AssignedBy { get; set; }
        public string requestedBy { get; set; }
        public string AcceptedBy { get; set; }


    }
}