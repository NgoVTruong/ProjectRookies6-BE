using Data.Entities;
using FinalAssignment.DTOs.Asset;
using TestWebAPI.Repositories.Interfaces;

namespace FinalAssignment.Repositories.Interfaces
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        Task<IEnumerable<AssetResponse>> GetAllAsset(string location);
        Task<AssetDetail> AssetDetail(string assetCode);
        Task<EditAssetResponse> EditAsset(EditAssetRequest asset, string assetCode);
        int GetAll(Guid id);
        Task<EditAssetResponse> getEditAsset(string assetCode);

    }
}