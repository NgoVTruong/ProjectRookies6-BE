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
        private readonly ICategoryRepository _categoryRepository;
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

        public async Task<IEnumerable<AsignedAsset>> GetAssignedAsset(string assetCode)
        {
           var getAssignedAsset = await _assignnment.GetAssignedAsset(assetCode);
            return getAssignedAsset;
        }

/*        public string AssetCodeGen(int number) //35
        {
            int check = number;
            int count = 0;
            while (check > 0) //35  //3
            {
                check = check / 10; //3 //0
                count++; //1 //2
            }
            string staffCode = "SD";
            for (int i = 0; i < 4 - count; i++)  //(int i = 0; i < 2; i++)
            {
                staffCode = staffCode + "0000"; // SD00
            }
            string num = (++number).ToString();
            staffCode = staffCode + num;
            return staffCode;
        }*/

        public async Task<Asset> Create(AssetRequest assetRequest)
        {
            var category = await _categoryRepository.GetOneAsync(x => x.Id == assetRequest.CategoryId);

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
                AssetCode= assetRequest.AssetCode,
                AssetName = assetRequest.AssetName,
                AssetStatus = assetRequest.AssetStatus,
                InstalledDate = assetRequest.InstalledDate,
                Specification = assetRequest.Specification,
                Location = assetRequest.Location
            };

            var createdAsset = await _asset.CreateAsync(newAsset);
            _asset.SaveChanges();

            if (createdAsset == null)
            {
                return null;
            }

            return createdAsset;
        }


    }
}
