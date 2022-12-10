using Data.Auth;
using Data.Entities;
using FinalAssignment.DTOs.Assignment;
using FinalAssignment.Repositories.Interfaces;
using FinalAssignment.Services.Interfaces;
using Humanizer;
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

        public async Task<CreateAssignmentResponse> AcceptAssignment(Guid id)
        {
            using var transaction = _assignmentRepository.DatabaseTransaction();
            try
            {
                var assignment = await _assignmentRepository.GetOneAsync(x => x.Id == id);

                if (assignment == null)
                {
                    return new CreateAssignmentResponse
                    {
                        IsSucced = false,
                        Message = "Assignment is not exists!"
                    };
                }

                assignment.AssignmentState = Common.Enums.AssignmentStateEnum.Accepted;
                await _assignmentRepository.UpdateAsync(assignment);

                _assignmentRepository.SaveChanges();
                transaction.Commit();

                return new CreateAssignmentResponse
                {
                    IsSucced = true,
                    Message = "Accept Assignment Succeed"
                };
            }
            catch (Exception)
            {
                transaction.RollBack();

                return new CreateAssignmentResponse
                {
                    IsSucced = false,
                    Message = "Accept Assignment Fail"
                };
            }
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
            var assignmentList = (await _assignmentRepository.GetAllAsync()).Where(x => x.IsDeleted == false);
            if (assignmentList == null)
            {
                return null;
            }
            var newAssignments = new List<GetAllAssignmentResponse>();
            foreach (var assignment in assignmentList)
            {
                var userTo = await _userManager.FindByIdAsync(assignment.AssignedTo);
                var userBy = await _userManager.FindByIdAsync(assignment.AssignedBy);
                var asset = await _assetRepository.GetOneAsync(x => x.AssetCode == assignment.AssetCode);
                var data = new GetAllAssignmentResponse()
                {
                    Id = assignment.Id,
                    AssetCode = assignment.AssetCode,
                    AssignedBy = userBy.UserName,
                    AssetName = assignment.AssetName,
                    AssignedDate = assignment.AssignedDate,
                    AssignedTo = userTo.UserName,
                    AssignmentState = assignment.AssignmentState,
                    Specification = asset.Specification,
                    Note = assignment.Note
                };
                newAssignments.Add(data);
            }
            return newAssignments;
        }

        public async Task<IEnumerable<GetAllAssignmentResponse>> GetAllDependUser(string userId)
        {
            var assignmentList = (await _assignmentRepository.GetAllAsync()).Where(x => x.IsDeleted == false && x.AssignedTo == userId && DateTime.Parse(x.AssignedDate) <= DateTime.Now);
            if (assignmentList == null)
            {
                return null;
            }
            var newAssignments = new List<GetAllAssignmentResponse>();
            foreach (var assignment in assignmentList)
            {
                var userTo = await _userManager.FindByIdAsync(assignment.AssignedTo);
                var userBy = await _userManager.FindByIdAsync(assignment.AssignedBy);
                var asset = await _assetRepository.GetOneAsync(x => x.AssetCode == assignment.AssetCode);
                var data = new GetAllAssignmentResponse()
                {
                    Id = assignment.Id,
                    AssetCode = assignment.AssetCode,
                    AssignedBy = userBy.UserName,
                    AssetName = assignment.AssetName,
                    AssignedDate = assignment.AssignedDate,
                    AssignedTo = userTo.UserName,
                    AssignmentState = assignment.AssignmentState,
                    Specification = asset.Specification,
                    Note = assignment.Note
                };
                newAssignments.Add(data);
            }
            return newAssignments;
        }

        public async Task<GetAssignmentDetailResponse> GetAssignmentDetail(string assetCode)
        {
            var assignment = await _assignmentRepository.GetOneAsync(x => x.AssetCode == assetCode);
            if (assignment == null)
            {
                return null;
            }
            var userTo = await _userManager.FindByIdAsync(assignment.AssignedTo);
            var userBy = await _userManager.FindByIdAsync(assignment.AssignedBy);
            return new GetAssignmentDetailResponse()
            {
                AssetCode = assignment.AssetCode,
                AssetId = assignment.AssetId.ToString(),
                AssignedBy = userBy.UserName,
                AssignedTo = userTo.UserName,
                Note = assignment.Note,
                Specification = assignment.Specification,
                AssignedDate = assignment.AssignedDate,
                state = assignment.AssignmentState
            };
        }
        public async Task<Assignment?> EditAssignment(EditAssignmentRequest editAssignmentRequest, Guid id)
        {
            var editAssignment = await _assignmentRepository.GetOneAsync(x => x.Id == id);


            if (editAssignment == null)
            {
                return null;
            }
           
            editAssignment.Id = id;
            editAssignment.Note = editAssignmentRequest.Note;
            editAssignment.AssignedDate = editAssignmentRequest.AssignedDate;
            editAssignment.AssetId = editAssignmentRequest.AssetId;
            editAssignment.AssetCode = editAssignmentRequest.AssetCode;
            editAssignment.AssetName = editAssignmentRequest.AssetName;
            editAssignment.AssignedTo = editAssignmentRequest.AssignedTo;
            editAssignment.AssignedBy = editAssignmentRequest.AssignedBy;

            await _assignmentRepository.UpdateAsync(editAssignment);

            _assignmentRepository.SaveChanges();


            return new Assignment
            {
                Id = id,
                AssetId = editAssignment.AssetId,
                AssignedTo = editAssignment.AssignedTo,
                AssignedBy = editAssignment.AssignedBy,
                AssignedDate = editAssignment.AssignedDate,
                Note = editAssignment.Note,
            };
        }

        public async Task<EditAssignmentResponse?> GetAssignmentById(Guid id)
        {
           
            var assignment = await _assignmentRepository.GetOneAsync(x => x.Id == id);
            if (assignment == null)
            {
                return null;
            }
            var userTo = await _userManager.FindByIdAsync(assignment.AssignedTo);
            var userBy = await _userManager.FindByIdAsync(assignment.AssignedBy);
            DateTime assignDate = DateTime.Parse(assignment.AssignedDate);
            return new EditAssignmentResponse()
            {
                Id = assignment.Id,
                AssetName = assignment.AssetName,
                AssetCode = assignment.AssetCode,
                AssetId = assignment.AssetId.ToString(),
                AssignedBy = userBy.UserName,
                AssignedTo = userTo.UserName,
                AssignedById = assignment.AssignedBy,
                AssignedToId = assignment.AssignedTo,
                Note = assignment.Note,
                Specification = assignment.Specification,
                AssignedDate = assignDate.ToString("yyyy-MM-dd"),
                AssignmentState = assignment.AssignmentState
            };
        }


    }
}
