using Data;
using Data.Entities;
using FinalAssignment.DTOs.Asset;
using FinalAssignment.Repositories.Interfaces;
using TestWebAPI.Repositories.Implements;

namespace FinalAssignment.Repositories.Implements
{

    public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(FinalAssignmentContext context) : base (context)
        {}
        public async Task<IEnumerable<AsignedAsset>> GetAssignedAsset(string assetCode)
        {
            var assignedAsset = _dbSet.Where(s => s.AssetCode == assetCode)
                .Select(a => new AsignedAsset
                {
                    AssignedTo = a.AssignedTo,
                    AssignedDate = a.AssignedDate,
                    AssetName = a.AssetName,
                    AssignmentState = a.AssignmentState,
                }).ToList();

            if (assignedAsset != null)
            {
                return assignedAsset;
            }
            return null;
        }
    }
}