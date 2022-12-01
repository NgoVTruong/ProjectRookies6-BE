using FinalAssignment.DTOs.Assignment;
using FinalAssignment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinalAssignment.Controllers
{
    [Route("/api/assignment-management")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAssignmentService _assignmentService;
        public AssignmentController(IAssignmentService assignmentService, ILogger logger)
        {
            _assignmentService = assignmentService;
            _logger = logger;
        }
        [HttpPost("assignments")]
        public async Task<IActionResult> Create(CreateAssignmentRequest assignmentRequest)
        {
            var result = await _assignmentService.Create(assignmentRequest);

            if (result == null) return StatusCode(500, "Result null");

            return Ok(result);
        }
    }
}
