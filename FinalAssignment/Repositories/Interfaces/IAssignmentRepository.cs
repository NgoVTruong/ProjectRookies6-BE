using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using FinalAssignment.DTOs.Asset;
using TestWebAPI.Repositories.Interfaces;

namespace FinalAssignment.Repositories.Interfaces
{
    public interface IAssignmentRepository : IBaseRepository<Assignment>
    {
        Task<IEnumerable<AsignedAsset>> GetAssignedAsset(string assetCode);

    }
}