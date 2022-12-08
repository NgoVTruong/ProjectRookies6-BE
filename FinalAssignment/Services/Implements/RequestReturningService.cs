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
            var getRequest = _requestReturningRepository.GetAllRequest();
            var requestList = new List<ReturningRequest>();

            foreach (var request in getRequest)
            {
                var userTo = await _user.GetOneAsync(s => s.Id == request.AcceptedBy);
                var userBy = await _user.GetOneAsync(s => s.Id == request.RequestBy);
                var data = new ReturningRequest()
                {
                    Id = request.Id,
                    AssetCode = request.AssetCode,
                    AssetName = request.AssetName,
                    AcceptedBy = userTo.UserName,
                    AssignedDate = request.AssignedDate,
                    RequestBy = userBy.UserName,
                    ReturnDate = request.ReturnDate,
                    RequestStatus = request.RequestStatus,
                    Time = request.Time,
                };
                requestList.Add(data);
            }
            //var requestListOrderBy = getRequest.OrderBy(a => a.AssetCode);

            //foreach (var item in requestListOrderBy)
            //{
            //    TimeSpan checkTime = DateTime.Now - item.Time;
            //    if (checkTime.TotalSeconds <= 10)
            //    {
            //        var getListRequestOrderByTime = getRequest.OrderByDescending(a => a.Time);
            //        return getListRequestOrderByTime.Select(i => new ReturningRequest
            //        {
            //            Id = i.Id,
            //            AssetCode = i.AssetCode,
            //            AssetName = i.AssetName,
            //            AcceptedBy = i.AcceptedBy,
            //            AssignedDate = i.AssignedDate,
            //            RequestBy = i.RequestBy,
            //            ReturnDate = i.ReturnDate,
            //            RequestStatus = i.RequestStatus,
            //            Time = i.Time,
            //        });
            //    }
            //}

            return requestList;

        }
    }
}