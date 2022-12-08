using Data;
using Data.Entities;
using FinalAssignment.DTOs.Request;
using FinalAssignment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TestWebAPI.Repositories.Implements;

namespace FinalAssignment.Repositories.Implements
{
    public class RequestReturningRepository : BaseRepository<RequestReturning>, IRequestReturningRepository
    {
        public RequestReturningRepository(FinalAssignmentContext context) : base(context)
        { }
        public IEnumerable<ReturningRequest> GetAllRequest()
        {
            var getData = _dbSet.Include(p => p.ApplicationUser)
                        .Include(a => a.Assignment).Select(i => new ReturningRequest
                        {
                            Id = i.Id,
                            AssetCode = i.Assignment.AssetCode,
                            AssetName = i.Assignment.AssetName,
                            AcceptedBy = i.Assignment.AcceptedBy,
                            AssignedDate = i.Assignment.AssignedDate,
                            RequestBy = i.Assignment.RequestBy,
                            ReturnDate = i.ReturnDate,
                            RequestStatus = i.RequestStatus,
                            Time = i.Time,
                        }).ToList();
            return getData;
        }
    }
}