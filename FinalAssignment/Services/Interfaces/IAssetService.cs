using Data.Entities;
using FinalAssignment.DTOs.Asset;

namespace FinalAssignment.Services.Interfaces
{
    public interface IAssetService
    {
        Task<Asset?> Create(AssetRequest assetRequest);

        Task<bool> DeleteAsset(string assetCode);

        Task<IEnumerable<AssetResponse>> GetAllAsset(string adminLocation);

        Task<AssetDetail> GetDetailAsset(string assetCode);

        Task<AsignedAsset> GetAssignedAsset(string assetCode);

    }
}
