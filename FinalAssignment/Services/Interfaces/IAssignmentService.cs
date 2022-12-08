using Data.Entities;
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

        Task<Assignment?> EditAssignment(EditAssignmentRequest editAssignmentRequest, Guid id);

        Task<Assignment> GetAssignmentById(Guid id);
    }
}
