using AssignmentLibrary.Api.Models;
using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Models;
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
        public IActionResult Create(AssignmentDto dto)
        {
            try
            {
                var assignmentdto = new Assignment(dto.Title, dto.Description, dto.Notes, false);
                var success = _service.AddAssignment(assignmentdto);
                if (!success)
                    return Conflict("Assignment with this title already exists.");
                return CreatedAtAction(nameof(GetByTitle), new { title = dto.Title }, dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("{title}/note")]
        public IActionResult AddNoteToAssignment(string title, [FromBody] string note)
        {
            var assignment = _service.FindAssignmentByTitle(title);
            if (assignment == null)
            {
                return NotFound("Assignment not found.");
            }

            assignment.Notes = note ?? string.Empty;
            return Ok("Note added or updated successfully.");
        }

        [HttpPut("{title}")]
        public IActionResult Update(string title, AssignmentDto dto)
        {
            try
            {
                var updated = _service.UpdateAssignment(title, dto.Title, dto.Description, dto.Notes, dto.IsCompleted, dto.Priority);
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
            if (!success) return NotFound();

            var assignment = _service.FindByTitle(title);
            return Ok(assignment);
        }

        [HttpDelete("{title}")]
        public IActionResult Delete(string title)
        {
            var success = _service.DeleteAssignment(title);
            return success ? NoContent() : NotFound();
        }
    }
}
