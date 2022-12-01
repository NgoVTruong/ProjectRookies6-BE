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

 /*       public int GetAll(string userName )
        {
            var getAll = _dbSet.Where(x => x.UserName == userName).Count();
            return getAll;
        }*/
    }
}