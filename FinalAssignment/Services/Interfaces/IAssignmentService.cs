using FinalAssignment.DTOs.Assignment;

namespace FinalAssignment.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<CreateAssignmentResponse> Create (CreateAssignmentRequest assignmentRequest);
        Task<IEnumerable<GetAllAssignmentResponse>> GetAll();
    }
}
