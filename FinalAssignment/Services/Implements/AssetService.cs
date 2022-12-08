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
                    var assignment = await _assignnment.GetOneAsync(a => a.AssetCode == assetCode && a.IsDeleted == false);

                    if (asset != null && assignment == null)
                    {
                        asset.IsDeleted = true;
                        _asset.UpdateAsync(asset);
                        _asset.SaveChanges();
                        transaction.Commit();

                        return true;
                    }

                    return false;
                }
                catch
                {
                    transaction.RollBack();
                    return false;
                }
            }
        }

        public async Task<IEnumerable<Asset>> GetAllAsset(string location)
        {
            var getListAsset = await _asset.GetAllAsync(s => s.IsDeleted == false
                                            && s.Location.ToLower().Contains(location.ToLower()));

            var getListAssetOrderBy = getListAsset.OrderBy(a => a.AssetCode);
            foreach (var item in getListAssetOrderBy)
            {
                TimeSpan checkTime = DateTime.Now - item.Time;

                if (checkTime.TotalSeconds <= 7)
                {
                    var getListAssetOrderByTime = getListAsset.OrderByDescending(a => a.Time);
                    return getListAssetOrderByTime;
                }
            }

            return getListAssetOrderBy;
        }

        public async Task<IEnumerable<AssetResponse>> GetAllAssetByStatus(string location)
        {
            var getListAsset = await _asset.GetAllAssetByStatus(location);

            return getListAsset;
        }

        public async Task<DetailAsset> GetAssignedAsset(string assetCode)
        {
            var asset = await _assignnment.GetOneAsync(id => id.AssetCode == assetCode);
            var userTo = await _user.GetOneAsync(x => x.Id == asset.AssignedTo);
            var userBy = await _user.GetOneAsync(x => x.Id == asset.AssignedBy);
            var cateName = await _asset.GetOneAsync(x => x.AssetCode == assetCode);
            return new DetailAsset
            {
                AssignedTo = userTo.UserName,
                AssignedBy = userBy.UserName,
                AssetName = asset.AssetName,
                CategoryName = cateName.CategoryName,
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
                    var assetCodeCheck = (await _asset.GetAllAsync()).Count();

                    string getAssetCode = category.CategoryName;
   
                    var assetcheck = _asset.GetAll(assetRequest.CategoryId);

                    string AssetCodeGen(int number) //35
                    {
                        int check = number;
                        int count = 0;
                        while (check > 0) //35  //3
                        {
                            check = check / 10; //3 //0
                            count++; //1 //2
                        }
                        string assetCode = "";

                        for (int i = 0; i < getAssetCode.Length; i++)
                        {
                            if (i <= 1) assetCode += getAssetCode[i];

                        };
                        for (int i = 0; i < 5 - count; i++)  //(int i = 0; i < 2; i++)
                        {
                            assetCode = assetCode + "0"; // SD00
                        }
                        string num = (++number).ToString();                      

                        assetCode = assetCode.ToUpper() + num;
                        return assetCode;
                    }

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
                        AssetCode = AssetCodeGen(assetCodeCheck),
                        AssetName = assetRequest.AssetName,
                        CategoryName = category.CategoryName,
                        AssetStatus = assetRequest.AssetStatus,
                        InstalledDate = assetRequest.InstalledDate,
                        Specification = assetRequest.Specification,
                        Time=DateTime.Now,
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

        public async Task<bool> CheckAsset(string assetCode)
        {
            var assignment = await _assignnment.GetOneAsync(a => a.AssetCode == assetCode && a.IsDeleted == false);
            if (assignment == null)
            {
                return true;
            }
            return false;
        }
    }
}
