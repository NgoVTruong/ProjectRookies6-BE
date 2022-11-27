using FinalAssignment.DTOs.Asset;
using FinalAssignment.DTOs.User;
using FinalAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {/*
        [HttpGet("list")]
        public async Task<IActionResult> CreateUser( RegisterModelRequest model)
        {
            var data = await _userService.Register(model);

            return Ok(data);
        }*/
        private readonly IAssetService _assetService;
        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpDelete("{assetCode}")]
        public async Task<bool> DeleteAsset(string assetCode)
        {
            var data = _assetService.DeleteAsset(assetCode);

            return await data;
        }

        [HttpGet]
        public async Task<IEnumerable<AssetResponse>> GetAllAsset(string userName)
        {
            return await _assetService.GetAllAsset(userName);
        }

        [HttpGet("detail-asset/{assetCode}")]
        public async Task<AssetDetail> GetDetailAsset(string assetCode)
        {
            var getDetailAsset = await _assetService.GetDetailAsset(assetCode);

            return getDetailAsset;
        }

        [HttpGet("assigned-asset/{assetCode}")]
        public async Task<IEnumerable<AsignedAsset>> GetAssignedAsset(string assetCode)
        {
            var getAssignedAsset = await _assetService.GetAssignedAsset(assetCode);
            return getAssignedAsset;
        }
    }
}
