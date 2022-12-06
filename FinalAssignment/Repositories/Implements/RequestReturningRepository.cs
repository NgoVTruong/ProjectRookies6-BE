using Data;
using Data.Entities;
using FinalAssignment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TestWebAPI.Repositories.Implements;

namespace FinalAssignment.Repositories.Implements
{
    public class RequestReturningRepository : BaseRepository<RequestReturning>, IRequestReturningRepository
    {
        public RequestReturningRepository(FinalAssignmentContext context) : base(context)
        { }
        public IEnumerable<RequestReturning> GetAllRequest()
        {
            return _dbSet.Include(p => p.ApplicationUser)
                        .Include(a => a.Assignment);
        }
    }
}