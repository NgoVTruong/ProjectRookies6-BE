using Data;
using Data.Entities;
using FinalAssignment.Repositories.Interfaces;
using TestWebAPI.Repositories.Implements;

namespace FinalAssignment.Repositories.Implements
{
    public class RequestReturningRepository : BaseRepository<RequestReturning>, IRequestReturningRepository
    {
        public RequestReturningRepository(FinalAssignmentContext context) : base (context)
        {}
    }
}