using Data.Auth;
using Data.Entities;
using FinalAssignment.DTOs.Assignment;
using FinalAssignment.Repositories.Interfaces;
using FinalAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace FinalAssignment.Services.Implements
{
    public class AssignmentService : IAssignmentService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAssetRepository _assetRepository;
        public AssignmentService(IAssignmentRepository assignmentRepository, IUserRepository userRepository, IAssetRepository assetRepository, UserManager<ApplicationUser> userManager)
        {
            _assignmentRepository = assignmentRepository;
            _userRepository = userRepository;
            _assetRepository = assetRepository;
            _userManager = userManager;
        }
        public async Task<CreateAssignmentResponse> Create(CreateAssignmentRequest assignmentRequest)
        {
            using var transaction = _assignmentRepository.DatabaseTransaction();
            try
            {
                var assetDetail = await _assetRepository.GetOneAsync(a => a.Id == assignmentRequest.AssetId);

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
                    Specification = assetDetail.Specification,
                    Note = assignmentRequest.Note,
                    AssetId = assignmentRequest.AssetId,
                    RequestBy = assignmentRequest.AssignedBy,
                    AssignedBy = assignmentRequest.AssignedBy,
                };

                var createAssignment = await _assignmentRepository.CreateAsync(newAssignment);

                _assignmentRepository.SaveChanges();

                var asset = await _assetRepository.GetOneAsync(x => x.Id == assignmentRequest.AssetId);
                asset.AssetStatus = (Common.Enums.AssetStateEnum)1;

                await _assetRepository.UpdateAsync(asset);
                _assetRepository.SaveChanges();
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


        public async Task<IEnumerable<GetAllAssignmentResponse>> GetAll()
        {
            var assignmentList = await _assignmentRepository.GetAllAsync();
            if (assignmentList == null)
            {
                return null;
            }
            var newAssignments = new List<GetAllAssignmentResponse>();
            foreach (var assignment in assignmentList)
            {
                var userTo = await _userManager.FindByIdAsync(assignment.AssignedTo);
                var userBy = await _userManager.FindByIdAsync(assignment.AssignedBy);
                var data = new GetAllAssignmentResponse()
                {
                    AssetCode = assignment.AssetCode,
                    AssignedBy = userBy.UserName,
                    AssetName = assignment.AssetName,
                    AssignedDate = assignment.AssignedDate,
                    AssignedTo = userTo.UserName,
                    AssignmentState = assignment.AssignmentState,
                };
                newAssignments.Add(data);
            }
            return newAssignments;
        }

    }
}
