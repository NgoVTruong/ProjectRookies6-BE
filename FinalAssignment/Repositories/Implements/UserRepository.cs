using Data;
using Data.Auth;
using FinalAssignment.Repositories.Interfaces;
using TestWebAPI.Repositories.Implements;

namespace FinalAssignment.Repositories.Implements
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(FinalAssignmentContext context) : base (context)
        {
        }
    }
}