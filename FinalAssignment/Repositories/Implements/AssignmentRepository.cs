using Data;
using Data.Entities;
using FinalAssignment.DTOs.Asset;
using FinalAssignment.Repositories.Interfaces;
using TestWebAPI.Repositories.Implements;

namespace FinalAssignment.Repositories.Implements
{

    public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(FinalAssignmentContext context) : base(context)
        {
        }
        public async Task<AsignedAsset> GetAssignedAsset(string assetCode)
        {
            var assignedAsset = _dbSet.FirstOrDefault(s => s.AssetCode == assetCode);


            if (assignedAsset != null)
            {
                return new AsignedAsset
                {
                    AssignedTo = assignedAsset.AssignedTo,
                    AssignedBy = assignedAsset.AssignedBy,
                    AssignedDate = assignedAsset.AssignedDate,
                };
            }
            return new AsignedAsset
            {
                AssignedTo = "null",
                AssignedBy = "null",
                AssignedDate = "0000-00-00",
            }; ;
        }
    }
}