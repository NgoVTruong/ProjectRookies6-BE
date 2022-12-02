using Data.Entities;
using FinalAssignment.DTOs.Assignment;
using FinalAssignment.Repositories.Implements;
using FinalAssignment.Repositories.Interfaces;
using FinalAssignment.Services.Interfaces;

namespace FinalAssignment.Services.Implements
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAssetRepository _assetRepository;
        public AssignmentService(IAssignmentRepository assignmentRepository, IUserRepository userRepository, IAssetRepository assetRepository)
        {
            _assignmentRepository = assignmentRepository;
            _userRepository = userRepository;
            _assetRepository = assetRepository;
        }
        public async Task<CreateAssignmentResponse> Create(CreateAssignmentRequest assignmentRequest)
        {
            using var transaction = _assignmentRepository.DatabaseTransaction();
            try
            {
                var newAssignment = new Assignment
                {   
                    Id = Guid.NewGuid(),
                    AcceptedBy = assignmentRequest.AssignedTo,
                    AssignedTo = assignmentRequest.AssignedTo,
                    AssetCode = assignmentRequest.AssetCode,
                    AssetName = assignmentRequest.AssetName,
                    AssignedDate = assignmentRequest.AsssignedDate,
                    AssignmentState = 0,
                    IsDeleted = false,
                    Specification = "null",
                    Note = assignmentRequest.Note,
                    AssetId = assignmentRequest.AssetId,
                    RequestBy = assignmentRequest.AssignedBy,
                    AssignedBy = assignmentRequest.AssignedBy,
                };

                var createAssignment = await _assignmentRepository.CreateAsync(newAssignment);

                _assignmentRepository.SaveChanges();

                transaction.Commit();


                return new CreateAssignmentResponse
                {
                    IsSucced = true,
                    Message = "Create succeed"
                };
            }
            catch (Exception)
            {
                transaction.RollBack();

                return new CreateAssignmentResponse
                {
                    IsSucced = false,
                    Message = "Create fail"
                };
            }
        }
    }
}
