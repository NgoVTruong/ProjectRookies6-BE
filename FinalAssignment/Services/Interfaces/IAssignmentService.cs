using FinalAssignment.DTOs.Assignment;

namespace FinalAssignment.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<CreateAssignmentResponse> Create (CreateAssignmentRequest assignmentRequest);
        Task<IEnumerable<GetAllAssignmentResponse>> GetAll();
        Task<IEnumerable<GetAllAssignmentResponse>> GetAllDependUser(string userId);
        Task<GetAssignmentDetailResponse> GetAssignmentDetail(string assetCode);
        Task<CreateAssignmentResponse> AcceptAssignment(Guid id);
    }
}
