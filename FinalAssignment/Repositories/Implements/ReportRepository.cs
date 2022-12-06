using Data;
using Data.Entities;
using FinalAssignment.Repositories.Interfaces;
using TestWebAPI.Repositories.Implements;

namespace FinalAssignment.Repositories.Implements
{
    public class ReportRepository : BaseRepository<Report>, IReportRepository
    {
        public ReportRepository(FinalAssignmentContext context) : base (context)
        {

        }
    }
}