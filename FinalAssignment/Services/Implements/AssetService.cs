using Common.Enums;
using Data.Entities;
using FinalAssignment.DTOs.Asset;
using FinalAssignment.Repositories.Interfaces;
using FinalAssignment.Services.Interfaces;

namespace FinalAssignment.Services.Implements
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _asset;
        private readonly IUserRepository _user;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAssignmentRepository _assignnment;


        public AssetService(IAssetRepository asset, IUserRepository user, IAssignmentRepository assignnment, ICategoryRepository categoryRepository)
        {
            _asset = asset;
            _assignnment = assignnment;
            _categoryRepository = categoryRepository;
            _user = user;
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

        public async Task<IEnumerable<AssetResponse>> GetAllAsset(string location)
        {
            var getListAsset = await _asset.GetAllAsset(location);

            return getListAsset;

        }
        public async Task<IEnumerable<AssetResponse>> GetAllAssetByStatus(string location)
        {
            var getListAsset = await _asset.GetAllAssetByStatus(location);

            return getListAsset;

        }

        public async Task<DetailAsset> GetAssignedAsset(string assetCode)
        {
            //var getAssignedAsset = await _assignnment.GetAssignedAsset(assetCode);
            //string getAssignedTo = _user.getUserName(getAssignedAsset.AssignedTo);
            //string getAssignedBy = _user.getUserName(getAssignedAsset.AssignedBy);
            var asset = await _assignnment.GetOneAsync(id => id.AssetCode == assetCode);
            var userTo = await _user.GetOneAsync(x => x.Id == asset.AssignedTo);
            var userBy = await _user.GetOneAsync(x => x.Id == asset.AssignedBy);
            return new DetailAsset
            {
                AssignedTo = userTo.UserName,
                AssignedBy = userBy.UserName,
                AssignedDate = asset.AssignedDate,
            };
        }

        public async Task<Asset?> Create(AssetRequest assetRequest)
        {
            using (var transaction = _asset.DatabaseTransaction())
            {
                try
                {
                    var category = await _categoryRepository.GetOneAsync(x => x.Id == assetRequest.CategoryId);

                    var getAssetCode = category.CategoryName;

                    var assetCode = "";
                    for (int i = 0; i < getAssetCode.Length; i++)
                    {
                        if (i <= 1) assetCode += getAssetCode[i];

                    }
                    ;
                    var assetcheck = _asset.GetAll(assetRequest.CategoryId);
                    int numberOfAsset = assetcheck + 1;
                    assetCode = assetCode.ToUpper() + "00000" + numberOfAsset;

                    if (category == null) return null;

                    var now = DateTime.Now;

                    var dateCompare = DateTime.Compare(now, assetRequest.InstalledDate);

                    if (dateCompare < 0)
                    {
                        assetRequest.AssetStatus = AssetStateEnum.NotAvailable;
                    }

                    var newAsset = new Asset
                    {
                        CategoryId = assetRequest.CategoryId,
                        AssetCode = assetCode,
                        AssetName = assetRequest.AssetName,
                        CategoryName = category.CategoryName,
                        AssetStatus = assetRequest.AssetStatus,
                        InstalledDate = assetRequest.InstalledDate,
                        Specification = assetRequest.Specification,
                        Location = assetRequest.Location
                    };

                    var createdAsset = await _asset.CreateAsync(newAsset);
                    _asset.SaveChanges();
                    transaction.Commit();

                    if (createdAsset == null)
                    {
                        return null;
                    }

                    return createdAsset;
                }
                catch
                {
                    transaction.RollBack();
                    return null;
                }
            }
        }

        public async Task<EditAssetResponse> EditAsset(EditAssetRequest asset, string assetCode)
        {
            using (var transaction = _asset.DatabaseTransaction())
            {
                try
                {
                    var getEditAsset = await _asset.EditAsset(asset, assetCode);
                    transaction.Commit();
                    return getEditAsset;
                }
                catch
                {
                    transaction.RollBack();
                    return null;
                }

            }
        }

        public async Task<EditAssetResponse> GetEditAsset(string assetCode)
        {
            var getEdit = await _asset.GetEditAsset(assetCode);
            return getEdit;
        }

        public async Task<Asset> GetAssetByName(string assetName)
        {
            return await _asset.GetOneAsync(x => x.AssetName == assetName);
        }
    }
}
