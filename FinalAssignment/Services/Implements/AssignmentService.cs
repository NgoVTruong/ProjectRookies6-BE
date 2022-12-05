using Data.Entities;
using FinalAssignment.DTOs.Assignment;
using FinalAssignment.Repositories.Implements;
using FinalAssignment.Repositories.Interfaces;
using FinalAssignment.Services.Interfaces;
using System.Collections.Generic;

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
            var assignmentList = (await _assignmentRepository.GetAllAsync()).ToList();
            foreach (var userId in assignmentList)
            {
                var userTo = await _userRepository.GetOneAsync(x => x.Id == userId.AssignedTo);
                var userBy = await _userRepository.GetOneAsync(x => x.Id == userId.AssignedBy);
                return assignmentList.Select(ass => new GetAllAssignmentResponse
                {
                    AssetCode = ass.AssetCode,
                    AssetName = ass.AssetName,
                    AssignedBy = userTo.UserName,
                    AssignedDate = ass.AssignedDate,
                    AssignedTo = userBy.UserName,
                    AssignmentState = ass.AssignmentState,
                });
            }
            if (assignmentList == null)
            {
                return null;
            }
            return null;
        }
    }
}
