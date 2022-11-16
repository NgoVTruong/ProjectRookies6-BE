using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class FinalAssignmentContext : DbContext
    {
        public FinalAssignmentContext(DbContextOptions<FinalAssignmentContext> options) : base(options)
        {

        }       
    }
}
