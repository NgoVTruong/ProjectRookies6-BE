using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Auth;
using Data.Entities;
using FinalAssignment.DTOs.Asset;
using FinalAssignment.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using TestWebAPI.Repositories.Implements;

namespace FinalAssignment.Repositories.Implements
{
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AssetRepository(FinalAssignmentContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<AssetResponse>> GetAllAsset(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Enumerable.Empty<AssetResponse>();
            }
            var location = user.Location;
            var getList = _dbSet.Where(s => s.IsDeleted == false
                                            && s.Location.ToLower().Contains(location.ToLower())
                                            && s.AssetStatus < (Common.Enums.AssetStateEnum)3)

                          .Select(i => new AssetResponse
                          {
                              AssetCode = i.AssetCode,
                              AssetName = i.AssetName,
                              CategoryName = i.CategoryName,
                              AssetStatus = i.AssetStatus,
                          }).ToList();

            return getList;

        }

        public async Task<AssetDetail> AssetDetail(string assetCode)
        {
            var getDetail = _dbSet.FirstOrDefault(s => s.AssetCode == assetCode);

            if (getDetail != null)
            {
                return new AssetDetail
                {
                    AssetName = getDetail.AssetName,
                    CategoryName = getDetail.CategoryName,
                    Specification = getDetail.Specification,
                    InstalledDate = getDetail.InstalledDate,
                    AssetStatus = getDetail.AssetStatus
                };
            }
            else
            {
                return new AssetDetail
                {
                    AssetName = "null",
                    CategoryName = "null",
                    Specification = "null",
                    InstalledDate = DateTime.Now,
                    AssetStatus = (Common.Enums.AssetStateEnum)5
                }; ;
            }

        }

    }
}