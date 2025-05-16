using AssignmentLibrary.Api.Dtos;
using AssignmentLibrary.Core.Models;
using AssignmentLibrary.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _service;

        public AssignmentController(IAssignmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.ListAll());
        }

        [HttpGet("incomplete")]
        public IActionResult GetIncomplete()
        {
            return Ok(_service.ListIncomplete());
        }

        [HttpGet("{title}")]
        public IActionResult GetByTitle(string title)
        {
            var assignment = _service.FindAssignmentByTitle(title);
            return assignment == null ? NotFound() : Ok(assignment);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AssignmentDto dto)
        {
            try
            {
                var assignment = new Assignment(dto.Title, dto.Description, dto.IsCompleted = false);
                var success = _service.AddAssignment(assignment);
                if (!success)
                    return Conflict("Assignment with this title already exists.");
                return CreatedAtAction(nameof(GetByTitle), new { title = assignment.Title }, assignment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{title}")]
        public IActionResult Update(string title, [FromBody] AssignmentDto dto)
        {
            try
            {
                var updated = _service.UpdateAssignment(title, dto.Title, dto.Description);
                return updated ? NoContent() : NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{title}/complete")]
        public IActionResult MarkComplete(string title)
        {
            var success = _service.MarkAssignmentComplete(title);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{title}")]
        public IActionResult Delete(string title)
        {
            var success = _service.DeleteAssignment(title);
            return success ? NoContent() : NotFound();
        }
    }
}
