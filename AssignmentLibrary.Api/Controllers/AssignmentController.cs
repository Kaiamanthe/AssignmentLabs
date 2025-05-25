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
        public IActionResult Create(Assignment _assignment)
        {
            try
            {
                var assignment = new Assignment(_assignment.Title, _assignment.Description, false);
                var success = _service.AddAssignment(_assignment);
                if (!success)
                    return Conflict("Assignment with this title already exists.");
                return CreatedAtAction(nameof(GetByTitle), new { title = _assignment.Title }, _assignment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{title}")]
        public IActionResult Update(string title, Assignment _assignment)
        {
            try
            {
                var updated = _service.UpdateAssignment(title, _assignment.Title, _assignment.Description, _assignment.IsCompleted, _assignment.Priority);
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
