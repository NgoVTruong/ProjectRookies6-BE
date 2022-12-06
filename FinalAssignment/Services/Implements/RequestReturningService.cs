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
        public RequestReturningService(IAssignmentRepository assignmentRepository, IRequestReturningRepository requestReturningRepository)
        {
            _requestReturningRepository = requestReturningRepository;
            _assignmentRepository = assignmentRepository;
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
    }
}