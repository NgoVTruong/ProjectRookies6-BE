using FinalAssignment.DTOs.Assignment;
using FinalAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace FinalAssignment.Controllers
{
    [Route("/api/assignment-management")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IAssignmentService _assignmentService;
        public AssignmentController(IAssignmentService assignmentService, ILoggerManager logger)
        {
            _logger = logger  ;
            _assignmentService = assignmentService;          
        }
        [HttpPost("assignments")]
        public async Task<IActionResult> Create(CreateAssignmentRequest assignmentRequest)
        {
            var result = await _assignmentService.Create(assignmentRequest);

            if (result == null) return StatusCode(500, "Result null");

            return Ok(result);
        }

        [HttpGet("assignments")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _assignmentService.GetAll();

            if (result == null) return StatusCode(500, "Result null");

            return Ok(result);
        }
        [HttpGet("assignments-detail")]
        public async Task<IActionResult> GetAssignmentDetail(string assetCode)
        {
            var result = await _assignmentService.GetAssignmentDetail(assetCode);

            if (result == null) return StatusCode(500, "Result null");

            return Ok(result);
        }
    }
}
