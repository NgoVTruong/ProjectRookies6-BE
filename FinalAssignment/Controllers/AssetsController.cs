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

            /*if (((int)assetRequest.AssetStatus) > 1) return BadRequest($"State invalid");*/

            var result = await _assetService.Create(assetRequest);

            if (result == null)
                return StatusCode(500, "Sorry the Request failed");

            return Ok(result);
        }

        [HttpDelete("{assetCode}")]
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

        [HttpGet("assigned-asset/{assetCode}")]
        public async Task<AsignedAsset> GetAssignedAsset(string assetCode)
        {
            var getAssignedAsset = await _assetService.GetAssignedAsset(assetCode);
            return getAssignedAsset;
        }
    }
}
