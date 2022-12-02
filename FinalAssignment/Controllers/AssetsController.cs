using Common.Enums;
using FinalAssignment.DTOs.Asset;
using FinalAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinalAssignment.Controllers
{
    [Route("api/asset-management")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;
        public AssetsController(IAssetService assetService )
        {
            _assetService = assetService;
         
        }

        [HttpPost("assets")]
        public async Task<IActionResult> Create(AssetRequest assetRequest)
        {

            if ((assetRequest.AssetName) !=null) return BadRequest($"State invalid");

            var result = await _assetService.Create(assetRequest);

            if (result == null)
                return StatusCode(500, "Sorry the Request failed");

            return Ok(result);
        }

        [HttpGet("edited-asset/{assetCode}")]
        public async Task<EditAssetResponse> getEditAsset(string assetCode)
        {
            var getEdit = await _assetService.getEditAsset(assetCode);
            return getEdit;
        }

        [HttpPut("assets/{assetCode}")]
        public async Task<IActionResult> EditAsset(EditAssetRequest asset, string assetCode)
        {
            var editAsset = await _assetService.EditAsset(asset, assetCode);
            if (editAsset.InstalledDate > DateTime.Now)
            {
                return BadRequest("Invalid InstallDate!");
            }
            if (editAsset.AssetName == "" || editAsset.Specification == ""
             )
            {
                return BadRequest("Must fill all blank!");
            }
            if (editAsset.AssetStatus == AssetStateEnum.Assigned)
            {
                return BadRequest("Invalid AssetStatus!");
            }
            if (editAsset == null)
            {
                return StatusCode(400, "Not found the Asset");
            }
            return StatusCode(200, "Edit successfully!");
        }

        [HttpDelete("assets/{assetCode}")]
        public async Task<bool> DeleteAsset(string assetCode)
        {
            var data = _assetService.DeleteAsset(assetCode);

            return await data;
        }

        [HttpGet]
        public async Task<IEnumerable<AssetResponse>> GetAllAsset(string location)
        {
            return await _assetService.GetAllAsset(location);
        }

        //[HttpGet("detail-asset/{assetCode}")]
        //public async Task<AssetDetail> GetDetailAsset(string assetCode)
        //{
        //    var getDetailAsset = await _assetService.GetDetailAsset(assetCode);

        //    return getDetailAsset;
        //}

        [HttpGet("detail-asset/{assetCode}")]
        public async Task<AsignedAsset> GetAssignedAsset(string assetCode)
        {
            var getAssignedAsset = await _assetService.GetAssignedAsset(assetCode);
            return getAssignedAsset;
        }
    }
}
