using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using FinalAssignment.DTOs.Asset;
using TestWebAPI.Repositories.Interfaces;

namespace FinalAssignment.Repositories.Interfaces
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        Task<IEnumerable<AssetResponse>> GetAllAsset(string location);
        Task<AssetDetail> AssetDetail(string assetCode);
    }
}