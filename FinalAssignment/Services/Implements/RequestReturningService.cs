using Common.Enums;
using Data.Entities;
using FinalAssignment.DTOs.Request;
using FinalAssignment.Repositories.Interfaces;
using FinalAssignment.Services.Interfaces;

namespace FinalAssignment.Services.Implements
{
    public class RequestReturningService : IRequestReturningService
    {
        private readonly IRequestReturningRepository _requestReturningRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserRepository _user;
        private readonly IAssetRepository _assetRepository;

        public RequestReturningService(IAssignmentRepository assignmentRepository, IUserRepository user,
            IAssetRepository asset, IRequestReturningRepository requestReturningRepository)
        {
            _requestReturningRepository = requestReturningRepository;
            _assignmentRepository = assignmentRepository;
            _assetRepository = asset;
            _user = user;
        }

        public async Task<bool> CancelRequest(Guid reqId)
        {
            using (var transaction = _requestReturningRepository.DatabaseTransaction())
            {
                try
                {
                    var getRequest = await _requestReturningRepository.GetOneAsync(s => s.Id == reqId
                    && s.RequestStatus == RequestStateEnum.WaitingForReturning);
                    if (getRequest != null)
                    {
                        _requestReturningRepository.DeleteAsync(getRequest);
                        _requestReturningRepository.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    return false;

                }
                catch
                {
                    transaction.RollBack();
                    return false;
                }
            }
        }

        public async Task<bool> CompleteRequest(string assetCode)
        {
            using (var transaction = _requestReturningRepository.DatabaseTransaction())
            {
                try
                {
                    var getRequest = await _requestReturningRepository.GetOneAsync(i => i.Assignment.AssetCode == assetCode
                                     && i.RequestStatus == RequestStateEnum.WaitingForReturning);
                    var getAssignment = await _assignmentRepository.GetOneAsync(i => i.AssetCode == assetCode && i.IsDeleted == false);
                    var getAsset = await _assetRepository.GetOneAsync(i => i.AssetCode == assetCode && i.IsDeleted == false);
                    if (getRequest != null)
                    {
                        getRequest.RequestStatus = RequestStateEnum.Completed;
                        getRequest.ReturnDate = DateTime.Now.ToString("yyyy-MM-dd");
                        getAssignment.IsDeleted = true;
                        getAsset.AssetStatus = AssetStateEnum.Available;
                        _assignmentRepository.UpdateAsync(getAssignment);
                        _requestReturningRepository.UpdateAsync(getRequest);
                        _assetRepository.UpdateAsync(getAsset);
                        _assignmentRepository.SaveChanges();
                        _assetRepository.SaveChanges();
                        _requestReturningRepository.SaveChanges();
                        transaction.Commit();

                        return true;
                    }

                    return false;
                }
                catch
                {
                    transaction.RollBack();
                    return false;
                }
            }
        }

        public async Task<CreateRequestReturningResponse> CreateRequestForReturning(CreateRequestReturningRequest model)
        {
            using var transaction = _requestReturningRepository.DatabaseTransaction();
            try
            {
                var assignment = await _assignmentRepository.GetOneAsync(x => x.Id == model.AssignmentId);
                if (assignment != null)
                {
                    assignment.AssignmentState = AssignmentStateEnum.WaitingForReturning;

                    var newRequest = new RequestReturning
                    {
                        Id = Guid.NewGuid(),
                        UserId = model.UserId,
                        AssignmentId = model.AssignmentId,
                        ReturnDate = null,
                        RequestStatus = RequestStateEnum.WaitingForReturning,
                        Time = DateTime.Now,
                    };

                    await _requestReturningRepository.CreateAsync(newRequest);

                    _requestReturningRepository.SaveChanges();
                    transaction.Commit();

                    return new CreateRequestReturningResponse
                    {
                        Status = "Success",
                        Message = "User created request for returning success!",
                    };

                }
                return null;
            }
            catch (Exception)
            {
                transaction.RollBack();

                return new CreateRequestReturningResponse
                {
                    Status = "Error",
                    Message = "User created request for returning fail!",
                };
            }
        }

        public async Task<IEnumerable<ReturningRequest>> GetAllReturningRequest()
        {
            var getRequest = _requestReturningRepository.GetAllRequest().OrderBy(a => a.Assignment.AssetCode).Select(i => new ReturningRequest()
                {
                    Id = i.Id,
                    AssetCode = i.Assignment.AssetCode,
                    AssetName = i.Assignment.AssetName,
                    AcceptedBy = i.Assignment.AssignedToUser.UserName,
                    AssignedDate = i.Assignment.AssignedDate,
                    RequestBy = i.ApplicationUser.UserName,
                    ReturnDate = i.ReturnDate,
                    RequestStatus = i.RequestStatus,
                    Time = i.Time,
                });
            if (getRequest == null)
            {
                return null;
            }

            return getRequest;

        }
    }
}