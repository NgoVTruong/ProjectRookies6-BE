using Data.Auth;
using Data.Entities;
using FinalAssignment.DTOs.Asset;
using FinalAssignment.Repositories.Interfaces;
using FinalAssignment.Services.Interfaces;

namespace FinalAssignment.Services.Implements
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _asset;
        private readonly IAssignmentRepository _assignnment;


        public AssetService(IAssetRepository asset , IAssignmentRepository assignnment)
        {
            _asset = asset;
            _assignnment = assignnment;
        }

        public async Task<AssetDetail> GetDetailAsset(string assetCode)
        {
            var getDetailAsset = await _asset.AssetDetail(assetCode);

            return getDetailAsset;
        }

        public async Task<bool> DeleteAsset(string assetCode)
        {
            using (var transaction = _asset.DatabaseTransaction())
            {
                try
                {
                    var asset = await _asset.GetOneAsync(s => s.AssetCode == assetCode);
                    var assignment = await _assignnment.GetOneAsync(a => a.AssetCode == assetCode);
                    
                    if (asset != null && assignment == null)
                    {
                        
                        asset.IsDeleted = true;
                        _asset.UpdateAsync(asset);
                        _asset.SaveChanges();
                        transaction.Commit();

                        return await Task.FromResult(true);
                    }

                    return await Task.FromResult(false);
                }
                catch
                {
                    transaction.RollBack();
                    return await Task.FromResult(false);
                }
            }
        }

        public async Task<IEnumerable<AssetResponse>> GetAllAsset(string userName)
        {
            var getListAsset = await _asset.GetAllAsset(userName);

            return getListAsset;

        }

        public async Task<AsignedAsset> GetAssignedAsset(string assetCode)
        {
           var getAssignedAsset = await _assignnment.GetAssignedAsset(assetCode);
            return getAssignedAsset;
        }
    }
}
