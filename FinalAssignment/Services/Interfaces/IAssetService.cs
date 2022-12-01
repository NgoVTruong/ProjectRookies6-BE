using Data.Entities;
using FinalAssignment.DTOs.Asset;

namespace FinalAssignment.Services.Interfaces
{
    public interface IAssetService
    {
        Task<Asset?> Create(AssetRequest assetRequest);
        Task<EditAssetResponse> EditAsset(EditAssetRequest asset, string assetCode);

        Task<bool> DeleteAsset(string assetCode);
        Task<IEnumerable<AssetResponse>> GetAllAsset(string location);
        Task<AssetDetail> GetDetailAsset(string assetCode);
        Task<AsignedAsset> GetAssignedAsset(string assetCode);
    }
}
